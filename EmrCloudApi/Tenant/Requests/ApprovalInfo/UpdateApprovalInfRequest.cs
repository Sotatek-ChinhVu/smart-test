namespace EmrCloudApi.Tenant.Requests.ApprovalInfo
{
    public class UpdateApprovalInfRequest
    {
        public List<UpdateApprovalInfListRequest> ApprovalIfnList { get; set; } = new List<UpdateApprovalInfListRequest>();
    }
    public class UpdateApprovalInfListRequest
    {
        public int Id { get; set; }
        public long RaiinNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int IsDeleted { get; set; }
    }
}