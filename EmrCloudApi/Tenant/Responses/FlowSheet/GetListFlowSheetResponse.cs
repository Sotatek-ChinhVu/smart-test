using Domain.Models.FlowSheet;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListFlowSheetResponse
    {
        public List<FlowSheetModel> ListFlowSheet { get; set; } = new List<FlowSheetModel>();
        public List<RaiinListMstModel> ListRaiinListMstModels { get; set; } = new List<RaiinListMstModel>();
        public List<CalendarGridModel> ListCalendarGridModel {get; set; } = new List<CalendarGridModel>();
    }
}
