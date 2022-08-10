using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
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

        public List<HolidayModel> ListHolidayModel { get; private set; }

        public GetListFlowSheetOutputData(List<FlowSheetModel> listFlowSheetModel, List<RaiinListMstModel> listRaiinListMstModel, List<HolidayModel> listHolidayModel)
        {
            ListFlowSheetModel = listFlowSheetModel;
            ListRaiinListMstModels = listRaiinListMstModel;
            ListHolidayModel = listHolidayModel;
        }
    }
}
