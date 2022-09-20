using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.LoadData
{
    public class LoadDataKubunSettingOutputData : IOutputData
    {
        public List<RaiinKubunMstModel> RaiinKubunList { get; private set; }
        public LoadDataKubunSettingStatus Status { get; private set; }

        public LoadDataKubunSettingOutputData(List<RaiinKubunMstModel> raiinKubunList, LoadDataKubunSettingStatus status)
        {
            RaiinKubunList = raiinKubunList;
            Status = status;
        }

        public LoadDataKubunSettingOutputData(LoadDataKubunSettingStatus status)
        {
            Status = status;
        }
    }
}
