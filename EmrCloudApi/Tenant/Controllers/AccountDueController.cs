using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.AccountDue;
using EmrCloudApi.Tenant.Requests.AccountDue;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountDueController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;
    public AccountDueController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetAccountDueListResponse>> GetList([FromQuery] GetAccountDueListRequest request)
    {
        var userId = _userService.GetUserId();
        var input = new GetAccountDueListInputData(request.HpId, request.PtId, request.SinDate, request.IsUnpaidChecked, request.PageIndex, request.PageSize);
        var output = _bus.Handle(input);

        var presenter = new GetAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAccountDueListResponse>>(presenter.Result);
    }
}
