namespace Domain.Models.KarteInfs
{
    public class KarteInf
    {
        public KarteInf(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, int isDeleted, byte[] richText)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDeleted = isDeleted;
            RichText = richText;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int KarteKbn { get; private set; }
        public long SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public int IsDeleted { get; private set; }
        public byte[] RichText { get; private set; }
    }
}
