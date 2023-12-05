using Entity.Logger;
using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class AdminDataContext : DbContext
    {
        public AdminDataContext(DbContextOptions options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasKey(a => new { a.LogId, a.TenantId });
        }

        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
    }
}
