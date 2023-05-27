using Domain.Models.RaiinListSetting;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class RaiinListSettingRepository : RepositoryBase, IRaiinListSettingRepository
    {
        public RaiinListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public List<FilingCategoryModel> GetFilingcategoryCollection(int hpId)
        {
            return NoTrackingDataContext.FilingCategoryMst.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None)
                        .OrderBy(f => f.SortNo)
                        .Select(x=> new FilingCategoryModel(x.HpId, x.SortNo, x.CategoryCd, x.CategoryName ?? string.Empty, x.DspKanzok)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
