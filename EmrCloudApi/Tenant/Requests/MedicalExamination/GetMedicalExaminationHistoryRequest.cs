namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class GetMedicalExaminationHistoryRequest
    {
        public long PtId { get; set; }
        public int HpId { get; set; }
        public int SinDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int UserId { get; set; }
    }
}
