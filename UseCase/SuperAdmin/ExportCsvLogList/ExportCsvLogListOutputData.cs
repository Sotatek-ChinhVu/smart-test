using Domain.SuperAdminModels.Logger;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvLogList;

public class ExportCsvLogListOutputData : IOutputData
{
    public ExportCsvLogListOutputData(List<AuditLogModel> auditLogList, ExportCsvLogListStatus status)
    {
        AuditLogList = auditLogList;
        Status = status;
    }

    public List<AuditLogModel> AuditLogList { get; private set; }

    public ExportCsvLogListStatus Status { get; private set; }
}
