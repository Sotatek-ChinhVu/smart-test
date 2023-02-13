namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class OdrInfItem
    {
        public OdrInfItem(int inOutKbn, int odrKouiKbn, List<OdrInfDetailItem> odrInfDetailItems)
        {
            InOutKbn = inOutKbn;
            OdrKouiKbn = odrKouiKbn;
            OdrInfDetailItems = odrInfDetailItems;
        }

        public int InOutKbn { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public List<OdrInfDetailItem> OdrInfDetailItems { get; private set; }
    }
}
