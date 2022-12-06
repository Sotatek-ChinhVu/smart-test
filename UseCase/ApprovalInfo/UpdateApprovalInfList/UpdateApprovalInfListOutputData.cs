using Domain.Models.ApprovalInfo;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInfo.UpdateApprovalInfList;

public class UpdateApprovalInfListOutputData : IOutputData
{
    public UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus status)
    {
        Status = status;
    }
    public UpdateApprovalInfListOutputData(UpdateApprovalInfListStatus status, List<ApprovalInfModel> approvalInfList)
    {
        Status = status;
        ApprovalInfList = approvalInfList;
    }
    public UpdateApprovalInfListStatus Status { get; private set; }
    public List<ApprovalInfModel> ApprovalInfList { get; private set; } = new List<ApprovalInfModel>();
}