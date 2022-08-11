using Domain.Models.KarteFilterMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KarteFilterMstRepository : IKarteFilterMstRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public KarteFilterMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<KarteFilterMst> GetList(int hpId, int userId)
    {
        return _tenantDataContext.KarteFilterMsts
                                 .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                                 .Select(u => new KarteFilterMst(
                                        u.HpId,
                                        u.UserId,
                                        u.FilterId,
                                        u.FilterName,
                                        u.SortNo,
                                        u.AutoApply
                                     ))
                                 .ToList();
    }
}
