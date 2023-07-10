﻿using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public GetListFlowSheetResponse(List<FlowSheetModel> listFlowSheet, List<RaiinListMstModel> listRaiinListMstModels, Dictionary<long, List<RaiinListInfModel>> listRaiinListInfModel, Dictionary<int, List<RaiinListInfModel>> listRaiinListInfForNextOrder, long totalFlowSheet)
        {
            ListFlowSheet = listFlowSheet;
            ListRaiinListMstModels = listRaiinListMstModels;
            ListRaiinListInfModel = listRaiinListInfModel;
            ListRaiinListInfForNextOrder = listRaiinListInfForNextOrder;
            TotalFlowSheet = totalFlowSheet;
        }

        public List<FlowSheetModel> ListFlowSheet { get; private set; }

        public List<RaiinListMstModel> ListRaiinListMstModels { get; private set; }

        public Dictionary<long, List<RaiinListInfModel>> ListRaiinListInfModel { get; private set; }

        public Dictionary<int, List<RaiinListInfModel>> ListRaiinListInfForNextOrder { get; private set; }

        public long TotalFlowSheet { get; private set; }
    }
}
