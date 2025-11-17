using DocQnA.Application.Services;

namespace DocQnA.Application.Interfaces
{
    public interface IDocumentIngestionService
    {
        Task<Guid> IngestAsync(DocumentUpload documentUpload);
    }
}