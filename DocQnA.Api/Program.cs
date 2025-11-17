using DocQnA.Api.Application.RAG.DocQnA.Api.Infrastructure.Parsing;
using DocQnA.Api.Infrastructure.OpenAI.DocQnA.Api.Infrastructure.OpenAI;
using DocQnA.Api.Middleware;
using DocQnA.Application.Interfaces;
using DocQnA.Infrastructure.Database;
using DocQnA.Infrastructure.Ingestion;
using DocQnA.Infrastructure.OpenAI;
using DocQnA.Infrastructure.Parsing;
using DocQnA.Infrastructure.Storage;
using FluentValidation;
using OpenAI;

namespace DocQnA.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddSingleton(new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]!));
            builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();
            builder.Services.AddScoped<IDocumentParser, PdfDocumentParser>();
            builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();
            builder.Services.AddScoped<DocumentIngestionService>();
            builder.Services.AddScoped<IParserSelector, ParserSelector>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddControllers();
            builder.Services.AddExceptionHandler<AppExceptionMiddleware>();
            builder.Services.AddProblemDetails();

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}