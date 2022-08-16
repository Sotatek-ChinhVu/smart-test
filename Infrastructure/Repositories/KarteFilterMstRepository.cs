

using Domain.Models.KarteFilterDetail;
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

    public List<KarteFilterMstModel> GetList(int hpId, int userId)
    {
        return _tenantDataContext.KarteFilterMsts
                                 .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                                 .Select(u => new KarteFilterMstModel(
                                        u.HpId,
                                        u.UserId,
                                        u.FilterId,
                                        u.FilterName,
                                        u.SortNo,
                                        u.AutoApply,
                                        u.IsDeleted,
                                        _tenantDataContext.KarteFilterDetails.Where(detail => detail.UserId == u.UserId && detail.HpId == u.HpId && detail.FilterId == u.FilterId)
                                                                             .Select(detail => new KarteFilterDetailModel(
                                                                                 detail.HpId,
                                                                                 detail.UserId,
                                                                                 detail.FilterId,
                                                                                 detail.FilterItemCd,
                                                                                 detail.FilterEdaNo,
                                                                                 detail.Val,
                                                                                 detail.Param ?? String.Empty
                                                                              )).ToList()
                                     )).ToList();
    }
}
