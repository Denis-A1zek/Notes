using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Models;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteDetailsQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper=mapper;
        _context=context;
    }

    public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Notes
                .FirstOrDefaultAsync(note => 
                note.Id == request.NoteId, cancellationToken);

        if(entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Note), request.NoteId);
        }

        return _mapper.Map<NoteDetailsVm>(entity);
    }
}
