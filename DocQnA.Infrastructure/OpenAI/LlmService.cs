using DocQnA.Application.Interfaces;
using OpenAI;
using OpenAI.Chat;

namespace DocQnA.Infrastructure.OpenAI
{
    public sealed class LlmService : ILlmService
    {
        private readonly ChatClient _chatClient;

        public LlmService(OpenAIClient client)
        {
            _chatClient = client.GetChatClient("gpt-4o-mini");
        }

        public async Task<string> GenerateAnswerAsync(string question, string context, CancellationToken cancellationToken = default)
        {
            var response = await _chatClient.CompleteChatAsync(
                [
                new SystemChatMessage("Use only the given context. If the context does not contain the answer, say you do not know."),
                new UserChatMessage($"Context:\n{context}\n\nQuestion:\n{question}")
                ],
                cancellationToken: cancellationToken
            );

            return response.Value.Content[0].Text;
        }
    }
}