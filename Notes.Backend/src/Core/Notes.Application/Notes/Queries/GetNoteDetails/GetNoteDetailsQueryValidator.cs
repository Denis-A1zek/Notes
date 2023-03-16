using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>  
{
    public GetNoteDetailsQueryValidator()
    {
        RuleFor(getNoteDetails =>  
            getNoteDetails.NoteId).NotEqual(Guid.Empty);
        RuleFor(getNoteDetails =>
            getNoteDetails.UserId).NotEqual(Guid.Empty);
    }
}
