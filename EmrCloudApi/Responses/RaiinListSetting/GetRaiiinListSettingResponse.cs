using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class GetRaiiinListSettingResponse
    {
        public GetRaiiinListSettingResponse(List<RaiinListMstModel> data, int grpIdMax, int sortNoMax)
        {
            Data = data;
            GrpIdMax = grpIdMax;
            SortNoMax = sortNoMax;
        }

        public List<RaiinListMstModel> Data { get; private set; }

        public int GrpIdMax { get; private set; }

        public int SortNoMax { get; private set; }
    }
}
