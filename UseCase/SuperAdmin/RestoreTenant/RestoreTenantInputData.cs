using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public class RestoreTenantInputData : IInputData<RestoreTenantOutputData>
    {
        public RestoreTenantInputData(int tenantId, dynamic webSocketService)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
        }
        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }
    }
}
