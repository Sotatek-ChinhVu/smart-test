using Domain.Models.AuditLog;
using Domain.Models.ColumnSetting;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.AuditLog;
using EmrCloudApi.Presenters.ColumnSetting;
using EmrCloudApi.Requests.AuditLog;
using EmrCloudApi.Requests.ColumnSetting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.AuditLog;
using EmrCloudApi.Responses.ColumnSetting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Sync;
using UseCase.SaveAuditLog;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class AuditLogController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public AuditLogController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.Save)]
    public ActionResult<Response<SaveAuditLogResponse>> Save([FromBody] SaveAuditLogRequest req)
    {
        var input = new SaveAuditTrailLogInputData(HpId, UserId, ConvertRequestToModel(req.AuditTrailLogModel));
        var output = _bus.Handle(input);
        var presenter = new SaveAuditLogPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    private AuditTrailLogModel ConvertRequestToModel(SaveAuditLogItem item)
    {
        return new AuditTrailLogModel(item.LogId, DateTime.MinValue, HpId, UserId, item.EventCd, item.PtId, item.SinDate, item.RaiinNo, item.Machine, item.Hosuke);
    }
}
