using Domain.SuperAdminModels.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.AuditLog;
using SuperAdminAPI.Reponse.AuditLog;
using SuperAdminAPI.Request.AuditLog;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.AuditLog;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuditLogController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public AuditLogController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet("GetAuditLogList")]
    public ActionResult<Response<GetAuditLogListResponse>> Login([FromQuery] GetAuditLogListRequest request)
    {
        var searchModel = new AuditLogSearchModel(
                              request.LogId,
                              request.StartDate,
                              request.EndDate,
                              request.Domain,
                              request.ThreadId,
                              request.LogType,
                              request.HpId,
                              request.UserId,
                              request.LoginKey,
                              request.DepartmentId,
                              request.SinDay,
                              request.EventCd,
                              request.PtId,
                              request.RaiinNo,
                              request.Path,
                              request.RequestInfo,
                              request.ClientIP,
                              request.Desciption);
        var input = new GetAuditLogListInputData(request.TenantId, searchModel, request.SortDictionary, request.Skip, request.Take);
        var output = _bus.Handle(input);
        var presenter = new GetAuditLogListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetAuditLogListResponse>>(presenter.Result);
    }
}
