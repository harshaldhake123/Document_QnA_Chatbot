namespace DocQnA.Application.Interfaces
{
    public interface IParserSelector
    {
        IDocumentParser Select(string fileName, string contentType);
    }
}