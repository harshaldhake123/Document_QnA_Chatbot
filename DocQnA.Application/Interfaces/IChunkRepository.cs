using DocQnA.Domain.Entities;
using Pgvector;

namespace DocQnA.Infrastructure.Database.Repository
{
    public interface IChunkRepository
    {
        Task<List<Chunk>> SearchByEmbeddingAsync(Vector queryVector, int topK);

        Task AddRangeAsync(IEnumerable<Chunk> chunks);
    }
}