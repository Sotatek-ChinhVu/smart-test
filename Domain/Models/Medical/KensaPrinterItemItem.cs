namespace Domain.Models.Medical
{
    public class KensaPrinterItemItem
    {
        public KensaPrinterItemItem(long logId, DateTime logDate, int hpId, int userId, string eventCd, long ptId, int sinDate, long raiinNo, string machine, string hosuke)
        {
            LogId = logId;
            LogDate = logDate;
            HpId = hpId;
            UserId = userId;
            EventCd = eventCd;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            Machine = machine;
            Hosuke = hosuke;
        }

        public long LogId { get; private set; }

        public DateTime LogDate { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string EventCd { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public string Machine { get; private set; }

        public string Hosuke { get; private set; }
    }
}
