using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreObjectS3Tenant
{
    public interface IRestoreObjectS3TenantInputPort : IInputPort<RestoreObjectS3TenantInputData, RestoreObjectS3TenantOutputData>
    {
    }
}
