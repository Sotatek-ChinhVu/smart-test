namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class OdrInfDetailItem
    {
        public OdrInfDetailItem(string itemCd, int sinKouiKbn)
        {
            ItemCd = itemCd;
            SinKouiKbn = sinKouiKbn;
        }

        public string ItemCd { get; private set; }
        public int SinKouiKbn { get; private set; }
    }
}
