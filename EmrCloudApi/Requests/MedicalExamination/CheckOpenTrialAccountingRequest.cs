namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CheckOpenTrialAccountingRequest
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public int SyosaiKbn { get; set; }
        public List<OrderItem> AllOdrInfItem { get; set; } = new();
        public List<int> OdrInfHokenPid { get; set; } = new();

    }

    public class OrderItem
    {
        public string ItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;
    }
}
