using Domain.Models.KarteFilterMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        var result = _tenantDataContext.KarteFilterMsts
                          .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                          .AsEnumerable()
                          .Select(u => new KarteFilterMstModel(
                                 u.HpId,
                                 u.UserId,
                                 u.FilterId,
                                 u.FilterName,
                                 u.SortNo,
                                 u.AutoApply,
                                 u.IsDeleted,
                                 GetKarteFilterDetailModel(u.HpId, u.UserId, u.FilterId)
                              )).ToList();
        return result;
    }

    private KarteFilterDetailModel GetKarteFilterDetailModel(int hpId, int userId, long filterId)
    {
        var listKarteFilterDetails = _tenantDataContext.KarteFilterDetails.Where(item => item.HpId == hpId && item.UserId == userId && item.FilterId == filterId).ToList();
        var isBookMarkChecked = listKarteFilterDetails.FirstOrDefault(detail => detail.FilterItemCd == 1) != null ? true : false;
        var listHokenId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 3).Select(item => item.FilterEdaNo).ToList();
        var listKaId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 4).Select(item => item.FilterEdaNo).ToList();
        var listUserId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 2).Select(item => item.FilterEdaNo).ToList();
        return new KarteFilterDetailModel(hpId, userId, filterId, isBookMarkChecked, listHokenId, listKaId, listUserId);
    }
}
