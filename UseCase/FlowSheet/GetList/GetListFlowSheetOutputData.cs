using Domain.Models.FlowSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetList
{
    public class GetListFlowSheetOutputData :IOutputData
    {
        public List<FlowSheetModel> ListFlowSheetModel { get; private set; }
        public List<RaiinListMstModel> ListRaiinListMstModels { get; private set; }
        public List<CalendarGridModel> ListCalendarGridModel { get; private set; }
        public GetListFlowSheetOutputData(List<FlowSheetModel> listFlowSheetModel, List<RaiinListMstModel> raiinListMstModels, List<CalendarGridModel> listCalendarGridModel)
        {
            ListFlowSheetModel = listFlowSheetModel;
            ListRaiinListMstModels = raiinListMstModels;
            ListCalendarGridModel = listCalendarGridModel;
        }
    }
}
