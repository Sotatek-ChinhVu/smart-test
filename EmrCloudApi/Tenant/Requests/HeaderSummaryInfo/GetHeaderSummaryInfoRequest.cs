namespace EmrCloudApi.Tenant.Requests.HeaderSummaryInfo
{
    public class GetHeaderSummaryInfoRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RainNo { get; set; }
    }
}
