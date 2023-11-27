using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpgradePremiumOutputData : IOutputData
    {
        public UpgradePremiumOutputData(bool result, UpgradePremiumStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public UpgradePremiumStatus Status { get; private set; }
    }
}
