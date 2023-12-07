using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Constants;
using AWSSDK.Dto;
using Npgsql;

namespace AWSSDK.Common
{
    public static class RDSAction
    {
        public static async Task<Dictionary<string, RDSInformation>> GetRDSInformation()
        {
            try
            {
                var rds = new AmazonRDSClient();
                var response = await rds.DescribeDBInstancesAsync();

                var rdsInformation = new Dictionary<string, RDSInformation>();

                foreach (var dbInstance in response.DBInstances)
                {
                    string rdsIdentifier = dbInstance.DBInstanceIdentifier;
                    string dbType = dbInstance.DBInstanceClass;
                    int dbStorage = (int)dbInstance.AllocatedStorage;
                    string dbEngine = dbInstance.Engine;

                    var rdsInfo = new RDSInformation
                    {
                        RDSIdentifier = rdsIdentifier,
                        InstanceType = dbType,
                        SizeStorage = dbStorage,
                        DBEngine = dbEngine
                    };

                    rdsInformation.Add(rdsIdentifier, rdsInfo);
                }
                return rdsInformation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"GetRDSInformation. {ex.Message}");
            }
        }

        public static async Task<bool> IsDedicatedTypeAsync(string dbIdentifier)
        {
            var rds = new AmazonRDSClient();
            var instances = await rds.DescribeDBInstancesAsync();
            var data = instances.DBInstances.FirstOrDefault(i => i.DBInstanceIdentifier == dbIdentifier);
            if (data != null)
            {
                if (data.DBInstanceClass == ConfigConstant.DedicateInstance)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static async Task CreateNewShardAsync(string dbIdentifier)
        {
            try
            {
                var rdsClient = new AmazonRDSClient();

                var request = new CreateDBInstanceRequest
                {
                    DBInstanceIdentifier = dbIdentifier,
                    AllocatedStorage = 20,
                    //DBName = "smartkarte",
                    Engine = "postgres",
                    EngineVersion = "14.4",
                    StorageType = "gp2",
                    StorageEncrypted = false,
                    AutoMinorVersionUpgrade = true,
                    MultiAZ = false,
                    MasterUsername = "postgres",
                    MasterUserPassword = "Emr!23456789",
                    DBSubnetGroupName = "develop-smartkarte-rds-subnetgroup",
                    VpcSecurityGroupIds = new List<string> { "sg-0cc9111542280b236" },
                    DBInstanceClass = "db.t4g.micro"
                };

                await rdsClient.CreateDBInstanceAsync(request);

                Console.WriteLine($"Starting RDS instance with ID: {dbIdentifier}");
            }
            catch (AmazonRDSException e)
            {
                if (e.ErrorCode == "DBInstanceAlreadyExists")
                {
                    Console.WriteLine($"Database Instance {dbIdentifier} exists already, continuing to poll ...");
                    throw new Exception($"Database Instance {dbIdentifier} exists already, continuing to poll ...");
                }
                else
                {
                    throw new Exception($"CreateNewShardAsync. {e.Message}");
                }
            }
        }

        public static async Task<bool> CheckSnapshotAvailableAsync(string dbSnapshotIdentifier)
        {
            try
            {
                bool available = false;
                DateTime startTime = DateTime.Now;
                bool running = true;

                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    // Create a request to describe DB snapshots
                    var describeSnapshotsRequest = new DescribeDBSnapshotsRequest
                    {
                        DBSnapshotIdentifier = dbSnapshotIdentifier
                    };

                    // Call DescribeDBSnapshotsAsync to asynchronously get information about the snapshot
                    var describeSnapshotsResponse = await rdsClient.DescribeDBSnapshotsAsync(describeSnapshotsRequest);

                    // Check if the snapshot exists
                    var snapshot = describeSnapshotsResponse.DBSnapshots.FirstOrDefault();
                    if (snapshot != null)
                    {
                        // Check if the snapshot is in the "available" state
                        if (snapshot.Status.Equals("available", StringComparison.OrdinalIgnoreCase))
                        {
                            available = true;
                            running = false;
                        }
                    }

                    // Check if more than Timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: Snapshot not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        throw new Exception("Checking Snapshot Available timeout");
                    }

                    // Wait for 5 seconds before the next attempt
                    Thread.Sleep(5000);
                }

                return available;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Check Snapshot Available. {ex.Message}");
            }
        }

        public static void CreateDatabase(string host, string dbName, string passwordConnect)
        {
            try
            {
                var connectionString = $"Host={host};Username=postgres;Password=Emr!23456789;Port=5432";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the database with the given tenantId already exists
                    using (var checkCommand = new NpgsqlCommand())
                    {
                        checkCommand.Connection = connection;
                        checkCommand.CommandText = $"SELECT datname FROM pg_database WHERE datname = '{dbName}'";

                        var existingDatabase = checkCommand.ExecuteScalar();

                        if (existingDatabase != null && existingDatabase.ToString() == dbName)
                        {
                            Console.WriteLine($"Database '{dbName}' already exists.");
                            return;
                        }
                    }
                    // If everything is okay, create the database
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"CREATE DATABASE {dbName}; CREATE ROLE {dbName} LOGIN PASSWORD '{passwordConnect}'; GRANT All ON ALL TABLES IN SCHEMA public TO {dbName};";
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Database '{dbName}' created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error CreateDatabase: {ex.Message}");
                throw new Exception($"CreateDatabase. {ex.Message}");
            }
        }

        public static void GenerateDumpfile(string tenantId)
        {
            try
            {
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "gdump.sql");
                string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", $"{tenantId}_dump.sql");

                string content = File.ReadAllText(templatePath);
                content = content.Replace("dumpdb.", $"{tenantId}.");

                File.WriteAllText(outputPath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static async Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier, string snapshotType)
        {
            try
            {
                // Assuming you have AWS credentials set up (access key and secret key)
                var rdsClient = new AmazonRDSClient();

                // Create a request to create a DB snapshot
                var createSnapshotRequest = new CreateDBSnapshotRequest
                {
                    DBSnapshotIdentifier = GenareateDBSnapshotIdentifier(dbInstanceIdentifier, snapshotType),
                    DBInstanceIdentifier = dbInstanceIdentifier
                };

                // Call the CreateDBSnapshotAsync method to asynchronously create the snapshot
                var response = await rdsClient.CreateDBSnapshotAsync(createSnapshotRequest);

                // Check the response for success
                if (response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.DBSnapshot.DBSnapshotIdentifier;
                }
                else
                {
                    Console.WriteLine($"DB snapshot creation failed. Response: {response}");
                    throw new Exception($"Create DB Snapshot. Code: {response?.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Create DB Snapshot: {ex.Message}");
            }
        }

        public static async Task<bool> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            try
            {
                var rdsClient = new AmazonRDSClient();
                var vpcSecurityGroupIds = new List<string> { "sg-0cc9111542280b236" };
                var response = await rdsClient.RestoreDBInstanceFromDBSnapshotAsync(
                    new RestoreDBInstanceFromDBSnapshotRequest
                    {
                        DBInstanceIdentifier = dbInstanceIdentifier,
                        DBSnapshotIdentifier = snapshotIdentifier,
                        DBSubnetGroupName = "develop-smartkarte-rds-subnetgroup",  // Todo update
                        VpcSecurityGroupIds = vpcSecurityGroupIds,  // Todo update
                        DBInstanceClass = "db.t4g.micro" // Todo update
                    });
                if (response?.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Restore DBInstance From Snapshot. Code: {response?.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Restore DBInstance From Snapshot. {ex.Message}");
            }
        }

        static string GenareateDBSnapshotIdentifier(string dbInstanceIdentifier, string snapshotType)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            string dbSnapshotIdentifier = $"{dbInstanceIdentifier}-{snapshotType}-{timestamp}";

            dbSnapshotIdentifier = string.Join("", dbSnapshotIdentifier.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            if (!char.IsLetter(dbSnapshotIdentifier[0]))
            {
                dbSnapshotIdentifier = "A" + dbSnapshotIdentifier.Substring(1);
            }

            dbSnapshotIdentifier = dbSnapshotIdentifier.TrimEnd('-');
            dbSnapshotIdentifier = dbSnapshotIdentifier.Length > 63 ? dbSnapshotIdentifier.Substring(0, 63) : dbSnapshotIdentifier;
            return dbSnapshotIdentifier;
        }

        public static async Task<List<string>> GetListDatabase(string serverEndpoint)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                int port = 5432;
                // Connection string format for PostgreSQL
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";
                var withOutDb = ConfigConstant.LISTSYSTEMDB;
                string strWithoutDb = string.Join(", ", withOutDb);
                strWithoutDb = "'" + strWithoutDb.Replace(", ", "', '") + "'";
                List<string> databaseList = new List<string>();

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Select databases
                        using (NpgsqlCommand command = new NpgsqlCommand($"SELECT datname FROM pg_catalog.pg_database WHERE datname NOT IN ({strWithoutDb}) AND NOT datistemplate", connection))
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string dbName = reader.GetString(0);
                                databaseList.Add(dbName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Get List Database connection  failed: {ex.Message}");
                    }
                }

                return databaseList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Get List Database. {ex.Message}");
            }
        }

        /// <summary>
        /// Delete RDS instance
        /// </summary>
        /// <param name="dbInstanceIdentifier"></param>
        /// <param name="SkipFinalSnapshot">Delete without create snapshot backup</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> DeleteRDSInstanceAsync(string dbInstanceIdentifier, bool SkipFinalSnapshot = false)
        {
            try
            {
                var rdsClient = new AmazonRDSClient();

                var deleteRequest = new DeleteDBInstanceRequest
                {
                    DBInstanceIdentifier = dbInstanceIdentifier,
                    SkipFinalSnapshot = SkipFinalSnapshot
                };

                if (!SkipFinalSnapshot)
                {
                    deleteRequest.FinalDBSnapshotIdentifier = GenareateDBSnapshotIdentifier(dbInstanceIdentifier, ConfigConstant.RdsSnapshotBackupTermiante);
                }

                var response = await rdsClient.DeleteDBInstanceAsync(deleteRequest);

                // Check if the HTTP status code indicates success
                if (response?.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Delete RDS Instance. Code: {response?.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Delete RDS Instance. {ex.Message}");
                throw new Exception($"Delete RDS Instance. {ex.Message}");
            }
        }

        public static async Task<bool> CheckRDSInstanceDeleted(string dbInstanceIdentifier)
        {
            try
            {
                var startTime = DateTime.Now;
                var timeout = TimeSpan.FromMinutes(ConfigConstant.TimeoutCheckingAvailable);
                while ((DateTime.Now - startTime) < timeout)
                {
                    var rdsClient = new AmazonRDSClient();

                    var describeRequest = new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbInstanceIdentifier
                    };

                    var describeResponse = await rdsClient.DescribeDBInstancesAsync(describeRequest);

                    // Check if the instance doesn't exist (status will be null if it doesn't)
                    if (!describeResponse.DBInstances.Any())
                    {
                        return true;
                    }

                    var instanceStatus = describeResponse.DBInstances[0].DBInstanceStatus;

                    // Check if the status is "deleting" or "deleted"
                    if (instanceStatus.Equals("deleting", StringComparison.OrdinalIgnoreCase) ||
                        instanceStatus.Equals("deleted", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    // Wait for a short duration before the next attempt
                    await Task.Delay(5000); // 5 seconds delay, adjust as needed
                }

                // If the loop runs for the entire timeout duration, return false
                throw new Exception("Checking Deleted RDSInstance timeout");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Checking Deleted RDSInstance. {ex.Message}");
            }
        }

        public static async Task<string> GetLastSnapshot(string dbInstanceIdentifier)
        {
            try
            {
                // Create an RDS client
                var rdsClient = new AmazonRDSClient();

                // Create a request to describe DB snapshots
                var describeSnapshotsRequest = new DescribeDBSnapshotsRequest
                {
                    DBInstanceIdentifier = dbInstanceIdentifier,
                    MaxRecords = 20,  // Limit 20 to get the latest snapshot
                };

                // Call DescribeDBSnapshots to get information about the snapshots
                var describeSnapshotsResponse = await rdsClient.DescribeDBSnapshotsAsync(describeSnapshotsRequest);

                // Extract information about the latest snapshot
                var snapshot = describeSnapshotsResponse.DBSnapshots.OrderByDescending(x => x.SnapshotCreateTime).FirstOrDefault();
                if (snapshot != null && snapshot.Status.Equals("available", StringComparison.OrdinalIgnoreCase))
                {
                    return snapshot.DBSnapshotIdentifier;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Get Last Snapshot. {ex.Message}");
            }
        }
    }
}