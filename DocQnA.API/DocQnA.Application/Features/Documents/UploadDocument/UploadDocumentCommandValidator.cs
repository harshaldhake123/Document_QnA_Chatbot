using FluentValidation;

namespace DocQnA.Application.Features.Documents.UploadDocument
{
    public class UploadDocumentCommandValidator
    : AbstractValidator<UploadDocumentCommand>
    {
        private static readonly string[] AllowedExtensions = [".pdf"];

        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .Must(fn => AllowedExtensions.Any(ext =>
                    fn.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Unsupported file format. Only PDF is supported.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("File is empty.");

            RuleFor(x => x.Stream)
                .NotNull().WithMessage("File stream is missing.");
        }
    }
}