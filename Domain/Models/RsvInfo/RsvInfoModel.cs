namespace Domain.Models.RsvInfo
{
    public class RsvInfoModel
    {
        public RsvInfoModel(int hpId, int rsvFrameId, int sinDate, int startTime, long raiinNo, long ptId, int rsvSbt, int tantoId, int kaId)
        {
            HpId = hpId;
            RsvFrameId = rsvFrameId;
            SinDate = sinDate;
            StartTime = startTime;
            RaiinNo = raiinNo;
            PtId = ptId;
            RsvSbt = rsvSbt;
            TantoId = tantoId;
            KaId = kaId;
        }

        public int HpId { get; private set; }
        public int RsvFrameId { get; private set; }
        public int SinDate { get; private set; }
        public int StartTime { get; private set; }
        public long RaiinNo { get; private set; }
        public long PtId { get; private set; }
        public int RsvSbt { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
    }
}
