namespace UseCase.NextOrder
{
    public class RsvKrtKarteInfItem
    {
        public RsvKrtKarteInfItem(long rsvkrtNo, int rsvDate, int karteKbn, int seqNo, string text, string richText, int isDeleted)
        {
            RsvkrtNo = rsvkrtNo;
            RsvDate = rsvDate;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            Text = text;
            RichText = richText;
            IsDeleted = isDeleted;
        }

        public long RsvkrtNo { get; private set; }

        public int RsvDate { get; private set; }

        public int KarteKbn { get; private set; }

        public int SeqNo { get; private set; }

        public string Text { get; private set; }

        public string RichText { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
