using Amazon.RDS;
using Amazon.RDS.Model;
using Domain.SuperAdminModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {

        public UpgradePremiumInteractor()
        {
        }
        public UpgradePremiumOutputData Handle(UpgradePremiumInputData inputData)
        {
            // Get infor Tenant
            
            var dbInstanceIdentifier = "";

            throw new NotImplementedException();
        }
    }
}
