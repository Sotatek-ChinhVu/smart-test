namespace EmrCloudApi.Requests.ChartApproval
{
    public class SaveApprovalInfListRequest
    {
        public SaveApprovalInfListRequest(List<ApprovalInfDto> approvalInfs)
        {
            ApprovalInfs = approvalInfs;
        }

        public List<ApprovalInfDto> ApprovalInfs { get; private set; }
    }
}