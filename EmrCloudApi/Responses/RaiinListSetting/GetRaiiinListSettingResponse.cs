using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class GetRaiiinListSettingResponse
    {
        public GetRaiiinListSettingResponse(List<RaiinListMstModel> data)
        {
            Data = data;
        }

        public List<RaiinListMstModel> Data { get; private set; }
    }
}
