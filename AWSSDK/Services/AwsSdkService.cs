using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Interfaces;

namespace AWSSDK.Services
{
    public class AwsSdkService : IAwsSdkService
    {
        public AwsSdkService() { }
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

        public async Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier)
        {
            return await RDSAction.CreateDBSnapshotAsync(dbInstanceIdentifier);
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

        public Task<string> GetInfTenantByTenant(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
