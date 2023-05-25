using Domain.Common;
using Domain.Models.RaiinListMst;

namespace Domain.Models.RaiinListSetting
{
    public interface IRaiinListSettingRepository : IRepositoryBase
    {
        List<RaiinListMstModel> GetRaiiinListSetting(int hpId);
    }
}
