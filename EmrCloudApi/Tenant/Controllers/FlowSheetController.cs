using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.FlowSheet;
using EmrCloudApi.Tenant.Requests.FlowSheet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;

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
            var input = new GetListFlowSheetInputData(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, false, 0, 0, false);
            var output = _bus.Handle(input);
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "Holiday")]
        public ActionResult<Response<GetListHolidayResponse>> GetListHoliday([FromQuery] GetListHolidayRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, 0, 0, 0, true, inputData.HolidayFrom, inputData.HolidayTo, false);
            var output = _bus.Handle(input);
            var presenter = new GetListHolidayPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListHolidayResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "RaiinMst")]
        public ActionResult<Response<GetListRaiinMstResponse>> GetListRaiinMst([FromQuery] GetListRaiinMstRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, 0, 0, 0, false, 0, 0, true);
            var output = _bus.Handle(input);
            var presenter = new GetListRaiinMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListRaiinMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertFlowSheetResponse>> Upsert([FromBody] UpsertFlowSheetRequest inputData)
        {
            var input = new UpsertFlowSheetInputData(inputData.RainNo, inputData.PtId, inputData.SinDate, inputData.TagNo, inputData.CmtKbn, inputData.Text, inputData.SeqNo);
            var output = _bus.Handle(input);
            var presenter = new UpsertFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertFlowSheetResponse>>(presenter.Result);
        }
    }
}
