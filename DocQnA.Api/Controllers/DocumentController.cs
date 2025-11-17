using DocQnA.Application.Interfaces;
using DocQnA.Application.Services;
using FluentValidation;
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
                return BadRequest("File is required.");

            if (request.File.Length > 5 * 1024 * 1024)
                return BadRequest("Max upload size is 5 MB.");

            var command = new UploadDocumentCommand(
                request.File.FileName,
                request.File.ContentType,
                request.File.Length,
                request.File.OpenReadStream()
            );

            var result = await handler.Handle(command);

            return Ok(new { documentId = result });
        }
    }

    public class UploadDocumentRequest
    {
        public IFormFile? File { get; set; }
    }
}