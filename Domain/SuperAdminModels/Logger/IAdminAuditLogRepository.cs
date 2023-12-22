using Domain.Common;
using Helper.Enum;

namespace Domain.SuperAdminModels.Logger;

public interface IAdminAuditLogRepository : IRepositoryBase
{
    List<AuditLogModel> GetAuditLogList(int tenantId, AuditLogSearchModel requestModel, Dictionary<AuditLogEnum, int> sortDictionary, int skip, int take, bool getDataReport = false);
}
