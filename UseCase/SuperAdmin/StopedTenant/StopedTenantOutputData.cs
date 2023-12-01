using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.TerminateTenant;

namespace UseCase.SuperAdmin.StopedTenant
{
    public class StopedTenantOutputData : IOutputData
    {
        public StopedTenantOutputData(bool result, StopedTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public StopedTenantStatus Status { get; private set; }
    }
}
