using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Constants;

public static class UketukeSbtMstConstant
{
    public enum ValidationStatus
    {
        Success,
        Failed,
        InvalidKbnId,
        InvalidKbnName,
        InvalidSortNo,
        InvalidIsDeleted,
        UketukeListExistedInputData,
        UketukeListInvalidNoExistedKbnId,
        InputNoData,
        Valid,
    }
}
