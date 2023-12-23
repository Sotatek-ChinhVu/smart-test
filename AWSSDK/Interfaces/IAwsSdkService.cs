using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace AWSSDK.Interfaces
{
    public interface IAwsSdkService
    {
        Task<Dictionary<string, Dictionary<string, string>>> SummaryCard();
        Task<List<string>> GetAvailableIdentifiersAsync();
        Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier, string snapshotType);
        Task<bool> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier);
        Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck);
        Task<bool> IsDedicatedTypeAsync(string dbIdentifier);
        Task<bool> CheckExitRDS(string dbIdentifier);
        bool DeleteTenantDb(string serverEndpoint, string tennantDB, string username, string password);
        Task CreateFolderAsync(string bucketName, string folderName);
        Task DeleteObjectsInFolderAsync(string bucketName, string folderKey);
        Task CreateFolderBackupAsync(string sourceBucket, string sourceFolder, string backupBucket, string backupFolder);
        Task UploadFileAsync(string bucketName, string folderName, string filePath);
        Task CopyObjectsInFolderAsync(string sourceBucketName, string objectName, string destinationBucketName, RestoreObjectS3TenantTypeEnum type, bool prefixDelete);
    }
}
