using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public class TokkiMstModel
    {
        public TokkiMstModel(string tokkiCd, string tokkiName)
        {
            TokkiCd = tokkiCd;
            TokkiName = tokkiName;
        }

        public string TokkiCd { get; private set; }

        public string TokkiName { get; private set; }

        public string DisplayTextMst
        {
            get { return TokkiCd + " " + TokkiName; } 
        }
    }
}
