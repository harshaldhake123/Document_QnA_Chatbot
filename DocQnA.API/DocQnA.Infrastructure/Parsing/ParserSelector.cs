using DocQnA.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DocQnA.Infrastructure.Parsing
{
    public class ParserSelector(IEnumerable<IDocumentParser> parsers, ILogger<ParserSelector>? logger = null) : IParserSelector
    {
        private readonly IEnumerable<IDocumentParser> _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));

        public IDocumentParser Select(string fileName, string contentType)
        {
            var parser = _parsers.FirstOrDefault(p => p.CanParse(fileName, contentType));
            if (parser is null)
            {
                logger?.LogWarning("No parser found for {FileName} / {ContentType}", fileName, contentType);
                throw new InvalidOperationException("Unsupported file type.");
            }

            logger?.LogDebug("Selected parser {Parser} for {FileName}", parser.GetType().Name, fileName);
            return parser;
        }
    }
}