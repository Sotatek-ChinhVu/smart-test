using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.GetList
{
    public enum GetInsuranceListStatus : byte
    {
        InvalidHpId = 3,
        InvalidPtId = 2,
        Successed = 1
    }
}