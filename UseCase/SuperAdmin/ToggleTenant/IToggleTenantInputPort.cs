using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.TerminateTenant;

namespace UseCase.SuperAdmin.StopedTenant
{
    public interface IToggleTenantInputPort : IInputPort<ToggleTenantInputData, ToggleTenantOutputData>
    {
    }
}
