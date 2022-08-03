using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public class ByomeiMstAftercareModel
    {
        public ByomeiMstAftercareModel(string byomeiCd, string byomei)
        {
            ByomeiCd = byomeiCd;
            Byomei = byomei;
        }

        public string ByomeiCd { get; private set; }

        public string Byomei { get; private set; }

        public string ByomeiDisplay
        {
            get { return string.Format("{0} {1}", ByomeiCd, Byomei); }
        }
    }
}