namespace Domain.Models.Medical
{
    public class SinKouiCountModel
    {
        public SinKouiCountModel(int hpId, long ptId, int sinYm, int sinDay, int sinDate, long raiinNo, int rpNo, int seqNo, int count)
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            SinDay = sinDay;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            SeqNo = seqNo;
            Count = count;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int SinDay { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int RpNo { get; private set; }

        public int SeqNo { get; private set; }

        public int Count { get; private set; }
    }
}
