using DocQnA.Application;
using DocQnA.Infrastructure;

namespace DocQnA.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddApplication();
            services.AddInfrastructure(config);

            return services;
        }
    }
}