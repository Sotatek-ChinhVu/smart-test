namespace EmrCalculateApi.Requests
{
    public class GetSinMeiListRequest
    {
        public List<long> RaiinNoList { get; set; } = new List<long>();

        public int SinDate { get; set; }

        public long PtId { get; set; }

        public int HpId { get; set; }
    }
}
