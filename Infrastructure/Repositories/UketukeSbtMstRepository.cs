using Domain.Constant;
using Domain.Models.UketukeSbtMst;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : IUketukeSbtMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UketukeSbtMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetDataContext();
    }

    public int GetKbnIdByKbnName(string kbnName)
    {
        var record = _tenantDataContext.UketukeSbtMsts.AsNoTracking()
                .Where(u => u.KbnName == kbnName).Select(u => new { u.KbnId }).FirstOrDefault();
        return record is null ? CommonConstants.InvalidId : record.KbnId;
    }
}
