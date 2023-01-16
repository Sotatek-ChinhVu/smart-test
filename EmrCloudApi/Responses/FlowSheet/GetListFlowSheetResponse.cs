using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public GetListFlowSheetResponse(List<FlowSheetModel> listFlowSheet, List<RaiinListMstModel> listRaiinListMstModels, List<RaiinListInfModel> listRaiinListInfModel, long totalFlowSheet)
        {
            ListFlowSheet = listFlowSheet;
            ListRaiinListMstModels = listRaiinListMstModels;
            ListRaiinListInfModel = listRaiinListInfModel;
            TotalFlowSheet = totalFlowSheet;
        }

        public List<FlowSheetModel> ListFlowSheet { get; private set; }

        public List<RaiinListMstModel> ListRaiinListMstModels { get; private set; }

        public List<RaiinListInfModel> ListRaiinListInfModel { get; private set; }

        public long TotalFlowSheet { get; private set; }
    }
}
