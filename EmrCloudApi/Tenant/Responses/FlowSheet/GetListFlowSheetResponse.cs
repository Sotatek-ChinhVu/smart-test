using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public List<FlowSheetModel> ListFlowSheet { get; set; } = new List<FlowSheetModel>();
    }
}
