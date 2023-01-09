namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class OrderInfItem
    {
        public OrderInfItem(int orderPosition, int orderDetailPosition, string itemCd)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            ItemCd = itemCd;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public string ItemCd { get; private set; }
    }
}
