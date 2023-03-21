using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commans;

public class UpdateNoteCommandTests : TestCommandBase
{
    [Fact]
    public async Task UpdateNoteCommandHandler_Success()
    {
        //Arrange
        var handler = new UpdateNoteCommandHandler(Context);
        var updateTitle = "new title";

        //Acr
        await handler.Handle(new UpdateNoteCommand()
        {
            Id = ApplicationDbContextFactory.NoteIdForUpdate,
            UserId = ApplicationDbContextFactory.UserBId,
            Title = updateTitle
        },CancellationToken.None);

        Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(note => note.Id == ApplicationDbContextFactory.NoteIdForUpdate && 
                    note.Title == updateTitle));
    }

    [Fact]
    public async Task UpdateNoteCommandHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new UpdateNoteCommandHandler(Context);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new UpdateNoteCommand()
                {
                    Id = Guid.NewGuid(),
                    UserId = ApplicationDbContextFactory.UserAId
                }, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateNoteCommandHandler_FailOrWrongUserId()
    {
        //Arrange
        var handler = new UpdateNoteCommandHandler(Context);
        //Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new UpdateNoteCommand()
                {
                    Id = ApplicationDbContextFactory.NoteIdForUpdate,
                    UserId = ApplicationDbContextFactory.UserAId,
                },CancellationToken.None));
    }
}
