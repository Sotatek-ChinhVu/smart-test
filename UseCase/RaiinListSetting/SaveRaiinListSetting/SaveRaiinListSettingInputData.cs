using Domain.Models.RaiinListMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.SaveRaiinListSetting
{
    public class SaveRaiinListSettingInputData : IInputData<SaveRaiinListSettingOutputData>
    {
        public SaveRaiinListSettingInputData(int hpId, List<RaiinListMstModel> raiinListSettings, int userId)
        {
            HpId = hpId;
            RaiinListSettings = raiinListSettings;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public List<RaiinListMstModel> RaiinListSettings { get; private set; }

        public int UserId { get; private set; }
    }
}
