using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemGenerationConf;
using EmrCloudApi.Tenant.Requests.SystemGenerationConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemGenerationConf;
using EmrCloudApi.Tenant.Services;
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
        private readonly IUserService _userService;
        public SystemGenerationConfController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetSettingValue)]
        public async Task<ActionResult<Response<GetSystemGenerationConfResponse>>> GetSettingValue([FromQuery] GetSystemGenerationConfRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetSystemGenerationConfResponse>>(new Response<GetSystemGenerationConfResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetSystemGenerationConfInputData(hpId, request.GrpCd, request.GrpEdaNo, request.PresentDate, request.DefaultValue, request.DefaultParam);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSystemGenerationConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemGenerationConfResponse>>(presenter.Result);
        }
    }
}
