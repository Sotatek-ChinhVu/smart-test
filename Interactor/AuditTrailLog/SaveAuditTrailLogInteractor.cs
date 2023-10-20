using Domain.Models.AuditLog;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SaveAuditLog;

namespace Interactor.AuditTrailLog
{
    public class SaveAuditTrailLogInteractor : ISaveAuditTrailLogInputPort
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveAuditTrailLogInteractor(ITenantProvider tenantProvider, IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveAuditTrailLogOutputData Handle(SaveAuditTrailLogInputData inputData)
        {
            try
            {
                var result = _auditLogRepository.SaveAuditLog(inputData.HpId, inputData.UserId, inputData.AuditTrailLogModel);
                if (result)
                {
                    return new SaveAuditTrailLogOutputData(SaveAuditTrailLogStatus.Successed);
                }

                return new SaveAuditTrailLogOutputData(SaveAuditTrailLogStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _auditLogRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
