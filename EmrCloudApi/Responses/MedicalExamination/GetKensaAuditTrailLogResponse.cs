using UseCase.MedicalExamination.GetKensaAuditTrailLog;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetKensaAuditTrailLogResponse
    {
        public GetKensaAuditTrailLogResponse(List<AuditTrailLogItem> auditTrailLogItems)
        {
            AuditTrailLogItems = auditTrailLogItems;
        }

        public List<AuditTrailLogItem> AuditTrailLogItems { get; private set; }
    }
}
