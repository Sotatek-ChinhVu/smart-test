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

            modelBuilder.Entity<Admin>().HasIndex(a => new { a.LoginId }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.AdminId }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.Hospital }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.SubDomain }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.EndSubDomain }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.EndPointDb }).IsUnique();
            modelBuilder.Entity<Tenant>().HasIndex(a => new { a.Db }).IsUnique();
        }

        public DbSet<Admin> Admins { get; set; } = default!;

        public DbSet<Notification> Notifications { get; set; } = default!;

        public DbSet<Scription> Scriptions { get; set; } = default!;

        public DbSet<Tenant> Tenants { get; set; } = default!;

    }
}
