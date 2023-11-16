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

        public async Task<string> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            var response = await RDSAction.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
            throw new NotImplementedException();
        }


        public static async Task RestoreDBInstanceFromSnapshotAsync(string sourceDBInstanceIdentifier, string targetDBInstanceIdentifier, string dbSnapshotIdentifier)
        {
            try
            {
                // Assuming you have AWS credentials set up (access key and secret key)
                var rdsClient = new AmazonRDSClient();

                // Create a request to restore a DB instance from a DB snapshot
                var restoreRequest = new RestoreDBInstanceFromDBSnapshotRequest
                {
                    DBInstanceIdentifier = targetDBInstanceIdentifier,
                    DBSnapshotIdentifier = dbSnapshotIdentifier,
                    DBInstanceClass = "db.t2.micro", // Replace with your desired instance class
                    MultiAZ = false, // Set to true if you want a Multi-AZ deployment
                    Engine = "mysql", // Replace with your database engine
                    Port = 3306, // Replace with your desired port
                    AutoMinorVersionUpgrade = true,
                    LicenseModel = "general-public-license", // Replace with your license model
                    PubliclyAccessible = true, // Set to true if the instance should be publicly accessible
                    StorageType = "gp2", // Replace with your desired storage type
                    VpcSecurityGroupIds = new List<string> { "your_security_group_id" }, // Replace with your security group IDs
                };

                // Call the RestoreDBInstanceFromDBSnapshotAsync method to asynchronously restore the DB instance
                var response = await rdsClient.RestoreDBInstanceFromDBSnapshotAsync(restoreRequest);

                // Check the response for success
                if (response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"DB instance restoration started. Instance Identifier: {response.DBInstance.DBInstanceIdentifier}");
                }
                else
                {
                    Console.WriteLine($"DB instance restoration failed. Response: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task<bool> IsSnapshotAvailableAsync(string dbSnapshotIdentifier)
        {
            return await RDSAction.IsSnapshotAvailableAsync(dbSnapshotIdentifier);
        }
    }
}
