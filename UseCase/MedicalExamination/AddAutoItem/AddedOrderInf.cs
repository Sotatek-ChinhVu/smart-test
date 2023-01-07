namespace UseCase.MedicalExamination.AddAutoItem
{
    public class AddedOrderInf
    {
        public AddedOrderInf(int orderPosition, int orderDetailPosition, string itemCd, long id)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            ItemCd = itemCd;
            Id = id;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public string ItemCd { get; private set; }
        public long Id { get; private set; }
    }
}
