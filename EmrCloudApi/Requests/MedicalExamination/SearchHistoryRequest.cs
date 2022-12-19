namespace EmrCloudApi.Requests.MedicalExamination
{
    public class SearchHistoryRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int CurrentIndex { get; set; }

        public int FilterId { get; set; }

        public int IsDeleted { get; set; }

        public string KeyWord { get; set; } = string.Empty;

        public int SearchType { get; set; }

        public bool IsNext { get; set; }
    }
}
