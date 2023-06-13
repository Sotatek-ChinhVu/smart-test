namespace EmrCloudApi.Requests.ExportPDF
{
    public class Sta1001Request : ReportRequestBase
    {
        public int MenuId { get; set; }

        public int DateFrom { get; set; }

        public int DateTo { get; set; }

        public int TimeFrom { get; set; }

        public int TimeTo { get; set; }
    }
}
