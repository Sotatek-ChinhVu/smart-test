using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.Save
{
    public class SaveDataKubunSettingInputData : IInputData<SaveDataKubunSettingOutputData>
    {
        public SaveDataKubunSettingInputData(List<RaiinKubunMstModel> raiinKubunMstModels, int userId, int hpId)
        {
            RaiinKubunMstModels = raiinKubunMstModels;
            UserId = userId;
            HpId = hpId;
        }

        public List<RaiinKubunMstModel> RaiinKubunMstModels { get; private set; }

        public int UserId { get; private set; }

        public int HpId { get; private set; }
    }
}
