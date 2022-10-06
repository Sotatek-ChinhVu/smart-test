namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemItem
    {
        public ValidationInputItemItem(int odrKouiKbn, int tosekiKbn, int daysCnt, List<ValidationInputItemDetailItem> odrDetails)
        {
            OdrKouiKbn = odrKouiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            OdrDetails = odrDetails;
        }
        public int OdrKouiKbn { get; private set; }
        public int InoutKbn { get; private set; }
        public int TosekiKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public List<ValidationInputItemDetailItem> OdrDetails { get; private set; }
    }
}
