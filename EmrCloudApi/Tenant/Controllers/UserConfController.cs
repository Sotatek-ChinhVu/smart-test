using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UserConf;
using EmrCloudApi.Tenant.Requests.UserConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UserConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserConfController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public UserConfController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetUserConfListResponse>> GetList([FromQuery] GetUserConfListRequest request)
    {
        int hpId = _userService.GetLoginUser().HpId;
        int userId = _userService.GetLoginUser().UserId;
        var input = new GetUserConfListInputData(hpId, userId);
        var output = _bus.Handle(input);

        var presenter = new GetUserConfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfListResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.UpdateAdoptedByomeiConfig)]
    public async Task<ActionResult<Response<UpdateAdoptedByomeiConfigResponse>>> UpdateAdoptedByomeiConfig([FromBody] UpdateAdoptedByomeiConfigRequest request)
    {
        var hpId = _userService.GetLoginUser().HpId;
        var userId = _userService.GetLoginUser().UserId;
        var input = new UpdateAdoptedByomeiConfigInputData(request.AdoptedValue, hpId, userId);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new UpdateAdoptedByomeiConfigPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateAdoptedByomeiConfigResponse>>(presenter.Result);
    }
}
