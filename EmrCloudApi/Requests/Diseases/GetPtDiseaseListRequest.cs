namespace EmrCloudApi.Requests.Diseases
{
    public class GetPtDiseaseListRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int HokenId { get; set; }
        public int RequestFrom { get; set; }
        public bool IsContiFiltered { get; set; }
        public bool IsInMonthFiltered { get; set; }
    }
}
