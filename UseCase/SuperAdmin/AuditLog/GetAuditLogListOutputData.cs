using Domain.SuperAdminModels.Logger;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.AuditLog;

public class GetAuditLogListOutputData : IOutputData
{
    public GetAuditLogListOutputData(List<AuditLogModel> auditLogList, GetAuditLogListStatus status)
    {
        AuditLogList = auditLogList;
        Status = status;
    }

    public List<AuditLogModel> AuditLogList { get; private set; }

    public GetAuditLogListStatus Status { get; private set; }
}
