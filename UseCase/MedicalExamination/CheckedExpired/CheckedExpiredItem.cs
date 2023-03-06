namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredItem
    {
        public CheckedExpiredItem(string itemCd, string itemName)
        {
            ItemCd = itemCd;
            ItemName = itemName;
        }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }
    }
}
