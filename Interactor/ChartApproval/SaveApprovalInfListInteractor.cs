using Domain.Models.ChartApproval;
using UseCase.ChartApproval.SaveApprovalInfList;

namespace Interactor.ChartApproval
{
    public class SaveApprovalInfListInteractor : ISaveApprovalInfListInputPort
    {
        private readonly IApprovalInfRepository _approvalInfRepository;

        public SaveApprovalInfListInteractor(IApprovalInfRepository approvalInfRepository) => _approvalInfRepository = approvalInfRepository;

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
            catch
            {
                return new SaveApprovalInfListOutputData(SaveApprovalInfStatus.Failed);
            }
        }
    }
}