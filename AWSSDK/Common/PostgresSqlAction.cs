﻿using AWSSDK.Constants;
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

                    // Create process Grant execute permissions to file .sh
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Grant execute permissions using chmod
                        ProcessStartInfo chmodInfo = new ProcessStartInfo
                        {
                            FileName = "chmod",
                            Arguments = $"+x {batFilePath}",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (System.Diagnostics.Process chmodProc = System.Diagnostics.Process.Start(chmodInfo))
                        {
                            chmodProc.WaitForExit();

                            if (chmodProc.ExitCode != 0)
                            {
                                // Handle chmod error, if any
                                string errorOutput = chmodProc.StandardError.ReadToEnd();
                                Console.WriteLine($"chmod error: {errorOutput}");
                                throw new Exception($"Failed to grant execute permissions to the script: {errorOutput}");
                            }
                        }
                    }

                    ProcessStartInfo info = ProcessInfoByOS(batFilePath);

                    using System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);

                    proc.WaitForExit();
                    var exit = proc.ExitCode;


                    proc.Close();
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
    }
}