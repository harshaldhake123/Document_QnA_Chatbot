using Pgvector;

namespace DocQnA.Domain.Entities
{
    public class Chunk
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ChunkIndex { get; set; }
        public string Text { get; set; } = string.Empty;
        public Vector? Embedding { get; set; }
        public Guid DocumentId { get; set; }
    }
}