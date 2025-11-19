using DocQnA.Application.Interfaces;
using DocQnA.Domain.Entities;
using DocQnA.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DocQnA.Infrastructure.Database.Repository
{
    public class DocumentRepository(AppDbContext db, ILogger<DocumentRepository> logger) : IDocumentRepository
    {
        private readonly AppDbContext _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<Document> CreateDocumentAsync(Document document, CancellationToken cancellationToken = default)
        {
            _db.Documents.Add(document);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Created document record {DocumentId}", document.Id);
            return document;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _db.Database.BeginTransactionAsync(cancellationToken);

        public async Task SaveChunksAsync(IEnumerable<Chunk> chunks, CancellationToken cancellationToken = default)
        {
            _db.Chunks.AddRange(chunks);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Saved {Count} chunks", (chunks as ICollection<Chunk>)?.Count ?? chunks.Count());
        }

        public Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
            => transaction.CommitAsync(cancellationToken);

        public async Task MarkDocumentFailedAsync(Document document, CancellationToken cancellationToken = default)
        {
            document.Status = DocumentStatus.Failed;
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Marked document {DocumentId} as failed", document.Id);
        }
    }
}