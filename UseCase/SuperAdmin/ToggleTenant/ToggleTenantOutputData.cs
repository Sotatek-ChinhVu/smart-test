using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.TerminateTenant;

namespace UseCase.SuperAdmin.StopedTenant
{
    public class ToggleTenantOutputData : IOutputData
    {
        public ToggleTenantOutputData(bool result, ToggleTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public ToggleTenantStatus Status { get; private set; }
    }
}
