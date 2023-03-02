namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredItem
    {
        public CheckedExpiredItem(string itemCd, string itemName, int bunkatuKoui, string bunkatu)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            BunkatuKoui = bunkatuKoui;
            Bunkatu = bunkatu;
        }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }

        public int BunkatuKoui { get; private set; }

        public string Bunkatu { get; private set; }
    }
}
