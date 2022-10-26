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
    [Route("api/[controller]")]
    [ApiController]
    public class FlowSheetController
    {
        private readonly UseCaseBus _bus;
        public FlowSheetController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "FlowSheet")]
        public Task<ActionResult<Response<GetListFlowSheetResponse>>> GetListFlowSheet([FromQuery] GetListFlowSheetRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.IsHolidayOnly, 0, 0, false, inputData.StartIndex, inputData.Count, inputData.Sort);
            var output = _bus.Handle(input);
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result));
        }

        [HttpGet(ApiPath.GetList + "Holiday")]
        public Task<ActionResult<Response<GetListHolidayResponse>>> GetListHoliday([FromQuery] GetListHolidayRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, 0, 0, 0, true, inputData.HolidayFrom, inputData.HolidayTo, false, 0, 0, string.Empty);
            var output = _bus.Handle(input);
            var presenter = new GetListHolidayPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetListHolidayResponse>>(presenter.Result));
        }

        [HttpGet(ApiPath.GetList + "RaiinMst")]
        public Task<ActionResult<Response<GetListRaiinMstResponse>>> GetListRaiinMst([FromQuery] GetListRaiinMstRequest inputData)
        {
            var input = new GetListFlowSheetInputData(inputData.HpId, 0, 0, 0, false, 0, 0, true, 0, 0, string.Empty);
            var output = _bus.Handle(input);
            var presenter = new GetListRaiinMstPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetListRaiinMstResponse>>(presenter.Result));
        }

        [HttpPost(ApiPath.Upsert)]
        public Task<ActionResult<Response<UpsertFlowSheetResponse>>> Upsert([FromBody] UpsertFlowSheetRequest inputData)
        {
            var input = new UpsertFlowSheetInputData(inputData.Items.Select(i => new UpsertFlowSheetItemInputData(
                    i.RainNo,
                    i.PtId,
                    i.SinDate,
                    i.Value,
                    i.Flag
                )).ToList());
            var output = _bus.Handle(input);
            var presenter = new UpsertFlowSheetPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<UpsertFlowSheetResponse>>(presenter.Result));
        }
    }
}
