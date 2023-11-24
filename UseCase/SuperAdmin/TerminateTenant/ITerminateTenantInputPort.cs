using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.UpgradePremium;

namespace UseCase.SuperAdmin.TerminateTenant
{
    public interface ITerminateTenantInputPort : IInputPort<TerminateTenantInputData, TerminateTenantOutputData>
    {
    }
}
