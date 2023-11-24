using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.UpgradePremium;

namespace UseCase.SuperAdmin.TerminateTenant
{
    public class TerminateTenantOutputData : IOutputData
    {
        public TerminateTenantOutputData(bool result, TerminateTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public TerminateTenantStatus Status { get; private set; }
    }
}
