namespace DocQnA.Application.Models
{
    public record DocumentUpload(
    string FileName,
    string ContentType,
    long Size,
    Stream Stream);
}