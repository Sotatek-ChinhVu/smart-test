using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateDataTenant
{
    public class UpdateDataTenantInputData : IInputData<UpdateDataTenantOutputData>
    {
        public UpdateDataTenantInputData(int tenantId, dynamic webSocketService, IFormFile fileUpdate)
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
