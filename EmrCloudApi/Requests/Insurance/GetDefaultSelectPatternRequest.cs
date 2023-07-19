namespace EmrCloudApi.Requests.Insurance
{
    public class GetDefaultSelectPatternRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public List<int> HistoryPids { get; set; }

        public int SelectedHokenPid { get; set; }
    }
}