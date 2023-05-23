namespace Domain.Models.FlowSheet
{
    public class HolidayModel
    {
        public int SinDate { get; private set; }

        public int HolidayKbn { get; private set; }

        public int KyusinKbn { get; private set; }

        public string HolidayName { get; private set; }

        public HolidayModel(int sinDate, int holidayKbn, int kyusinKbn, string holidayName)
        {
            SinDate = sinDate;
            HolidayKbn = holidayKbn;
            KyusinKbn = kyusinKbn;
            HolidayName = holidayName;
        }

        public HolidayModel(int sinDate, int holidayKbn, string holidayName)
        {
            SinDate = sinDate;
            HolidayKbn = holidayKbn;
            HolidayName = holidayName;
        }
    }
}
