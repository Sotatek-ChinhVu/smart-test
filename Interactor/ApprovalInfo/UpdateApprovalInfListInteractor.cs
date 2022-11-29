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
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfoListInputNoData);
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
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            var checkInputRaiinNo = datas.Where(u => u.Id > 0).Select(u => u.Id);
            if (checkInputRaiinNo.Count() != checkInputRaiinNo.Distinct().Count())
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            foreach (var data in datas )
            {
                var status = data.Validation();
                if(status != ValidationStatus.Valid)
                {
                    return new UpdateApprovalInfListOutputData(ConvertStatusApprovalInf(status));
                }    
            }

            if (!_approvalInfRepository.CheckExistedId(datas.Where(u => u.Id > 0).Select(u => u.Id).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId);
            }

            if (!_approvalInfRepository.CheckExistedRaiinNo(datas.Where(u => u.RaiinNo > 0).Select(u => u.RaiinNo).ToList()))
            {
                return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo);
            }

            _approvalInfRepository.UpdateApprovalInfs(datas);

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
            return UpdateApprovalInfListStatus.InvalidHpId;
        if (status == ValidationStatus.InvalidId)
            return UpdateApprovalInfListStatus.InvalidId;
        if (status == ValidationStatus.InvalidPtId)
            return UpdateApprovalInfListStatus.InvalidPtId;
        if (status == ValidationStatus.InvalidSinDate)
            return UpdateApprovalInfListStatus.InvalidSinDate;
        if (status == ValidationStatus.InvalidRaiinNo)
            return UpdateApprovalInfListStatus.InvalidRaiinNo;
        if (status == ValidationStatus.InvalidSeqNo)
            return UpdateApprovalInfListStatus.InvalidSeqNo;
        if (status == ValidationStatus.InvalidIsDeleted)
            return UpdateApprovalInfListStatus.InvalidIsDeleted;
        if (status == ValidationStatus.InvalidCreateId)
            return UpdateApprovalInfListStatus.InvalidCreateId;
        if (status == ValidationStatus.InvalidUpdateId)
            return UpdateApprovalInfListStatus.InvalidUpdateId;
        if (status == ValidationStatus.InvalidCreateMachine)
            return UpdateApprovalInfListStatus.InvalidCreateMachine;
        if (status == ValidationStatus.InvalidUpdateMachine)
            return UpdateApprovalInfListStatus.InvalidCreateMachine;
        if (status == ValidationStatus.ApprovalInfListExistedInputData)
            return UpdateApprovalInfListStatus.ApprovalInfListExistedInputData;
        if (status == ValidationStatus.ApprovalInfListInvalidNoExistedId)
            return UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId;
        if (status == ValidationStatus.ApprovalInfListInvalidNoExistedRaiinNo)
            return UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo;

        return UpdateApprovalInfListStatus.Success;
    }
}
