﻿using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UseCase.SuperAdmin.RestoreTenant;

namespace Interactor.SuperAdmin
{
    public class RestoreTenantInteractor : IRestoreTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IConfiguration _configuration;
        public RestoreTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService, INotificationRepository notificationRepository, IConfiguration configuration)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
            _notificationRepository = notificationRepository;
            _configuration = configuration;

        }

        public RestoreTenantOutputData Handle(RestoreTenantInputData inputData)
        {
            string pathFileDumpRestore = _configuration["PathFileDumpRestore"];

            if (string.IsNullOrEmpty(pathFileDumpRestore))
            {
                return new RestoreTenantOutputData(false, RestoreTenantStatus.PathFileDumpRestoreNotAvailable);
            }
            //var c = new CommonHub();
            //c.OnConnectedAsync();
            //string rString2 = CommonConstants.GenerateRandomString(6);
            //var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync("develop-smartkarte-postgres-nnefqy", ConfigConstant.RdsSnapshotBackupRestore);
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                PostgreSqlDump(@$"{pathFileDumpRestore}\{tenant.Db}.sql", tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, "postgres", "Emr!23456789").Wait();

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.TenantDoesNotExist);
                }
                return new RestoreTenantOutputData(false, RestoreTenantStatus.TenantDoesNotExist);
                // Get laster snapshot restore to tmp tenant 
                var lastSnapshotIdentifier = RDSAction.GetLastSnapshot(tenant.RdsIdentifier).Result;
                if (string.IsNullOrEmpty(lastSnapshotIdentifier))
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.SnapshotNotAvailable);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);
                        Console.WriteLine($"Start  restore  tenant. RdsIdentifier: {tenant.RdsIdentifier}");

                        // Create snapshot backup
                        var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier, ConfigConstant.RdsSnapshotBackupRestore);

                        if (string.IsNullOrEmpty(snapshotIdentifier) || !await RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier))
                        {
                            throw new Exception("Snapshot is not Available");
                        }

                        // Create tmp RDS from snapshot
                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"{tenant.SubDomain}-{rString}";
                        var isSuccessRestoreInstance = await _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, lastSnapshotIdentifier);
                        var endpoint = await CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId);

                        // Restore tenant dedicate
                        if (tenant.Type == ConfigConstant.TypeDedicate)
                        {
                            // Update data enpoint
                            var updateEndPoint = _tenantRepository.UpdateInfTenant(tenant.TenantId, ConfigConstant.StatusTenantDictionary()["available"], tenant.EndSubDomain, endpoint.Address, dbInstanceIdentifier);
                            if (updateEndPoint)
                            {
                                throw new Exception("Update end sub domain failed");
                            }

                            // Finished restore
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }

                        // Restore tenant sharing
                        else
                        {
                            // dump data,
                            await PostgreSqlDump(@$"{pathFileDumpRestore}\{tenant.Db}.sql", endpoint.Address, ConfigConstant.PgPostDefault, tenant.Db, "postgres", "Emr!23456789");

                            // restore db 
                            // delete old db

                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }

                        // Check Restore success 
                    }
                    catch (Exception ex)
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                        // Notification  restore failed
                        var messenge = $"{tenant.EndSubDomain} is restore failed. Error: {ex.Message}.";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                });
                return new RestoreTenantOutputData(true, RestoreTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        private async Task PostgreSqlDump(string outFile, string host, int port, string database, string user, string password)
        {
            host = "localhost";
            port = 22;
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";

            string dumpCommand =
                 $"{Set} PGPASSWORD={password}\n" +
                 $"pg_dump" + " -Fc" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";

            string batchContent = "" + dumpCommand + "  > " + "\"" + outFile + "\"" + "\n";
            if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);

            await Execute(batchContent);
        }

        private Task Execute(string dumpCommand)
        {
            return Task.Run(() =>
            {
                string batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
                try
                {
                    string batchContent = "";
                    batchContent += $"{dumpCommand}";

                    System.IO.File.WriteAllText(batFilePath, batchContent.ToString(), Encoding.ASCII);

                    ProcessStartInfo info = ProcessInfoByOS(batFilePath);

                    using System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);


                    proc.WaitForExit();
                    var exit = proc.ExitCode;


                    proc.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw new Exception($"Execute sql Dump. {ex.Message}");

                }
                finally
                {
                    if (System.IO.File.Exists(batFilePath)) System.IO.File.Delete(batFilePath);
                }
            });
        }

        private static ProcessStartInfo ProcessInfoByOS(string batFilePath)
        {
            ProcessStartInfo info;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                info = new ProcessStartInfo(batFilePath)
                {
                    Arguments = $"{batFilePath}"
                };
            }
            else
            {
                info = new ProcessStartInfo("sh")
                {
                    Arguments = $"{batFilePath}"
                };
            }
            //info.EnvironmentVariables.Add("PGPASSWORD", "1234$");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            info.RedirectStandardError = true;

            return info;
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
                                _tenantRepository.UpdateStatusTenant(tenantId, statusTenant);
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