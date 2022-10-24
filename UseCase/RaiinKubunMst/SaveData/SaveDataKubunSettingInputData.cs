using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.Save
{
    public class SaveDataKubunSettingInputData : IInputData<SaveDataKubunSettingOutputData>
    {
        public SaveDataKubunSettingInputData(List<RaiinKubunMstModel> raiinKubunMstModels)
        {
            RaiinKubunMstModels = raiinKubunMstModels;
        }

        public List<RaiinKubunMstModel> RaiinKubunMstModels { get; private set; }
    }
}
