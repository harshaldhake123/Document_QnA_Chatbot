namespace DocQnA.Api.Application.RAG
{
    namespace DocQnA.Api.Infrastructure.Parsing
    {
        public interface IDocumentParser
        {
            bool CanParse(string fileName, string contentType);

            Task<string> ParseAsync(Stream stream);
        }
    }
}