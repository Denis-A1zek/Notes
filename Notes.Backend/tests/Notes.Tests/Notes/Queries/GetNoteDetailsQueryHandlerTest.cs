using AutoMapper;
using Notes.Application.Notes.Models;
using Notes.Application.Notes.Queries.GetNoteDetails;
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
    public class GetNoteDetailsQueryHandlerTest
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNoteDetailsQueryHandlerTest(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNoteDetailsQueryHandler_Success()
        {
            //Arrange
            var handler = new GetNoteDetailsQueryHandler(_mapper,_context);

            //Act
            var result = await handler.Handle(new Application.Notes.Queries.GetNoteDetailsQuery()
            {
                UserId = ApplicationDbContextFactory.UserBId,
                NoteId = Guid.Parse("0134C2A1-9B7F-43E9-A3AC-A8EB2CF5230C")
            },CancellationToken.None);

            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.Date.ShouldBe(DateTime.Today);
        }
    }
}
