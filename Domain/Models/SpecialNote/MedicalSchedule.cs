using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SpecialNote
{
    public class MedicalSchedule
    {
         public string? Time { get; private set; }

        public string? Date { get; private set; }

        public void ConvertTime(string time)
        {
           if (time == null) return;
           time = time.Trim();
           if (time.Length == 3)
            {
                time = "0" + time;
            }
            string hour = time.Substring(0, 2);
            string minute = time.Substring(2, 2);
            this.Time = hour + ":" + minute;
        }

        public void ConvertDate(string date)
        {
            if(date == null) return;
            date = date.Trim();
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);
            string day = date.Substring(6, 2);
            this.Date = year + "/" + month + "/" + day;
        }
    }
}
