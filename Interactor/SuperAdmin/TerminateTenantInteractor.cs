using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
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
        private readonly INotificationRepository _notificationRepository;
        public TerminateTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService, INotificationRepository notificationRepository)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
            _notificationRepository = notificationRepository;
        }
        public TerminateTenantOutputData Handle(TerminateTenantInputData inputData)
        {
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null)
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantDoesNotExist);
                }

                var listTenantDb = RDSAction.GetListDatabase(tenant.EndPointDb).Result;
                // Check valid delete tennatDb
                if (listTenantDb == null || listTenantDb.Count() == 0 || !listTenantDb.Contains(tenant.Db))
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantDbDoesNotExistInRDS);
                }

                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating"]);

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        Console.WriteLine($"Start Terminate tenant: {tenant.RdsIdentifier}");
                        bool deleteRDSAction = false;

                        // Connect RDS delete TenantDb
                        if (listTenantDb.Count > 1)
                        {
                            if (_awsSdkService.DeleteTenantDb(tenant.EndPointDb, tenant.Db))
                            {
                                deleteRDSAction = true;
                            }
                        }

                        // Deleted RDS
                        else
                        {
                            deleteRDSAction = await RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier);
                        }

                        // Delete DNS
                        var deleteDNSAction = await Route53Action.DeleteTenantDomain(tenant.SubDomain);

                        // Delete item cname in cloud front
                        var deleteItemCnameAction = await CloudFrontAction.RemoveItemCnameAsync(tenant.SubDomain);
                        // Check action deleted  RDS, DNS, Cloud front
                        if (deleteRDSAction && deleteDNSAction && deleteItemCnameAction)
                        {
                            // Check finshed terminate
                            if (await RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier) && !await Route53Action.CheckSubdomainExistence(tenant.SubDomain))
                            {
                                _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminated"]);
                                // Notification  terminating success
                                var messenge = tenant.EndSubDomain + $"is teminate successfully. ";
                                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                                await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                                cts.Cancel();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminate-failed"]);
                        // Notification  terminating failed
                        var messenge = $"{tenant.EndSubDomain} is teminate failed. Error: {ex.Message}.";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                });
                return new TerminateTenantOutputData(true, TerminateTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }
    }
}
