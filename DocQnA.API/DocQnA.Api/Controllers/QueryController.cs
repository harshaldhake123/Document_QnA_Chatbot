using DocQnA.Api.Contracts;
using DocQnA.Application.Features.Query.QueryDocument;
using DocQnA.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("api/query")]
    public class QueryController(
        IQueryService queryService
        ) : ControllerBase
    {
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryDocumentRequest request, CancellationToken cancellationToken)
        {
            var command = new QueryDocumentCommand(request.Query);

            var result = await queryService.QueryAsync(command, cancellationToken);

            return Ok(result);
        }
    }
}