using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.DrugDetail
{
    public enum GetDrugDetailStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidSindate = 3,
        InValidItemCd = 4
    }
}
