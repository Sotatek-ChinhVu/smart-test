namespace EmrCloudApi.Tenant.Requests.ApprovalInfo
{
    public class GetApprovalInfListRequest
    {
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int KaId { get; set; }
        public int TantoId { get; set; }

    }
}
