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
            modelBuilder.Entity<AuditLog>().HasKey(a => new { a.LogId });
            modelBuilder.Entity<NewAuditLog>().HasKey(a => new { a.LogId });
        }

        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
    }
}
