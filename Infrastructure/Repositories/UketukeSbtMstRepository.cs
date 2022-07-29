using Domain.Models.UketukeSbtMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : IUketukeSbtMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UketukeSbtMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public int GetKbnIdByKbnName(string kbnName)
    {
        var record = _tenantDataContext.UketukeSbtMsts
                .Where(u => u.KbnName == kbnName).Select(u => new { u.KbnId }).FirstOrDefault();
        return record is null ? CommonConstants.InvalidId : record.KbnId;
    }
}
