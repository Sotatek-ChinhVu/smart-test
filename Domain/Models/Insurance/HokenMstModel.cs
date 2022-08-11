using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public HokenMstModel(int futanKbn, int futanRate, string houbetsuNumber, int kaiLimitFutan, int monthLimitFutan, int dayLimitFutan)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            HoubetsuNumber = houbetsuNumber;
            KaiLimitFutan = kaiLimitFutan;
            MonthLimitFutan = monthLimitFutan;
            DayLimitFutan = dayLimitFutan;
        }

        public int FutanKbn { get; private set; }
        public int FutanRate { get; private set; }
        public string HoubetsuNumber { get; private set; }
        public int KaiLimitFutan { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public int DayLimitFutan { get; private set; }

    }
}
