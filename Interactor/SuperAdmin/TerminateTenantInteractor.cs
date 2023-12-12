using AWSSDK.Common;
using AWSSDK.Constants;
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

                string pathFileDumpTerminate = _configuration["PathFileDumpTerminate"] ?? string.Empty;

                if (string.IsNullOrEmpty(pathFileDumpTerminate))
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.PathFileDumpRestoreNotAvailable);
                }

                if (inputData.TenantId <= 0)
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantDoesNotExist);
                }

                //var cancellationTokenSource = _memoryCache.Get<CancellationTokenSource>(tenant.SubDomain);
                //cancellationTokenSource.Cancel();
                var a = RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier, true).Result;
                return new TerminateTenantOutputData(false, TerminateTenantStatus.PathFileDumpRestoreNotAvailable);

                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating"]);

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(() =>
                {
                    try
                    {
                        var cancellationTokenSource = _memoryCache.Get<CancellationTokenSource>(tenant.SubDomain);
                        cancellationTokenSource.Cancel();

                        var listTenantDb = RDSAction.GetListDatabase(tenant.EndPointDb, tenant.UserConnect, tenant.PasswordConnect).Result;
                        Console.WriteLine($"Start Terminate tenant: {tenant.RdsIdentifier}");

                        // Backup tenant
                        if (inputData.Type == 1) // sort terminate
                        {
                            // Create folder backup S3
                            var backupFolderName = @$"bk-{tenant.EndSubDomain}";
                            _awsSdkService.CreateFolderBackupAsync(ConfigConstant.DestinationBucketName, tenant.EndSubDomain, ConfigConstant.RestoreBucketName, backupFolderName).Wait();

                            // Dump DB backup
                            var pathFileDump = @$"{pathFileDumpTerminate}{tenant.Db}.sql"; // path save file sql dump
                            PostgresSqlAction.PostgreSqlDump(pathFileDump, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, tenant.UserConnect, tenant.PasswordConnect).Wait();

                            // check valid file sql dump
                            if (!System.IO.File.Exists(pathFileDump))
                            {
                                throw new Exception("File sql dump doesn't exist");
                            }

                            long length = new System.IO.FileInfo(pathFileDump).Length;
                            if (length <= 0)
                            {
                                throw new Exception("Invalid file sql dump");
                            }

                            // Upload file sql dump to folder backup S3
                            _awsSdkService.UploadFileAsync(ConfigConstant.RestoreBucketName, $@"{backupFolderName}/{tenant.Db}", pathFileDump).Wait();
                        }

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
                            deleteRDSAction = RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier).Result;
                        }

                        // Delete DNS
                        var deleteDNSAction = Route53Action.DeleteTenantDomain(tenant.SubDomain).Result;

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
                        _tenantRepositoryRunTask.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminate-failed"]);
                        // Notification  terminating failed
                        var messenge = $"{tenant.EndSubDomain} is teminate failed. Error: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
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
