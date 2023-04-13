using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetContainerMst
{
    public class GetContainerMstOutputData : IOutputData
    {
        public GetContainerMstOutputData(GetContainerMstStatus status, List<AuditTrailLogItem> auditTrailLogItems)
        {
            Status = status;
            AuditTrailLogItems = auditTrailLogItems;
        }

        public GetContainerMstStatus Status { get; private set; }
        public List<AuditTrailLogItem> AuditTrailLogItems { get; private set; }
    }
}
