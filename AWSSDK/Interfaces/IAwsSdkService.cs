namespace AWSSDK.Interfaces
{
    public interface IAwsSdkService
    {
        Task<Dictionary<string, Dictionary<string, string>>> SummaryCard();
        Task<List<string>> GetAvailableIdentifiersAsync();
        Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier);
        Task<string> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier);
        Task<bool> IsSnapshotAvailableAsync(string dbSnapshotIdentifier);
         Task<string> GetInfTenantByTenant(string Id);
        Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck);
        Task<bool> IsDedicatedTypeAsync(string dbIdentifier);
    }
}
