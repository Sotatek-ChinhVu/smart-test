using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public interface IRestoreTenantInputPort : IInputPort<RestoreTenantInputData, RestoreTenantOutputData>
    {
    }
}
