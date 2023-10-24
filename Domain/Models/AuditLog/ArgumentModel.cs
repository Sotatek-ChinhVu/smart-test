namespace Domain.Models.AuditLog
{
    public class ArgumentModel
    {
        public ArgumentModel(string eventCd, long ptId, int sinDate, long raiinNo, int misyu, int nyukinDate, int nyukin, int nyukinSortNo, string hosoku)
        {
            EventCd = eventCd;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            Misyu = misyu;
            NyukinDate = nyukinDate;
            Nyukin = nyukin;
            NyukinSortNo = nyukinSortNo;
            Hosoku = hosoku;
        }

        public string EventCd { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int Misyu { get; private set; }
        public int NyukinDate { get; private set; }
        public int Nyukin { get; private set; }
        public int NyukinSortNo { get; private set; }
        public string Hosoku { get; private set; }
    }
}
