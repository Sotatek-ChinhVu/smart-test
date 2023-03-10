using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Base
{
    public class RepositoryBase
    {
        private readonly ITenantProvider _tenantProvider;
        public RepositoryBase(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public string GetCacheKey()
        {
            return _tenantProvider.GetClinicID() + "-" + this.GetType().Name;
        }

        private TenantDataContext? _trackingDataContext;
        public TenantDataContext TrackingDataContext
        {
            get
            {
                if (_trackingDataContext == null)
                {
                    _trackingDataContext = _tenantProvider.GetTrackingTenantDataContext();
                }
                return _trackingDataContext;
            }
        }

        private TenantNoTrackingDataContext? _noTrackingDataContext;
        public TenantNoTrackingDataContext NoTrackingDataContext
        {
            get
            {
                if (_noTrackingDataContext == null)
                {
                    _noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext();
                }
                return _noTrackingDataContext;
            }
        }

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
        }
    }
}
