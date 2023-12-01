using Entity.Logger;
using Entity.SuperAdmin;
using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class SuperAdminContext : DbContext
    {
        public SuperAdminContext(DbContextOptions options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasKey(a => new { a.Id });
            modelBuilder.Entity<Notification>().HasKey(a => new { a.Id });
            modelBuilder.Entity<Scription>().HasKey(a => new { a.Id });
            modelBuilder.Entity<Tenant>().HasKey(a => new { a.TenantId });
            modelBuilder.Entity<MigrationTenantHistory>().HasKey(a => new { a.Id });

            modelBuilder.Entity<Admin>().HasIndex(a => new { a.LoginId }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.Hospital }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.SubDomain }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.EndSubDomain }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.EndPointDb }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.Db }).HasFilter($"\"IS_DELETED\" = 0").IsUnique();
        }

        public DbSet<Admin> Admins { get; set; } = default!;

        public DbSet<Notification> Notifications { get; set; } = default!;

        public DbSet<Scription> Scriptions { get; set; } = default!;

        public DbSet<Tenant> Tenants { get; set; } = default!;

        public DbSet<MigrationTenantHistory> MigrationTenantHistories { get; set; } = default!;

    }
}
