using Domain.Common;

namespace Domain.SuperAdminModels.Logger;

public interface IAuditLogRepository : IRepositoryBase
{
    List<AuditLogModel> GetAuditLogList(AuditLogModel requestModel, int pageIndex, int pageSize);
}
