namespace EmrCloudApi.Requests.Accounting
{
    public class GetMeiHoGaiRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public List<long> RaiinNos { get; set; } = new();
    }
}
