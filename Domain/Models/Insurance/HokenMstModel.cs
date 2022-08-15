namespace Domain.Models.Insurance
{
    public class HokenMstModel
    {
        public HokenMstModel(int futanKbn, int futanRate)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            HoubetsuNumber = String.Empty;
            KaiLimitFutan = 0;
            MonthLimitFutan = 0;
            DayLimitFutan = 0;
            DayLimitCount = 0;
            MonthLimitCount = 0;
        }

        public HokenMstModel(int futanKbn, int futanRate, string houbetsuNumber, int kaiLimitFutan, int monthLimitFutan, int dayLimitFutan, int dayLimitCount, int monthLimitCount)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            HoubetsuNumber = houbetsuNumber;
            KaiLimitFutan = kaiLimitFutan;
            MonthLimitFutan = monthLimitFutan;
            DayLimitFutan = dayLimitFutan;
            DayLimitCount = dayLimitCount;
            MonthLimitCount = monthLimitCount;
        }

        public int FutanKbn { get; private set; }
        public int FutanRate { get; private set; }
        public string HoubetsuNumber { get; private set; }
        public int KaiLimitFutan { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public int DayLimitFutan { get; private set; }
        public int DayLimitCount { get; private set; }
        public int MonthLimitCount { get; private set; }


    }
}
