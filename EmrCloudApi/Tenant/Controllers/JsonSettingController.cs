using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ColumnSetting;
using EmrCloudApi.Tenant.Requests.JsonSetting;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.JsonSetting;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JsonSettingController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public JsonSettingController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.Get)]
    public ActionResult<Response<GetJsonSettingResponse>> Get([FromQuery] GetJsonSettingRequest req)
    {
        int userId = _userService.GetLoginUser().UserId;
        var input = new GetJsonSettingInputData(userId, req.Key);
        var output = _bus.Handle(input);
        var presenter = new GetJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertJsonSettingResponse>> Upsert([FromBody] UpsertJsonSettingRequest req)
    {
        var input = new UpsertJsonSettingInputData(req.Setting);
        var output = _bus.Handle(input);
        var presenter = new UpsertJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
