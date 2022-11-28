using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Responses.RaiinKubun
{
    public class LoadDataKubunSettingResponse
    {
        public List<RaiinKubunMstModel> RaiinKubunMstList { get; set; } = new();

        public LoadDataKubunSettingResponse(List<RaiinKubunMstModel> raiinKubunMstList)
        {
            RaiinKubunMstList = raiinKubunMstList;
        }
    }
}
