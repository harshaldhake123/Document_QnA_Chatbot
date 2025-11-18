using DocQnA.Api.Requests;
using DocQnA.Application.Features.Documents.UploadDocument;
using Microsoft.AspNetCore.Mvc;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentController(UploadDocumentHandler handler) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request)
        {
            if (request.File == null)
            {
                return BadRequest("File is required.");
            }

            var command = new UploadDocumentCommand(
                request.File.FileName,
                request.File.ContentType ?? "",
                request.File.Length,
                request.File.OpenReadStream()
            );

            var result = await handler.Handle(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(new { documentId = result.Value });
        }
    }
}