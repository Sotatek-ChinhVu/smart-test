using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Logger;

namespace Interactor.Logger
{
    public class WriteLogInteractor : IWriteLogInputPort
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public WriteLogInteractor(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public WriteLogOutputData Handle(WriteLogInputData inputData)
        {
            var status = _loggingHandler.WriteAuditLog(
                 inputData.Path, inputData.RequestInfo, inputData.EventCd, inputData.PtId, inputData.RaiinNo, inputData.SinDay, inputData.Description, inputData.LogType, inputData.LoginId);

            return new WriteLogOutputData(status ? WriteLogStatus.Successed : WriteLogStatus.Failed);
        }
    }
}
