﻿using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Helper.Redis;
using Interactor.Realtime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
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
        private readonly IConfiguration _configuration;
        private readonly IDatabase _cache;
        private readonly IMemoryCache _memoryCache;
        public UpdateTenantInteractor(
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

                if (inputData.Size <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidSize);
                }

                if (inputData.SizeType <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidSizeType);
                }

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

                var oldTenant = _tenantRepository.Get(inputData.TenantId);

                if (oldTenant.TenantId <= 0 || oldTenant.TenantId <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantDoesNotExist);
                }

                if (oldTenant.Type == ConfigConstant.TypeDedicate && inputData.Type == ConfigConstant.TypeSharing)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.NotAllowUpdateTenantDedicateToSharing);
                }

                if (oldTenant.Status != ConfigConstant.StatusTenantDictionary()["available"] && oldTenant.Status != ConfigConstant.StatusTenantDictionary()["stoped"] && oldTenant.Status != ConfigConstant.StatusTenantDictionary()["storage-full"])
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantNotReadyToUpdate);
                }

                if (!_awsSdkService.CheckExitRDS(oldTenant.RdsIdentifier).Result)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.RdsDoesNotExist);
                }

                if (oldTenant.SubDomain != inputData.SubDomain)
                {
                    if (Route53Action.CheckSubdomainExistence(inputData.SubDomain).Result)
                    {
                        return new UpdateTenantOutputData(false, UpdateTenantStatus.NewDomainAleadyExist);
                    }
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
                        // Set tenant info to cache memory
                        _memoryCache.Set(oldTenant.SubDomain, new TenantCacheMemory(cts, string.Empty));

                        // Update subdomain
                        if (oldTenant.SubDomain != inputData.SubDomain)
                        {
                            // Create New subdomain
                            if (Route53Action.CreateTenantDomain(inputData.SubDomain).Result != null)
                            {
                                // Delete old subdomain
                                var actionDeleteDomain = Route53Action.DeleteTenantDomain(oldTenant.SubDomain).Result;
                            }
                            else
                            {
                                throw new Exception("Create New subdomain failed");
                            }
                        }

                        // Upgrade tenant Sharing to Dedicate
                        if (oldTenant.Type == ConfigConstant.TypeSharing && inputData.Type == ConfigConstant.TypeDedicate)
                        {
                            // Create SnapShot
                            var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync(oldTenant.RdsIdentifier, ConfigConstant.RdsSnapshotUpdate).Result;

                            if (string.IsNullOrEmpty(snapshotIdentifier) || !RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier).Result)
                            {
                                throw new Exception("Snapshot is not Available");
                            }

                            // Create new RDS Instance from snapshot
                            Console.WriteLine($"Start Upgrade Dedicate");

                            string rString = CommonConstants.GenerateRandomString(6);
                            var newRdsIdentifier = $"{inputData.SubDomain}-{rString}";

                            Console.WriteLine($"Upgrade. newRdsIdentifier : {newRdsIdentifier}");

                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                var isSuccessRestoreInstance = _awsSdkService.RestoreDBInstanceFromSnapshot(newRdsIdentifier, snapshotIdentifier).Result;

                                // Set tenant info to cache memory
                                _memoryCache.Set(oldTenant.SubDomain, new TenantCacheMemory(cts, newRdsIdentifier));
                            }

                            // Check Restore success 
                            var newEndpoint = CheckRestoredInstanceAvailableAsync(newRdsIdentifier, inputData.TenantId).Result;

                            // Update new value  
                            rdsIdentifier = newRdsIdentifier;
                            endPointDb = newEndpoint.Address;

                            //Delete list Db without tenantDB in new RDS
                            Console.WriteLine($"Start Terminate old tenant: {oldTenant.RdsIdentifier}");
                            var isDeleteSuccess = ConnectAndDeleteDatabases(endPointDb, oldTenant.Db, oldTenant.UserConnect, oldTenant.PasswordConnect);


                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                // Delete DB in old RDS
                                var listTenantDb = RDSAction.GetListDatabase(oldTenant.EndPointDb, oldTenant.UserConnect, oldTenant.PasswordConnect).Result;
                                Console.WriteLine($"listTenantDb: {listTenantDb}");

                                // Connect RDS delete TenantDb
                                if (listTenantDb.Count > 1)
                                {
                                    Console.WriteLine($"Connect RDS delete TenantDb: {oldTenant.RdsIdentifier}");
                                    _awsSdkService.DeleteTenantDb(oldTenant.EndPointDb, oldTenant.Db, oldTenant.UserConnect, oldTenant.PasswordConnect);
                                }

                                // Deleted RDS
                                else
                                {
                                    Console.WriteLine($"Deleted RDS: {oldTenant.RdsIdentifier}");
                                    var actionDeleteRDS = RDSAction.DeleteRDSInstanceAsync(oldTenant.RdsIdentifier);
                                }
                            }
                        }

                        // Update adminId, password
                        if (oldTenant.AdminId != inputData.AdminId || oldTenant.Password != oldTenant.Password)
                        {
                            UpdateLoginIdLoginPass(endPointDb, oldTenant.Db, oldTenant.UserConnect, oldTenant.PasswordConnect, oldTenant.AdminId, oldTenant.Password, inputData.AdminId, inputData.Password);
                        }

                        // Update tenant
                        TenantModel tenantUpgrade = new TenantModel();
                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {

                            tenantUpgrade = _tenantRepositoryRunTask.UpdateTenant(inputData.TenantId, rdsIdentifier, endPointDb, inputData.SubDomain, inputData.Size, inputData.SizeType,
                               inputData.Hospital, inputData.AdminId, inputData.Password);
                        }

                        // Finished update tenant
                        if (tenantUpgrade.TenantId > 0)
                        {
                            // set cache to tenantId
                            var key = "cache_tenantId_" + tenantUpgrade.SubDomain;
                            if (_cache.KeyExists(key))
                            {
                                _cache.KeyDelete(key);
                                _cache.StringSet(key, tenantUpgrade.TenantId.ToString());
                            }

                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                var messenge = $"{oldTenant.EndSubDomain} is update tenant successfully.";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusTenantDictionary()["available"], messenge);

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
                            throw new Exception("Update new data tenant failed");
                        }
                    }

                    catch (Exception ex)
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["update-failed"]);

                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {
                            // Notification  upgrade failed
                            _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["update-failed"]);
                            var messenge = $"{oldTenant.EndSubDomain} is update update failed. Error: {ex.Message}.";
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
                string connectionString = $"Host={serverEndpoint};Port={ConfigConstant.PgPostDefault};Username={username};Password={password};";
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
        public bool UpdateLoginIdLoginPass(string serverEndpoint, string tennantDB, string username, string password, int loginId, string loginPass, int newLoginId, string newLoginPass)
        {
            try
            {
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={ConfigConstant.PgPostDefault};Username={username};Password={password};";


                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Update database
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText += $"UPDATE \"{tennantDB}\".public.\"USER_MST\" SET  \"LOGIN_ID\"  = '{newLoginId}', \"LOGIN_PASS\"  = '{newLoginPass}' WHERE \"LOGIN_ID\"  ='{loginId}' and \"LOGIN_PASS\"  ='{loginPass}';";
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
