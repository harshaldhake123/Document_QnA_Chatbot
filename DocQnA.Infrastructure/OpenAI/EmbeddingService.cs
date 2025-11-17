using DocQnA.Application.Interfaces;
using OpenAI;
using Pgvector;

namespace DocQnA.Infrastructure.OpenAI
{
    public class EmbeddingService(OpenAIClient client) : IEmbeddingService
    {
        public async Task<Vector> Embed(string text)
        {
            var embeddingClient = client.GetEmbeddingClient("text-embedding-3-small");
            var result = await embeddingClient.GenerateEmbeddingAsync(text);

            return new Vector(result.Value.ToFloats());
        }
    }
    public interface ILlmService
    {
        Task<string> AnswerAsync(string question, string context);
    }

}