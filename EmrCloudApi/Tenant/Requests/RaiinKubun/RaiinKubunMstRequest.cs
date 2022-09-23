using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Tenant.Requests.RaiinKubun
{
    public class RaiinKubunMstRequest
    {
        public int HpId { get; set; }
        public int GroupId { get; set; }

        public int SortNo { get; set; }

        public string GroupName { get; set; } = String.Empty;

        public bool IsDeleted { get; set; }

        public List<RaiinKubunDetailRequest> RaiinKubunDetailModels { get; set; } = new List<RaiinKubunDetailRequest>();
        public RaiinKubunMstModel Map()
        {
            return new RaiinKubunMstModel(HpId,GroupId,SortNo,GroupName,IsDeleted,RaiinKubunDetailModels.Select(x => x.Map()).ToList());
        }

    }
}
