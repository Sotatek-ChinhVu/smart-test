namespace Domain.Models.RainListTag
{
    public class RaiinListTagModel
    {
        public RaiinListTagModel(int hpId, long ptId, int sinDate, long raiinNo, int seqNo, int tagNo, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            TagNo = tagNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int SeqNo { get; private set; }
        public int TagNo { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
