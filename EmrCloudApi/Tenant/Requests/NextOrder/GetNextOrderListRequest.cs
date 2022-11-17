namespace EmrCloudApi.Tenant.Requests.NextOrder
{
    public class GetNextOrderListRequest
    {
        public long PtId { get; set; }

        public int RsvkrtKbn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
