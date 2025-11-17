using DocQnA.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace DocQnA.Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> CreateDocumentAsync(Document document, CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task SaveChunksAsync(IEnumerable<Chunk> chunks, CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        Task MarkDocumentFailedAsync(Document document, CancellationToken cancellationToken = default);
    }
}