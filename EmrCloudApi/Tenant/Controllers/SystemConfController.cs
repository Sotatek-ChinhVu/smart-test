using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SytemConf;
using EmrCloudApi.Tenant.Requests.SystemConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;

namespace EmrCloudApi.Tenant.Controllers
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
