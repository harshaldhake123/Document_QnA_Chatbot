using DocQnA.Application.Features.Documents.UploadDocument;
using DocQnA.Application.Models;

namespace DocQnA.Application.Services
{
    public class DocumentService(UploadDocumentHandler handler) : IDocumentService
    {
        public Task<Result<Guid>> UploadAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
        {
            return handler.HandleAsync(command, cancellationToken);
        }
    }
}