namespace EmrCloudApi.Requests.ReceSeikyu
{
    public class GetListReceSeikyuRequest
    {
        public int SinDate { get; set; }

        public int SinYm { get; set; }

        public bool IsIncludingUnConfirmed { get; set; }

        public long PtNumSearch { get; set; }

        public bool NoFilter { get; set; }

        public bool IsFilterMonthlyDelay { get; set; }

        public bool IsFilterReturn { get; set; }

        public bool IsFilterOnlineReturn { get; set; }
    }
}
