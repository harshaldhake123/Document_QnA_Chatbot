using DocQnA.Application.Interfaces;
using DocQnA.Application.Models;
using DocQnA.Application.Utils;
using DocQnA.Domain.Entities;
using DocQnA.Domain.Enums;
using DocQnA.Infrastructure.Database;

namespace DocQnA.Infrastructure.Ingestion
{
    public class DocumentIngestService(
        IFileStorageService storage,
        IParserSelector parserSelector,
        IEmbeddingService embedding,
        AppDbContext db) : IDocumentIngestService
    {
        private const int MinimumInputDocumentTextLength = 200;
        private static readonly TimeSpan EmbeddingTimeout = TimeSpan.FromSeconds(15);

        public async Task<Guid> IngestAsync(DocumentUpload documentUpload, CancellationToken cancellationToken)
        {
            var fileKey = await storage.SaveAsync(documentUpload.Stream, documentUpload.FileName, cancellationToken);

            var document = new Document
            {
                FileName = documentUpload.FileName,
                FileKey = fileKey,
                Size = documentUpload.Size,
                Status = DocumentStatus.Processing
            };

            db.Documents.Add(document);
            await db.SaveChangesAsync(cancellationToken);

            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var parser = parserSelector.Select(documentUpload.FileName, documentUpload.ContentType);

                await using var storedStream = await storage.OpenReadAsync(fileKey, cancellationToken);
                var text = await parser.ParseAsync(storedStream, cancellationToken);

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException("The document contains no readable text.");
                }

                if (text.Length < MinimumInputDocumentTextLength)
                {
                    throw new InvalidOperationException(
                        "The extracted text is too short for meaningful processing.");
                }

                const int maxCharsPerChunk = 1000;
                const int chunkCharOverlap = 150;
                var chunks = Chunker.ChunkText(text, maxCharsPerChunk, chunkCharOverlap);

                if (chunks.Count == 0)
                {
                    throw new InvalidOperationException("Failed to create chunks from the document.");
                }

                var chunkEntities = await Task.WhenAll(
                    chunks.Select(async (chunk, idx) =>
                    {
                        var vector = await embedding
                            .Embed(chunk)
                            .WaitAsync(EmbeddingTimeout);

                        return new Chunk
                        {
                            DocumentId = document.Id,
                            ChunkIndex = idx,
                            Text = chunk,
                            Embedding = vector
                        };
                    })
                );

                db.Chunks.AddRange(chunkEntities);

                document.Status = DocumentStatus.Uploaded;
                await db.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return document.Id;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                document.Status = DocumentStatus.Failed;
                await db.SaveChangesAsync(cancellationToken);

                throw;
            }
        }
    }
}