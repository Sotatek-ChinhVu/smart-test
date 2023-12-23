using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpdateTenantOutputData : IOutputData
    {
        public UpdateTenantOutputData(bool result, UpdateTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public UpdateTenantStatus Status { get; private set; }
    }
}
