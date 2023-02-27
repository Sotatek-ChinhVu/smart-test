namespace EmrCloudApi.Requests.Accounting
{
    public class SaveAccountingRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public int SumAdjust { get; set; }
        public int ThisWari { get; set; }
        public int Credit { get; set; }
        public int PayType { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsDisCharged { get; set; }
    }
}
