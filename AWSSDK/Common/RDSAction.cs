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
                        running = false;
                    }

                    // Wait for 5 seconds before the next attempt
                    Thread.Sleep(5000);
                }

                return available;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static void CreateDatabase(string host, string subDomain, string passwordConnect)
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
                        checkCommand.CommandText = $"SELECT datname FROM pg_database WHERE datname = '{subDomain}'";

                        var existingDatabase = checkCommand.ExecuteScalar();

                        if (existingDatabase != null && existingDatabase.ToString() == subDomain)
                        {
                            Console.WriteLine($"Database '{subDomain}' already exists.");
                            return;
                        }
                    }
                    // If everything is okay, create the database
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"CREATE DATABASE {subDomain}; CREATE ROLE {subDomain} LOGIN PASSWORD '{passwordConnect}'; GRANT All ON ALL TABLES IN SCHEMA public TO {subDomain};";
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Database '{subDomain}' created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"CreateDatabase. {ex.Message}");
            }
        }

        public static void CreateDatas(string host, string tenantId, List<string> listMigration)
        {
            try
            {
                var connectionString = $"Host={host};Database={tenantId};Username=postgres;Password=Emr!23456789;Port=5432";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        _CreateTable(command, listMigration);
                        _CreateFunction(command, listMigration);
                        _CreateTrigger(command, listMigration);
                        _CreateDataMaster(command, listMigration);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Create Data.  {ex.Message}");
            }
        }
        private static void _CreateTable(NpgsqlCommand command, List<string> listMigration)
        {
            var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Table");

            if (Directory.Exists(folderPath))
            {
                var sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                if (sqlFiles.Length > 0)
                {
                    var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                    var uniqueFileNames = fileNames.Except(listMigration).ToList();

                    // insert table
                    if (uniqueFileNames.Any())
                    {
                        foreach (var fileName in uniqueFileNames)
                        {
                            var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                            if (File.Exists(filePath))
                            {
                                var sqlScript = File.ReadAllText(filePath);
                                command.CommandText = sqlScript;
                                command.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine("SQL scripts trigger executed successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Info create table: No SQL files found in the specified folder.");
                }
            }
            else
            {
                Console.WriteLine("Info create table: Specified folder not found");
            }
        }

        private static void _CreateDataMaster(NpgsqlCommand command, List<string> listMigration)
        {
            var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "DataMaster");

            if (Directory.Exists(folderPath))
            {
                var sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                if (sqlFiles.Length > 0)
                {
                    var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                    var uniqueFileNames = fileNames.Except(listMigration).ToList();

                    // insert data master
                    if (uniqueFileNames.Any())
                    {
                        foreach (var fileName in uniqueFileNames)
                        {
                            var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                            if (File.Exists(filePath))
                            {
                                var sqlScript = File.ReadAllText(filePath);
                                command.CommandText = sqlScript;
                                command.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine("SQL scripts trigger executed successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Info create data master: No SQL files found in the specified folder.");
                }
            }
            else
            {
                Console.WriteLine("Info create data master: Specified folder not found");
            }
        }

        private static void _CreateFunction(NpgsqlCommand command, List<string> listMigration)
        {
            var folderPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Function");
            if (Directory.Exists(folderPath))
            {
                var sqlFiles = Directory.GetFiles(folderPath, "*.sql");
                if (sqlFiles.Length > 0)
                {
                    var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                    var uniqueFileNames = fileNames.Except(listMigration).ToList();
                    // insert function
                    if (uniqueFileNames.Any())
                    {
                        foreach (var fileName in uniqueFileNames)
                        {
                            var filePath = Path.Combine(folderPath, $"{fileName}.sql");
                            if (File.Exists(filePath))
                            {
                                var sqlScript = File.ReadAllText(filePath);
                                command.CommandText = sqlScript;
                                command.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine("SQL scripts trigger executed successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Info create function: No SQL files found in the specified folder.");
                }
            }
            else
            {
                Console.WriteLine("Info create function: Specified folder not found");
            }
        }

        private static void _CreateTrigger(NpgsqlCommand command, List<string> listMigration)
        {
            var folderFunctionPath = Path.Combine("\\SmartKarteBE\\emr-cloud-be\\SuperAdmin\\Template", "Trigger");
            if (Directory.Exists(folderFunctionPath))
            {
                var sqlFiles = Directory.GetFiles(folderFunctionPath, "*.sql");
                if (sqlFiles.Length > 0)
                {
                    var fileNames = sqlFiles.Select(Path.GetFileNameWithoutExtension).ToList();
                    var uniqueFileNames = fileNames.Except(listMigration).ToList();
                    // insert trigger
                    if (uniqueFileNames.Any())
                    {
                        foreach (var fileName in uniqueFileNames)
                        {
                            var filePath = Path.Combine(folderFunctionPath, $"{fileName}.sql");
                            if (File.Exists(filePath))
                            {
                                var sqlScript = File.ReadAllText(filePath);
                                command.CommandText = sqlScript;
                                command.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine("SQL scripts trigger executed successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Info create trigger: No SQL files found in the specified folder.");
                }
            }
            else
            {
                Console.WriteLine("Info create trigger: Specified folder not found");
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

        public static async Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier)
        {
            try
            {
                // Assuming you have AWS credentials set up (access key and secret key)
                var rdsClient = new AmazonRDSClient();

                // Create a request to create a DB snapshot
                var createSnapshotRequest = new CreateDBSnapshotRequest
                {
                    DBSnapshotIdentifier = GenareateDBSnapshotIdentifier(dbInstanceIdentifier),
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
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return string.Empty;
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
                return response?.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        static string GenareateDBSnapshotIdentifier(string dbInstanceIdentifier)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            string dbSnapshotIdentifier = $"{dbInstanceIdentifier}-Snapshot-{timestamp}";

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
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                return databaseList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<string>();
            }
        }

        public static async Task<bool> DeleteRDSInstanceAsync(string dbInstanceIdentifier)
        {
            try
            {
                var rdsClient = new AmazonRDSClient();

                var deleteRequest = new DeleteDBInstanceRequest
                {
                    DBInstanceIdentifier = dbInstanceIdentifier,
                    SkipFinalSnapshot = false
                };

                deleteRequest.FinalDBSnapshotIdentifier = GenareateDBSnapshotIdentifier(dbInstanceIdentifier);

                var response = await rdsClient.DeleteDBInstanceAsync(deleteRequest);

                // Check if the HTTP status code indicates success
                return response?.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // TODO: Log the exception details
                Console.WriteLine($"Error: {ex.Message}");
                return false;
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
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}