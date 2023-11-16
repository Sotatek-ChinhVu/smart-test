using Domain.SuperAdminModels.Logger;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.AuditLog;

public class GetAuditLogListInputData : IInputData<GetAuditLogListOutputData>
{
    public GetAuditLogListInputData(int tenantId, AuditLogSearchModel requestModel, Dictionary<AuditLogEnum, int> sortDictionary, int skip, int take)
    {
        TenantId = tenantId;
        RequestModel = requestModel;
        SortDictionary = sortDictionary;
        Skip = skip;
        Take = take;
    }

    public int TenantId { get; private set; }

    public AuditLogSearchModel RequestModel { get; private set; }

    public Dictionary<AuditLogEnum, int> SortDictionary { get; private set; }

    public int Skip { get; private set; }

    public int Take { get; private set; }
}
