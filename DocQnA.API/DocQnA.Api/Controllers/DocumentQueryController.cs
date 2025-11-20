using DocQnA.Api.Contracts;
using DocQnA.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentQueryController(IQueryService queryService) : ControllerBase
    {
        [HttpPost("{documentId:guid}/query")]
        public async Task<IActionResult> QuerySingle(Guid documentId, [FromBody] QueryDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = await queryService.QueryAsync(new(request.Query, documentId), cancellationToken);

            return Ok(result);
        }
    }
}