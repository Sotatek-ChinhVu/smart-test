using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MstItem.GetDosageDrugList
{
    public enum GetDosageDrugListStatus: byte
    {
        Successed = 1,
        InputDataNull = 2,
        NoData = 3,
        Fail = 4
    }
}
