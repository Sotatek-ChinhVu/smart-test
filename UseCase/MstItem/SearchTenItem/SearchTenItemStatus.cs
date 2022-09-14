using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MstItem.SearchTenItem
{
    public enum SearchTenItemStatus: byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidKeyword = 3,
        InvalidSindate = 4,
        InvalidPageIndex = 5,
        InvalidPageCount = 6,
        InvalidKouiKbn = 7,
        InvalidPointFrom = 8,
        InvalidPointTo = 9,

    }
}
