namespace EmrCloudApi.Requests.ExportCsv
{
    public class ExportCsvSta9000Request
    {

        public List<string> OutputColumns { get; set; } = new();

        public bool IsPutColName { get; set; }

        public string OutputFileName { get; set; } = string.Empty;
    }
}
