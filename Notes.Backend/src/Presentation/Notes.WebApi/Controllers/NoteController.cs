using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Models;
using Notes.Application.Notes.Queries;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers;

[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/{version:apiVersion}/[controller]")]
public class NoteController : BaseController
{
    private readonly IMapper _mapper;

    public NoteController(IMapper mapper)
    {
        _mapper=mapper;
    }


    /// <summary>
    /// Gets the list of notes
    /// </summary>
    /// <remarks>
    /// Sample request: GET /note
    /// </remarks>
    /// <returns>Returns NoteListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If not is unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteListVm>> GetAll() =>
        Ok(await Mediator.Send(new GetNoteListQuery { UserId = UserId }));


    /// <summary>
    /// Gets the note by id
    /// </summary>
    /// <remarks>
    /// Sample: 
    /// GET /note/6A479592-E6F2-4E49-A3DC-813A4681AADB
    /// </remarks>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns NoteDetailsVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If not is unauthorized</response>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDetailsVm>> Get(Guid id) =>
        Ok(await Mediator.Send(new GetNoteDetailsQuery { NoteId = id, UserId = UserId }));


    /// <summary>
    /// Create the note
    /// </summary>
    /// <remarks>
    /// Sample:
    /// POST /note
    /// {
    ///     title: "note title",
    ///     details: "note details"
    /// }
    /// </remarks>
    /// <param name="createNoteDto">CreateNoteDto object</param>
    /// <returns>Returns id (guid)</returns>
    /// <response code="201">Success</response>
    /// <response code="401">If not is unauthorized</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;
        
        return Ok(await Mediator.Send(command));
    }


    /// <summary>
    /// Update the note
    /// </summary>
    /// <remarks>
    /// Sample:
    /// PUT /note
    /// {
    ///     title: "note title",
    /// }
    /// </remarks>
    /// <param name="updateNoteDto">UpdateNoteDto object</param>
    /// <returns></returns>
    /// <response code="204">Success</response>
    /// <response code="401">If not is unauthorized</response>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Delelte the note by id
    /// </summary>
    /// <remarks>
    /// Sample:
    /// DELETE /note/6A479592-E6F2-4E49-A3DC-813A4681AADB
    /// </remarks>
    /// <param name="id">id (guid)</param>
    /// <returns>Return NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If not is unauthorized</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteNoteCommand { NoteId = id, UserId = UserId };
        await Mediator.Send(command);

        return NoContent();
    }
}
