using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.CalculationInf
{
    public class CalculationInfModel
    {
        public CalculationInfModel(int hpId, long ptId, int kbnNo, int edaNo, int kbnVal, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            KbnNo = kbnNo;
            EdaNo = edaNo;
            KbnVal = kbnVal;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int KbnNo { get; private set; }
        public int EdaNo { get; private set; }
        public int KbnVal { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
    }
}
