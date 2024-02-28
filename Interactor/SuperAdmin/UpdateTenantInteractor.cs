using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using Domain.Models.User;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Helper.Redis;
using Interactor.Realtime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
using System.Text;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpdateTenantInteractor : IUpdateTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;
        private readonly IUserRepository _userRepositoryRunTask;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _cache;
        private readonly IMemoryCache _memoryCache;
        public UpdateTenantInteractor(
            ITenantRepository tenantRepository,
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask,
            IUserRepository userRepositoryRunTask,
            IConfiguration configuration,
            IMemoryCache memoryCache
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
            _userRepositoryRunTask = userRepositoryRunTask;
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
            _memoryCache = memoryCache;
        }

        private void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public UpdateTenantOutputData Handle(UpdateTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (inputData.TenantId <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidTenantId);
                }

                var oldTenant = _tenantRepository.Get(inputData.TenantId);



                if (oldTenant.Status == ConfigConstant.StatusTenantDictionary()["available"] || oldTenant.Status == ConfigConstant.StatusTenantDictionary()["stoped"])
                {
                    if (string.IsNullOrEmpty(inputData.SubDomain))
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidDomain);
                    }

                    if (string.IsNullOrEmpty(inputData.Hospital))
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidHospital);
                    }

                    if (inputData.AdminId <= 0)
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidAdminId);
                    }

                    if (string.IsNullOrEmpty(inputData.Password))
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidPassword);
                    }


                    if (oldTenant.TenantId <= 0 || oldTenant.TenantId <= 0)
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantDoesNotExist);
                    }

                    if (oldTenant.Status != ConfigConstant.StatusTenantDictionary()["available"] && oldTenant.Status != ConfigConstant.StatusTenantDictionary()["stoped"])
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantNotReadyToUpdate);
                    }

                    if (oldTenant.SubDomain != inputData.SubDomain)
                    {
                        if (Route53Action.CheckSubdomainExistence(inputData.SubDomain).Result)
                        {
                            return new UpdateTenantOutputData(false, UpdateTenantStatus.NewDomainAleadyExist);
                        }
                    }
                }
                else if (oldTenant.Status == ConfigConstant.StatusTenantDictionary()["terminated"])
                {
                    if (string.IsNullOrEmpty(inputData.Hospital))
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidHospital);
                    }
                }
                else
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantNotReadyToUpdate);
                }


                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["updating"]);
                var cts = new CancellationTokenSource();
                CancellationToken ct = cts.Token;
                _ = Task.Run(() =>
                {
                    try
                    {
                        ct.ThrowIfCancellationRequested();
                        string rdsIdentifier = oldTenant.RdsIdentifier;
                        string endPointDb = oldTenant.EndPointDb;
                        string endSubDomain = oldTenant.EndSubDomain;
                        // Set tenant info to cache memory
                        _memoryCache.Set(oldTenant.SubDomain, new TenantCacheMemory(cts, string.Empty));

                        if (oldTenant.Status == ConfigConstant.StatusTenantDictionary()["available"] || oldTenant.Status == ConfigConstant.StatusTenantDictionary()["stoped"])
                        {
                            // Update subdomain
                            if (oldTenant.SubDomain != inputData.SubDomain)
                            {
                                // Create New subdomain
                                if (Route53Action.CreateTenantDomain(inputData.SubDomain).Result != null)
                                {
                                    // Delete old subdomain
                                    var actionDeleteDomain = Route53Action.DeleteTenantDomain(oldTenant.SubDomain).Result;

                                    // Update end subdomain
                                    endSubDomain = inputData.SubDomain + "." + ConfigConstant.Domain;
                                }
                                else
                                {
                                    throw new Exception("サブドメインの作成に失敗しました。");
                                }
                            }

                            // Update adminId, password
                            if (oldTenant.AdminId != inputData.AdminId || oldTenant.Password != oldTenant.Password)
                            {
                                bool changeAdminId = false;
                                if (oldTenant.AdminId != inputData.AdminId)
                                {
                                    changeAdminId = true;
                                }
                                UpdateLoginIdLoginPass(changeAdminId, endPointDb, oldTenant.Db, ConfigConstant.PgUserDefault, ConfigConstant.PgPasswordDefault, oldTenant.AdminId, inputData.TenantId, inputData.AdminId, inputData.Password);
                            }
                        }

                        // Update tenant
                        TenantModel tenantUpdate = new TenantModel();
                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {
                            tenantUpdate = _tenantRepositoryRunTask.UpdateTenant(inputData.TenantId, inputData.SubDomain, inputData.Hospital, inputData.AdminId, inputData.Password, endSubDomain, oldTenant.Status);
                        }

                        // Finished update tenant
                        if (tenantUpdate.TenantId > 0)
                        {
                            // set cache to tenantId
                            var key = Helper.Constants.CacheKeyConstant.CacheKeyTenantId + tenantUpdate.EndSubDomain;
                            if (_cache.KeyExists(key))
                            {
                                _cache.KeyDelete(key);
                                _cache.StringSet(key, tenantUpdate.TenantId.ToString());
                            }

                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                var messenge = $"{oldTenant.EndSubDomain} の情報更がが完了しました。";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);

                                // Add info tenant for notification
                                notification.SetTenantId(oldTenant.TenantId);
                                notification.SetStatusTenant(ConfigConstant.StatusTenantRunning);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                            }

                            // Delete cache memory
                            _memoryCache.Remove(oldTenant.SubDomain);

                            cts.Cancel();
                            return;
                        }
                        else
                        {
                            throw new Exception("の情報更新に失敗しました。エラー");
                        }
                    }

                    catch (Exception ex)
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["update-failed"]);

                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {
                            // Notification  upgrade failed
                            _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["update-failed"]);
                            var messenge = $"{oldTenant.EndSubDomain} の情報更新に失敗しました。エラー: {ex.Message}.";
                            var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                            // Add info tenant for notification
                            notification.SetTenantId(oldTenant.TenantId);
                            notification.SetStatusTenant(ConfigConstant.StatusTenantFailded);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        }

                        // Delete cache memory
                        _memoryCache.Remove(oldTenant.SubDomain);

                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
                    }
                }, cts.Token);

                var key = "connect_db_" + oldTenant.EndSubDomain;
                _cache.KeyDelete(key);
                return new UpdateTenantOutputData(true, UpdateTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        /// <summary>
        /// Delete redundant Databases in new RDS
        /// </summary>
        /// <param name="serverEndpoint"></param>
        /// <param name="tennantDB"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ConnectAndDeleteDatabases(string serverEndpoint, string tennantDB, string username, string password)
        {
            try
            {
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={ConfigConstant.PgPortDefault};Username={username};Password={password};";
                var listTenantDb = RDSAction.GetListDatabase(serverEndpoint, username, password).Result;
                if (listTenantDb.Contains(tennantDB))
                {
                    listTenantDb.Remove(tennantDB);
                }
                else
                {
                    throw new Exception($"Connect AndDelete Databases. tennantDB doesn't exists");
                }

                if (listTenantDb.Count <= 0)
                {
                    return true;
                }

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete database
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            foreach (var item in listTenantDb)
                            {
                                command.CommandText += $"DROP DATABASE {item};";
                            }
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new Exception($"Connect And Delete Databases Failed. {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Delete redundant Databases in new RDS. {ex.Message}");
            }
        }

        /// <summary>
        /// Update loginId, loginPass in tenant db
        /// </summary>
        /// <param name="serverEndpoint"></param>
        /// <param name="tennantDB"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateLoginIdLoginPass(bool changeAdminId, string serverEndpoint, string tennantDB, string username, string password, int loginId, int hpId, int newLoginId, string newLoginPass)
        {
            try
            {
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint}; Database={tennantDB}; Port={ConfigConstant.PgPortDefault};Username={username};Password={password};";


                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        string updateUserMst = $"UPDATE {tennantDB}.public.user_mst SET login_id = '{newLoginId}', user_id = '{newLoginId}', hash_password = @hashPassword, salt = @salt" +
                                $" WHERE login_id ='{loginId}' and hp_id = {hpId};";
                        string updateUserPermission = $"UPDATE {tennantDB}.public.user_permission SET user_id = '{newLoginId}'" +
                                $" WHERE hp_id = {hpId};";
                        byte[] salt = _userRepositoryRunTask.GenerateSalt();
                        byte[] hashPassword = _userRepositoryRunTask.CreateHash(Encoding.UTF8.GetBytes(newLoginPass ?? string.Empty), salt);
                        connection.Open();

                        // Update database
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText += updateUserMst;
                            if (changeAdminId)
                            {
                                command.CommandText += updateUserPermission;
                            }
                            command.Parameters.AddWithValue("hashPassword", hashPassword);
                            command.Parameters.AddWithValue("salt", salt);
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database Update successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new Exception($"Connect And Update Databases Failed. {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Update AdminId, Password. {ex.Message}");
            }
        }

        public async Task<Endpoint> CheckRestoredInstanceAvailableAsync(string dbInstanceIdentifier, int tenantId)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                bool running = true;
                string status = string.Empty;
                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    // Create a request to describe DB instances
                    var describeInstancesRequest = new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbInstanceIdentifier
                    };

                    // Call DescribeDBInstancesAsync to asynchronously get information about the DB instance
                    var describeInstancesResponse = await rdsClient.DescribeDBInstancesAsync(describeInstancesRequest);

                    // Check if the DB instance exists
                    var dbInstances = describeInstancesResponse.DBInstances;
                    if (dbInstances.Count == 1)
                    {
                        var dbInstance = dbInstances[0];
                        var checkStatus = dbInstance.DBInstanceStatus;

                        if (status != checkStatus)
                        {
                            status = checkStatus;
                            var rdsStatusDictionary = ConfigConstant.StatusTenantDictionary();
                            if (rdsStatusDictionary.TryGetValue(checkStatus, out byte statusTenant))
                            {
                                _tenantRepositoryRunTask.UpdateStatusTenant(tenantId, statusTenant);
                            }
                        }
                        Console.WriteLine($"DB Instance status: {checkStatus}");

                        // Check if the DB instance is in the "available" state
                        if (status.Equals("available", StringComparison.OrdinalIgnoreCase))
                        {
                            running = false;
                            return describeInstancesResponse.DBInstances[0].Endpoint;
                        }
                    }
                    else
                    {
                        running = false;
                        throw new Exception($"Checking Restored Instance Available. DB instance doesn't exists");
                    }

                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                        throw new Exception($"Checking Restored Instance Available. Timeout");
                    }

                    // Wait for 5 seconds before the next attempt
                    Thread.Sleep(5000);
                }

                // Return an empty Endpoint if the loop exits without finding an available instance
                return new Endpoint();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Checking Restored Instance Available. {ex.Message}");
            }
        }
    }
}
