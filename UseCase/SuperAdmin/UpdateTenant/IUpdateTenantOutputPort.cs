using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.UpgradePremium;

namespace UseCase.SuperAdmin.UpdateTenant
{
    public interface IUpdateTenantOutputPort : IOutputPort<UpdateTenantOutputData>
    {
    }
}
