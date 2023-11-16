using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Base;

public class AuditLogRepositoryBase
{
    private readonly ITenantProvider _tenantProvider;
    public AuditLogRepositoryBase(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    private AdminDataContext? _trackingDataContext;
    public AdminDataContext TrackingDataContext
    {
        get
        {
            if (_trackingDataContext == null)
            {
                _trackingDataContext = _tenantProvider.GetAuditLogTrackingDataContext();
            }
            return _trackingDataContext;
        }
    }

    private AdminNoTrackingContext? _noTrackingDataContext;
    public AdminNoTrackingContext NoTrackingDataContext
    {
        get
        {
            if (_noTrackingDataContext == null)
            {
                _noTrackingDataContext = _tenantProvider.GetAuditLogNoTrackingDataContext();
            }
            return _noTrackingDataContext;
        }
    }

    public void DisposeDataContext()
    {
        _tenantProvider.DisposeDataContext();
    }
}
