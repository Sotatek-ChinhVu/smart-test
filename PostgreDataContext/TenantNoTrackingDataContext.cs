using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class TenantNoTrackingDataContext : TenantDataContext
    {
        public TenantNoTrackingDataContext(string connectionString) : base(connectionString)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
