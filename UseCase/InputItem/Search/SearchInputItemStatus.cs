using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.InputItem.Search
{
    public enum SearchInputItemStatus: byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidKeyword = 3,
        InvalidSindate = 4,
        InvalidStartIndex = 5,
        InvalidPageCount = 6,
        InvalidKouiKbn = 7,

    }
}
