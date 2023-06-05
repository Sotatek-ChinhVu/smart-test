namespace EmrCloudApi.Requests.ExportPDF
{
    public class AccountingCardReportingRequest
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public int SinYm { get; set; }
        public int HokenId { get; set; }
        public bool IncludeOutDrug { get; set; }
    }
}
