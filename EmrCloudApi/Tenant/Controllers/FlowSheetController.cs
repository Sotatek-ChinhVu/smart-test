using EmrCloudApi.Tenant.Constants;
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

        [HttpGet(ApiPath.GetList + "FlowSheet")]
        public ActionResult<Response<GetListFlowSheetResponse>> GetListFlowSheet([FromQuery] GetListFlowSheetRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, false, 0, 0);
            var output = _bus.Handle(input);
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "Holiday")]
        public ActionResult<Response<GetListHolidayResponse>> GetListHoliday([FromQuery] GetListHolidayRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, 0, 0, 0, true, inputData.HolidayFrom, inputData.HolidayTo);
            var output = _bus.Handle(input);
            var presenter = new GetListHolidayPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListHolidayResponse>>(presenter.Result);
        }
    }
}
