using Domain.Constant;
using Domain.Models.KaMst;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KaMstRepository : IKaMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public KaMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetDataContext();
    }
    public int GetKaIdByKaSname(string kaSname)
    {
        var record = _tenantDataContext.KaMsts.AsNoTracking()
                .Where(k => k.KaSname == kaSname).Select(k => new { k.KaId }).FirstOrDefault();
        return record is null ? CommonConstants.InvalidId : record.KaId;
    }
}
