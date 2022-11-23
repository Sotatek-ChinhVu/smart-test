using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemGenerationConf;
using EmrCloudApi.Tenant.Requests.SystemGenerationConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemGenerationConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemGenerationConf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemGenerationConfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public SystemGenerationConfController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetSettingValue)]
        public ActionResult<Response<GetSystemGenerationConfResponse>> GetSettingValue([FromQuery] GetSystemGenerationConfRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetSystemGenerationConfInputData(hpId, request.GrpCd, request.GrpEdaNo, request.PresentDate, request.DefaultValue, request.DefaultParam);
            var output = _bus.Handle(input);

            var presenter = new GetSystemGenerationConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemGenerationConfResponse>>(presenter.Result);
        }
    }
}
