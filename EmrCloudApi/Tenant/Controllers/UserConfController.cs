using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UserConf;
using EmrCloudApi.Tenant.Requests.UserConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UserConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
public class UserConfController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public UserConfController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetUserConfListResponse>> GetList([FromQuery] GetUserConfListRequest request)
    {
        var input = new GetUserConfListInputData(HpId, UserId);
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
