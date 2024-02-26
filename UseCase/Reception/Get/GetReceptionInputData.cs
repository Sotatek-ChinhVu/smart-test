using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Get
{
    public class GetReceptionInputData : IInputData<GetReceptionOutputData>
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public bool Flag { get; private set; }

        public GetReceptionInputData(int hpId, long raiinNo, bool flag)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            Flag = flag;
        }
    }
}
