using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvTenantList;

public interface IExportCsvTenantListInputPort : IInputPort<ExportCsvTenantListInputData, ExportCsvTenantListOutputData>
{
}
