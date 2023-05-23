using Domain.Common;
using Domain.Models.Document;

namespace Domain.Models.RaiinListSetting
{
    public interface IRaiinListSettingRepository : IRepositoryBase
    {
        List<DocCategoryModel> GetDocCategoryCollection(int hpId);
    }
}
