namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class GetMedicalExaminationHistoryRequest
    {
        public long PtId { get; set; }
        public int HpId { get; set; }
        public int SinDate { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int UserId { get; set; }
        public int SearchType { get; set; }
        public int SearchCategory { get; set; }
        public string SearchText { get; set; } = string.Empty;
    }
}
