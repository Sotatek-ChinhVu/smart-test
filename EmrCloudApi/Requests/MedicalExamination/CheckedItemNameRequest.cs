using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CheckedItemNameRequest
    {
        public List<OdrInfItemInputData> OdrInfs { get; set; } = new();
    }
}
