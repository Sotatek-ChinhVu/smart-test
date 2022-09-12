using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class HokenMstModel
    {
        public HokenMstModel(int futanKbn, int futanRate, int startDate, int endDate)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            StartDate = startDate;
            EndDate = endDate;
        }

        public HokenMstModel()
        {
            FutanKbn = 0;
            FutanRate = 0;
            StartDate = 0;
            EndDate = 0;
        }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}
