using FluentValidation;

namespace DocQnA.Application.Features.Query.QueryDocument
{
    public class QueryDocumentCommandValidator : AbstractValidator<QueryDocumentCommand>
    {
        public QueryDocumentCommandValidator()
        {
            RuleFor(x => x.Query)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Query is required.")
                .Must(q => !string.IsNullOrWhiteSpace(q)).WithMessage("Query cannot be empty or whitespace.")
                .MinimumLength(3).WithMessage("Query must be at least 3 characters long.")
                .MaximumLength(2000).WithMessage("Query must not exceed 2000 characters.");
        }
    }
}