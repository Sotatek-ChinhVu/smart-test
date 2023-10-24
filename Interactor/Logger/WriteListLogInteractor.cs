using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Logger.WriteListLog;

namespace Interactor.Logger;

public class WriteListLogInteractor : IWriteListLogInputPort
{
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public WriteListLogInteractor(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public WriteListLogOutputData Handle(WriteListLogInputData inputData)
    {
        var status = _loggingHandler.WriteAuditLog(inputData.AuditLogList);

        return new WriteListLogOutputData(status ? WriteListLogStatus.Successed : WriteListLogStatus.Failed);
    }
}
