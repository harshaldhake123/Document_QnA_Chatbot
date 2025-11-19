namespace DocQnA.Application.Interfaces
{
    public interface ILlmService
    {
        Task<string> GenerateAnswerAsync(string question, string context, CancellationToken cancellationToken = default);
    }
}