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
        var karteMstList = _tenantDataContext.KarteFilterMsts
                          .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                          .ToList();

        var filterMstIdList = karteMstList.Select(k => k.FilterId).ToList();

        var filterDetailList = _tenantDataContext.KarteFilterDetails
            .Where(item => item.HpId == hpId && item.UserId == userId && filterMstIdList.Contains(item.FilterId))
            .ToList();

        List<KarteFilterMstModel> result = new();
        foreach (var karteMst in karteMstList)
        {
            var filterId = karteMst.FilterId;
            var isBookMarkChecked = filterDetailList.FirstOrDefault(detail => detail.FilterId == filterId && detail.FilterItemCd == 1) != null;
            var listHokenId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 3).Select(item => item.FilterEdaNo).ToList();
            var listKaId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 4).Select(item => item.FilterEdaNo).ToList();
            var listUserId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 2).Select(item => item.FilterEdaNo).ToList();

            var detailModel = new KarteFilterDetailModel(hpId, userId, filterId, isBookMarkChecked, listHokenId, listKaId, listUserId);

            result.Add(new KarteFilterMstModel(
                karteMst.HpId,
                karteMst.UserId,
                karteMst.FilterId,
                karteMst.FilterName,
                karteMst.SortNo,
                karteMst.AutoApply,
                karteMst.IsDeleted,
                detailModel));
        }
        return result;
    }
}
