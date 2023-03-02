using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ChangeAfterAutoCheckOrderResponse
    {
        public ChangeAfterAutoCheckOrderResponse(List<OdrInfItemInputData> odrInfItems)
        {
            OdrInfItems = odrInfItems;
        }

        public List<OdrInfItemInputData> OdrInfItems { get; private set; }
    }
}
