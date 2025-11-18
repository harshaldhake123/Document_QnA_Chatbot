using DocQnA.Application.Interfaces;
using DocQnA.Application.Services;
using FluentValidation;

namespace DocQnA.Application.Features.Documents.UploadDocument
{
    public class UploadDocumentHandler(
        IValidator<UploadDocumentCommand> validator,
        IDocumentIngestService documentIngestService)
    {
        public async Task<Result<Guid>> Handle(UploadDocumentCommand command)
        {
            var validation = await validator.ValidateAsync(command);

            if (!validation.IsValid)
            {
                var errorText = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
                return Result<Guid>.Fail(errorText);
            }

            var upload = new DocumentUpload(
                command.FileName,
                command.ContentType,
                command.Size,
                command.Stream
            );

            var id = await documentIngestService.IngestAsync(upload);
            return Result<Guid>.Success(id);
        }
    }

    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Fail(string error) => new(error);
    }
}