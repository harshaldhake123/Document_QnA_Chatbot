using DocQnA.Application.Models;

namespace DocQnA.Application.Interfaces
{
    public interface IDocumentIngestService
    {
        Task<Guid> IngestAsync(DocumentUpload documentUpload, CancellationToken cancellationToken);
    }
}