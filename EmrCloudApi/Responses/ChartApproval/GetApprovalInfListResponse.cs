using Domain.Models.ChartApproval;

namespace EmrCloudApi.Responses.ChartApproval
{
    public class GetApprovalInfListResponse
    {
        public GetApprovalInfListResponse(List<ApprovalInfModel> approvalInfList, string message, int messageType)
        {
            ApprovalInfList = approvalInfList;
            Message = message;
            MessageType = messageType;
        }

        public List<ApprovalInfModel> ApprovalInfList { get; private set; }

        public string Message { get; private set; }

        public int MessageType { get; private set; }
    }
}
