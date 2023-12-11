using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.StopedTenant
{
    public class ToggleTenantInputData : IInputData<ToggleTenantOutputData>
    {
        public ToggleTenantInputData(int tenantId, dynamic webSocketService, int type)
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
