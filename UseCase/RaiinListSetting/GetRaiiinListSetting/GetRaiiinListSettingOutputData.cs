using Domain.Models.RaiinListMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetRaiiinListSetting
{
    public class GetRaiiinListSettingOutputData : IOutputData
    {
        public GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus status, List<RaiinListMstModel> data)
        {
            Status = status;
            Data = data;
        }

        public GetRaiiinListSettingStatus Status { get; private set; }

        public List<RaiinListMstModel> Data { get; private set; }
    }
}
