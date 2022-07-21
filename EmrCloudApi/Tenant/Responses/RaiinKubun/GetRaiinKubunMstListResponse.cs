using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Tenant.Responses.RaiinKubun
{
    public class GetRaiinKubunMstListResponse
    {
        public List<RaiinKubunMstModel> RaiinKubunMstList { get; set; } = new();
    }
}
