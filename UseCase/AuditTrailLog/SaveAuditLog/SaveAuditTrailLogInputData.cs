using Domain.Models.AuditLog;
using UseCase.Core.Sync.Core;

namespace UseCase.SaveAuditLog
{
    public class SaveAuditTrailLogInputData : IInputData<SaveAuditTrailLogOutputData>
    {
        public SaveAuditTrailLogInputData(int hpId, int userId, AuditTrailLogModel auditTrailLogModel)
        {
            HpId = hpId;
            UserId = userId;
            AuditTrailLogModel = auditTrailLogModel;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public AuditTrailLogModel AuditTrailLogModel { get; private set; }
    }
}
