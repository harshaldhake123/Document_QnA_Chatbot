using DocQnA.Application.Interfaces;
using DocQnA.Infrastructure.Database;
using DocQnA.Infrastructure.Database.Repository;
using DocQnA.Infrastructure.Ingestion;
using DocQnA.Infrastructure.OpenAI;
using DocQnA.Infrastructure.Parsing;
using DocQnA.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;

namespace DocQnA.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
            o => o.UseVector()
        ));

        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IChunkRepository, ChunkRepository>();

        services.AddSingleton(_ => new OpenAIClient(config["OpenAI:ApiKey"]));

        services.AddScoped<IEmbeddingService, EmbeddingService>();
        services.AddScoped<ILlmService, LlmService>();

        services.AddScoped<IDocumentParser, PdfDocumentParser>();
        services.AddScoped<IParserSelector, ParserSelector>();

        services.AddScoped<IDocumentIngestService, DocumentIngestService>();

        return services;
    }
}