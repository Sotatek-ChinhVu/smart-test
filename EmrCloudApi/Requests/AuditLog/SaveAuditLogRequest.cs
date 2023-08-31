using Domain.Models.AuditLog;

namespace EmrCloudApi.Requests.AuditLog;

public class SaveAuditLogRequest
{
    public AuditTrailLogModel AuditTrailLogModel { get; set; } = new();
}
