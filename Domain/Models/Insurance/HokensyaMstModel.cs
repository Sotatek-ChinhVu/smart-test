using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class HokensyaMstModel
    {
        public HokensyaMstModel(int isKigoNa)
        {
            IsKigoNa = isKigoNa;
        }
        
        public HokensyaMstModel()
        {
        }

        public int IsKigoNa { get; private set; }
    }
}
