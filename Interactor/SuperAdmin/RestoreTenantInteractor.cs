using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly IConfiguration _configuration;
        public RestoreTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, INotificationRepository notificationRepository, IConfiguration configuration)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _configuration = configuration;

        }

        public RestoreTenantOutputData Handle(RestoreTenantInputData inputData)
        {
            IWebSocketService _webSocketService;
            _webSocketService = (IWebSocketService)inputData.WebSocketService;
            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, "11");
            string pathFileDumpRestore = _configuration["PathFileDumpRestore"];

            if (string.IsNullOrEmpty(pathFileDumpRestore))
            {
                return new RestoreTenantOutputData(false, RestoreTenantStatus.PathFileDumpRestoreNotAvailable);
            }
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.TenantDoesNotExist);
                }

                // Get laster snapshot restore to tmp tenant 
                var lastSnapshotIdentifier = RDSAction.GetLastSnapshot(tenant.RdsIdentifier).Result;
                if (string.IsNullOrEmpty(lastSnapshotIdentifier))
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.SnapshotNotAvailable);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(() =>
                {
                    try
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);
                        Console.WriteLine($"Start  restore  tenant. RdsIdentifier: {tenant.RdsIdentifier}");

                        // Create snapshot backup
                        var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier, ConfigConstant.RdsSnapshotBackupRestore).Result;

                        if (string.IsNullOrEmpty(snapshotIdentifier) || !RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier).Result)
                        {
                            throw new Exception("Snapshot is not Available");
                        }

                        // Create tmp RDS from snapshot
                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"{tenant.SubDomain}-{rString}";
                        var isSuccessRestoreInstance = _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, lastSnapshotIdentifier).Result;
                        var endpoint = CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId).Result;

                        // Restore tenant dedicate
                        if (tenant.Type == ConfigConstant.TypeDedicate)
                        {
                            // Update data enpoint
                            var updateEndPoint = _tenantRepository.UpdateInfTenant(tenant.TenantId, ConfigConstant.StatusTenantDictionary()["available"], tenant.EndSubDomain, endpoint.Address, dbInstanceIdentifier);
                            if (!updateEndPoint)
                            {
                                throw new Exception("Update end sub domain failed");
                            }

                            // delete old RDS
                            var actionDeleteOldRDS = RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier).Result;
                            var checkDeleteActionOldRDS = RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier).Result;
                            // Finished restore
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }

                        // Restore tenant sharing
                        else
                        {
                            // dump data,
                            var pathFileDump = @$"{pathFileDumpRestore}{tenant.Db}.sql"; // path save file sql dump
                            PostgreSqlDump(pathFileDump, endpoint.Address, ConfigConstant.PgPostDefault, tenant.Db, "postgres", "Emr!23456789").Wait();

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

                            // restore db 
                            PostgreSqlExcuteFileDump(pathFileDump, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, "postgres", "Emr!23456789").Wait();

                            // delete Tmp RDS
                            var actionDeleteTmpRDS = RDSAction.DeleteRDSInstanceAsync(dbInstanceIdentifier).Result;
                            var checkDeleteActionTmpRDS = RDSAction.CheckRDSInstanceDeleted(dbInstanceIdentifier).Result;
                            // Finished restore
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
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
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
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

        /// <summary>
        /// Genarate conent script dump db from tmp RDS
        /// </summary>
        /// <param name="outFile"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task PostgreSqlDump(string outFile, string host, int port, string database, string user, string password)
        {
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
            outFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? outFile : outFile.Replace("\\", "/");
            string batchContent;

            string dumpCommand =
                 $"{Set} PGPASSWORD={password}\n" +
                 $"pg_dump" + " -Fc" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // path file windown
                batchContent = "" + dumpCommand + "  > " + "\"" + outFile + "\"" + "\n";
            }
            else
            {
                // path file linux
                batchContent = "" + dumpCommand + "  > " + outFile + "\n";
            }
            if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);

            await Execute(batchContent);
        }

        /// <summary>
        ///  Genarate conent script resore db from file sql dump
        /// </summary>
        /// <param name="pathFileDump"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task PostgreSqlExcuteFileDump(string pathFileDump, string host, int port, string database, string user, string password)
        {
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
            pathFileDump = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? pathFileDump : pathFileDump.Replace("\\", "/");
            string batchContent;
            string dumpCommand =
                 $"{Set} PGPASSWORD={password}\n" +
                 $"pg_restore" + " -F c" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";
            
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // path file windown
                batchContent = "" + dumpCommand + "  -c -v " + "\"" + pathFileDump + "\"" + "\n";
            }
            else
            {   // path file linux
                batchContent = "" + dumpCommand + "  -c -v " + pathFileDump + "\n";
            }

            await Execute(batchContent);
        }

        /// <summary>
        ///  Create file .sh / .bat to execute conent script sql
        /// </summary>
        /// <param name="dumpCommand"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

                    // Create process Grant execute permissions to file .sh
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Grant execute permissions using chmod
                        ProcessStartInfo chmodInfo = new ProcessStartInfo
                        {
                            FileName = "chmod",
                            Arguments = $"+x {batFilePath}",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (System.Diagnostics.Process chmodProc = System.Diagnostics.Process.Start(chmodInfo))
                        {
                            chmodProc.WaitForExit();

                            if (chmodProc.ExitCode != 0)
                            {
                                // Handle chmod error, if any
                                string errorOutput = chmodProc.StandardError.ReadToEnd();
                                Console.WriteLine($"chmod error: {errorOutput}");
                                throw new Exception($"Failed to grant execute permissions to the script: {errorOutput}");
                            }
                        }
                    }

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

        /// <summary>
        /// Get process info
        /// </summary>
        /// <param name="batFilePath"></param>
        /// <returns></returns>
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
                            Console.WriteLine($"DB Instance status: {checkStatus}");
                        }

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
