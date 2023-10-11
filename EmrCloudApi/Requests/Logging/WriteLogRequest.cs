namespace EmrCloudApi.Requests.Logging
{
    public class WriteLogRequest
    {
        public string EventCd { get; set; } = string.Empty;

        public long PtId { get; set; }

        public int SinDay { get; set; }

        public long RaiinNo { get; set; }

        public string Path { get; set; } = string.Empty;

        public string RequestInfo { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LogType { get; set; } = string.Empty;
    }
}
