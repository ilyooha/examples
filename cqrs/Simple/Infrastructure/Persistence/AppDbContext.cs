using Microsoft.EntityFrameworkCore;

namespace Simple.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<EfProduct> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}