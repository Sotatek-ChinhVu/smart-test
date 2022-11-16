using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.LoadData
{
    public class LoadDataKubunSettingInputData : IInputData<LoadDataKubunSettingOutputData>
    {
        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public LoadDataKubunSettingInputData(int hpId, int userId)
        {
            HpId = hpId;
            UserId = userId;
        }
    }
}
