using Domain.Models.ChartApproval;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.ChartApproval.SaveApprovalInfList;

namespace Interactor.ChartApproval
{
    public class SaveApprovalInfListInteractor : ISaveApprovalInfListInputPort
    {
        private readonly IApprovalInfRepository _approvalInfRepository;
        private readonly ILoggingHandler _loggingHandler;

        public SaveApprovalInfListInteractor(ITenantProvider tenantProvider, IApprovalInfRepository approvalInfRepository)
        {
            _approvalInfRepository = approvalInfRepository;
            _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveApprovalInfListOutputData Handle(SaveApprovalInfListInputData input)
        {
            try
            {
                if (input.HpId < 0)
                    return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.InvalidHpId);

                if (input.UserId < 0)
                    return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.InvalidUserId);

                if (input.ApprovalInfs == null || !input.ApprovalInfs.Any())
                    return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.InvalidInputListApporoval);

                bool result = _approvalInfRepository.SaveApprovalInfs(input.ApprovalInfs, input.HpId, input.UserId);
                if (result)
                    return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.Success);
                else
                    return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _approvalInfRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}