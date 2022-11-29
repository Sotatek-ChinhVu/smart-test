using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Responses.RaiinKubun
{
    public class GetRaiinKubunMstListResponse
    {
        public List<RaiinKubunMstModel> RaiinKubunMstList { get; set; } = new();
    }
}
