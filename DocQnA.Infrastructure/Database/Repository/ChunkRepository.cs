using DocQnA.Application.Interfaces;
using DocQnA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Pgvector;

namespace DocQnA.Infrastructure.Database.Repository
{
    public class ChunkRepository(AppDbContext db) : IChunkRepository
    {
        public async Task<List<Chunk>> SearchByEmbeddingAsync(Vector queryVector, int topK)
        {
            var npgParam = new NpgsqlParameter("query_embedding", queryVector);

            return await db.Chunks
                .FromSqlRaw(
                """

                SELECT
                    "Id",
                    "DocumentId",
                    "ChunkIndex",
                    "Text",
                    "Embedding"
                FROM "Chunks"
                    ORDER BY "Embedding" <=> @query_embedding
                    LIMIT {0}

                """,
             topK,
             npgParam)
         .ToListAsync();
        }

        public async Task<List<Chunk>> FindSimilarAsync(float[] queryEmbedding, int limit, CancellationToken ct = default)
        {
            var param = new NpgsqlParameter("query_embedding", queryEmbedding);

            return await db.Chunks
                .FromSqlRaw(
                    """

                    SELECT
                        "Id",
                        "DocumentId",
                        "ChunkIndex",
                        "Text",
                        "Embedding"
                    FROM "Chunks"
                        ORDER BY "Embedding" <=> @query_embedding
                        LIMIT { 0}

                    """,
                    limit,
                param)
            .ToListAsync(ct);
        }

        public async Task AddRangeAsync(IEnumerable<Chunk> chunks, CancellationToken ct = default)
        {
            await db.Chunks.AddRangeAsync(chunks, ct);
        }
    }
}