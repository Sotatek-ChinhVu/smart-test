using Domain.Models.ApprovalInfo;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInfo.UpdateApprovalInfList;

public class UpdateApprovalInfListInputData : IInputData<UpdateApprovalInfListOutputData>
{
    public UpdateApprovalInfListInputData(List<ApprovalInfModel> approvalInfs, int userId, DateTime dateTime)
    {
        ApprovalInfs = approvalInfs;
        UserId = userId;
        Time = dateTime;
    }

    public List<ApprovalInfModel> ApprovalInfs { get; private set; }

    public int UserId { get; private set; }

    public DateTime Time { get; private set; }

    public List<ApprovalInfModel> ToList()
    {
        return ApprovalInfs;
    }
}