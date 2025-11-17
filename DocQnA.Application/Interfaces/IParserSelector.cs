using DocQnA.Api.Application.RAG.DocQnA.Api.Infrastructure.Parsing;

namespace DocQnA.Application.Interfaces
{
    public interface IParserSelector
    {
        IDocumentParser Select(string fileName, string contentType);
    }
}