using DocQnA.Api.Application.RAG.DocQnA.Api.Infrastructure.Parsing;
using UglyToad.PdfPig;

namespace DocQnA.Infrastructure.Parsing
{
    public class PdfDocumentParser : IDocumentParser
    {
        public bool CanParse(string fileName, string contentType)
        {
            return fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ParseAsync(Stream stream)
        {
            stream.Position = 0;

            using var doc = PdfDocument.Open(stream);
            var texts = doc.GetPages()
                .Select(page => page.Text)
                .Where(text => !string.IsNullOrWhiteSpace(text));

            var result = string.Join(Environment.NewLine + Environment.NewLine, texts);

            return Task.FromResult(result);
        }
    }
}