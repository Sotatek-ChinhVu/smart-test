using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.CalculationInf
{
    public class CalculationInfInputData : IInputData<CalculationInfOutputData>
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }

        public CalculationInfInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }
    }
}
