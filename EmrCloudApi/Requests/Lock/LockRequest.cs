namespace EmrCloudApi.Requests.Lock
{
    public class LockRequest
    {
        public long PtId { get; set; }

        public long RaiinNo { get; set; }

        public int SinDate { get; set; }

        public string FunctionCod { get; set; } = string.Empty;

        public string TabKey { get; set; } = string.Empty;

        public string LoginKey { get; set; } = string.Empty;
    }
}
