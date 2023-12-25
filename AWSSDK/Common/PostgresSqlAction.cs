﻿using AWSSDK.Constants;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace AWSSDK.Common
{
    public static class PostgresSqlAction
    {
        /// <summary>
        /// Genarate conent script dump db from tmp RDS
        /// </summary>
        /// <param name="outFile"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async static Task PostgreSqlDump(string outFile, string host, int port, string database, string user, string password)
        {
            Console.WriteLine($"Start: run  PostgreSqlDump");
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
            outFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? outFile : outFile.Replace("\\", "/");
            string batchContent;

            string dumpCommand =
                 $"{Set} PGPASSWORD={password}\n" +
                 $"pg_dump" + " -Fc" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // path file windown
                batchContent = "" + dumpCommand + "  > " + "\"" + outFile + "\"" + "\n";
            }
            else
            {
                // path file linux
                batchContent = "" + dumpCommand + "  > " + outFile + " 2> /app/restore/log-create-dump.txt" + "\n";
            }
            if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);

            await Execute(batchContent);
        }

        /// <summary>
        ///  Genarate conent script resore db from file sql dump
        /// </summary>
        /// <param name="pathFileDump"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task PostgreSqlExcuteFileDump(string pathFileDump, string host, int port, string database, string user, string password)
        {
            Console.WriteLine($"Start: run  PostgreSqlExcuteFileDump");
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
            pathFileDump = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? pathFileDump : pathFileDump.Replace("\\", "/");
            string batchContent;
            string dumpCommand =
                 $"{Set} PGPASSWORD={password}\n" +
                 $"pg_restore" + " -F c" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // path file window
                batchContent = "" + dumpCommand + "  -c -v " + "\"" + pathFileDump + "\"" + "\n";
            }
            else
            {   // path file linux
                batchContent = "" + dumpCommand + "  -c -v " + pathFileDump + " 2> /app/restore/log-excute-dump.txt" + "\n";
            }

            await Execute(batchContent);
        }

        /// <summary>
        /// Execute list file script sql
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public static void ExecuteSqlFiles(string[] filePaths, string host, int port, string database, string user, string password)
        {
            string connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";
            string filePathRun = string.Empty;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Begin a transaction
                    using (NpgsqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Execute all SQL files in a transaction
                            foreach (var filePath in filePaths)
                            {
                                filePathRun = filePath;
                                // Read the content of the SQL file
                                string sqlScript;
                                using (StreamReader reader = new StreamReader(filePath))
                                {
                                    sqlScript = reader.ReadToEnd();
                                }

                                // Execute the SQL command
                                using (NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection, transaction))
                                {
                                    command.CommandType = CommandType.Text;
                                    command.ExecuteNonQuery();
                                }

                                // Save SYSTEM_CHANGE_LOG
                                using (NpgsqlCommand command = new NpgsqlCommand(QueryConstant.SaveSystemChangeLog, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@FileName", filePath);
                                    command.Parameters.AddWithValue("@IsPG", 1);
                                    command.Parameters.AddWithValue("@IsDB", 1);
                                    command.Parameters.AddWithValue("@IsMaster", 1);
                                    command.Parameters.AddWithValue("@IsNote", 0);
                                    command.Parameters.AddWithValue("@Status", 9);
                                    command.Parameters.AddWithValue("@ErrMessage", "");
                                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                                    command.Parameters.AddWithValue("@IsRun", 0);
                                    command.Parameters.AddWithValue("@IsDrugPhoto", 0);

                                    command.ExecuteNonQuery();
                                }
                            }

                            // If everything is successful, commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // If there's an error, rollback the transaction
                            transaction.Rollback();
                            Console.WriteLine($"Error executing SQL files: {ex.Message}");
                            // Save SYSTEM_CHANGE_LOG
                            using (NpgsqlCommand command = new NpgsqlCommand(QueryConstant.SaveSystemChangeLog, connection))
                            {
                                command.Parameters.AddWithValue("@FileName", filePathRun);
                                command.Parameters.AddWithValue("@IsPG", 1);
                                command.Parameters.AddWithValue("@IsDB", 1);
                                command.Parameters.AddWithValue("@IsMaster", 1);
                                command.Parameters.AddWithValue("@IsNote", 0);
                                command.Parameters.AddWithValue("@Status", 9);
                                command.Parameters.AddWithValue("@ErrMessage", ex.Message);
                                command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                                command.Parameters.AddWithValue("@IsRun", 0);
                                command.Parameters.AddWithValue("@IsDrugPhoto", 0);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }


        /// <summary>
        ///  Create file .sh / .bat to execute conent script sql
        /// </summary>
        /// <param name="dumpCommand"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Task Execute(string dumpCommand)
        {
            return Task.Run(() =>
            {
                string batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
                try
                {
                    string batchContent = "";
                    batchContent += $"{dumpCommand}";

                    System.IO.File.WriteAllText(batFilePath, batchContent.ToString(), Encoding.ASCII);

                    ProcessStartInfo info = ProcessInfoByOS(batFilePath);

                    var proc = System.Diagnostics.Process.Start(info);
                    Console.WriteLine("Start...");
                    proc?.WaitForExit();
                    var exit = proc?.ExitCode;
                    Console.WriteLine("End...");
                    proc?.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw new Exception($"Execute sql Dump. {ex.Message}");

                }
                finally
                {
                    //if (System.IO.File.Exists(batFilePath)) System.IO.File.Delete(batFilePath);
                }
            });
        }

        /// <summary>
        /// Get process info
        /// </summary>
        /// <param name="batFilePath"></param>
        /// <returns></returns>
        private static ProcessStartInfo ProcessInfoByOS(string batFilePath)
        {
            ProcessStartInfo info;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                info = new ProcessStartInfo(batFilePath)
                {
                    Arguments = $"{batFilePath}"
                };
            }
            else
            {
                info = new ProcessStartInfo("sh")
                {
                    Arguments = $"{batFilePath}"
                };
            }
            //info.EnvironmentVariables.Add("PGPASSWORD", "1234$");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;

            return info;
        }

        /// <summary>
        /// Checking file is still being accessed or modified
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool CheckingFinishedAccessedFile(string filePath)
        {
            // Check if the file is still being accessed or modified
            DateTime startTime = DateTime.Now;
            bool running = true;
            while (running)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // If the FileStream is successfully opened, it means the file is not being written to
                        running = false;
                        return true;
                    }
                }
                catch (IOException)
                {
                    // If an IOException occurs, the file is still being written to
                    Thread.Sleep(3000);
                }

                // Check if more than Timeout
                if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                {
                    Console.WriteLine($"Timeout: Checking Finished Accessed File {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                    throw new Exception($"Checking Finished Accessed File: {filePath} timeout");
                }
            }
            return false;
        }

        #region insert data master
        public static async Task PostgreSqlExcuteFileSQLDataMaster(string pathFileDump, string host, int port, string database, string user, string password)
        {
            Console.WriteLine($"Start: run  PostgreSqlExcuteFileDataMaster");
            string Set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
            pathFileDump = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? pathFileDump : pathFileDump.Replace("\\", "/");
            string dumpCommand =
                $"echo \"scipt chay\" >> /app/test.txtttt\n" +
                 $"{Set} PGPASSWORD={password}\n";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // path file window
                dumpCommand = dumpCommand + $"psql -h {host} -p {port} -U {user} -d {database} -c \"SET client_encoding = 'UTF8';\" -f {pathFileDump}";
            }
            else
            {   // path file linux
                dumpCommand = dumpCommand + $"psql" + " -h " + host + " -p " + port + " -U " + user + " -d " + database + " -c \"SET client_encoding = 'UTF8';\"" + " -f " + pathFileDump;
            }
            await ExecuteDataMaster(dumpCommand);
        }

        private static Task ExecuteDataMaster(string dumpCommand)
        {
            return Task.Run(() =>
            {
                string batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
                try
                {
                    string batchContent = "";
                    batchContent += $"{dumpCommand}";

                    System.IO.File.WriteAllText(batFilePath, batchContent.ToString(), Encoding.ASCII);
                    Console.WriteLine(batFilePath);
                    Console.WriteLine(batchContent);
                    Console.WriteLine("Check access file: " + CheckingFinishedAccessedFile(batFilePath).ToString());
                    ProcessStartInfo info = ProcessInfoByOS(batFilePath);
                    using (Process proc = new Process())
                    {
                        proc.StartInfo = info;
                        proc.Start();
                        Console.WriteLine("Process Start");
                        proc.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");
                        proc.BeginOutputReadLine();
                        proc.BeginErrorReadLine();
                        proc.WaitForExit();
                        Console.WriteLine("Process End");
                        var exitCode = proc.ExitCode;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw new Exception($"Execute sql insert data master. {ex.Message}");

                }
                finally
                {
                }
            });
        }
        #endregion insert data master
    }
}
