using DocQnA.Application.Features.Documents.UploadDocument;
using DocQnA.Application.Models;

namespace DocQnA.Application.Services
{
    public interface IDocumentService
    {
        Task<Result<Guid>> UploadAsync(UploadDocumentCommand command, CancellationToken cancellationToken);
    }
}