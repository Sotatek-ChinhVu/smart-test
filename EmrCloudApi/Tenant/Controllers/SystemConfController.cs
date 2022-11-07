using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemConf;
using EmrCloudApi.Tenant.Requests.SystemConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemConfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public SystemConfController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.Get)]
        public async Task<ActionResult<Response<GetSystemConfResponse>>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetSystemConfResponse>>(new Response<GetSystemConfResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetSystemConfInputData(hpId, request.GrpCd, request.GrpEdaNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfResponse>>(presenter.Result);
        }
    }
}
