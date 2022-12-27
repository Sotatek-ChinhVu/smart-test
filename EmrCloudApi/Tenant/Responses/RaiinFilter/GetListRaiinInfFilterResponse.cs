using Domain.Models.FlowSheet;
using Domain.Models.RaiinFilterMst;

namespace EmrCloudApi.Tenant.Responses.RaiinFilter
{
    public class GetListRaiinInfFilterResponse
    {
        public GetListRaiinInfFilterResponse(List<RaiinFilterMstModel> raiinFilters)
        {
            RaiinInfFilter = raiinFilters;
        }

        public List<RaiinFilterMstModel> RaiinInfFilter { get; private set; } = new List<RaiinFilterMstModel>();
    }
}
