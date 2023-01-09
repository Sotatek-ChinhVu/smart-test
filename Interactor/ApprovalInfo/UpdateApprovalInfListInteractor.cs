using Domain.Models.ApprovalInfo;
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

            var datas = input.ApprovalInfs.Select(x => new ApprovalInfModel(
               x.Id,
               x.HpId,
               x.PtId,
               x.SinDate,
               x.RaiinNo,
               x.SeqNo,
               x.IsDeleted,
               x.CreateDate,
               x.CreateId,
               x.CreateMachine,
               x.UpdateDate,
               x.UpdateId,
               x.UpdateMachine
                )).ToList();

            var checkInputId = datas.Where(u => u.Id > 0).Select(u => u.Id);
            if (checkInputId.Count() != checkInputId.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfoInvalidId);
            }

            var checkInputRaiinNo = input.ToList().Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo);
            if (checkInputRaiinNo.Count() != checkInputRaiinNo.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            foreach (var data in input.ToList())
            {
                var status = data.Validation();
                if(status != ValidationStatus.Valid)
                {
                    return new UpdateApprovalInfListOutputData(ConvertStatusApprovalInf(status));
                }
            }

            if (!_approvalInfRepository.CheckExistedId(datas.Where(u => u.Id > 0).Select(u => u.Id).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfListInvalidNoId);
            }

            if (!_approvalInfRepository.CheckExistedRaiinNo(datas.Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.ApprovalInfListInvalidNoRaiinNo);
            }

            _approvalInfRepository.UpdateApprovalInfs(input.ToList());

            return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.Success);
        }
        catch
        {
            return new UpdateApprovalInfListOutputData(UseCase.ApprovalInfo.UpdateApprovalInfList.ApprovalInfConstant.Failed);
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