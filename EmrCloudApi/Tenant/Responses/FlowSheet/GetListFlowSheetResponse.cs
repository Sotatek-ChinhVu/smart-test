using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public List<FlowSheetModel> ListFlowSheet { get; set; } = new List<FlowSheetModel>();

        public List<RaiinListMstModel> ListRaiinListMstModels { get; set; } = new List<RaiinListMstModel>();
        public long TotalFlowSheet { get; set; } = 0;
    }
}
