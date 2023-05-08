using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.TimeZoneConf;
using UseCase.TimeZoneConf.GetTimeZoneConfGroup;
using EmrCloudApi.Presenters.TimeZoneConf;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneConfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public TimeZoneConfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetTimeZoneConfGroup)]
        public ActionResult<Response<GetTimeZoneConfGroupResponse>> GetListReceSeikyu()
        {
            var input = new GetTimeZoneConfGroupInputData(HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new GetTimeZoneConfGroupPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTimeZoneConfGroupResponse>>(presenter.Result);
        }
    }
}
