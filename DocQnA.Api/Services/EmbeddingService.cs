using OpenAI;
using Pgvector;

namespace DocQnA.Api.Services;

public class EmbeddingService(OpenAIClient client)
{
    public async Task<Vector> Embed(string text)
    {
        var embeddingClient = client.GetEmbeddingClient("text-embedding-3-small");
        var result = await embeddingClient.GenerateEmbeddingAsync(text);

        return new Vector(result.Value.ToFloats());
    }
}