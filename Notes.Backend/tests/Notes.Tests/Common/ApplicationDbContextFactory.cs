using Microsoft.EntityFrameworkCore;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common;

public class ApplicationDbContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid NoteIdForDelete = Guid.NewGuid();
    public static Guid NoteIdForUpdate = Guid.NewGuid();

    public static ApplicationDbContext  Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        context.Notes.AddRange(
            new Domain.Note()
            {
                CreationDate = DateTime.Now,
                Details = "Details1",
                EditDate = null,
                Id = Guid.Parse("4EF59677-6973-4602-848F-35A6B680CA13"),
                Title = "Title1",
                UserId = UserAId,
            },
            new Domain.Note()
            {
                CreationDate = DateTime.Now,
                Details = "Details2",
                EditDate = null,
                Id = Guid.Parse("0134C2A1-9B7F-43E9-A3AC-A8EB2CF5230C"),
                Title = "Title2",
                UserId = UserBId,
            },
            new Domain.Note()
            {
                CreationDate = DateTime.Now,
                Details = "Details3",
                EditDate = null,
                Id = NoteIdForDelete,
                Title = "Title3",
                UserId = UserAId,
            },
            new Domain.Note()
            {
                CreationDate = DateTime.Now,
                Details = "Details4",
                EditDate = null,
                Id = NoteIdForUpdate,
                Title = "Title4",
                UserId = UserBId,
            });

        context.SaveChanges();
        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
