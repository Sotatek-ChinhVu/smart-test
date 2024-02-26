using Domain.Models.ColumnSetting;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ColumnSetting;
using EmrCloudApi.Requests.ColumnSetting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ColumnSetting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ColumnSetting.GetColumnSettingByTableNameList;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

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
        var input = new GetColumnSettingListInputData(HpId, UserId, req.TableName);
        var output = _bus.Handle(input);
        var presenter = new GetColumnSettingListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.GetColumnSettingByTableNameList)]
    public ActionResult<Response<GetColumnSettingByTableNameListResponse>> GetColumnSettingByTableNameList([FromBody] GetColumnSettingByTableNameListRequest request)
    {
        var input = new GetColumnSettingByTableNameListInputData(HpId, UserId, request.TableNameList);
        var output = _bus.Handle(input);
        var presenter = new GetColumnSettingByTableNameListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveColumnSettingListResponse>> SaveList([FromBody] SaveColumnSettingListRequest req)
    {
        var input = new SaveColumnSettingListInputData(ConvertToColumnSettingModel(HpId, UserId, req));
        var output = _bus.Handle(input);
        var presenter = new SaveColumnSettingListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    private List<ColumnSettingModel> ConvertToColumnSettingModel(int hpId, int userId, SaveColumnSettingListRequest request)
    {
        return request.Settings.Select(item => new ColumnSettingModel(
                                                   hpId,
                                                   userId,
                                                   item.TableName,
                                                   item.ColumnName,
                                                   item.DisplayOrder,
                                                   item.IsPinned,
                                                   item.IsHidden,
                                                   item.Width,
                                                   item.OrderBy))
                               .ToList();
    }
}
