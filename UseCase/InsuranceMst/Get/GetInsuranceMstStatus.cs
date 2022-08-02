using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.InsuranceMst.Get
{
    public enum GetInsuranceMstStatus : byte
    {
        InvalidHokenId = 5,
        InvalidSinDate = 4,
        InvalidHpId = 3,
        InvalidPtId = 2,
        Successed = 1
    }
}
