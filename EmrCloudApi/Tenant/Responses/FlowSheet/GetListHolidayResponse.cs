using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Tenant.Responses.FlowSheet
{
    public class GetListHolidayResponse
    {
        public List<HolidayModel> ListHolidayModel {get; set; } = new List<HolidayModel>();
    }
}
