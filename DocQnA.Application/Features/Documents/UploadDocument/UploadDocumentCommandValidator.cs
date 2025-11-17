using FluentValidation;

namespace DocQnA.Api
{
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        private readonly string[] _allowedExtensions = [".pdf"];

        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(f => f!.Length > 0).WithMessage("File is empty.")
                .Must(f => _allowedExtensions.Any(ext => f!.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Unsupported file format. Only PDF is supported right now.");
        }
    }
}