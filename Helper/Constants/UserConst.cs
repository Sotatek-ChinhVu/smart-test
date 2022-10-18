using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Constants
{
    public static class UserConst
    {
        public enum ValidationStatus
        {
            InvalidHpId,
            InvalidId,
            InvalidUserId,
            InvalidExistedUserId,
            InvalidJobCd,
            InvalidManagerKbn,
            InvalidKanaName,
            InvalidKaId,
            InvalidName,
            InvalidSname,
            InvalidLoginId,
            InvalidExistedLoginId,
            InvalidLoginPass,
            InvalidMayakuLicenseNo,
            InvalidStartDate,
            InvalidEndDate,
            InvalidSortNo,
            InvalidRenkeiCd1,
            InvalidRDrName,
            InvalidIsDeleted,
            UserListKaIdNoExist,
            UserListJobCdNoExist,
            UserListIdNoExist,
            Valid
        };
    }
}
