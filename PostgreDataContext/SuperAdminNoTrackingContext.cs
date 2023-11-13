using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class SuperAdminNoTrackingContext : SuperAdminContext
    {
        public SuperAdminNoTrackingContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
