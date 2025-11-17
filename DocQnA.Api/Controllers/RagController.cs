using DocQnA.Application.Features.RAG.QueryDocument;
using DocQnA.Application.Interfaces;
using DocQnA.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using OpenAI.Chat;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("rag")]
    public class RagController(AppDbContext db, IEmbeddingService embeddingService, OpenAIClient client) : ControllerBase
    {
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryDocumentCommand req)
        {
            var queryEmbedding = await embeddingService.Embed(req.Query);

            var parameters = new[]
            {
            new Npgsql.NpgsqlParameter("query_embedding", queryEmbedding)
        };

            var results = await db.Chunks
                .FromSqlRaw(
                    """SELECT * FROM "Chunks" ORDER BY "Embedding" <=> @query_embedding LIMIT 5""",
                    parameters)
                .ToListAsync();

            var context = string.Join("\n\n", results.Select(r => r.Text));

            var answer = await GenerateAnswer(req.Query, context);

            return Ok(new
            {
                answer,
                sources = results.Select(r => new { r.Id, r.ChunkIndex })
            });
        }

        private async Task<string> GenerateAnswer(string question, string context)
        {
            var chat = client.GetChatClient("gpt-4o-mini");

            var response = await chat.CompleteChatAsync(
                    new SystemChatMessage("Use only the given context."),
                    new UserChatMessage($"Context:\n{context}\n\nQuestion: {question}")
                );

            return response.Value.Content[0].Text;
        }
    }
}