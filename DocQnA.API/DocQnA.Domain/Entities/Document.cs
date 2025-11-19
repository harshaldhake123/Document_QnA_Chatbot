using DocQnA.Domain.Enums;

namespace DocQnA.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = "";
        public string FileKey { get; set; } = "";
        public long Size { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.Uploaded;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Chunk>? Chunks { get; set; }
    }
}