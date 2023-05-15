namespace EmrCloudApi.Requests.ExportPDF
{
    public class ReceiptPrintRequest
    {
        public int HpId { get; set; }

        public int PrefNo { get; set; }

        public int ReportId { get; set; }

        public int ReportEdaNo { get; set; }

        public int PtId { get; set; }

        public int SeikyuYm { get; set; }

        public int SinYm { get; set; }

        public int HokenId { get; set; }
    }
}
