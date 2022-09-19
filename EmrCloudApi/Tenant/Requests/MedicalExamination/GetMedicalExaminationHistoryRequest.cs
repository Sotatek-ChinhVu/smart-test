namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class GetMedicalExaminationHistoryRequest
    {
        public long PtId { get; set; }
        public int HpId { get; set; }
        public int SinDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int DeleteCondition { get; set; }
        public int KarteDeleteHistory { get; set; }
        public int UserId { get; set; }
        public long FilterId { get; set; }
        public int IsShowApproval { get; set; }
    }
}
