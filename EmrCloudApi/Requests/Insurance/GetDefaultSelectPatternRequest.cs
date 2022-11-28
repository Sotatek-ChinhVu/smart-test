namespace EmrCloudApi.Requests.Insurance
{
    public class GetDefaultSelectPatternRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int HistoryPid { get; set; }

        public int SelectedHokenPid { get; set; }
    }
}