using Domain.Common;

namespace Domain.Models.AuditLog
{
    public interface IAuditLogRepository : IRepositoryBase
    {
        bool SaveAuditLog(int hpId, int userId, AuditTrailLogModel auditTrailLogModel);

        void AddAuditTrailLog(int hpId, int userId, ArgumentModel arg);
    }
}
