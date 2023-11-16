using Domain.SuperAdminModels.Logger;
using SuperAdminAPI.Reponse.AuditLog.Dto;

namespace SuperAdminAPI.Reponse.AuditLog;

public class GetAuditLogListResponse
{
    public GetAuditLogListResponse(List<AuditLogModel> auditLogList)
    {
        AuditLogList = auditLogList.Select(item => new AuditLogDto(item)).ToList();
    }

    public List<AuditLogDto> AuditLogList { get; private set; }
}
