using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreObjectS3Tenant
{
    public sealed class RestoreObjectS3TenantOutputData : IOutputData
    {
        public RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus status)
        {
            Status = status;
        }
        public RestoreObjectS3TenantStatus Status { get; private set; }
    }
}
