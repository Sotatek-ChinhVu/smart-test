using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SearchHokensyaMst.Get
{
    public enum SearchHokensyaMstStatus : byte
    {
        InvalidKeyword = 6,
        InvalidSinDate = 5,
        InvalidPageCount = 4,
        InvalidPageIndex = 3,
        InvalidHpId = 2,
        Successed = 1
    }
}
