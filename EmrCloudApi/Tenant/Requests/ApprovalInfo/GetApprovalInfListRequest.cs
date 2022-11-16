namespace EmrCloudApi.Tenant.Requests.ApprovalInfo
{
    public class GetApprovalInfListRequest
    {
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public string KaName { get; set; }
        public string DrName { get; set; }

    }
}
