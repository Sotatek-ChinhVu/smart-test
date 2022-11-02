using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemGenerationConf;
using EmrCloudApi.Tenant.Requests.SystemGenerationConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemGenerationConf;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemGenerationConf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemGenerationConfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemGenerationConfController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetSettingValue)]
        public Task<ActionResult<Response<GetSystemGenerationConfResponse>>> GetSettingValue([FromQuery] GetSystemGenerationConfRequest request)
        {
            var input = new GetSystemGenerationConfInputData(request.HpId, request.GrpCd, request.GrpEdaNo, request.PresentDate, request.DefaultValue, request.DefaultParam);
            var output = _bus.Handle(input);

            var presenter = new GetSystemGenerationConfPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetSystemGenerationConfResponse>>(presenter.Result));
        }
    }
}
