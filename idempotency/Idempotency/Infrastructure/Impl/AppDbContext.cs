using Infrastructure.Idempotency.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Idempotency.Infrastructure.Impl
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbOrder> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbIdempotencyRecord).Assembly);
        }
    }
}