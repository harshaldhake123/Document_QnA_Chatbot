using DocQnA.Domain.Entities;
using Pgvector;

namespace DocQnA.Application.Interfaces
{
    public interface IChunkRepository
    {
        Task<List<Chunk>> SearchByEmbeddingAsync(Vector queryVector, Guid documentId, int topK, CancellationToken cancellationToken);

        Task<List<Chunk>> FindSimilarAsync(float[] queryEmbedding, int limit, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<Chunk> chunks, CancellationToken cancellationToken);
    }
}