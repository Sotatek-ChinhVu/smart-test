using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Base
{
    public class SuperAdminRepositoryBase
    {
        private readonly ITenantProvider _tenantProvider;
        public SuperAdminRepositoryBase(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        private SuperAdminContext? _trackingDataContext;
        public SuperAdminContext TrackingDataContext
        {
            get
            {
                if (_trackingDataContext == null)
                {
                    _trackingDataContext = _tenantProvider.GetSuperAdminTrackingTenantDataContext();
                }
                return _trackingDataContext;
            }
        }

        private SuperAdminNoTrackingContext? _noTrackingDataContext;
        public SuperAdminNoTrackingContext NoTrackingDataContext
        {
            get
            {
                if (_noTrackingDataContext == null)
                {
                    _noTrackingDataContext = _tenantProvider.GetSuperAdminNoTrackingDataContext();
                }
                return _noTrackingDataContext;
            }
        }

        public void DisposeDataContext()
        {
            _tenantProvider.DisposeDataContext();
        }
    }
}
