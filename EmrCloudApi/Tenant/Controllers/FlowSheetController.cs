using EmrCloudApi.Tenant.Presenters.FlowSheet;
using EmrCloudApi.Tenant.Requests.FlowSheet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    public class FlowSheetController
    {
        private readonly UseCaseBus _bus;
        public FlowSheetController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetFlowSheet")]
        public ActionResult<Response<GetListFlowSheetResponse>> GetList([FromQuery] GetListFlowSheetRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.IsHolidayOnly, inputData.HolidayFrom, inputData.HolidayTo);
            var output = _bus.Handle(input);
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result);
        }
    }
}
