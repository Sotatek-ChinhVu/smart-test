using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class AutoCheckOrderRequest
    {
        public int SinDate { get; set; }

        public long PtId { get; set; }

        public List<OdrInfItemInputData> OdrInfs { get; set; } = new();
    }
}
