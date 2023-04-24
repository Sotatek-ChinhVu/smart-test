using UseCase.ChartApproval.SaveApprovalInfList;

namespace EmrCloudApi.Responses.ChartApproval
{
    public class SaveApprovalInfListResponse
    {
        public SaveApprovalInfListResponse(SaveApprovalInfStatus status)
        {
            Status = status;
        }

        public SaveApprovalInfStatus Status { get; private set; }
    }
}