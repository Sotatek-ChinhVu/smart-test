namespace EmrCloudApi.Requests.Accounting
{
    public class GetAccountingConfigRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SumAdjust { get; set; }
    }
}
