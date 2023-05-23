using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetListHolidayResponse
    {
        public List<HolidayDto> ListHolidayModel {get; set; } = new();
    }
}
