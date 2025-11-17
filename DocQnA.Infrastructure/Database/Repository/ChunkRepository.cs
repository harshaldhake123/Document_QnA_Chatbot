using DocQnA.Domain.Entities;
using Pgvector;

namespace DocQnA.Infrastructure.Database.Repository
{
    public class ChunkRepository : IChunkRepository
    {
        public Task AddRangeAsync(IEnumerable<Chunk> chunks)
        {
            throw new NotImplementedException();
        }

        public Task<List<Chunk>> SearchByEmbeddingAsync(Vector queryVector, int topK)
        {
            throw new NotImplementedException();
        }
    }
}