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
        public KarteItemInputData()
        {
            HpId = 0;
            RaiinNo = 0;
            PtId = 0;
            SinDate = 0;
            Text = string.Empty;
            IsDeleted = 0;
            RichText = string.Empty;
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
