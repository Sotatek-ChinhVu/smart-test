using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListRaiinMstResponse
    {
        public List<RaiinListMstModel> ListRaiinListMstModels { get; set; } = new List<RaiinListMstModel>();
    }
}
