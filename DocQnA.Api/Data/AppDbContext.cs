using DocQnA.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DocQnA.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : DbContext(options)
    {
        public DbSet<Chunk> Chunks => Set<Chunk>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<Chunk>()
                .Property(c => c.Embedding)
                .HasColumnType("vector(1536)");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"), 
                o => o.UseVector());
        }
    }
}