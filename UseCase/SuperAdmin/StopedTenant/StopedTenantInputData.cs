using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.StopedTenant
{
    public class StopedTenantInputData : IInputData<StopedTenantOutputData>
    {
        public StopedTenantInputData(int tenantId)
        {
            TenantId = tenantId;
        }
        public int TenantId { get; private set; }
    }
}
