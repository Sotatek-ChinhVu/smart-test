using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class TenantNoTrackingDataContext : TenantDataContext
    {
        public TenantNoTrackingDataContext(string connectionString, bool useStaging = false) : base(connectionString, useStaging)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
