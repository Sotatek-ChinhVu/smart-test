using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.GetComboList
{
    public class GetInsuranceComboItemOuputData
    {
        public GetInsuranceComboItemOuputData(int hokenPid, string hokenName, bool isExpired, string displayRateOnly)
        {
            HokenPid = hokenPid;
            HokenName = hokenName;
            IsExpired = isExpired;
            DisplayRateOnly = displayRateOnly;
        }

        public int HokenPid { get; private set; }

        public string HokenName { get; private set; }

        public bool IsExpired { get; private set; }

        public string DisplayRateOnly { get; private set; }
    }
}
