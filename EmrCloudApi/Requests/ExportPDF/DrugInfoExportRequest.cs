namespace EmrCloudApi.Requests.ExportPDF
{
    public class DrugInfoExportRequest : ReportRequestBase
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
    }
}