using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class TenantNoTrackingDataContext : TenantDataContextBase
    {
        public TenantNoTrackingDataContext(string connectionString) : base(connectionString)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
