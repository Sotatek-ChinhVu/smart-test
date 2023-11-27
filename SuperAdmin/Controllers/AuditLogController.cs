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

    [HttpPost("GetAuditLogList")]
    public ActionResult<Response<GetAuditLogListResponse>> Login([FromBody] GetAuditLogListRequest request)
    {
        var searchModel = new AuditLogSearchModel(
                              request.RequestModel.LogId,
                              request.RequestModel.StartDate,
                              request.RequestModel.EndDate,
                              request.RequestModel.Domain,
                              request.RequestModel.ThreadId,
                              request.RequestModel.LogType,
                              request.RequestModel.HpId,
                              request.RequestModel.UserId,
                              request.RequestModel.LoginKey,
                              request.RequestModel.DepartmentId,
                              request.RequestModel.SinDay,
                              request.RequestModel.EventCd,
                              request.RequestModel.PtId,
                              request.RequestModel.RaiinNo,
                              request.RequestModel.Path,
                              request.RequestModel.RequestInfo,
                              request.RequestModel.ClientIP,
                              request.RequestModel.Desciption);
        var input = new GetAuditLogListInputData(request.TenantId, searchModel, request.SortDictionary, request.Skip, request.Take);
        var output = _bus.Handle(input);
        var presenter = new GetAuditLogListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetAuditLogListResponse>>(presenter.Result);
    }
}
