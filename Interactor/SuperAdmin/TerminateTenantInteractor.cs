using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public TerminateTenantInteractor(
            ITenantRepository tenantRepository,
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask,
            IConfiguration configuration,
             IMemoryCache memoryCache
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
            _configuration = configuration;
            _memoryCache = memoryCache;
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

                if (tenant.Status == ConfigConstant.StatusTenantDictionary()["terminating"])
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantIsTerminating);
                }

                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating"]);
                var messenge = tenant.EndSubDomain + $"がシャットダウン中です。";
                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, new NotificationModel(tenant.TenantId, ConfigConstant.StatusNotiSuccess, ConfigConstant.StatusSuttingDown, messenge));
                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(() =>
                {
                    try
                    {
                        Console.WriteLine($"Start Terminate tenant: {tenant.Hospital}");

                        if (tenant.StatusTenant == ConfigConstant.StatusTenantPending || tenant.StatusTenant == ConfigConstant.StatusTenantStopping) // terminate tenant  creating, updating, restoring
                        {
                            var tenantInfo = _memoryCache.Get<TenantCacheMemory>(tenant.SubDomain) ?? new TenantCacheMemory();
                            // Cancel task run creating, updating, restoring
                            tenantInfo.CancelToken.Cancel();
                        }

                        // Delete data master tenant
                        var deleteRDSAction = _awsSdkService.DeleteDataMasterTenant(tenant.EndPointDb, tenant.Db, tenant.UserConnect, tenant.PasswordConnect, tenant.TenantId, tenant.Db);
                        // Delete DNS
                        bool deleteDNSAction = false;
                        if (Route53Action.CheckSubdomainExistence(tenant.SubDomain).Result) // Check exist DNS
                        {
                            deleteDNSAction = Route53Action.DeleteTenantDomain(tenant.SubDomain).Result;
                        }
                        else
                        {
                            deleteDNSAction = true;
                        }

                        // Delete item cname in cloud front
                        var deleteItemCnameAction = CloudFrontAction.RemoveItemCnameAsync(tenant.SubDomain).Result;

                        //Delete folder S3
                        _awsSdkService.DeleteObjectsInFolderAsync(ConfigConstant.DestinationBucketName, tenant.EndSubDomain).Wait();

                        // Check action deleted  RDS, DNS, Cloud front
                        if (deleteRDSAction && deleteDNSAction && deleteItemCnameAction)
                        {
                            // Check finshed terminate
                            if (!Route53Action.CheckSubdomainExistence(tenant.SubDomain).Result)
                            {
                                _tenantRepositoryRunTask.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminated"]);
                                // Notification  terminating success
                                var messenge = tenant.EndSubDomain + $"の終了が完了しました。";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);

                                // Add info tenant for notification
                                notification.SetTenantId(tenant.TenantId);
                                notification.SetStatusTenant(ConfigConstant.StatusTenantTeminated);

                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                                // Delete cache memory
                                _memoryCache.Remove(tenant.SubDomain);

                                cts.Cancel();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminate-failed"]);
                        // Notification  terminating failed
                        var messenge = $"{tenant.EndSubDomain} の終了に失敗しました。エラー: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);

                        // Add info tenant for notification
                        notification.SetTenantId(tenant.TenantId);
                        notification.SetStatusTenant(ConfigConstant.StatusTenantFailded);

                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                        // Delete cache memory
                        _memoryCache.Remove(tenant.SubDomain);

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
