namespace Domain.Models.RsvInf
{
    public class RsvInfModel
    {
        public RsvInfModel(int hpId, int rsvFrameId, int sinDate, int startTime, long raiinNo, long ptId, int rsvSbt, int tantoId, int kaId, string rsvFrameName, string rsvGrpName)
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
            RsvFrameName = rsvFrameName;
            RsvGrpName = rsvGrpName;
            UketukeTime = string.Empty;
            KaSName = string.Empty;
            YoyakuTime = string.Empty;
            TantoName = string.Empty;
            RaiinCmt = string.Empty;
        }

        public RsvInfModel(int sinDate, string uketukeTime, string kaSName, string yoyakuTime, string tantoName, string raiinCmt)
        {
            SinDate = sinDate;
            UketukeTime = uketukeTime;
            RsvFrameName = string.Empty;
            RsvGrpName = string.Empty;
            KaSName = kaSName;
            YoyakuTime = yoyakuTime;
            TantoName = tantoName;
            RaiinCmt = raiinCmt;
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

        public string RsvFrameName { get; private set; }

        public string RsvGrpName { get; private set; }

        public string UketukeTime { get; private set; }

        public string KaSName { get; private set; }

        public string YoyakuTime { get; private set; }

        public string TantoName { get; private set; }

        public string RaiinCmt { get; private set; }
    }
}
