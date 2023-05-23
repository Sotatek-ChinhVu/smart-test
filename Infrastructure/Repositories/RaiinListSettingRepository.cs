using Domain.Models.Document;
using Domain.Models.RaiinListSetting;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class RaiinListSettingRepository : RepositoryBase, IRaiinListSettingRepository
    {
        public RaiinListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public List<DocCategoryModel> GetDocCategoryCollection(int hpId)
        {
            return NoTrackingDataContext.DocCategoryMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None)
                            .OrderBy(d => d.SortNo)
                            .Select(x => new DocCategoryModel(x.CategoryCd, x.CategoryName ?? string.Empty, x.SortNo)).ToList();;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
