namespace DocQnA.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);

        Task<Stream> OpenReadAsync(string key);
    }
}