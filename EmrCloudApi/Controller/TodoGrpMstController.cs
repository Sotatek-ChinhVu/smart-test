using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.TodoGroupMst;
using EmrCloudApi.Requests.Todo;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Todo;
using UseCase.Todo.TodoGrpMst;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class TodoGrpMstController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public TodoGrpMstController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.UpsertList)]
    public ActionResult<Response<UpsertTodoGrpMstResponse>> Upsert([FromBody] UpsertTodoGrpMstRequest request)
    {
        var input = new UpsertTodoGrpMstInputData(request.TodoGrpMstList.Select(x => new InsertTodoGrpMstDto(
                                                                                x.TodoGrpNo,
                                                                                x.TodoGrpName,
                                                                                x.GrpColor,
                                                                                x.SortNo,
                                                                                x.IsDeleted)).ToList(), UserId, HpId);
        var output = _bus.Handle(input);

        var presenter = new UpsertTodoGrpMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<UpsertTodoGrpMstResponse>>(presenter.Result);
    }
}
