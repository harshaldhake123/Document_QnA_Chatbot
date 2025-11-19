using DocQnA.Application.Features.Query.QueryDocument;

namespace DocQnA.Application.Services
{
    public interface IQueryService
    {
        Task<QueryDocumentResult> QueryAsync(QueryDocumentCommand command, CancellationToken cancellationToken);
    }
}