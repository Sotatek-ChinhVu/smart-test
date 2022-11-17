using Domain.Models.ApprovalInfo;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInfo.GetApprovalInfList;

public class GetApprovalInfListOutputData : IOutputData
{
    public GetApprovalInfListOutputData(GetApprovalInfListStatus status)
    {
        Status = status;
    }
    public GetApprovalInfListOutputData(GetApprovalInfListStatus status, List<ApprovalInfModel> approvalInfList)
    {
        Status = status;
        ApprovalInfList = approvalInfList;
    }
    public GetApprovalInfListStatus Status { get; private set; }
    public List<ApprovalInfModel> ApprovalInfList { get; private set; } = new List<ApprovalInfModel>();
}
