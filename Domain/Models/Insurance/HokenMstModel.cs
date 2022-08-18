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
        }

        public int FutanKbn { get; private set; }
        public int FutanRate { get; private set; }
    }
}
