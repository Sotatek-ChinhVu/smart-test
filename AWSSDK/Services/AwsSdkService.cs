using Amazon.S3;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace AWSSDK.Services
{
    public class AwsSdkService : IAwsSdkService
    {
        private readonly IConfiguration _configuration;
        private readonly string _sourceAccessKey;
        private readonly string _sourceSecretKey;

        public AwsSdkService(IConfiguration configuration)
        {
            _configuration = configuration;
            _sourceAccessKey = _configuration.GetSection("AmazonS3")["AwsAccessKeyId"] ?? string.Empty;
            _sourceSecretKey = _configuration.GetSection("AmazonS3")["AwsSecretAccessKey"] ?? string.Empty;
        }
        public async Task<Dictionary<string, Dictionary<string, string>>> SummaryCard()
        {
            return await CloudWatchAction.GetSummaryCardAsync();
        }
        public async Task<List<string>> GetAvailableIdentifiersAsync()
        {
            var sumaryCard = await CloudWatchAction.GetSummaryCardAsync();
            var result = sumaryCard.Where(entry => entry.Value["available"] == "yes").Select(entry => entry.Key).ToList();
            return result;
        }

        public async Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier, string snapshotType)
        {
            return await RDSAction.CreateDBSnapshotAsync(dbInstanceIdentifier, snapshotType);
        }

        public async Task<bool> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            return await RDSAction.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
        }

        public async Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck)
        {
            var exits = await Route53Action.CheckSubdomainExistence(subdomainToCheck);
            return exits;
        }

        public async Task<bool> IsDedicatedTypeAsync(string dbIdentifier)
        {
            var result = await RDSAction.IsDedicatedTypeAsync(dbIdentifier);
            return result;
        }

        public async Task<bool> CheckExitRDS(string dbIdentifier)
        {
            var RDSInfs = await RDSAction.GetRDSInformation();
            if (RDSInfs.ContainsKey(dbIdentifier))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  Delete data master tenant
        /// </summary>
        /// <param name="serverEndpoint"></param>
        /// <param name="tennantDB"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool DeleteDataMasterTenant(string serverEndpoint, string tennantDB, string username, string password, int hpId, string db)
        {
            try
            {
#if DEBUG
                serverEndpoint = "10.2.15.78";
                password = "Emr!23";
#endif
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint}; Database ={db}; Port={ConfigConstant.PgPostDefault};Username={username};Password={password};";

                string FormartNameZTable(string tableName)
                {
                    int indexOfZ = tableName.IndexOf('z');
                    string modifiedString = tableName.Remove(indexOfZ, 2);
                    return modifiedString;
                }

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        // Delete database
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "BEGIN;\n";
                            foreach (string table in ConfigConstant.listTableMaster)
                            {
                                // Delete data table
                                //command.CommandText += $"DELETE FROM public.{table} WHERE hp_id = {hpId};\n";
                                // Drop partition
                                if (table.StartsWith("z")) // Drop partition with z_table
                                {
                                    command.CommandText += $"DROP TABLE IF EXISTS public.z_p_{FormartNameZTable(table)}_{hpId};\n";
                                }
                                else
                                {
                                    command.CommandText += $"DROP TABLE IF EXISTS public.p_{table}_{hpId};\n";
                                }
                            }
                            command.CommandText += "COMMIT;";
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Data master: {tennantDB} deleted successfully.");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Delete  data master {ex.Message}");
                        throw new Exception($"Error: Delete  data master {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task CreateFolderAsync(string bucketName, string folderName)
        {
            using (var destinationS3Client = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey))
            {
                await S3Action.CreateFolderAsync(destinationS3Client, bucketName, folderName);
            }
        }

        public async Task DeleteObjectsInFolderAsync(string bucketName, string folderKey)
        {
            using (var destinationS3Client = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey))
            {
                await S3Action.DeleteObjectsInFolderAsync(destinationS3Client, bucketName, folderKey);
            }
        }

        public async Task CopyObjectsInFolderAsync(string sourceBucketName, string objectName, string destinationBucketName, List<RestoreObjectS3TenantTypeEnum> type, bool prefixDelete)
        {
            using (var sourceS3Client = GetAmazonS3Client(_sourceAccessKey, _sourceSecretKey))
            using (var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey))
            {
                var folderKeyMap = new Dictionary<RestoreObjectS3TenantTypeEnum, string>
                {
                    { RestoreObjectS3TenantTypeEnum.All , $"{objectName}/" },
                    { RestoreObjectS3TenantTypeEnum.Files, $"{objectName}/store/files/" },
                    { RestoreObjectS3TenantTypeEnum.InsuranceCard, $"{objectName}/store/InsuranceCard/" },
                    { RestoreObjectS3TenantTypeEnum.Karte, $"{objectName}/store/karte/" },
                    { RestoreObjectS3TenantTypeEnum.NextPic, $"{objectName}/store/karte/nextPic/" },
                    { RestoreObjectS3TenantTypeEnum.SetPic, $"{objectName}/store/karte/setPic/" }
                };
                foreach (var item in type)
                {
                    if (item == RestoreObjectS3TenantTypeEnum.All)
                    {
                        continue;
                    }
                    string folderKey = folderKeyMap.TryGetValue(item, out var value) ? value : string.Empty;
                    if (!string.IsNullOrEmpty(folderKey))
                    {
                        await S3Action.CopyObjectsInFolderAsync(sourceS3Client, sourceBucketName, folderKey, sourceS3ClientDestination, destinationBucketName, prefixDelete);
                    }
                }
            }
        }

        public async Task CreateFolderBackupAsync(string sourceBucket, string sourceFolder, string backupBucket, string backupFolder)
        {
            using (var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey))
            {
                await S3Action.BackupFolderAsync(sourceS3ClientDestination, sourceBucket, sourceFolder, backupBucket, backupFolder);
            }
        }

        public async Task UploadFileAsync(string bucketName, string folderName, string filePath)
        {
            using (var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey))
            {
                await S3Action.UploadFileWithProgressAsync(sourceS3ClientDestination, bucketName, folderName, filePath);
            }
        }

        #region [get AmazonS3Client]
        private AmazonS3Client GetAmazonS3Client(string sourceAccessKey, string sourceSecretKey)
        {
            return new AmazonS3Client(sourceAccessKey, sourceSecretKey, ConfigConstant.RegionSource);
        }
        private AmazonS3Client GetAmazonS3ClientDestination(string sourceAccessKey, string sourceSecretKey)
        {
            return new AmazonS3Client(sourceAccessKey, sourceSecretKey, ConfigConstant.RegionDestination);
        }
        #endregion [get AmazonS3Client]
    }
}
