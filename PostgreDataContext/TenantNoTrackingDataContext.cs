using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class TenantNoTrackingDataContext : TenantDataContext
    {
        public TenantNoTrackingDataContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
