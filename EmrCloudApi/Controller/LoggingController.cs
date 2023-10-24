using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Logger;
using EmrCloudApi.Requests.Logging;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Logger;
using EmrCloudApi.Services;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Logger;
using UseCase.Logger.WriteListLog;

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
            //var input = new WriteLogInputData(request.EventCd, request.PtId, request.SinDay, request.RaiinNo, request.Path, request.RequestInfo, request.Description, request.LogType);
            //var output = _bus.Handle(input);

            //var presenter = new WriteLogPresenter();
            //presenter.Complete(output);

            //return new ActionResult<Response<WriteLogResponse>>(presenter.Result);
            return Ok();
        }

        [HttpPost(ApiPath.WriteListLog)]
        public ActionResult<Response<WriteListLogResponse>> WriteListLog([FromBody] WriteListLogRequest request)
        {
            var auditLogModelList = request.WriteListLogRequests.Select(item => new AuditLogModel(
                                                                                item.EventCd,
                                                                                item.PtId,
                                                                                item.SinDay,
                                                                                item.RaiinNo,
                                                                                item.Path,
                                                                                item.RequestInfo,
                                                                                item.Description,
                                                                                item.LogType
                                                                )).ToList();
            var input = new WriteListLogInputData(auditLogModelList);
            var output = _bus.Handle(input);

            var presenter = new WriteListLogPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<WriteListLogResponse>>(presenter.Result);
        }
    }
}
