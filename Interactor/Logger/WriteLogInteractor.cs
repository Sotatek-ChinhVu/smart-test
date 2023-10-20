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
            try
            {
                var status = _loggingHandler.WriteAuditLog(
                 inputData.Path, inputData.RequestInfo, inputData.EventCd, inputData.PtId, inputData.RaiinNo, inputData.SinDay, inputData.Description, inputData.LogType);

                return new WriteLogOutputData(status ? WriteLogStatus.Successed : WriteLogStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _loggingHandler.Dispose();
            }
        }
    }
}
