using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemConf;
using EmrCloudApi.Tenant.Requests.SystemConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemConf;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemConfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemConfController(UseCaseBus bus)
        {
            _bus = bus;
        }
        [HttpGet(ApiPath.Get)]
        public Task<ActionResult<Response<GetSystemConfResponse>>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            var input = new GetSystemConfInputData(request.HpId, request.GrpCd, request.GrpEdaNo);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetSystemConfResponse>>(presenter.Result));
        }
    }
}
