namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class AddedOrderDetail
    {
        public AddedOrderDetail(string itemCd, string itemName, long id)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            Id = id;
        }

        public string ItemCd { get; private set; }
        public string ItemName { get; private set; }
        public long Id { get; private set; }
    }
}
