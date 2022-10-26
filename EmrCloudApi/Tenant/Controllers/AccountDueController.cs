using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.AccountDue;
using EmrCloudApi.Tenant.Requests.AccountDue;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using Microsoft.AspNetCore.Mvc;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountDueController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public AccountDueController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetAccountDueListResponse>> GetList([FromQuery] GetAccountDueListRequest request)
    {
        var input = new GetAccountDueListInputData(request.HpId, request.PtId, request.SinDate, request.IsUnpaidChecked, request.PageIndex, request.PageSize);
        var output = _bus.Handle(input);

        var presenter = new GetAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAccountDueListResponse>>(presenter.Result);
    }
}
