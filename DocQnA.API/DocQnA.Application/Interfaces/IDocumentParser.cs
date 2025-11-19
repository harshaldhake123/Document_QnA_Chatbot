namespace DocQnA.Application.Interfaces
{
    public interface IDocumentParser
    {
        bool CanParse(string fileName, string contentType);

        Task<string> ParseAsync(Stream stream, CancellationToken cancellationToken);
    }
}