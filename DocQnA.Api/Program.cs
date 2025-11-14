using DocQnA.Api.Data;
using DocQnA.Api.Services;
using Microsoft.EntityFrameworkCore;
using OpenAI;

namespace DocQnA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DB
            builder.Services.AddDbContext<AppDbContext>();

            // OpenAI
            builder.Services.AddSingleton(new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]!));

            // Services
            builder.Services.AddScoped<EmbeddingService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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