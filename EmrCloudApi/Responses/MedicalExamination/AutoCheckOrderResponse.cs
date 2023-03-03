using UseCase.MedicalExamination.AutoCheckOrder;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class AutoCheckOrderResponse
    {
        public AutoCheckOrderResponse(List<AutoCheckOrderItem> autoCheckOrderItems)
        {
            AutoCheckOrderItems = autoCheckOrderItems;
        }

        public List<AutoCheckOrderItem> AutoCheckOrderItems { get; private set; }
    }
}
