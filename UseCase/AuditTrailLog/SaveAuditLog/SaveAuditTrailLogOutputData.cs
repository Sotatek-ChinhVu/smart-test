using Domain.Models.CalculationInf;
using UseCase.Core.Sync.Core;

namespace UseCase.SaveAuditLog
{
    public class SaveAuditTrailLogOutputData : IOutputData
    {
        public SaveAuditTrailLogOutputData(SaveAuditTrailLogStatus status)
        {
            Status = status;
        }

        public SaveAuditTrailLogStatus Status { get; private set; }

    }
}
