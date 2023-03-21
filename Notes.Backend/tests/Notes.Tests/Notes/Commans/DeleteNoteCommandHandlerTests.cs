using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commans;

public class DeleteNoteCommandHandlerTests : TestCommandBase
{
    //Проверка на успешное удаление заметки из базы данных
    [Fact]
    public async Task DeleteNoteCommandHandler_Success()
    {
        var handler = new DeleteNoteCommandHandler(Context);


        await handler.Handle(new DeleteNoteCommand()
        {
            NoteId = ApplicationDbContextFactory.NoteIdForDelete,
            UserId = ApplicationDbContextFactory.UserAId
        }, CancellationToken.None);

        //Assert
        Assert.Null(Context.Notes.SingleOrDefault(note =>
            note.Id == ApplicationDbContextFactory.NoteIdForDelete));
    }


    //Удаляем заметку, которой нет в базе данных
    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new DeleteNoteCommandHandler(Context);
        //Act 

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new DeleteNoteCommand()
                {
                    NoteId = Guid.NewGuid(),
                    UserId = ApplicationDbContextFactory.UserAId
                }, CancellationToken.None));
    }

    //Удаление заметки от пользователя, кому не принадлежит заметки
    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
    {
        //Arrange
        var deleteHandler = new DeleteNoteCommandHandler(Context);
        var createHandler = new CreateNoteCommandHandler(Context);
        var noteId = await createHandler.Handle(new CreateNoteCommand()
        {
            UserId = ApplicationDbContextFactory.UserAId,
            Title = "Test",
            Details = "Test Details"
        }, CancellationToken.None);
        //Act 

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(new DeleteNoteCommand()
                {
                    NoteId = noteId,
                    UserId = ApplicationDbContextFactory.UserBId
                }, CancellationToken.None));
    }
}
