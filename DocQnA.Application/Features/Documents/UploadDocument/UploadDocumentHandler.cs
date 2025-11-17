using DocQnA.Application.Interfaces;
using DocQnA.Application.Services;

namespace DocQnA.Api
{
    public class UploadDocumentHandler(IDocumentIngestionService ingestion)
    {
        public Task<Guid> Handle(UploadDocumentCommand command)
        {
            var upload = new DocumentUpload(
                command.FileName,
                command.ContentType,
                command.Size,
                command.Content
            );

            return ingestion.IngestAsync(upload);
        }
    }
}