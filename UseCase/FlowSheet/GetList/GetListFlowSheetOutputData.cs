﻿using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetList
{
    public class GetListFlowSheetOutputData : IOutputData
    {
        public List<FlowSheetModel> ListFlowSheetModel { get; private set; }

        public List<RaiinListMstModel> ListRaiinListMstModel { get; private set; }

        public List<HolidayDto> ListHolidayModel { get; private set; }

        public Dictionary<long, List<RaiinListInfModel>> ListRaiinListInfModel { get; private set; }

        public long TotalListFlowSheet { get; private set; }

        public GetListFlowSheetOutputData(List<HolidayDto> listHolidayModel)
        {
            ListFlowSheetModel = new List<FlowSheetModel>();
            ListRaiinListMstModel = new List<RaiinListMstModel>();
            ListRaiinListInfModel = new Dictionary<long, List<RaiinListInfModel>>();
            ListHolidayModel = listHolidayModel;
            TotalListFlowSheet = 0;
        }

        public GetListFlowSheetOutputData(List<FlowSheetModel> listFlowSheetModel, List<RaiinListMstModel> listRaiinListMstModel, Dictionary<long, List<RaiinListInfModel>> listRaiinListInfModel, long totalListFlowSheet = 0)
        {
            ListFlowSheetModel = listFlowSheetModel;
            ListRaiinListMstModel = listRaiinListMstModel;
            ListRaiinListInfModel = listRaiinListInfModel;
            ListHolidayModel = new List<HolidayDto>();
            TotalListFlowSheet = totalListFlowSheet;
        }
    }
}
