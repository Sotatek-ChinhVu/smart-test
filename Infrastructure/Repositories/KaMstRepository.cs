using Domain.Models.KaMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KaMstRepository : IKaMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public KaMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }
    public int GetKaIdByKaSname(string kaSname)
    {
        var record = _tenantDataContext.KaMsts
                .Where(k => k.KaSname == kaSname).Select(k => new { k.KaId }).FirstOrDefault();
        return record is null ? CommonConstants.InvalidId : record.KaId;
    }
}
