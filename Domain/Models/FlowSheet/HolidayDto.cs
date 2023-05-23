namespace Domain.Models.FlowSheet
{
    public class HolidayDto
    {
        public HolidayDto(long seqNo, int sinDate, int holidayKbn, int kyusinKbn, string holidayName)
        {
            SeqNo = seqNo;
            SinDate = sinDate;
            HolidayKbn = holidayKbn;
            KyusinKbn = kyusinKbn;
            HolidayName = holidayName;
        }

        public long SeqNo { get; private set; }

        public int SinDate { get; private set; }

        public int HolidayKbn { get; private set; }

        public int KyusinKbn { get; private set; }

        public string HolidayName { get; private set; }
    }
}
