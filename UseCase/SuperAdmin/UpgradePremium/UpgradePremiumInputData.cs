using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpgradePremiumInputData : IInputData<UpgradePremiumOutputData>
    {
        public UpgradePremiumInputData(int tenantId, int size, string domain)
        {
            TenantId = tenantId;
            Size = size;
            Domain = domain;
        }
        public int TenantId { get; private set; }
        public string Domain { get; private set; }
        public int Size { get; private set; }
    }
}
