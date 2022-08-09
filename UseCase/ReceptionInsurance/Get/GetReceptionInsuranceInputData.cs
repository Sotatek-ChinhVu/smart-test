using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.Reception.Get;

namespace UseCase.ReceptionInsurance.Get
{
    public class GetReceptionInsuranceInputData : IInputData<GetReceptionInsuranceOutputData>
    {
        public GetReceptionInsuranceInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }
        
        public int SinDate { get; private set; }

    }
}
