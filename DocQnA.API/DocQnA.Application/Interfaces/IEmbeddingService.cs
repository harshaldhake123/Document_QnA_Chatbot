using Pgvector;

namespace DocQnA.Application.Interfaces
{
    public interface IEmbeddingService
    {
        Task<Vector> Embed(string text);
    }
}