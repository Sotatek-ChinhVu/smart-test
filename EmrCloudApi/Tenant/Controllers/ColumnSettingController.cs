using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ColumnSetting;
using EmrCloudApi.Tenant.Requests.ColumnSetting;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ColumnSetting;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ColumnSettingController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public ColumnSettingController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetColumnSettingListResponse>> GetList([FromQuery] GetColumnSettingListRequest req)
    {
        int userId = _userService.GetLoginUser().UserId;
        var input = new GetColumnSettingListInputData(userId, req.TableName);
        var output = _bus.Handle(input);
        var presenter = new GetColumnSettingListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveColumnSettingListResponse>> SaveList([FromBody] SaveColumnSettingListRequest req)
    {
        var input = new SaveColumnSettingListInputData(req.Settings);
        var output = _bus.Handle(input);
        var presenter = new SaveColumnSettingListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
