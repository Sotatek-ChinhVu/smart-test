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
            InvalidJobCd,
            InvalidManagerKbn,
            InvalidKanaName,
            InvalidKaId,
            InvalidName,
            InvalidSname,
            InvalidLoginId,
            InvalidLoginPass,
            InvalidMayakuLicenseNo,
            InvalidStartDate,
            InvalidEndDate,
            InvalidSortNo,
            InvalidRenkeiCd1,
            InvalidRDrName,
            InvalidIsDeleted,
            Valid
        };
    }
}
