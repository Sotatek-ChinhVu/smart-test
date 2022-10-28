using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UserConf;
using EmrCloudApi.Tenant.Requests.UserConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UserConf;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserConfController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public UserConfController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public Task<ActionResult<Response<GetUserConfListResponse>>> GetList([FromQuery] GetUserConfListRequest request)
    {
        var input = new GetUserConfListInputData(request.HpId, request.UserId);
        var output = _bus.Handle(input);

        var presenter = new GetUserConfListPresenter();
        presenter.Complete(output);

        return Task.FromResult(new ActionResult<Response<GetUserConfListResponse>>(presenter.Result));
    }
}
