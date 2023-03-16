using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Note> Notes { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
