using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.HeaderSumaryInfo;
using EmrCloudApi.Tenant.Requests.HeaderSummaryInfo;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.GetHeaderSummaryInfo;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.HeaderSumaryInfo.Get;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderSummaryInfoController
    {
        private readonly UseCaseBus _bus;
        public HeaderSummaryInfoController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetHeaderSummaryInfoResponse>> GetList([FromQuery] GetHeaderSummaryInfoRequest request)
        {
            var input = new GetHeaderSumaryInfoInputData(request.HpId, request.UserId, request.PtId, request.SinDate, request.RainNo);
            var output = _bus.Handle(input);

            var presenter = new GetHeaderSumaryInfoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHeaderSummaryInfoResponse>>(presenter.Result);
        }
    }
}
