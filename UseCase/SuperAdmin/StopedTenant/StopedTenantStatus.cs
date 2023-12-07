using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SuperAdmin.StopedTenant
{
    public enum StopedTenantStatus : byte
    {
        Successed = 1,
        InvalidTenantId = 2,
        Failed = 3,
        TenantDoesNotExist = 4,
        TenantNotAvailable = 5,
        TenantNotStoped = 6,
    }
}
