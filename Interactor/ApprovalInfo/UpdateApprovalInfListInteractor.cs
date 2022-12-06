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
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfoListInputNoData);
            }

            var checkInputId = input.ToList().Where(u => u.Id > 0).Select(u => u.Id);
            if (checkInputId.Count() != checkInputId.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            var checkInputRaiinNo = input.ToList().Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo);
            if (checkInputRaiinNo.Count() != checkInputRaiinNo.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            foreach (var data in input.ToList())
            {
                var status = data.Validation();
                if (status != ValidationStatus.Valid)
                {
                    return new UpdateApprovalInfListOutputData(ConvertStatusApprovalInf(status));
                }
            }

            if (!_approvalInfRepository.CheckExistedId(input.ToList().Where(u => u.Id > 0).Select(u => u.Id).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            if (!_approvalInfRepository.CheckExistedRaiinNo(input.ToList().Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo);
            }

            _approvalInfRepository.UpdateApprovalInfs(input.ToList());

            return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.Success);
        }
        catch
        {
            return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.Failed);
        }
    }

    private static UpdateApprovalInfListStatus ConvertStatusApprovalInf(ValidationStatus status)
    {
        if (status == ValidationStatus.InvalidHpId)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidHpId;
        if (status == ValidationStatus.InvalidId)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidId;
        if (status == ValidationStatus.InvalidPtId)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidPtId;
        if (status == ValidationStatus.InvalidSinDate)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidSinDate;
        if (status == ValidationStatus.InvalidRaiinNo)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidRaiinNo;
        if (status == ValidationStatus.InvalidSeqNo)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidSeqNo;
        if (status == ValidationStatus.InvalidIsDeleted)
            return UpdateApprovalInfListStatus.ApprovalInfoInvalidIsDeleted;
        if (status == ValidationStatus.ApprovalInfListExistedInputData)
            return UpdateApprovalInfListStatus.ApprovalInfListExistedInputData;
        if (status == ValidationStatus.ApprovalInfListInvalidNoExistedId)
            return UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId;
        if (status == ValidationStatus.ApprovalInfListInvalidNoExistedRaiinNo)
            return UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo;

        return UpdateApprovalInfListStatus.Success;
    }
}