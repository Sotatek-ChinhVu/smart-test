using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientInfor.SearchEmptyId
{
    public enum SearchEmptyIdStatus
    {
        Success = 1,
        Failed = 2,
        NoData = 3,
        InvalidPageIndex = 4,
        InvalidPageSize = 5,
    }
}
