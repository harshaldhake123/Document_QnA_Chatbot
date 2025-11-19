using DocQnA.Application.Interfaces;
using DocQnA.Application.Models;
using DocQnA.Application.Services;
using FluentValidation;

namespace DocQnA.Application.Features.Documents.UploadDocument
{
    public class UploadDocumentHandler(
        IValidator<UploadDocumentCommand> validator,
        IDocumentIngestService documentIngestService)
    {
        public async Task<Result<Guid>> HandleAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(command, cancellationToken);

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

            var id = await documentIngestService.IngestAsync(upload, cancellationToken);
            return Result<Guid>.Success(id);
        }
    }
}