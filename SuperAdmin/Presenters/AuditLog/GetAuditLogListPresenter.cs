using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.AuditLog;
using UseCase.SuperAdmin.AuditLog;

namespace SuperAdminAPI.Presenters.AuditLog;

public class GetAuditLogListPresenter : IGetAuditLogListOutputPort
{
    public Response<GetAuditLogListResponse> Result { get; private set; } = new();

    public void Complete(GetAuditLogListOutputData outputData)
    {
        Result.Data = new GetAuditLogListResponse(outputData.AuditLogList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetAuditLogListStatus status) => status switch
    {
        GetAuditLogListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
