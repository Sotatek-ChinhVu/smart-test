using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.ReceptionLock
{
    public class GetReceptionLockInputData : IInputData<GetReceptionLockOutputData>
    {
        public GetReceptionLockInputData(long raiinNo)
        {
            RaiinNo = raiinNo;
        }

        public long RaiinNo { get; private set; }
    }
}
