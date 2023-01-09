using Domain.Models.MedicalExamination;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetCheckedOrderResponse
    {
        public GetCheckedOrderResponse(List<CheckedOrderModel> checkedOrderModels)
        {
            CheckedOrderModels = checkedOrderModels;
        }

        public List<CheckedOrderModel> CheckedOrderModels { get; private set; }
    }
}
