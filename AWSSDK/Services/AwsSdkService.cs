using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
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

        public async Task<string> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            var response = await RDSAction.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
            throw new NotImplementedException();
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
    }
}
