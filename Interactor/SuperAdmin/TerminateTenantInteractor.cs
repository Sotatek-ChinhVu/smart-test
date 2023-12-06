using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Interactor.Realtime;
using UseCase.SuperAdmin.TerminateTenant;

namespace Interactor.SuperAdmin
{
    public class TerminateTenantInteractor : ITerminateTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public TerminateTenantInteractor(
            ITenantRepository tenantRepository,
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }
        public TerminateTenantOutputData Handle(TerminateTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;
                if (inputData.TenantId <= 0)
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null || tenant.TenantId <= 0)
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
                _ = Task.Run( () =>
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
                            deleteRDSAction =  RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier).Result;
                        }

                        // Delete DNS
                        var deleteDNSAction =  Route53Action.DeleteTenantDomain(tenant.SubDomain).Result;

                        // Delete item cname in cloud front
                        var deleteItemCnameAction = CloudFrontAction.RemoveItemCnameAsync(tenant.SubDomain).Result;

                        //Delete folder S3
                        _awsSdkService.DeleteObjectsInFolderAsync(ConfigConstant.DestinationBucketName, tenant.EndSubDomain).Wait();

                        // Check action deleted  RDS, DNS, Cloud front
                        if (deleteRDSAction && deleteDNSAction && deleteItemCnameAction)
                        {
                            // Check finshed terminate
                            if (RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier).Result && !Route53Action.CheckSubdomainExistence(tenant.SubDomain).Result)
                            {
                                _tenantRepositoryRunTask.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminated"]);
                                // Notification  terminating success
                                var messenge = tenant.EndSubDomain + $"is teminate successfully. ";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
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
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
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
