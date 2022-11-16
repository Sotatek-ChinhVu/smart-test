using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.Save
{
    public class SaveDataKubunSettingInputData : IInputData<SaveDataKubunSettingOutputData>
    {
        public SaveDataKubunSettingInputData(List<RaiinKubunMstModel> raiinKubunMstModels, int userId)
        {
            RaiinKubunMstModels = raiinKubunMstModels;
            UserId = userId;
        }

        public List<RaiinKubunMstModel> RaiinKubunMstModels { get; private set; }

        public int UserId { get; private set; }
    }
}
