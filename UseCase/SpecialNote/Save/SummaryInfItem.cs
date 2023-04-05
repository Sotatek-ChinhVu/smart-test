namespace UseCase.SpecialNote.Save
{
    public class SummaryInfItem
    {
        public SummaryInfItem(long id, int hpId, long ptId, long seqNo, string text, string rtext)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Text = text;
            Rtext = rtext;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string Rtext { get; private set; }
    }
}
