namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredItem
    {
        public CheckedExpiredItem(int sinKouiKbn, string itemCd, string itemName)
        {
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            ItemName = itemName;
        }

        public int SinKouiKbn { get; private set; }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }
    }
}
