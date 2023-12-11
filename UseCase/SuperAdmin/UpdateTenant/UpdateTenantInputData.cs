using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateTenant
{
    public class UpdateTenantInputData : IInputData<UpdateTenantOutputData>
    {
        public UpdateTenantInputData(int tenantId, dynamic webSocketService, IFormFile fileUpdate)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
            FileUpdate = fileUpdate;
        }

        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }

        public IFormFile FileUpdate { get; private set; }
    }
}
