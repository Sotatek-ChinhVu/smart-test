using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class GetRaiiinListSettingResponse
    {
        public GetRaiiinListSettingResponse(List<RaiinListMstModel> data, int grpIdMax, int sortNoMax, int sortNoDetailMax, int kbnCdMax)
        {
            Data = data;
            GrpIdMax = grpIdMax;
            SortNoMax = sortNoMax;
            SortNoDetailMax = sortNoDetailMax;
            KbnCdMax = kbnCdMax;
        }

        public List<RaiinListMstModel> Data { get; private set; }

        public int GrpIdMax { get; private set; }

        public int SortNoMax { get; private set; }

        public int SortNoDetailMax { get; private set; }

        public int KbnCdMax { get; private set; }
    }
}
