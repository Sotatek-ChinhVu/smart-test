using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ColumnSetting;
using EmrCloudApi.Tenant.Requests.ColumnSetting;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ColumnSetting;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
public class ColumnSettingController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public ColumnSettingController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetColumnSettingListResponse>> GetList([FromQuery] GetColumnSettingListRequest req)
    {
        var input = new GetColumnSettingListInputData(UserId, req.TableName);
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
