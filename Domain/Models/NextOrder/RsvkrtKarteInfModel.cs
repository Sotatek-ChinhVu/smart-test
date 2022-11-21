namespace Domain.Models.NextOrder
{
    public class RsvkrtKarteInfModel
    {
        public RsvkrtKarteInfModel(int hpId, long ptId, int rsvDate, long rsvkrtNo, int karteKbn, long seqNo, string text, string richText, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            RsvDate = rsvDate;
            RsvkrtNo = rsvkrtNo;
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
            RsvDate = 0;
            RsvkrtNo = 0;
            KarteKbn = 0;
            SeqNo = 0;
            Text = string.Empty;
            RichText = string.Empty;
            IsDeleted = 0;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int RsvDate { get; private set; }

        public long RsvkrtNo { get; private set; }

        public int KarteKbn { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string RichText { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
