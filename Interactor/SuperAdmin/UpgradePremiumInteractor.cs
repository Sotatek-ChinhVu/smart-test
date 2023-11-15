using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Interfaces;
using AWSSDK.Services;
using Domain.SuperAdminModels.Admin;
using Domain.SuperAdminModels.Tenant;
using Infrastructure.SuperAdminRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SuperAdmin.Login;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        public UpgradePremiumInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
        }
        public UpgradePremiumOutputData Handle(UpgradePremiumInputData inputData)
        {
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant.Type == 1)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }


                // Create Snapshot
                var rdsIdentifier =  _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier);

                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
