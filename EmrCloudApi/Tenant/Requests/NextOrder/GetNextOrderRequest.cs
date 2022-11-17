namespace EmrCloudApi.Tenant.Requests.NextOrder
{
    public class GetNextOrderRequest
    {
        public long PtId { get; set; }

        public long RsvkrtNo { get; set; }

        public int SinDate { get; set; }

        public int Type { get; set; }
    }
}
