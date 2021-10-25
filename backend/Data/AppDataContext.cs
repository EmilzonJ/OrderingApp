using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext() { }

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
        }

        public DbSet<Product> Products { get; set; }
    }
}