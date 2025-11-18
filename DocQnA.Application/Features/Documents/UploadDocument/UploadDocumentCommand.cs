namespace DocQnA.Application.Features.Documents.UploadDocument
{
    public record UploadDocumentCommand(string FileName, string ContentType, long Size, Stream Content);
}