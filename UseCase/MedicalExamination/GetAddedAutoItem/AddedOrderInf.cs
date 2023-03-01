namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class AddedOrderInf
    {
        public AddedOrderInf(int orderPosition, int orderDetailPosition, long id)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            Id = id;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public long Id { get; private set; }
    }
}
