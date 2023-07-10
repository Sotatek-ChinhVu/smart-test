namespace EmrCloudApi.Requests.Lock
{
    public class GetLockInfoRequest
    {
        public long PtId { get; set; }
        public List<string> ListFunctionCdB { get; set; } = new();
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
    }
}
