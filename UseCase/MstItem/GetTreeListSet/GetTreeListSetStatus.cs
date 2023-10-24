using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MstItem.GetTreeListSet
{
    public enum GetTreeListSetStatus
    {

        Successed = 1,
        InvalidHpId = 2,
        InvalidSinDate = 3,
        InvalidKouiKbn = 4,
        DataNotFound = 5
    }
}
