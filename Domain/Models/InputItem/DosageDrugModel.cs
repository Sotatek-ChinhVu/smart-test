using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InputItem
{
    public class DosageDrugModel
    {
        public DosageDrugModel(string yjCd, string doeiCd, string drugKbn, string kikakiUnit, string yakkaiUnit, decimal rikikaRate, string rikikaUnit, string youkaiekiCd)
        {
            YjCd = yjCd;
            DoeiCd = doeiCd;
            DrugKbn = drugKbn;
            KikakiUnit = kikakiUnit;
            YakkaiUnit = yakkaiUnit;
            RikikaRate = rikikaRate;
            RikikaUnit = rikikaUnit;
            YoukaiekiCd = youkaiekiCd;
        }

        public string YjCd { get; private set; }
        public string DoeiCd { get; private set; }
        public string DrugKbn { get; private set; }
        public string KikakiUnit { get; private set; }
        public string YakkaiUnit { get; private set; }
        public decimal RikikaRate { get; private set; }
        public string RikikaUnit { get; private set; }
        public string YoukaiekiCd { get; private set; }
    }
}
