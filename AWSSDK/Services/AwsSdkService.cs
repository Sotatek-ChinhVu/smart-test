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

        public bool DeleteTenantDb(string serverEndpoint, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                int port = 5432;
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete database
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @$"
                                                    DO $$ 
                                                    DECLARE
                                                        pid_list text;
                                                        query_text text;
                                                    BEGIN
                                                        -- Get a comma-separated list of active process IDs (pids) for the specified database
                                                        SELECT string_agg(pid::text, ',') INTO pid_list
                                                        FROM pg_stat_activity
                                                        WHERE datname = '{tennantDB}';

                                                        -- Construct the query to terminate each connection
                                                        query_text := 'SELECT pg_terminate_backend(' || pid_list || ')';

                                                        -- Execute the query to terminate connections
                                                        EXECUTE query_text;
                                                    END $$;";
                            command.CommandText += @$"DROP DATABASE {tennantDB};";
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database: {tennantDB} deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Delete TenantDb {ex.Message}");
                        throw new Exception($"Error: Delete TenantDb {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task CreateFolderAsync(string bucketName, string folderName)
        {
            var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey);
            await S3Action.CreateFolderAsync(sourceS3ClientDestination, bucketName, folderName);
        }

        public async Task DeleteObjectsInFolderAsync(string bucketName, string folderKey)
        {
            var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey);
            await S3Action.DeleteObjectsInFolderAsync(sourceS3ClientDestination, bucketName, folderKey);
        }

        public async Task CopyObjectsInFolderAsync(string sourceBucketName, string objectName, string destinationBucketName, RestoreObjectS3TenantTypeEnum type, bool prefixDelete)
        {
            string folderKey = type switch
            {
                RestoreObjectS3TenantTypeEnum.All => objectName,
                RestoreObjectS3TenantTypeEnum.Files => $"{objectName}/store/files/",
                RestoreObjectS3TenantTypeEnum.InsuranceCard => $"{objectName}/store/InsuranceCard/",
                RestoreObjectS3TenantTypeEnum.Karte => $"{objectName}/store/karte/",
                RestoreObjectS3TenantTypeEnum.NextPic => $"{objectName}/store/karte/nextPic/",
                RestoreObjectS3TenantTypeEnum.SetPic => $"{objectName}/store/karte/setPic/",
                _ => string.Empty
            };
            var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey);
            var sourceS3Client = GetAmazonS3Client(_sourceAccessKey, _sourceSecretKey);
            await S3Action.CopyObjectsInFolderAsync(sourceS3Client, sourceBucketName, folderKey, sourceS3ClientDestination, destinationBucketName, prefixDelete);
        }

        private AmazonS3Client GetAmazonS3ClientDestination(string sourceAccessKey, string sourceSecretKey)
        {
            return new AmazonS3Client(sourceAccessKey, sourceSecretKey, ConfigConstant.RegionDestination);
        }

        public async Task CreateFolderBackupAsync(string sourceBucket, string sourceFolder, string backupBucket, string backupFolder)
        {
            var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey);
            await S3Action.BackupFolderAsync(sourceS3ClientDestination, sourceBucket, sourceFolder, backupBucket, backupFolder);
        }

        public async Task UploadFileAsync(string bucketName, string folderName, string filePath)
        {
            var sourceS3ClientDestination = GetAmazonS3ClientDestination(_sourceAccessKey, _sourceSecretKey);
            await S3Action.UploadFileWithProgressAsync(sourceS3ClientDestination, bucketName, folderName, filePath);
        }

        private AmazonS3Client GetAmazonS3Client(string sourceAccessKey, string sourceSecretKey)
        {
            return new AmazonS3Client(sourceAccessKey, sourceSecretKey, ConfigConstant.RegionSource);
        }
    }
}
