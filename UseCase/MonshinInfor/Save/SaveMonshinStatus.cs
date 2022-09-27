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
        InputDataDoesNotExists = 3,
        Failed = 4,
    }
}
