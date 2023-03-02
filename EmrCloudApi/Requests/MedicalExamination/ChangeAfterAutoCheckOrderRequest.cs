using UseCase.MedicalExamination.AutoCheckOrder;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ChangeAfterAutoCheckOrderRequest
    {
        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long PtId { get; set; }

        public List<OdrInfItemInputData> OdrInfs { get; set; } = new();

        public List<AutoCheckOrderItem> TargetItems { get; set; } = new();
    }
}
