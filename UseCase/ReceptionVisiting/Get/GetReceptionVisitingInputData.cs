using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionVisiting.Get
{
    public class GetReceptionVisitingInputData : IInputData<GetReceptionVisitingOutputData>
    {
        public GetReceptionVisitingInputData(int hpId, long raiinNo)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
        }

        public int HpId { get; set; }
        public long RaiinNo { get; private set; }

    }
}
