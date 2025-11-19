using DocQnA.Application.Interfaces;

namespace DocQnA.Application.Features.Query.QueryDocument
{
    public class QueryDocumentHandler(
        IEmbeddingService embeddingService,
        IChunkRepository chunkRepository,
        ILlmService llmService)
    {
        public async Task<QueryDocumentResult> Handle(QueryDocumentCommand command, CancellationToken cancellationToken)
        {
            var queryVector = await embeddingService.Embed(command.Query);

            const int numRelevantChunks = 8;
            var chunks = await chunkRepository.SearchByEmbeddingAsync(queryVector, numRelevantChunks, cancellationToken);

            var context = string.Join("\n\n", chunks.Select(c => c.Text));

            var answer = await llmService.GenerateAnswerAsync(command.Query, context, cancellationToken);

            return new QueryDocumentResult(answer, [.. chunks.Select(c => new QueryDocumentSource(c.Id, c.ChunkIndex))]
            );
        }
    }
}