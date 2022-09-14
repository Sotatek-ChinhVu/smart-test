using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.KohiHokenMst.Get
{
    public enum GetKohiHokenMstStatus : byte
    {
        InvalidFutansyaNo = 4,
        InvalidSinDate = 3,
        InvalidHpId = 2,
        Successed = 1
    }
}
