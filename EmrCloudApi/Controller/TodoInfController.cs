using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.Todo;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Todo;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Todo;
using UseCase.Todo.GetTodoInfFinder;
using UseCase.Todo.UpsertTodoInf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class TodoInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;

        public TodoInfController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpPost(ApiPath.UpsertList)]
        public async Task<ActionResult<Response<UpsertTodoInfResponse>>> Upsert([FromBody] UpsertTodoInfRequest request)
        {
            var input = new UpsertTodoInfInputData(request.UpsertTodoInf.Select(x => new TodoInfDto(
                                                        x.TodoNo,
                                                        x.TodoEdaNo,
                                                        x.PtId,
                                                        x.SinDate,
                                                        x.RaiinNo,
                                                        x.TodoKbnNo,
                                                        x.TodoGrpNo,
                                                        x.Tanto,
                                                        x.Term,
                                                        x.Cmt1,
                                                        x.Cmt2,
                                                        x.IsDone,
                                                        x.IsDeleted)).ToList(),
                                                        UserId,
                                                        HpId
                                                        );

            var output = _bus.Handle(input);

            if (output.Status == UpsertTodoInfStatus.Success)
            {
                var first = request.UpsertTodoInf.FirstOrDefault();
                int firstType = first?.TodoNo > 0 ? 1 : 2;
                var type = (first?.TodoNo > 0 && first?.IsDeleted == 1) ? 0 : firstType;
                var functionCode = type == 1 ? FunctionCodes.TodoInfUpdated : FunctionCodes.TodoInfInserted;
                await _webSocketService.SendMessageAsync(type == 0 ? FunctionCodes.TodoInfIsDeleted : functionCode, new TodoInfMessage(output.OutputItems));
            }

            var presenter = new UpsertTodoInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpsertTodoInfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetTodoInfFinderResponse>> GetList([FromQuery] GetTodoInfFinderRequest request)
        {
            var input = new GetTodoInfFinderInputData(HpId, request.TodoNo, request.TodoEdaNo, request.IncDone);
            var output = _bus.Handle(input);

            var presenter = new GetTodoInfFinderPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTodoInfFinderResponse>>(presenter.Result);
        }
    }
}