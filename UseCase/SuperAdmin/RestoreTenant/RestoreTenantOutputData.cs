using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.StopedTenant;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public class RestoreTenantOutputData :IOutputData
    {
        public RestoreTenantOutputData(bool result, RestoreTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public RestoreTenantStatus Status { get; private set; }
    }
}
