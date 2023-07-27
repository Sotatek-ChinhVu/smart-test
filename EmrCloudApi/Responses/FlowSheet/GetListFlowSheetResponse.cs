using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using EmrCloudApi.Responses.FlowSheet.GetListFlowSheetDto;
using System.Diagnostics;
using System.Linq;

namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public GetListFlowSheetResponse(List<FlowSheetModel> listFlowSheet, List<RaiinListMstModel> listRaiinListMstModels, Dictionary<long, List<RaiinListInfModel>> listRaiinListInfModel, Dictionary<int, List<RaiinListInfModel>> listRaiinListInfForNextOrder, long totalFlowSheet)
        {
            ListFlowSheet = listFlowSheet.Select(f => new FlowSheetDto(f)).ToList();
            ListRaiinListMstModels = listRaiinListMstModels.Select(f => new RaiinListMstDto(f)).ToList();
            ListRaiinListInfModel = listRaiinListInfModel.ToDictionary(f => f.Key, f => f.Value.Select(f => new RaiinListInfDto(f)).ToList());
            ListRaiinListInfForNextOrder = listRaiinListInfForNextOrder.ToDictionary(f => f.Key, f => f.Value.Select(f => new RaiinListInfDto(f)).ToList());
            TotalFlowSheet = totalFlowSheet;
        }

        public List<FlowSheetDto> ListFlowSheet { get; private set; }

        public List<RaiinListMstDto> ListRaiinListMstModels { get; private set; }

        public Dictionary<long, List<RaiinListInfDto>> ListRaiinListInfModel { get; private set; }

        public Dictionary<int, List<RaiinListInfDto>> ListRaiinListInfForNextOrder { get; private set; }

        public long TotalFlowSheet { get; private set; }
    }
}
