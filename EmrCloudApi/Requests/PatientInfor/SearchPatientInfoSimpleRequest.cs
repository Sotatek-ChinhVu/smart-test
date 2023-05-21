namespace EmrCloudApi.Requests.PatientInfor
{
    public class SearchPatientInfoSimpleRequest
    {
        public string Keyword { get; set; } = string.Empty;

        public bool IsContainMode { get; set; } = false;

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<SortCol> SortData { get; set; }
    }

    public class SortCol
    {
        public string Col { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}
