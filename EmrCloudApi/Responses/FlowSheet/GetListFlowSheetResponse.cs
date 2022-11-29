using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public GetListFlowSheetResponse(List<FlowSheetModel> listFlowSheet, List<RaiinListMstModel> listRaiinListMstModels, long totalFlowSheet)
        {
            ListFlowSheet = listFlowSheet;
            ListRaiinListMstModels = listRaiinListMstModels;
            TotalFlowSheet = totalFlowSheet;
        }

        public List<FlowSheetModel> ListFlowSheet { get; private set; }

        public List<RaiinListMstModel> ListRaiinListMstModels { get; private set; }
        public long TotalFlowSheet { get; private set; }
    }
}
