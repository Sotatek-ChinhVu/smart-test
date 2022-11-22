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
        public ActionResult<Response<GetSystemConfResponse>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetSystemConfInputData(hpId, request.GrpCd, request.GrpEdaNo);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfResponse>>(presenter.Result);
        }
    }
}
