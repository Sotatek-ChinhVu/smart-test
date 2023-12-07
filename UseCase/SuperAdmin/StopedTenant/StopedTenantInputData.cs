using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.StopedTenant
{
    public class StopedTenantInputData : IInputData<StopedTenantOutputData>
    {
        public StopedTenantInputData(int tenantId, dynamic webSocketService)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
        }

        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }
    }
}
