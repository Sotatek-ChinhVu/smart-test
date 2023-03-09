using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ChangeAfterAutoCheckOrderResponse
    {
        public ChangeAfterAutoCheckOrderResponse(List<ChangeAfterAutoCheckOrderItem> odrInfItems)
        {
            OdrInfItems = odrInfItems;
        }

        public List<ChangeAfterAutoCheckOrderItem> OdrInfItems { get; private set; }
    }
}
