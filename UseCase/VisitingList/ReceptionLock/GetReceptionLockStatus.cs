using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.VisitingList.ReceptionLock
{
    public enum GetReceptionLockStatus : byte
    {
        Success = 1,
        InvalidRaiinNo = 2,
        NoData = 3
    }
}
