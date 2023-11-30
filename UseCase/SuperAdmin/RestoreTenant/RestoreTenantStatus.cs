using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public enum RestoreTenantStatus
    {
        Successed = 1,
        InvalidTenantId = 2,
        Failed = 3,
        TenantDoesNotExist = 4,
        SnapshotNotAvailable = 5
    }
}
