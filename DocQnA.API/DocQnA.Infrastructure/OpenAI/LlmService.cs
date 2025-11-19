using DocQnA.Application.Interfaces;
using OpenAI;
using OpenAI.Chat;

namespace DocQnA.Infrastructure.OpenAI
{
    public sealed class LlmService(OpenAIClient client) : ILlmService
    {
        private const string SystemPrompt = """
            You are an expert assistant that provides concise and accurate answers based on the provided context.
            Use the given context to answer.
            If the context is clearly unrelated or incomplete, say "I do not know."
            Otherwise, answer using the best available context.
            """;

        private readonly ChatClient _chatClient = client.GetChatClient("gpt-4o-mini");

        public async Task<string> GenerateAnswerAsync(string question, string context, CancellationToken cancellationToken = default)
        {
            var response = await _chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage(SystemPrompt),
                    new UserChatMessage($@"
                            Context:  {context}
                            Question: {question}
                            Answer: ")
                ],
                cancellationToken: cancellationToken
            );

            return response.Value.Content[0].Text;
        }
    }
}