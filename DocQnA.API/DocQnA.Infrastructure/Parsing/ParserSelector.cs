using DocQnA.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DocQnA.Infrastructure.Parsing
{
    public class ParserSelector(IEnumerable<IDocumentParser> parsers, ILogger<ParserSelector> logger) : IParserSelector
    {
        public IDocumentParser Select(string fileName, string contentType)
        {
            var parser = parsers.FirstOrDefault(p => p.CanParse(fileName, contentType));
            if (parser is null)
            {
                logger.LogWarning("No parser found for {FileName} / {ContentType}", fileName, contentType);
                throw new InvalidOperationException("Unsupported file type.");
            }

            logger.LogDebug("Selected parser {Parser} for {FileName}", parser.GetType().Name, fileName);
            return parser;
        }
    }
}