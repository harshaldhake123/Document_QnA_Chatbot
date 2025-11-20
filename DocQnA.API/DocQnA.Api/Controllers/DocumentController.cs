using DocQnA.Api.Contracts;
using DocQnA.Application.Features.Documents.UploadDocument;
using DocQnA.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentController(IDocumentService documentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentRequest request, CancellationToken cancellationToken)
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

            var result = await documentService.UploadAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(new { documentId = result.Value });
        }
    }
}