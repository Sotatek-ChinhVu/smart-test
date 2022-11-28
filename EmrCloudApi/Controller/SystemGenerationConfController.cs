using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemGenerationConf;
using EmrCloudApi.Requests.SystemGenerationConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemGenerationConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemGenerationConf;

namespace EmrCloudApi.Controller
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
