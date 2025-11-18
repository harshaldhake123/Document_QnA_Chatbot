using DocQnA.Application.Features.Documents.UploadDocument;
using DocQnA.Application.Features.RAG.QueryDocument;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DocQnA.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Handlers
            services.AddScoped<UploadDocumentHandler>();
            services.AddScoped<QueryDocumentHandler>();

            // Validators
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}