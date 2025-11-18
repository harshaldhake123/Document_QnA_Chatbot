using DocQnA.Api.Requests;
using DocQnA.Application.Features.RAG.QueryDocument;
using Microsoft.AspNetCore.Mvc;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("rag")]
    public class RagController(
        QueryDocumentHandler queryDocumentHandler
        ) : ControllerBase
    {
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] RagQueryRequest request)
        {
            var command = new QueryDocumentCommand(request.Query);

            var result = await queryDocumentHandler.Handle(command);

            return Ok(result);
        }
    }
}