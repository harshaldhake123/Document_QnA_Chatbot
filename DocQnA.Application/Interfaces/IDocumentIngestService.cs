using DocQnA.Application.Services;

namespace DocQnA.Application.Interfaces
{
    public interface IDocumentIngestService
    {
        Task<Guid> IngestAsync(DocumentUpload documentUpload);
    }
}