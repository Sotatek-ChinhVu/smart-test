using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateTenant
{
    public class UpdateTenantInputData : IInputData<UpdateTenantOutputData>
    {
        public UpdateTenantInputData(int tenantId, dynamic webSocketService, int type)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
            File = type;
        }

        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }

        public int File { get; private set; }
    }
}
