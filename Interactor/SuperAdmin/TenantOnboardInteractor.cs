using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.MigrationTenantHistory;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using UseCase.SuperAdmin.TenantOnboard;

namespace Interactor.SuperAdmin
{
    public class TenantOnboardInteractor : ITenantOnboardInputPort
    {
        private readonly string _userNameDefault = ConfigConstant.PgUserDefault;
        private readonly string _passwordDefault = ConfigConstant.PgPasswordDefault;
        private readonly int _portDefault = ConfigConstant.PgPortDefault;
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenant2Repository;
        private readonly IMigrationTenantHistoryRepository _migrationTenantHistoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private IWebSocketService _webSocketService;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        public TenantOnboardInteractor(
            IAwsSdkService awsSdkService,
            ITenantRepository tenantRepository,
            ITenantRepository tenant2Repository,
            IMigrationTenantHistoryRepository migrationTenantHistoryRepository,
            INotificationRepository notificationRepository,
            IWebSocketService webSocketService,
            IMemoryCache memoryCache,
            IConfiguration configuration
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _migrationTenantHistoryRepository = migrationTenantHistoryRepository;
            _notificationRepository = notificationRepository;
            _tenant2Repository = tenant2Repository;
            _webSocketService = webSocketService;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }
        public TenantOnboardOutputData Handle(TenantOnboardInputData inputData)
        {
            try
            {
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (string.IsNullOrEmpty(inputData.Hospital) || string.IsNullOrEmpty(inputData.SubDomain) || string.IsNullOrEmpty(inputData.Password) || inputData.AdminId <= 0 || inputData.TenantId < 0)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidRequest);
                }
                bool creatTenant = true;
                if (inputData.TenantId > 0)
                {
                    creatTenant = false;
                    var statusTenantFaild = ConfigConstant.StatusTenantDictionary()["failed"];
                    var tenantFaild = _tenantRepository.GetByStatus(inputData.TenantId, statusTenantFaild);
                    if (tenantFaild.TenantId == 0)
                    {
                        return new TenantOnboardOutputData(new(), TenantOnboardStatus.TenantOnboardFailed);
                    }
                    var teminatedTenant = TeminatedTenant(inputData.TenantId);
                    if (!teminatedTenant)
                    {
                        return new TenantOnboardOutputData(new(), TenantOnboardStatus.TenantOnboardFailed);
                    }
                }
                var checkValidSubDomain = CommonConstants.IsSubdomainValid(inputData.SubDomain);
                var isExistHospital = _tenantRepository.CheckExistsHospital(inputData.Hospital);
                var checkSubDomainDB = _tenantRepository.CheckExistsSubDomain(inputData.SubDomain);
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(inputData.SubDomain).Result;
                if (creatTenant && isExistHospital)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.HopitalExists);
                }
                else if (!checkValidSubDomain)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSubDomain);
                }
                else if (creatTenant && (checkSubDomain || checkSubDomainDB))
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.SubDomainExists);
                }
#if DEBUG
                var dbName = "smart_karte_new";
#else
                var dbName = "smartkarte_new";
#endif
                var tenantUrl = $"{inputData.SubDomain}.{ConfigConstant.Domain}";
                var rdsIdentifier = "develop-smartkarte-postgres";
                var tenantModel = new TenantModel(inputData.TenantId, inputData.Hospital, 0, inputData.AdminId, inputData.Password, inputData.SubDomain.ToLower(), dbName, string.Empty, tenantUrl, 0, rdsIdentifier);
                var tenantOnboard = TenantOnboardAsync(tenantModel).Result;
                var message = string.Empty;
                if (tenantOnboard.TryGetValue("Error", out string? errorValue))
                {
                    Console.WriteLine($"Exception: {errorValue}");
                    return new TenantOnboardOutputData(new TenantOnboardItem(errorValue), TenantOnboardStatus.TenantOnboardFailed);
                }
                if (tenantOnboard.TryGetValue("message", out string? messageValue))
                {
                    message = messageValue;
                }
                var data = new TenantOnboardItem(message);

                return new TenantOnboardOutputData(data, TenantOnboardStatus.TenantOnboardSuccessed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        #region private Function        

        private async Task<Dictionary<string, string>> TenantOnboardAsync(TenantModel model)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            // Set tenant info to cache memory
            _memoryCache.Set(model.SubDomain, new TenantCacheMemory(cancellationTokenSource, string.Empty));

            int id = model.TenantId;
            string tenantUrl = model.EndSubDomain;
            try
            {
                ct.ThrowIfCancellationRequested();
                if (!ct.IsCancellationRequested) // Check task run is not canceled
                {
                    await CloudFrontAction.UpdateNewTenantAsync(model.SubDomain);
                    await Route53Action.CreateTenantDomain(model.SubDomain);
                }
                if (model.SubDomain.Length > 0 && !ct.IsCancellationRequested)
                {
                    id = _tenantRepository.CreateTenant(model);
                    if (!ct.IsCancellationRequested)
                    {
                        _ = Task.Run(() =>
                        {
                            AddData(id, tenantUrl, model, ct);
                        });
                    }
                }

                // Return message for Super Admin
                Dictionary<string, string> result = new Dictionary<string, string>
                {
                    { "message", "医療機関が作成されました。全てのリソースが得られるまで 45 分がかかります。" }
                };

                return result;
            }
            catch (Exception ex)
            {
                if (!ct.IsCancellationRequested) // Check task run is not canceled
                {
                    var message = $"新しい医療機関 {model.SubDomain} の作成に失敗しました: {ex.Message}。";
                    var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                    var statusTenantFaild = ConfigConstant.StatusTenantDictionary()["failed"];
                    var updateStatus = _tenantRepository.UpdateInfTenantStatus(id, statusTenantFaild);
                    // Add info tenant for notification
                    saveDBNotify.SetTenantId(id);
                    saveDBNotify.SetStatusTenant(ConfigConstant.StatusTenantFailded);

                    await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
                }

                // Delete cache memory
                _memoryCache.Remove(model.SubDomain);

                return new Dictionary<string, string> { { "Error", ex.Message } };
            }
        }

        private void AddData(int tenantId, string tenantUrl, TenantModel model, CancellationToken ct)
        {
            try
            {
                string host = ConfigConstant.EndPointSmartKarte;
                var updateStatus = _tenant2Repository.UpdateInfTenant(tenantId, 2, tenantUrl, host, model.RdsIdentifier);
                if (!string.IsNullOrEmpty(host))
                {
                    CreateDatas(host, model.Db, tenantId, model);
                    // create folder S3
                    _awsSdkService.CreateFolderAsync(ConfigConstant.DestinationBucketName, tenantUrl).Wait();

                    if (!ct.IsCancellationRequested)
                    {
                        var message = $"新しい医療機関 {tenantUrl} が作成されました。";
                        var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, message);

                        // Add info tenant for notification
                        saveDBNotify.SetTenantId(tenantId);
                        saveDBNotify.SetStatusTenant(ConfigConstant.StatusTenantRunning);

                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
                    }
                    _tenant2Repository.UpdateStatusTenant(tenantId, 1);
                    // Delete cache memory
                    _memoryCache.Remove(model.SubDomain);
                }
            }
            catch (Exception ex)
            {
                if (!ct.IsCancellationRequested)
                {
                    var message = $"新しい医療機関 {tenantUrl} の作成に失敗しました: {ex.Message}。";
                    var saveDBNotify = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                    var statusTenantFaild = ConfigConstant.StatusTenantDictionary()["failed"];
                    var updateStatus = _tenant2Repository.UpdateInfTenantStatus(tenantId, statusTenantFaild);
                    // Add info tenant for notification
                    saveDBNotify.SetTenantId(tenantId);
                    saveDBNotify.SetStatusTenant(ConfigConstant.StatusTenantFailded);
                    _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, saveDBNotify);
                }

                // Delete cache memory
                _memoryCache.Remove(model.SubDomain);
            }
            finally
            {
                _tenant2Repository.ReleaseResource();
                _migrationTenantHistoryRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        private void CreateDatas(string host, string dbName, int tenantId, TenantModel model)
        {
            try
            {
                string password = _passwordDefault;
#if DEBUG
                host = "10.2.15.78";
                password = "Emr!23";
#endif

                AddPartitions(host, dbName, _userNameDefault, password, tenantId);
                _CreatePartitionsAuditLog(tenantId, _userNameDefault, password, _portDefault);
                var connectionString = $"Host={host};Database={dbName};Username={_userNameDefault};Password={password};Port={_portDefault}";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        byte[] salt = GenerateSalt();
                        string hashPassword = CreateHash(Encoding.UTF8.GetBytes(model.Password ?? string.Empty), salt);
                        var sqlInsertUser = QueryConstant.SqlUser;
                        var sqlInsertUserPermission = QueryConstant.SqlUserPermission;
                        command.CommandText = sqlInsertUser + sqlInsertUserPermission;
                        command.Parameters.AddWithValue("hashPassword", hashPassword);
                        command.Parameters.AddWithValue("salt", Convert.ToHexString(salt));
                        command.Parameters.AddWithValue("hpId", tenantId);
                        command.Parameters.AddWithValue("adminId", model.AdminId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private void AddPartitions(string host, string tennantDB, string username, string password, int hpId)
        {
            try
            {
                // Connection string format for SQL Server
                string connectionString = $"Host={host}; Database ={tennantDB}; Port={ConfigConstant.PgPortDefault};Username={username};Password={password};";

                string FormartNameZTable(string tableName)
                {
                    int indexOfZ = tableName.IndexOf('z');
                    string modifiedString = tableName.Remove(indexOfZ, 2);
                    return modifiedString;
                }

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    // Delete database
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "BEGIN;\n";
                        foreach (string table in ConfigConstant.listTableMaster)
                        {
                            // Add partition
                            if (table.StartsWith("z")) // Add partition with z_table
                            {
                                command.CommandText += $"CREATE TABLE  z_p_{FormartNameZTable(table)}_{hpId} PARTITION OF public.{table} FOR VALUES IN ({hpId});\n";
                            }
                            else
                            {
                                command.CommandText += $"CREATE TABLE p_{table}_{hpId} PARTITION OF public.{table} FOR VALUES IN ({hpId});\n";
                            }
                        }
                        command.CommandText += "COMMIT;";
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Add partitions successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: add partitions. {ex.Message}");
                throw new Exception($"Error: add partitions. {ex.Message}");
            }
        }

        private void _CreatePartitionsAuditLog(int tenantId, string userName, string password, int port)
        {
            try
            {
#if DEBUG
                var host = "10.2.15.78";
                var dbName = "smartkartelogging_stagging";
#else
var host = "develop-smartkarte-logging.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com";
                var dbName = "smartkartelogging";
#endif
                var connectionString = $"Host={host};Database={dbName};Username={userName};Password={password};Port={port}";
                var addParttion = $"CREATE TABLE IF NOT EXISTS PARTITION_{tenantId} PARTITION OF public.\"AuditLogs\" FOR VALUES IN ({tenantId});";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'AuditLogs')";
                        var tableExists = command.ExecuteScalar();
                        string createCommandText = string.Empty;
                        if (tableExists != null && (bool)tableExists)
                        {
                            createCommandText = addParttion;
                            using (var createTableCommand = new NpgsqlCommand())
                            {
                                createTableCommand.Connection = connection;
                                createTableCommand.CommandText = createCommandText;
                                createTableCommand.ExecuteNonQuery();
                                Console.WriteLine("SQL scripts partition AuditLog executed successfully.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insert partition AuditLog: {ex.Message}");
                throw new Exception($"Error insert partition AuditLog: {ex.Message}");
            }
        }

        private void _DeletePartitionsAuditLog(int tenantId, string userName, string password, int port)
        {
            try
            {
#if DEBUG
                var host = "10.2.15.78";
                var dbName = "smartkartelogging_stagging";
                password = "Emr!23";
#else
var host = "develop-smartkarte-logging.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com";
                var dbName = "smartkartelogging";
#endif
                var connectionString = $"Host={host};Database={dbName};Username={userName};Password={password};Port={port}";
                string deletePartitionsAuditLog = $"DROP TABLE IF EXISTS public.partition_{tenantId};";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'AuditLogs')";
                        var tableExists = command.ExecuteScalar();
                        if (tableExists != null && (bool)tableExists)
                        {
                            using (var createTableCommand = new NpgsqlCommand())
                            {
                                createTableCommand.Connection = connection;
                                createTableCommand.CommandText = deletePartitionsAuditLog;
                                createTableCommand.ExecuteNonQuery();
                                Console.WriteLine("SQL scripts delete partition AuditLog executed successfully.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error delete partition AuditLog: {ex.Message}");
                throw new Exception($"Error delete partition AuditLog: {ex.Message}");
            }
        }

        private bool TeminatedTenant(int tenantId)
        {
            try
            {
                var statusTenant = ConfigConstant.StatusTenantDictionary()["terminating"];
                var updateStatus = _tenantRepository.UpdateInfTenantStatus(tenantId, statusTenant);

                var tenant = _tenantRepository.Get(tenantId);

                bool deleteRDSAction = false;
                bool deleteDNSAction = false;
                bool deleteItemCnameAction = false;

                // datete data
                deleteRDSAction = _awsSdkService.DeleteDataMasterTenant(tenant.EndPointDb, tenant.Db, _userNameDefault, _passwordDefault, tenant.TenantId, tenant.Db);

                // delete partition AuditLogs
                _DeletePartitionsAuditLog(tenantId, _userNameDefault, _passwordDefault, _portDefault);

                // Delete DNS
                var checkExistsSubDomain = Route53Action.CheckSubdomainExistence(tenant.SubDomain).Result;
                if (checkExistsSubDomain)
                {
                    deleteDNSAction = Route53Action.DeleteTenantDomain(tenant.SubDomain).Result;
                }
                else { deleteDNSAction = true; }
                // Delete item cname in cloud front
                deleteItemCnameAction = CloudFrontAction.RemoveItemCnameAsync(tenant.SubDomain).Result;

                //Delete folder S3
                _awsSdkService.DeleteObjectsInFolderAsync(ConfigConstant.DestinationBucketName, tenant.EndSubDomain).Wait();

                // Check action deleted  RDS, DNS, Cloud front
                if (deleteRDSAction && deleteDNSAction && deleteItemCnameAction)
                {
                    // Check finshed terminate
                    if (!Route53Action.CheckSubdomainExistence(tenant.SubDomain).Result)
                    {
                        return true;
                    }
                }
                var statusTenantFaild = ConfigConstant.StatusTenantDictionary()["failed"];
                _tenantRepository.UpdateInfTenantStatus(tenantId, statusTenantFaild);
                return false;
            }
            catch (Exception ex)
            {
                var statusTenantFaild = ConfigConstant.StatusTenantDictionary()["failed"];
                _tenantRepository.UpdateInfTenantStatus(tenantId, statusTenantFaild);
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private byte[] GenerateSalt()
        {
            var buffer = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        public string CreateHash(byte[] password, byte[] salt)
        {
            using var argon2 = new Argon2id(password);
            var preper = _configuration["Pepper"] ?? string.Empty;
            salt = salt.Union(Encoding.UTF8.GetBytes(preper)).ToArray();
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 128;
            return Convert.ToHexString(argon2.GetBytes(32));
        }

        #endregion
    }
}
