using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MstItem.SaveSetNameMnt
{
    public enum SaveSetNameMntStatus
    {
        Success,
        ListDataEmpty,
        InvalidHpId,
        InvalidSinDate,
        InvalidUserId,
        Faild
    }
}
