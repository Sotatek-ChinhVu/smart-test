using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class AddedAutoItem : IOutputData
    {
        public AddedAutoItem(int orderPosition, int orderDetailPosition, List<AddedOrderDetail> addedOrderDetails)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            AddedOrderDetails = addedOrderDetails;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public List<AddedOrderDetail> AddedOrderDetails { get; private set; }
    }
}
