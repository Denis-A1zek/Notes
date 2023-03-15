using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Dto;
using Notes.Application.Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Queries.GetNoteList;

public class GetNoteListQueryHandler : IRequestHandler<GetNoteListQuery, NoteListVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteListQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper=mapper;
        _context=context;
    }

    public async Task<NoteListVm> Handle(GetNoteListQuery request, CancellationToken cancellationToken)
    {
        var notes = await _context.Notes.Where(note =>
                 note.UserId == request.UserId)
                .ProjectTo<NoteLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        return new NoteListVm { Notes = notes};
    }
}
