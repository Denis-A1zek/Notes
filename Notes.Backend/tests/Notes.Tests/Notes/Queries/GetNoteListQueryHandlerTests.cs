using AutoMapper;
using Notes.Application.Notes.Models;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNoteListQueryHandlerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNoteListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async void GetNoteListQueryHandler_Success()
        {
            //Arrange
            var handler = new GetNoteListQueryHandler(_mapper, _context);

            //Act 
            var result = await handler.Handle(new GetNoteListQuery()
            {
                UserId = ApplicationDbContextFactory.UserBId
            },CancellationToken.None);

            //Assert
            result.ShouldBeOfType<NoteListVm>();
            result.Notes.Count.ShouldBe(2);
        }
    }
}
