using Domain.Models.ApprovalInfo;
using Helper.Constants;
using UseCase.ApprovalInfo.UpdateApprovalInfList;
using static Helper.Constants.ApprovalInfConstant;
namespace Interactor.ApprovalInfo;

public class UpdateApprovalInfListInteractor : IUpdateApprovalInfListInputPort
{
    private readonly IApprovalInfRepository _approvalInfRepository;

    public UpdateApprovalInfListInteractor(IApprovalInfRepository approvalInfRepository)
    {
        _approvalInfRepository = approvalInfRepository;
    }

    public UpdateApprovalInfListOutputData Handle(UpdateApprovalInfListInputData input)
    {
        try
        {
            if (input.ToList() == null || input.ToList().Count == 0)
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfoListInputNoData);
            }

            var checkInputId = input.ToList().Where(u => u.Id > 0).Select(u => u.Id);
            if (checkInputId.Count() != checkInputId.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfoInvalidId);
            }

            foreach (var data in input.ToList())
            {
                var status = data.Validation();
                if (status != ValidationStatus.Valid)
                {
                    return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.Failed);
                }
            }

            if (!_approvalInfRepository.CheckExistedId(input.ToList().Where(u => u.Id > 0).Select(u => u.Id).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfListInvalidNoId);
            }

            if (!_approvalInfRepository.CheckExistedRaiinNo(input.ToList().Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfListInvalidNoRaiinNo);
            }

            _approvalInfRepository.UpdateApprovalInfs(input.ToList(), input.UserId);

            return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.Success);
        }
        catch
        {
            return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.Failed);
        }
    }
}