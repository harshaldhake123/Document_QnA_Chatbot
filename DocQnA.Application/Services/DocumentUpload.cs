namespace DocQnA.Application.Services
{
    public record DocumentUpload(
    string FileName,
    string ContentType,
    long Size,
    Stream Stream);
}