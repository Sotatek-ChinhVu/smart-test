namespace EmrCloudApi.Tenant.Responses.ApprovalInfo
{
    public class UpdateApprovalInfListResponse
    {
        public UpdateApprovalInfListResponse(bool success)
        {
            Success = success;
        }
        public bool Success { get; private set; }
    }
}