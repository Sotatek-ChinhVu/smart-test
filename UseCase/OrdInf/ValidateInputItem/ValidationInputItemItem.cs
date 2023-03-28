namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemItem
    {
        public ValidationInputItemItem(int odrKouiKbn, int inoutKbn, int daysCnt, List<ValidationInputItemDetailItem> odrDetails, bool isAutoAddItem)
        {
            OdrKouiKbn = odrKouiKbn;
            DaysCnt = daysCnt;
            OdrDetails = odrDetails;
            InoutKbn = inoutKbn;
            IsAutoAddItem = isAutoAddItem;
        }
        public int OdrKouiKbn { get; private set; }
        public int InoutKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public bool IsAutoAddItem { get; private set; }
        public List<ValidationInputItemDetailItem> OdrDetails { get; private set; }
    }
}
