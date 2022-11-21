using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.FlowSheet;
using EmrCloudApi.Tenant.Requests.FlowSheet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlowSheetController
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public FlowSheetController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList + "FlowSheet")]
        public async Task<ActionResult<Response<GetListFlowSheetResponse>>> GetListFlowSheet([FromQuery] GetListFlowSheetRequest inputData)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            var input = new GetListFlowSheetInputData(hpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.IsHolidayOnly, 0, 0, false, inputData.StartIndex, inputData.Count, inputData.Sort);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            var result = new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result);
            watch.Stop();
            Console.WriteLine("TimeLog_Flowsheet" + watch.ElapsedMilliseconds);
            return result;
        }

        [HttpGet(ApiPath.GetList + "Holiday")]
        public async Task<ActionResult<Response<GetListHolidayResponse>>> GetListHoliday([FromQuery] GetListHolidayRequest inputData)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetListFlowSheetInputData(hpId, 0, 0, 0, true, inputData.HolidayFrom, inputData.HolidayTo, false, 0, 0, string.Empty);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new GetListHolidayPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListHolidayResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "RaiinMst")]
        public async Task<ActionResult<Response<GetListRaiinMstResponse>>> GetListRaiinMst([FromQuery] GetListRaiinMstRequest inputData)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetListFlowSheetInputData(hpId, 0, 0, 0, false, 0, 0, true, 0, 0, string.Empty);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new GetListRaiinMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListRaiinMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public async Task<ActionResult<Response<UpsertFlowSheetResponse>>> Upsert([FromBody] UpsertFlowSheetRequest inputData)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new UpsertFlowSheetInputData(inputData.Items.Select(i => new UpsertFlowSheetItemInputData(
                                                        i.RainNo,
                                                        i.PtId,
                                                        i.SinDate,
                                                        i.Value,
                                                        i.Flag
                                                    )).ToList(),
                                                    hpId,
                                                    userId
                                                    );
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new UpsertFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertFlowSheetResponse>>(presenter.Result);
        }
    }
}
