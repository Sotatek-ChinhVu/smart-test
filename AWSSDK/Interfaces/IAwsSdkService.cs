﻿using Amazon.RDS.Model;

namespace AWSSDK.Interfaces
{
    public interface IAwsSdkService
    {
        Task<Dictionary<string, Dictionary<string, string>>> SummaryCard();
        Task<List<string>> GetAvailableIdentifiersAsync();
        Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier);
        Task<Endpoint> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier);
        Task<bool> IsSnapshotAvailableAsync(string dbSnapshotIdentifier);
        Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck);
        Task<bool> IsDedicatedTypeAsync(string dbIdentifier);
    }
}
