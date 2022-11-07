 namespace Domain.Models.AuditTrailLog;

public interface IAuditTrailLogRepository
{
    bool AddListAuditTrailLog(List<AuditTraiLogModel> listAuditTraiLogModels);
}