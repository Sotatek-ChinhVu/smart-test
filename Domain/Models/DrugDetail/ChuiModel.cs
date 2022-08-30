using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class ChuiModel
    {
        public ChuiModel(int propertyCd, string property, string precautionCmt)
        {
            PropertyCd = propertyCd;
            Property = property;
            PrecautionCmt = precautionCmt;
        }

        public ChuiModel()
        {
            PropertyCd = 0;
            Property = "";
            PrecautionCmt = "";
        }

        public int PropertyCd { get; private set; }
        public string Property { get; private set; }
        public string PrecautionCmt { get; private set; }
    }
}
