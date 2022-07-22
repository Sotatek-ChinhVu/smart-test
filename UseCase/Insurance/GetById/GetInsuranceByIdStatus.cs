using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.GetById
{
    public enum GetInsuranceByIdStatus : byte
    {
        InvalidHokenPid = 5,
        InvalidSinDate = 4,
        InvalidHpId = 3,
        InvalidPtId = 2,
        Successed = 1
    }
}
