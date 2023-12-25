using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.SuperAdminModels.MigrationTenantHistory;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Infrastructure.SuperAdminRepositories;
using Interactor.Realtime;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using UseCase.SuperAdmin.TenantOnboard;

namespace Interactor.SuperAdmin
{
    public class TenantOnboardInteractor : ITenantOnboardInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenant2Repository;
        private readonly IMigrationTenantHistoryRepository _migrationTenantHistoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private IWebSocketService _webSocketService;
        private readonly IMemoryCache _memoryCache;
        public TenantOnboardInteractor(
            IAwsSdkService awsSdkService,
            ITenantRepository tenantRepository,
            ITenantRepository tenant2Repository,
            IMigrationTenantHistoryRepository migrationTenantHistoryRepository,
            INotificationRepository notificationRepository,
            IWebSocketService webSocketService,
            IMemoryCache memoryCache
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _migrationTenantHistoryRepository = migrationTenantHistoryRepository;
            _notificationRepository = notificationRepository;
            _tenant2Repository = tenant2Repository;
            _webSocketService = webSocketService;
            _memoryCache = memoryCache;
        }
        public TenantOnboardOutputData Handle(TenantOnboardInputData inputData)
        {
            try
            {
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (string.IsNullOrEmpty(inputData.Hospital) || string.IsNullOrEmpty(inputData.SubDomain) || string.IsNullOrEmpty(inputData.Password) || inputData.AdminId <= 0 || inputData.Size <= 0 || inputData.TenantId < 0)
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
                else if (inputData.SizeType != ConfigConstant.SizeTypeMB && inputData.SizeType != ConfigConstant.SizeTypeGB)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSizeType);
                }
                else if (inputData.ClusterMode != ConfigConstant.TypeSharing && inputData.ClusterMode != ConfigConstant.TypeDedicate)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidClusterMode);
                }
                else if (inputData.SizeType == ConfigConstant.SizeTypeMB)
                {
                    // default 150MB
                    if (inputData.Size > 150)
                        return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                }
                else if (inputData.SizeType == ConfigConstant.SizeTypeGB)
                {
                    if (inputData.ClusterMode == ConfigConstant.TypeSharing)
                    {
                        if (inputData.Size > 250)
                            return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                    }
                    else
                    {
                        if (inputData.Size > 1024)
                            return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                    }
                }
                var dbName = CommonConstants.GenerateDatabaseName(inputData.SubDomain);
                var tenantUrl = $"{inputData.SubDomain}.{ConfigConstant.Domain}";
                var tenantModel = new TenantModel(inputData.TenantId, inputData.Hospital, 0, inputData.AdminId, inputData.Password, inputData.SubDomain.ToLower(), dbName, inputData.Size, inputData.SizeType, inputData.ClusterMode, string.Empty, tenantUrl, 0, string.Empty, inputData.SubDomain, CommonConstants.GenerateRandomPassword());
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
            string rString = CommonConstants.GenerateRandomString(6);
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
                // Checking Available RDS Cluster
                if (model.SubDomain.Length > 0 && !ct.IsCancellationRequested)
                {
                    // Checking tenant tier, if dedicated, provision new RDS instance
                    if (model.Type == ConfigConstant.TypeDedicate)
                    {
                        string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                        var rdsInfo = await RDSAction.GetRDSInformation();
                        if (rdsInfo.ContainsKey(dbIdentifier))
                        {
                            id = _tenantRepository.CreateTenant(model);
                            if (!ct.IsCancellationRequested)
                            {
                                model.ChangeRdsIdentifier(dbIdentifier);
                                _ = Task.Run(() =>
                                {
                                    AddData(id, tenantUrl, model, ct);
                                });
                            }
                        }
                        else
                        {
                            id = _tenantRepository.CreateTenant(model);
                            if (!ct.IsCancellationRequested)
                            {
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                model.ChangeRdsIdentifier(dbIdentifier);
                                _ = Task.Run(() =>
                                {
                                    AddData(id, tenantUrl, model, ct);
                                });
                            }
                        }
                    }
                    else // In the rest cases, checking available RDS for new Tenant
                    {
                        var card = await CloudWatchAction.GetSummaryCardAsync();
                        List<string> availableIdentifier = card.Keys.Where(ids => card[ids]["available"] == "yes").ToList();

                        // If there's not any available RDS Cluster, provision new RDS cluster
                        if (availableIdentifier.Count == 0)
                        {
                            string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                            id = _tenantRepository.CreateTenant(model);
                            if (!ct.IsCancellationRequested)
                            {
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                model.ChangeRdsIdentifier(dbIdentifier);
                                _ = Task.Run(() =>
                                {
                                    AddData(id, tenantUrl, model, ct);
                                });
                            }
                        }
                        else // Else, returning the first available RDS Cluster in the list
                        {
                            bool checkAvailableIdentifier = false;
                            foreach (var dbIdentifier in availableIdentifier)
                            {
                                var sumSubDomainToDbIdentifier = _tenantRepository.SumSubDomainToDbIdentifier(dbIdentifier);
                                if (sumSubDomainToDbIdentifier <= 3)
                                {
                                    checkAvailableIdentifier = true;
                                    if (!ct.IsCancellationRequested)
                                    {
                                        model.ChangeRdsIdentifier(dbIdentifier);
                                        id = _tenantRepository.CreateTenant(model);
                                        _ = Task.Run(() =>
                                        {
                                            AddData(id, tenantUrl, model, ct);
                                        });
                                    }
                                    break;
                                }
                            }
                            if (!checkAvailableIdentifier)
                            {
                                string dbIdentifierNew = $"develop-smartkarte-postgres-{rString}";
                                id = _tenantRepository.CreateTenant(model);
                                if (!ct.IsCancellationRequested)
                                {
                                    await RDSAction.CreateNewShardAsync(dbIdentifierNew);
                                    model.ChangeRdsIdentifier(dbIdentifierNew);
                                    _ = Task.Run(() =>
                                    {
                                        AddData(id, tenantUrl, model, ct);
                                    });
                                }
                            }
                        }
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
                    var message = $"{model.SubDomain} is created failed. Error: {ex.Message}";
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

        private string CheckingRDSStatusAsync(string dbIdentifier, int tenantId, string tenantUrl)
        {
            try
            {
                string host = string.Empty;
                bool running = true;
                string status = string.Empty;
                DateTime startTime = DateTime.Now;

                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    var response = rdsClient.DescribeDBInstancesAsync(new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbIdentifier
                    }).Result;

                    var dbInstances = response.DBInstances;

                    if (dbInstances.Count != 1)
                    {
                        throw new Exception("More than one Database Instance returned; this should never happen");
                    }

                    var dbInstance = dbInstances[0];
                    var checkStatus = dbInstance.DBInstanceStatus;
                    if (status != checkStatus)
                    {
                        Console.WriteLine($"Last Database Shard status: {checkStatus}");
                        status = checkStatus;
                        var rdsStatusDictionary = ConfigConstant.StatusTenantDictionary();
                        if (rdsStatusDictionary.TryGetValue(checkStatus, out byte statusTenant))
                        {
                            if (dbInstance.Endpoint != null && dbInstance.Endpoint.Address != null)
                            {
                                host = dbInstance.Endpoint.Address;
                            }
                            var updateStatus = _tenant2Repository.UpdateInfTenant(tenantId, statusTenant, tenantUrl, host, dbIdentifier);
                        }
                    }

                    Thread.Sleep(5000);

                    if (checkStatus == "available")
                    {
                        if (dbInstance.Endpoint != null && dbInstance.Endpoint.Address != null)
                        {
                            host = dbInstance.Endpoint.Address;
                        }
                        // update status available: 1
                        var updateStatus = _tenant2Repository.UpdateInfTenant(tenantId, 2, tenantUrl, host, dbIdentifier);
                        running = false;
                        return host;
                    }
                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                        throw new Exception($"CheckingRDSStatus. Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                    }
                }

                return host;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"CheckingRDSStatus. {ex.Message}");
            }
        }

        private void AddData(int tenantId, string tenantUrl, TenantModel model, CancellationToken ct)
        {
            try
            {
                string host = CheckingRDSStatusAsync(model.RdsIdentifier, tenantId, tenantUrl);
                if (!string.IsNullOrEmpty(host))
                {
                    var dataMigration = _migrationTenantHistoryRepository.GetMigration(tenantId);
                    RDSAction.CreateDatabase(host, model.Db, model.PasswordConnect);
                    CreateDatas(host, model.Db, dataMigration, tenantId, model);
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

        private void CreateDatas(string host, string dbName, List<string> listMigration, int tenantId, TenantModel model)
        {
            try
            {
                var connectionString = $"Host={host};Database={dbName};Username=postgres;Password=Emr!23456789;Port=5432";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        _CreateTable(command, listMigration, tenantId);
                        var sqlGrant = $"GRANT All ON ALL TABLES IN SCHEMA public TO \"{dbName}\";";
                        var sqlInsertUser = string.Format(QueryConstant.SqlUser, model.AdminId, model.Password);
                        var sqlInsertUserPermission = QueryConstant.SqlUserPermission;
                        command.CommandText = sqlGrant + sqlInsertUser + sqlInsertUserPermission;
                        command.ExecuteNonQuery();
                        _CreateFunction(command, listMigration, tenantId);
                        _CreateTrigger(command, listMigration, tenantId);
                        _CreateAuditLog(tenantId);
                        _CreateDataMaster(host, dbName, model.UserConnect, model.PasswordConnect);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private void _CreateTable(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                string folderPath = Path.Combine(templatePath, "Table");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();

                        // insert table
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts table executed successfully.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error create table: no files found");
                    throw new Exception($"Error create table. No files found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create table: {ex.Message}");
                throw new Exception($"Error create table.  {ex.Message}");
            }
        }

        private void _CreateAuditLog(int tenantId)
        {
            try
            {
                var host = "develop-smartkarte-logging.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com";
                var dbName = "smartkartelogging";
                var connectionString = $"Host={host};Database={dbName};Username=postgres;Password=Emr!23456789;Port=5432";
                string sqlCreateAuditLog = QueryConstant.CreateAuditLog;
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
                        if (tableExists != null && !(bool)tableExists)
                        {
                            createCommandText = sqlCreateAuditLog + addParttion;
                        }
                        else
                        {
                            createCommandText = addParttion;
                        }
                        using (var createTableCommand = new NpgsqlCommand())
                        {
                            createTableCommand.Connection = connection;
                            createTableCommand.CommandText = createCommandText;
                            createTableCommand.ExecuteNonQuery();
                            Console.WriteLine("SQL scripts AuditLog, Parttion executed successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insert AuditLog/ Parttion: {ex.Message}");
                throw new Exception($"Error insert AuditLog/ Parttion: {ex.Message}");
            }
        }

        private void _CreateDataMaster(string host, string database, string user, string password)
        {
            try
            {
                string pathFile = "/app/data-master.sql";
                PostgresSqlAction.PostgreSqlExcuteFileSQLDataMaster(pathFile, host, 5432, database, user, password).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insert data master: {ex.Message}");
                throw new Exception($"Error insert data master: {ex.Message}");
            }
        }

        private void _CreateFunction(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                string folderPath = Path.Combine(templatePath, "Function");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");
                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();
                        // insert function
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts function executed successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Create function: no files found");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create function: {ex.Message}");
                throw new Exception($"Error create function.  {ex.Message}");
            }
        }

        private void _CreateTrigger(NpgsqlCommand command, List<string> listMigration, int tenantId)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template");
                string folderPath = Path.Combine(templatePath, "Trigger");
                if (Directory.Exists(folderPath))
                {
                    var sqlFiles = Directory.GetFiles(folderPath, "*.sql");
                    if (sqlFiles.Length > 0)
                    {
                        var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                        var uniqueFileNames = fileNames.Except(listMigration).ToList();
                        // insert trigger
                        if (uniqueFileNames.Any())
                        {
                            foreach (var fileName in uniqueFileNames)
                            {
                                var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                                if (File.Exists(filePath))
                                {
                                    var sqlScript = File.ReadAllText(filePath);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        _migrationTenantHistoryRepository.AddMigrationHistory(tenantId, fileName);
                                    }
                                }
                            }
                            Console.WriteLine("SQL scripts trigger executed successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Create trigger: no files found");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error create trigger: {ex.Message}");
                throw new Exception($"Error create trigger.  {ex.Message}");
            }
        }

        private bool TeminatedTenant(int tenantId)
        {
            try
            {
                bool skipFinalSnapshot = false;
                var statusTenant = ConfigConstant.StatusTenantDictionary()["terminating"];
                var updateStatus = _tenantRepository.UpdateInfTenantStatus(tenantId, statusTenant);

                var tenant = _tenantRepository.Get(tenantId);

                bool deleteRDSAction = false;
                bool deleteDNSAction = false;
                bool deleteItemCnameAction = false;
                var listTenantDb = RDSAction.GetListDatabase(tenant.EndPointDb, tenant.UserConnect, tenant.PasswordConnect).Result;
                // Connect RDS delete TenantDb
                if (listTenantDb.Count > 1)
                {
                    if (listTenantDb.Contains(tenant.Db))
                    {
                        deleteRDSAction = _awsSdkService.DeleteTenantDb(tenant.EndPointDb, tenant.Db, tenant.UserConnect, tenant.PasswordConnect);
                    }
                    else
                    {
                        deleteRDSAction = true;
                    }
                }
                // Deleted RDS
                else
                {
                    if (RDSAction.CheckRDSInstanceExists(tenant.RdsIdentifier).Result)
                    {
                        if (!string.IsNullOrEmpty(tenant.RdsIdentifier))
                        {
                            deleteRDSAction = RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier, skipFinalSnapshot).Result;
                        }
                    }
                    else
                    {
                        deleteRDSAction = true;
                    }
                }

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
                    if (RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier).Result)
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

        #endregion
    }
}
