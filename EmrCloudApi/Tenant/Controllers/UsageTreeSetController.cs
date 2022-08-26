using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UsageTreeSet;
using EmrCloudApi.Tenant.Requests.UsageTreeSet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UsageTreeSetResponse;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.UsageTreeSet.GetTree;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsageTreeSetController : ControllerBase
    {
        private readonly UseCaseBus _bus;

        public UsageTreeSetController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetUsageTreeSetListResponse>> GetUsageTree([FromQuery] GetUsageTreeSetListRequest request)
        {
            var input = new GetUsageTreeSetInputData(request.HpId, request.SinDate, request.SetUsageKbn);
            var output = _bus.Handle(input);

            var present = new GetUsageTreeSetListPresenter();
            present.Complete(output);
            return new ActionResult<Response<GetUsageTreeSetListResponse>>(present.Result);
        }
    }
}