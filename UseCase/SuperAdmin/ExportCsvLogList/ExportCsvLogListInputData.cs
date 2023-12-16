using Domain.SuperAdminModels.Logger;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvLogList;

public class ExportCsvLogListInputData : IInputData<ExportCsvLogListOutputData>
{
    public ExportCsvLogListInputData(List<AuditLogEnum> columnView, int tenantId, AuditLogSearchModel requestModel, Dictionary<AuditLogEnum, int> sortDictionary)
    {
        ColumnView = columnView;
        TenantId = tenantId;
        RequestModel = requestModel;
        SortDictionary = sortDictionary;
    }

    public List<AuditLogEnum> ColumnView { get; private set; }

    public int TenantId { get; private set; }

    public AuditLogSearchModel RequestModel { get; private set; }

    public Dictionary<AuditLogEnum, int> SortDictionary { get; private set; }
}
