using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public GetListFlowSheetResponse(List<FlowSheetModel> listFlowSheet, List<RaiinListMstModel> listRaiinListMstModels, long totalFlowSheet)
        {
            ListFlowSheet = listFlowSheet;
            ListRaiinListMstModels = listRaiinListMstModels;
            TotalFlowSheet = totalFlowSheet;
        }

        public List<FlowSheetModel> ListFlowSheet { get; private set; } = new List<FlowSheetModel>();

        public List<RaiinListMstModel> ListRaiinListMstModels { get; private set; } = new List<RaiinListMstModel>();
        public long TotalFlowSheet { get; private set; } = 0;
    }
}
