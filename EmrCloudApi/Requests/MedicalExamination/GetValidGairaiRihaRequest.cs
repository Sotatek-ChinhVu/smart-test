namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetValidGairaiRihaRequest
    {
        public int PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public int SyosaiKbn { get; set; }
        public List<AllOrderItem> AllOdrInfItems { get; set; } = new();
    }

    public class AllOrderItem
    {
        public string ItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;
    }
}
