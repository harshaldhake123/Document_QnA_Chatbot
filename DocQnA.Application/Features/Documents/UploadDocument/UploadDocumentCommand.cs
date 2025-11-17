namespace DocQnA.Api
{
    public record UploadDocumentCommand(string FileName, string ContentType, long Size, Stream Content);
}