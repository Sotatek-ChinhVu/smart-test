namespace EmrCloudApi.Tenant.Requests.ApprovalInf
{
    public class GetApprovalInfListRequest
    {
        public int StarDate { get; set; }
        public int EndDate { get; set; }
        public string DrName { get; set; }
        public string KaName { get; set; }
    }
}
