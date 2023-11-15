using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpgradePremiumInputData : IInputData<UpgradePremiumOutputData>
    {
        public UpgradePremiumInputData(int tenantId)
        {
            TenantId = tenantId;
        }
        public int TenantId { get; private set; }
    }
}
