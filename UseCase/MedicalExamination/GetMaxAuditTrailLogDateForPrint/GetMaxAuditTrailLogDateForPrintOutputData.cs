using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint
{
    public class GetMaxAuditTrailLogDateForPrintOutputData : IOutputData
    {
        public GetMaxAuditTrailLogDateForPrintOutputData(GetMaxAuditTrailLogDateForPrintStatus status, Dictionary<string, string> checkedItemNames)
        {
            Status = status;
            CheckedItemNames = checkedItemNames;
        }

        public GetMaxAuditTrailLogDateForPrintStatus Status { get; private set; }
        public Dictionary<string, string> CheckedItemNames { get; private set; }
    }
}
