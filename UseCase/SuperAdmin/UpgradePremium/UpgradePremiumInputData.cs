using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SuperAdmin.Login;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpgradePremiumInputData : IInputData<UpgradePremiumOutputData>
    {
        public int TenantId { get; private set; }
    }
}
