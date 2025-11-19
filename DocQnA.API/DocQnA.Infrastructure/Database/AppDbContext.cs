using DocQnA.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocQnA.Infrastructure.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Chunk> Chunks => Set<Chunk>();
        public DbSet<Document> Documents => Set<Document>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder
                .Entity<Chunk>()
                .Property(c => c.Embedding)
                .HasColumnType("vector(1536)");
        }
    }
}