using DocQnA.Application.Interfaces;
using DocQnA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Pgvector;

namespace DocQnA.Infrastructure.Database.Repository
{
    public class ChunkRepository(AppDbContext db) : IChunkRepository
    {
        public async Task<List<Chunk>> SearchByEmbeddingAsync(Vector queryVector, Guid documentId, int topK, CancellationToken cancellationToken)
        {
            var embeddingParam = new NpgsqlParameter("query_embedding", queryVector);
            var documentParam = new NpgsqlParameter("document_id", documentId);

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
                    WHERE "DocumentId" = @document_id
                    ORDER BY "Embedding" <=> @query_embedding
                    LIMIT {0}
                    """,
                topK, documentParam, embeddingParam).ToListAsync(cancellationToken);
        }

        public async Task<List<Chunk>> FindSimilarAsync(float[] queryEmbedding, int limit, CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<Chunk> chunks, CancellationToken cancellationToken)
        {
            await db.Chunks.AddRangeAsync(chunks, cancellationToken);
        }
    }
}