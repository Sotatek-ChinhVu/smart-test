﻿using Amazon.RDS;
using Amazon.RDS.Model;
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
                return null;
            }
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
                    DBName = "smartkarte",
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
                    Console.WriteLine($"Database Shard {dbIdentifier} exists already, continuing to poll ...");
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<string> CheckingRDSStatusAsync(string dbIdentifier)
        {
            try
            {
                string host = string.Empty;
                bool running = true;

                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    var response = await rdsClient.DescribeDBInstancesAsync(new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbIdentifier
                    });

                    var dbInstances = response.DBInstances;

                    if (dbInstances.Count != 1)
                    {
                        throw new Exception("More than one Database Shard returned; this should never happen");
                    }

                    var dbInstance = dbInstances[0];
                    var status = dbInstance.DBInstanceStatus;

                    Console.WriteLine($"Last Database Shard status: {status}");

                    Thread.Sleep(5000);

                    if (status == "available")
                    {
                        var endpoint = dbInstance.Endpoint;
                        host = endpoint.Address;
                        running = false;
                    }
                }

                return host;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public static void CreateDatabase(string host, string tenantId)
        {
            try
            {
                var connectionString = $"Host={host};Username=postgres;Password=Emr!23456789;Port=5432";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"CREATE DATABASE {tenantId}";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void CreateTables(string host, string tenantId)
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

                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "gdump.sql");

                        if (File.Exists(filePath))
                        {
                            var sqlScript = File.ReadAllText(filePath);
                            command.CommandText = sqlScript;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("Error: SQL file not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
                    return response.DBSnapshot.DBInstanceIdentifier;
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

        public static async Task<string> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            return string.Empty;
            try
            {
                var rdsClient = new AmazonRDSClient();
                var response = await rdsClient.RestoreDBInstanceFromDBSnapshotAsync(
                new RestoreDBInstanceFromDBSnapshotRequest
                {
                    DBInstanceIdentifier = dbInstanceIdentifier,
                    DBSnapshotIdentifier = snapshotIdentifier,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return string.Empty;
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
    }
}