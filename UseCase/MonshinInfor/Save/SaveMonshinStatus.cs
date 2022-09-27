using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MonshinInfor.Save
{
    public enum SaveMonshinStatus
    {
        Success = 1,
        InputDataNull = 2,
        InvalidHpId = 3,
        InvalidPtId = 4,
        InvalidRaiinNo = 5,
        InvalidSinDate = 6,
        Failed = 7,
    }
}
