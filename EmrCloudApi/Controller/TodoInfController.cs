using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Presenters.Todo;
using EmrCloudApi.Requests.Todo;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Todo;
using UseCase.Todo.GetTodoInf;
using UseCase.Todo.TodoInf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class TodoInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public TodoInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.UpsertList)]
        public ActionResult<Response<UpsertTodoInfResponse>> Upsert([FromBody] UpsertTodoInfRequest request)
        {
            var input = new UpsertTodoInfInputData(request.UpsertTodoInf.Select(x => new InsertTodoInfDto(
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

            var presenter = new UpsertTodoInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpsertTodoInfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetTodoInfResponse>> GetList([FromQuery] GetTodoInfRequest request)
        {
            var input = new GetTodoInfInputData(HpId, request.TodoNo, request.TodoEdaNo, request.PtId, request.IsDone);
            var output = _bus.Handle(input);

            var presenter = new GetTodoInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTodoInfResponse>>(presenter.Result);
        }
    }
}