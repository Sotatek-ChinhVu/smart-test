using Domain.Models.AuditLog;
using UseCase.SaveAuditLog;

namespace Interactor.AuditTrailLog
{
    public class SaveAuditTrailLogInteractor : ISaveAuditTrailLogInputPort
    {
        private readonly IAuditLogRepository _auditLogRepository;
        public SaveAuditTrailLogInteractor(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
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
            finally
            {
                _auditLogRepository.ReleaseResource();
            }
        }
    }
}
