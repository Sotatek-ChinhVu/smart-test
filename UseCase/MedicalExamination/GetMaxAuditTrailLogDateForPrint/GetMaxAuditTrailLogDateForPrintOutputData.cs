using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint
{
    public class GetMaxAuditTrailLogDateForPrintOutputData : IOutputData
    {
        public GetMaxAuditTrailLogDateForPrintOutputData(Dictionary<string, DateTime> values, GetMaxAuditTrailLogDateForPrintStatus status)
        {
            Values = values;
            Status = status;
        }

        public Dictionary<string, DateTime> Values { get; private set; }

        public GetMaxAuditTrailLogDateForPrintStatus Status { get; private set; }
    }
}
