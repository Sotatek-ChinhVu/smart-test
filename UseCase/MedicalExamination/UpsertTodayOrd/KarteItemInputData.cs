namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class KarteItemInputData
    {
        public KarteItemInputData(int hpId, long raiinNo, long ptId, int sinDate, string text, int isDeleted, string richText)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDeleted = isDeleted;
            RichText = richText;
        }
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public int IsDeleted { get; private set; }
        public string RichText { get; private set; }
    }
}
