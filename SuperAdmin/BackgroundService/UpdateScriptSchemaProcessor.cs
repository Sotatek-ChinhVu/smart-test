using AWSSDK.Common;
using AWSSDK.Constants;
using Domain.SuperAdminModels.MigrationTenantHistory;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;

namespace SuperAdminAPI.BackgroundService
{
    public class UpdateScriptSchemaProcessor : BackgroundService
    {

        private readonly ITenantRepository _tenantRepository;
        private readonly IMigrationTenantHistoryRepository _migrationTenantHistoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IWebSocketService _webSocketService;
        public UpdateScriptSchemaProcessor(ITenantRepository tenantRepository, IMigrationTenantHistoryRepository migrationTenantHistoryRepository, INotificationRepository notificationRepository, IWebSocketService webSocketService)
        {
            _tenantRepository = tenantRepository;
            _migrationTenantHistoryRepository = migrationTenantHistoryRepository;
            _notificationRepository = notificationRepository;
            _webSocketService = webSocketService;
        }
        protected override Task Process()
        {
            CancellationTokenSource ct = new CancellationTokenSource();
            return Task.Run(() =>
            {
                try
                {

                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                    string folderPath = Path.Combine(templatePath, "Table");
                    if (Directory.Exists(folderPath))
                    {
                    }
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");
                    var migrations = sqlFiles.Select(x => Path.GetFileNameWithoutExtension(x)).OrderBy(x => x).ToList();

                    List<byte> listStatus = new List<byte> {
                    ConfigConstant.StatusTenantDictionary()["available"],
                    ConfigConstant.StatusTenantDictionary()["stoped"],
                };

                    var listTenant = _tenantRepository.GetTenantByStatus(listStatus);
                    var tenantIds = listTenant.Select(x => x.TenantId).ToList();
                    var dataMigration = _migrationTenantHistoryRepository.GetMigrationByTenants(tenantIds);

                    var combinedList = tenantIds.SelectMany(tenantId => migrations.Select(name => new MigrationTenantHistoryModel(tenantId, name)))
                                        .Where(x => !dataMigration.Any(b => x.TenantId == b.TenantId && x.MigrationId == b.MigrationId))
                                        .GroupBy(x => x.TenantId).Select(group => new
                                        {
                                            TenantId = group.Key,
                                            MigrationIds = group.Select(item => item.MigrationId).ToList()
                                        })
                                        .ToList();

                    foreach (var item in combinedList)
                    {
                        var tenant = listTenant.FirstOrDefault(x => x.TenantId == item.TenantId);
                        if (tenant != null)
                        {

                            _tenantRepository.UpdateStatusTenant(item.TenantId, ConfigConstant.StatusTenantDictionary()["update-schema"]);
                            try
                            {
                                foreach (var migrationId in item.MigrationIds)
                                {
                                    try
                                    {
                                        var pathFile = Directory.GetFiles(folderPath, $"{migrationId}.sql").FirstOrDefault();
                                        PostgresSqlAction.PostgreSqlExcuteFileScript(pathFile, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, tenant.UserConnect, tenant.PasswordConnect).Wait();
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenant.TenantId, migrationId);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"{migrationId}: " + ex.Message);
                                    }
                                }
                                _tenantRepository.UpdateStatusTenant(item.TenantId, tenant.Status);
                                var messenge = $"{tenant.EndSubDomain} データベーススキーマが更新されました。。";
                                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                                // Add info tenant for notification
                                notification.SetTenantId(tenant.TenantId);
                                notification.SetStatusTenant(tenant.StatusTenant);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            }
                            catch (Exception ex)
                            {
                                _tenantRepository.UpdateStatusTenant(item.TenantId, ConfigConstant.StatusTenantDictionary()["update-schema-failed"]);
                                //SendNotification
                                var messenge = $"{tenant.EndSubDomain} データベーススキーマの更新に失敗しました。エラー: {ex.Message}";
                                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                                // Add info tenant for notification
                                notification.SetTenantId(tenant.TenantId);
                                notification.SetStatusTenant(ConfigConstant.StatusTenantFailded);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            }
                        }

                    }
                }
                finally
                {
                    _tenantRepository.ReleaseResource();
                    _migrationTenantHistoryRepository.ReleaseResource();
                    _notificationRepository.ReleaseResource();
                    StopAsync(ct.Token).Wait();
                }
            }, ct.Token);
        }
    }
}
