using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetMaxAuditTrailLogDateForPrintRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }
    }
}
