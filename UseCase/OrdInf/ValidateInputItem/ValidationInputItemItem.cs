namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemItem
    {
        public ValidationInputItemItem(int hpId, int sinDate, int odrKouiKbn, int inoutKbn, int daysCnt, List<ValidationInputItemDetailItem> odrDetails)
        {
            OdrKouiKbn = odrKouiKbn;
            DaysCnt = daysCnt;
            OdrDetails = odrDetails;
            HpId = hpId;
            SinDate = sinDate;
            InoutKbn = inoutKbn;
        }
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public int InoutKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public List<ValidationInputItemDetailItem> OdrDetails { get; private set; }
    }
}
