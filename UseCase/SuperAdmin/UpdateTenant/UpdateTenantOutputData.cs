using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.StopedTenant;

namespace UseCase.SuperAdmin.UpdateTenant
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
