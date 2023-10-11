using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Logger;
using EmrCloudApi.Requests.Logging;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Logger;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Logger;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public LoggingController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.WriteLog)]
        public ActionResult<Response<WriteLogResponse>> WriteLog([FromBody] WriteLogRequest request)
        {
            var input = new WriteLogInputData(request.EventCd, request.PtId, request.SinDay, request.RaiinNo, request.Path, request.RequestInfo, request.Description, request.LogType, request.LoginId);
            var output = _bus.Handle(input);

            var presenter = new WriteLogPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<WriteLogResponse>>(presenter.Result);
        }
    }
}
