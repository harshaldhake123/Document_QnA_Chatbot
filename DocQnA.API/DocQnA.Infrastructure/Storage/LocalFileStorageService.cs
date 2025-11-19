using DocQnA.Application.Interfaces;

namespace DocQnA.Infrastructure.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private const string FileStorageFolder = "Uploads";
        private readonly string _root;

        public LocalFileStorageService()
        {
            _root = Path.Combine(Directory.GetCurrentDirectory(), FileStorageFolder);
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
        {
            var sanitized = SanitizeFileName(fileName);
            var key = $"{Guid.NewGuid():N}_{sanitized}";
            var fullPath = Path.Combine(_root, key);

            using var fs = File.Create(fullPath);
            await fileStream.CopyToAsync(fs, cancellationToken);

            return key;
        }

        public Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken)
        {
            var fullPath = Path.Combine(_root, key);
            return Task.FromResult<Stream>(File.OpenRead(fullPath));
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }
    }
}