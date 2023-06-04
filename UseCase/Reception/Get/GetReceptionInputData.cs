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
        public long RaiinNo { get; private set; }
        public bool Flag { get; private set; }

        public GetReceptionInputData(long raiinNo, bool flag)
        {
            RaiinNo = raiinNo;
            Flag = flag;
        }
    }
}
