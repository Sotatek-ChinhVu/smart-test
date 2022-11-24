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
    public class SystemGenerationConfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemGenerationConfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetSettingValue)]
        public ActionResult<Response<GetSystemGenerationConfResponse>> GetSettingValue([FromQuery] GetSystemGenerationConfRequest request)
        {
            var input = new GetSystemGenerationConfInputData(HpId, request.GrpCd, request.GrpEdaNo, request.PresentDate, request.DefaultValue, request.DefaultParam);
            var output = _bus.Handle(input);

            var presenter = new GetSystemGenerationConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemGenerationConfResponse>>(presenter.Result);
        }
    }
}
