using Domain.Common;

namespace Domain.Models.RaiinListSetting
{
    public interface IRaiinListSettingRepository : IRepositoryBase
    {
        List<FilingCategoryModel> GetFilingcategoryCollection(int hpId);
    }
}
