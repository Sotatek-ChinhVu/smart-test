using Domain.Models.RaiinListMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetRaiiinListSetting
{
    public class GetRaiiinListSettingOutputData : IOutputData
    {
        public GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus status, List<RaiinListMstModel> data, int grpIdMax, int sortNoMax, int sortNoDetailMax, int kbnCdMax)
        {
            Status = status;
            Data = data;
            GrpIdMax = grpIdMax;
            SortNoMax = sortNoMax;
            SortNoDetailMax = sortNoDetailMax;
            KbnCdMax = kbnCdMax;
        }

        public GetRaiiinListSettingStatus Status { get; private set; }

        public List<RaiinListMstModel> Data { get; private set; }

        public int GrpIdMax { get; private set; }

        public int SortNoMax { get; private set; }

        public int KbnCdMax { get; private set; }

        public int SortNoDetailMax { get; private set; }
    }
}
