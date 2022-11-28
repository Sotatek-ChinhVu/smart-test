using Domain.Models.ApprovalInfo;
using UseCase.ApprovalInfo.UpdateApprovalInfList;

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
               x.RaiiNo,
               x.SeqNo,
               x.IsDeleted,
               x.CreateDate,
               x.CreateId,
               x.CreateMachine,
               x.UpdateDate,
               x.UpdateId,
               x.UpdateMachine
                )).ToList();
            _approvalInfRepository.UpdateApprovalInfs(datas);
            return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.Success);
        }
        catch
        {
            return new UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus.Failed);
        }
    }
}
