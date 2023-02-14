namespace EmrCloudApi.Requests.Accounting
{
    public class GetAccountingRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }
    }
}
