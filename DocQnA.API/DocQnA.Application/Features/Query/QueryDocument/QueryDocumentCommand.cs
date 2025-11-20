namespace DocQnA.Application.Features.Query.QueryDocument
{
    public record QueryDocumentCommand(string? Query, Guid DocumentId);

    public record QueryDocumentResult(string Answer, IReadOnlyList<QueryDocumentSource> Sources);

    public record QueryDocumentSource(Guid ChunkId, int ChunkIndex);
}