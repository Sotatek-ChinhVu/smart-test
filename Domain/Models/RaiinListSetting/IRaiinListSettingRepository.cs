using Domain.Common;
using Domain.Models.RaiinListMst;

namespace Domain.Models.RaiinListSetting
{
    public interface IRaiinListSettingRepository : IRepositoryBase
    {
        List<FilingCategoryModel> GetFilingcategoryCollection(int hpId);

        (List<RaiinListMstModel> raiinListMsts, int grpIdMax, int sortNoMax) GetRaiiinListSetting(int hpId);

        bool SaveRaiinListSetting(int hpId, List<RaiinListMstModel> raiinListMstModels, int userId);
    }
}
