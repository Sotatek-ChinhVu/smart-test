using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemConf;
using EmrCloudApi.Requests.SystemConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SystemConfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemConfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetSystemConfResponse>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            var input = new GetSystemConfInputData(HpId, request.GrpCd, request.GrpEdaNo);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfResponse>>(presenter.Result);
        }
    }
}
