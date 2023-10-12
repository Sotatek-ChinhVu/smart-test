using Infrastructure.Common;
using UseCase.Core.Sync.Core;

namespace UseCase.Logger.WriteListLog;

public class WriteListLogInputData : IInputData<WriteListLogOutputData>
{
    public WriteListLogInputData(List<AuditLogModel> auditLogList)
    {
        AuditLogList = auditLogList;
    }

    public List<AuditLogModel> AuditLogList { get; private set; }
}
