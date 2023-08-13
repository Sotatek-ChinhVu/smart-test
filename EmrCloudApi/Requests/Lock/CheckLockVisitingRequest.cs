namespace EmrCloudApi.Requests.Lock
{
    public class CheckLockVisitingRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public string FunctionCode { get; set; } = string.Empty;
        public string TabKey { get; set; } = string.Empty;
    }
}
