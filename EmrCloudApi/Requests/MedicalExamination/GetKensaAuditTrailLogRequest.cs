namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetKensaAuditTrailLogRequest
    {
        public long RaiinNo { get; set; }

        public int SinDate { get; set; }

        public long PtId { get; set; }

        public string EventCd { get; set; } = string.Empty;
    }
}
