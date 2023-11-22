using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.TerminateTenant;

namespace Interactor.SuperAdmin
{
    public class TerminateTenantInteractor : ITerminateTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        public TerminateTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
        }
        public TerminateTenantOutputData Handle(TerminateTenantInputData inputData)
        {
            if (inputData.TenantId <= 0)
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
            }

            var tenant = _tenantRepository.Get(inputData.TenantId);
            if (tenant == null)
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
            }

            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating"]);
            var listTenantDb = RDSAction.GetListDatabase(tenant.RdsIdentifier).Result;

            // Check valid delete tennatDb
            if (listTenantDb == null || listTenantDb.Count() == 0 || !listTenantDb.Contains(tenant.Db))
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantDbDoesNotExistInRDS);
            }

            CancellationTokenSource cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                Console.WriteLine($"Start Terminate tenant: {tenant.RdsIdentifier}");
                bool isDeleteDb = false;

                // Connect RDS delete TenantDb
                if (listTenantDb.Count > 1)
                {
                    if (!_awsSdkService.DeleteTenantDb(tenant.EndPointDb, tenant.Db))
                    {
                        isDeleteDb = true;
                    }
                    else
                    {
                        _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating-failed"]);
                    }

                }

                // Deleted RDS
                else
                {
                    await RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier);
                    isDeleteDb = await RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier);
                    if (!isDeleteDb)
                    {
                        _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating-failed"]);
                        cts.Cancel();
                        return;
                    }
                }

                // Delete DNS
                // Delete Could font

                // Check Deleted Finished RDS, DNS, Could font
                if (!isDeleteDb)
                {
                    _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating-failed"]);
                    cts.Cancel();
                    return;
                }

                //Finished terminate
                _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminated"]);
                // Notification
                cts.Cancel();
                return;
            });
            return new TerminateTenantOutputData(true, TerminateTenantStatus.Successed);
        }
    }
}
