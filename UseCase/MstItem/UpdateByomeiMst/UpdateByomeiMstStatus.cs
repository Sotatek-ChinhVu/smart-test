using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MstItem.UpdateByomeiMst
{
    public enum UpdateByomeiMstStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidDataUpdate = 4,
        Failed = 5
    }
}
