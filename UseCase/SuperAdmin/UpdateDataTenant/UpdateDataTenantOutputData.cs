using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateDataTenant
{
    public class UpdateDataTenantOutputData : IOutputData
    {
        public UpdateDataTenantOutputData(bool result, UpdateDataTenantStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public UpdateDataTenantStatus Status { get; private set; }
    }
}
