using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetOrderSheetGroup
{
    public class GetOrderSheetGroupOutputData : IOutputData
    {
        public GetOrderSheetGroupOutputData(GetOrderSheetGroupStatus status, List<OrderSheetItem> orderSheetItems)
        {
            Status = status;
            this.orderSheetItems = orderSheetItems;
        }

        public GetOrderSheetGroupStatus Status { get; private set; }
        public List<OrderSheetItem> orderSheetItems { get; private set; }
    }
}
