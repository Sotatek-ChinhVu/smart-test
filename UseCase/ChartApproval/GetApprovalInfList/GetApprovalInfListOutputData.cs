using Domain.Models.ChartApproval;
using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.GetApprovalInfList
{
    public class GetApprovalInfListOutputData : IOutputData
    {
        public GetApprovalInfListOutputData(GetApprovalInfListStatus status, string message, int messageType, List<ApprovalInfModel> approvalInfList)
        {
            Status = status;
            Message = message;
            ApprovalInfList = approvalInfList;
        }

        public GetApprovalInfListStatus Status { get; private set; }

        public string Message { get; private set; }

        public int MessageType { get; private set; }

        public List<ApprovalInfModel> ApprovalInfList { get; private set; }
    }
}