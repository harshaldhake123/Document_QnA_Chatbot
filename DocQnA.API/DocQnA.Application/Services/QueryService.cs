using DocQnA.Application.Features.Query.QueryDocument;

namespace DocQnA.Application.Services
{
    public class QueryService(QueryDocumentHandler handler) : IQueryService
    {
        public Task<QueryDocumentResult> QueryAsync(QueryDocumentCommand command, CancellationToken cancellationToken)
        {
            return handler.Handle(command, cancellationToken);
        }
    }
}