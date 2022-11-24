using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.FlowSheet;
using EmrCloudApi.Tenant.Requests.FlowSheet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class FlowSheetController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public FlowSheetController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "FlowSheet")]
        public ActionResult<Response<GetListFlowSheetResponse>> GetListFlowSheet([FromQuery] GetListFlowSheetRequest inputData)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            var input = new GetListFlowSheetInputData(HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.IsHolidayOnly, 0, 0, false, inputData.StartIndex, inputData.Count, inputData.Sort);
            var output = _bus.Handle(input);
            var presenter = new GetListFlowSheetPresenter();
            presenter.Complete(output);

            var result = new ActionResult<Response<GetListFlowSheetResponse>>(presenter.Result);
            watch.Stop();
            Console.WriteLine("TimeLog_Flowsheet" + watch.ElapsedMilliseconds);
            return result;
        }

        [HttpGet(ApiPath.GetList + "Holiday")]
        public ActionResult<Response<GetListHolidayResponse>> GetListHoliday([FromQuery] GetListHolidayRequest inputData)
        {

            var input = new GetListFlowSheetInputData(HpId, 0, 0, 0, true, inputData.HolidayFrom, inputData.HolidayTo, false, 0, 0, string.Empty);
            var output = _bus.Handle(input);
            var presenter = new GetListHolidayPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListHolidayResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "RaiinMst")]
        public ActionResult<Response<GetListRaiinMstResponse>> GetListRaiinMst([FromQuery] GetListRaiinMstRequest inputData)
        {

            var input = new GetListFlowSheetInputData(HpId, 0, 0, 0, false, 0, 0, true, 0, 0, string.Empty);
            var output = _bus.Handle(input);
            var presenter = new GetListRaiinMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListRaiinMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertFlowSheetResponse>> Upsert([FromBody] UpsertFlowSheetRequest inputData)
        {
            var input = new UpsertFlowSheetInputData(inputData.Items.Select(i => new UpsertFlowSheetItemInputData(
                                                        i.RainNo,
                                                        i.PtId,
                                                        i.SinDate,
                                                        i.Value,
                                                        i.Flag
                                                    )).ToList(),
                                                    HpId,
                                                    UserId
                                                    );
            var output = _bus.Handle(input);
            var presenter = new UpsertFlowSheetPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertFlowSheetResponse>>(presenter.Result);
        }
    }
}
