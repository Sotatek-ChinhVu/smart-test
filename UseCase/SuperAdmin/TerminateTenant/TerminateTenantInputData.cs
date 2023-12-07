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
        public TerminateTenantInputData(int tenantId, dynamic webSocketService, int type)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
            Type = type;
        }

        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }

        public int Type { get; private set; }
    }
}
