using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Models;
using Notes.Application.Notes.Queries;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers;

[Route("api/[controller]")]
public class NoteController : BaseController
{
    private readonly IMapper _mapper;

    public NoteController(IMapper mapper)
    {
        _mapper=mapper;
    }

    [HttpGet]
    public async Task<ActionResult<NoteListVm>> GetAll() =>
        Ok(await Mediator.Send(new GetNoteListQuery { UserId = UserId }));

    [HttpGet("{id}")]
    public async Task<ActionResult<NoteDetailsVm>> Get(Guid id) =>
        Ok(await Mediator.Send(new GetNoteDetailsQuery { NoteId = id, UserId = UserId }));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;
        
        return Ok(await Mediator.Send(command));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteNoteCommand { NoteId = id, UserId = UserId };
        await Mediator.Send(command);

        return NoContent();
    }
}
