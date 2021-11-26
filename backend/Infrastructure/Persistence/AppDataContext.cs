using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities;
using Infrastructure.Persistence.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        public AppDataContext(DbContextOptions<AppDataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMapping());
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IAuditEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedDate = utcNow;
                        entry.Entity.UpdatedBy = userId;
                        entry.Entity.UpdatedDate = utcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = userId;
                        entry.Entity.UpdatedDate = utcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Product> Products { get; set; }
    }
}