using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.RevokeInsertPermission;

namespace UseCase.SuperAdmin.RestoreTenant
{
    public interface IRestoreTenantOutputPort : IOutputPort<RestoreTenantOutputData>
    {
    }
}
