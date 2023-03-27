using UseCase.MedicalExamination.GetOrderSheetGroup;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetOrderSheetGroupResponse
    {
        public GetOrderSheetGroupResponse(List<OrderSheetItem> orderSheetItems)
        {
            OrderSheetItems = orderSheetItems;
        }

        public List<OrderSheetItem> OrderSheetItems { get; private set; }
    }
}
