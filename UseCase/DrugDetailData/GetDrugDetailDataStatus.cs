using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.DrugDetailData
{
    public enum GetDrugDetailDataStatus : byte
    {
        Successed = 1,
        InvalidItemCd = 2,
        InvalidYJCode = 3,
        Failed = 4
    }
}
