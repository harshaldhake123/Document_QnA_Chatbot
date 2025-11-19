using DocQnA.Application.Features.Documents.UploadDocument;
using DocQnA.Application.Features.Query.QueryDocument;
using DocQnA.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DocQnA.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IDocumentService, DocumentService>();

            // Handlers
            services.AddScoped<UploadDocumentHandler>();
            services.AddScoped<QueryDocumentHandler>();

            // Validators
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}