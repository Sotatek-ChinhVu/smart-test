using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvTenantList;

public class ExportCsvTenantListOutputData : IOutputData
{
    public ExportCsvTenantListOutputData(List<string> data, ExportCsvTenantListStatus status)
    {
        Data = data;
        Status = status;
    }

    public List<string> Data { get; private set; }

    public ExportCsvTenantListStatus Status { get; private set; }
}
