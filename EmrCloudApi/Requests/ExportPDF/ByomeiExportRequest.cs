namespace EmrCloudApi.Requests.ExportPDF
{
    public class ByomeiExportRequest
    {
        public long PtId { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public bool TenkiIn { get; set; }

        public List<int> HokenIdList { get; set; } = new();
    }
}
