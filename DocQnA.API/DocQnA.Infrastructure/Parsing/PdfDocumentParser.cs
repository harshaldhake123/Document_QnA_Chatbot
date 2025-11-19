using DocQnA.Application.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace DocQnA.Infrastructure.Parsing
{
    public class PdfDocumentParser : IDocumentParser
    {
        public bool CanParse(string fileName, string contentType)
        {
            return fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ParseAsync(Stream stream, CancellationToken cancellationToken)
        {
            stream.Position = 0;

            using var doc = PdfDocument.Open(stream);
            var sb = new StringBuilder();

            foreach (var page in doc.GetPages())
            {
                var words = page.GetWords();

                if (words != null && words.Any())
                {
                    var line = string.Join(" ", words.Select(w => w.Text));
                    sb.AppendLine(line);
                    sb.AppendLine();
                }
            }

            return Task.FromResult(sb.ToString());
        }
    }
}