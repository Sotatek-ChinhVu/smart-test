using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.TerminateTenant 
{
    public class TerminateTenantInputData : IInputData<TerminateTenantOutputData>
    {
        public TerminateTenantInputData(int tenantId)
        {
            TenantId = tenantId;
        }
        public int TenantId { get; private set; }
    }
}
