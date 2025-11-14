using DocQnA.Api.Data;
using DocQnA.Api.Models;
using DocQnA.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using OpenAI.Chat;
using Pgvector;

namespace DocQnA.Api.Controllers
{
    [ApiController]
    [Route("rag")]
    public class RagController(AppDbContext db, EmbeddingService embeddingService, IConfiguration configuration, OpenAIClient client) : ControllerBase
    {
        [HttpPost("index")]
        public async Task<IActionResult> IndexText([FromBody] IndexRequest req)
        {
            var chunks = Chunker.ChunkText(req.Text);

            var dbChunks = new List<Chunk>();

            int index = 0;
            foreach (var chunk in chunks)
            {
                var embed = await embeddingService.Embed(chunk);

                dbChunks.Add(new Chunk
                {
                    ChunkIndex = index++,
                    Text = chunk,
                    Embedding = embed
                });
            }

            db.Chunks.AddRange(dbChunks);
            await db.SaveChangesAsync();

            return Ok(new { count = dbChunks.Count });
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryRequest req)
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

    public record IndexRequest(string Text);
    public record QueryRequest(string Query);
}