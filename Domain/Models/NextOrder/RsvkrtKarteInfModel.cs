namespace Domain.Models.NextOrder
{
    public class RsvkrtKarteInfModel
    {
        public RsvkrtKarteInfModel(int hpId, long ptId, int sinDate, long raiinNo, int karteKbn, long seqNo, string text, string richText, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            Text = text;
            RichText = richText;
            IsDeleted = isDeleted;
        }

        public RsvkrtKarteInfModel()
        {
            HpId = 0;
            PtId = 0;
            SinDate = 0;
            RaiinNo = 0;
            KarteKbn = 0;
            SeqNo = 0;
            Text = string.Empty;
            RichText = string.Empty;
            IsDeleted = 0;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int KarteKbn { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string RichText { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
