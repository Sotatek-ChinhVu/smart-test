using Domain.Models.ApprovalInfo;
using Helper.Constants;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInfo.UpdateApprovalInfList;

public class UpdateApprovalInfListOutputData : IOutputData
{
    public UpdateApprovalInfListOutputData(ApprovalInfConstant status)
    {
        Status = status;
    }
    public UpdateApprovalInfListOutputData(ApprovalInfConstant status, List<ApprovalInfModel> approvalInfList)
    {
        Status = status;
        ApprovalInfList = approvalInfList;
    }
    public ApprovalInfConstant Status { get; private set; }
    public List<ApprovalInfModel> ApprovalInfList { get; private set; } = new List<ApprovalInfModel>();
}