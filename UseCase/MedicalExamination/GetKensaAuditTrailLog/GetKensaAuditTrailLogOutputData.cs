using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetKensaAuditTrailLog
{
    public class GetKensaAuditTrailLogOutputData : IOutputData
    {
        public GetKensaAuditTrailLogOutputData(GetKensaAuditTrailLogStatus status, List<AuditTrailLogItem> auditTrailLogItems)
        {
            Status = status;
            AuditTrailLogItems = auditTrailLogItems;
        }

        public GetKensaAuditTrailLogStatus Status { get; private set; }
        public List<AuditTrailLogItem> AuditTrailLogItems { get; private set; }
    }
}
