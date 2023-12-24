using Helper.Messaging;
using Microsoft.AspNetCore.Http;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateDataTenant
{
    public class UpdateDataTenantInputData : IInputData<UpdateDataTenantOutputData>
    {
        public UpdateDataTenantInputData(int tenantId, dynamic webSocketService, IFormFile fileUpdateData)
        {
            TenantId = tenantId;
            WebSocketService = webSocketService;
            FileUpdateData = fileUpdateData;
        }

        public int TenantId { get; private set; }

        public dynamic WebSocketService { get; private set; }

        public IFormFile FileUpdateData { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public IMessenger Messenger { get; private set; }
    }
}
