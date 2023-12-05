using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public class RestoreTenantInputData : IInputData<RestoreTenantOutputData>
    {
        public RestoreTenantInputData(int tenantId)
        {
            TenantId = tenantId;
        }
        public int TenantId { get; private set; }
    }
}
