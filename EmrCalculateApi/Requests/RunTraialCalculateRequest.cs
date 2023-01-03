namespace EmrCalculateApi.Requests
{
    public class RunTraialCalculateRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public List<OrderInfo> OrderInfoList { get; set; } = new List<OrderInfo>();
    }
}
