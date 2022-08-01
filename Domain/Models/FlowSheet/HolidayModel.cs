using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class HolidayModel
    {
        public int SinDate { get; private set; }
        public int HolidayKbn { get; private set; }
        public int KyusinKbn { get; private set; }
        public string HolidayName { get; private set; }
        public HolidayModel(HolidayMst holiday)
        {
            SinDate = holiday.SinDate;
            HolidayKbn = holiday.HolidayKbn;
            KyusinKbn = holiday.KyusinKbn;
            HolidayName = holiday.HolidayName;
        }
        public HolidayModel(int sinDate, int holidayKbn, int kyusinKbn, string holidayName)
        {
            SinDate = sinDate;
            HolidayKbn = holidayKbn;
            KyusinKbn = kyusinKbn;
            HolidayName = holidayName;
        }
    }
}
