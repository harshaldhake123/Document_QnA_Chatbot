using DocQnA.Application.Interfaces;

namespace DocQnA.Application.Features.RAG.QueryDocument
{
    public class QueryDocumentHandler(
        IEmbeddingService embeddingService,
        IChunkRepository chunkRepository,
        ILlmService llmService)
    {
        public async Task<QueryDocumentResult> Handle(
            QueryDocumentCommand command,
            CancellationToken cancellationToken = default)
        {
            var queryVector = await embeddingService.Embed(command.Query);

            var chunks = await chunkRepository.SearchByEmbeddingAsync(queryVector, 5);

            var context = string.Join("\n\n", chunks.Select(c => c.Text));

            var answer = await llmService.GenerateAnswerAsync(
                command.Query,
                context,
                cancellationToken
            );

            return new QueryDocumentResult(answer, [.. chunks.Select(c => new QueryDocumentSource(c.Id, c.ChunkIndex))]
            );
        }
    }
}