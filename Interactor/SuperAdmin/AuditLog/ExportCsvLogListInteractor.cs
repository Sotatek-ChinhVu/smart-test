using Domain.SuperAdminModels.Logger;
using UseCase.SuperAdmin.ExportCsvLogList;

namespace Interactor.SuperAdmin.AuditLog;

public class ExportCsvLogListInteractor : IExportCsvLogListInputPort
{
    private readonly IAdminAuditLogRepository _adminAuditLogRepository;

    public ExportCsvLogListInteractor(IAdminAuditLogRepository adminAuditLogRepository)
    {
        _adminAuditLogRepository = adminAuditLogRepository;
    }

    public ExportCsvLogListOutputData Handle(ExportCsvLogListInputData inputData)
    {
        try
        {
            // get logs data from database
            var logDataList = _adminAuditLogRepository.GetAuditLogList(inputData.TenantId, inputData.RequestModel, inputData.SortDictionary, 0, 0, true);
            if (!logDataList.Any())
            {
                return new ExportCsvLogListOutputData(logDataList, ExportCsvLogListStatus.NoData);
            }

            return new ExportCsvLogListOutputData(logDataList, ExportCsvLogListStatus.Successed);
        }
        finally
        {
            _adminAuditLogRepository.ReleaseResource();
        }
    }
}

