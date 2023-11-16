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
            throw new NotImplementedException();
        }

        public async Task<UpgradePremiumOutputData> HandleAsync(UpgradePremiumInputData inputData)
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

                // Check type in AWS

                // Update tenant status

                // Create Snapshot asynchronously
                var createSnapshotTask = _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier);

                // Create RSD  preminum
                //var createDBTask = _awsSdkService.CreateDBAsync();

                // Continue with other logic without waiting for the tasks to complete
                var snapshotIdentifierTask = createSnapshotTask.ContinueWith(task => task.Result);
                var dbInstanceIdentifierTask = createSnapshotTask.ContinueWith(task => task.Result);

                // Continue with other logic without waiting for the tasks to complete
                var snapshotIdentifier = await snapshotIdentifierTask;
                var dbInstanceIdentifier = await dbInstanceIdentifierTask;

                // Restore DB Instance from snapshot
                _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);

                // Continue with other logic...

                // Return a response immediately
                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
