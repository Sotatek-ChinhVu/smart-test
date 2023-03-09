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
                    System.Console.WriteLine("DI get tracking context before get provider" + _trackingDataContext?.GetHashCode());
                    _trackingDataContext = _tenantProvider.GetTrackingTenantDataContext();
                }
                System.Console.WriteLine("DI create tracking context " + _trackingDataContext.GetHashCode());
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
                    System.Console.WriteLine("DI get no tracking context before get provider " + _trackingDataContext?.GetHashCode());
                    _noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext();
                }
                System.Console.WriteLine("DI create no tracking context " + _noTrackingDataContext.GetHashCode());
                return _noTrackingDataContext;
            }
        }

        public void DisposeDataContext()
        {
            System.Console.WriteLine("DI disposable tracking " + _trackingDataContext?.GetHashCode());
            System.Console.WriteLine("DI disposable no  tracking " + _noTrackingDataContext?.GetHashCode());
            _tenantProvider.DisposeDataContext();
        }
    }
}
