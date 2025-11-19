namespace DocQnA.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);

        Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken);
    }
}