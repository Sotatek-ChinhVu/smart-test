using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.GroupInf.GetList
{
    public enum GetListGroupInfSatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InvalidPtId = 3
    }
}
