using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.GetComboList
{
    public class GetInsuranceComboItemOuputData
    {
        public GetInsuranceComboItemOuputData(int hokenPid, string hokenName, bool isExpired)
        {
            HokenPid = hokenPid;
            HokenName = hokenName;
            IsExpired = isExpired;
        }

        public int HokenPid { get; private set; }

        public string HokenName { get; private set; }

        public bool IsExpired { get; private set; }
    }
}
