using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Helper.Common;

namespace AWSSDK.Common
{
    public static class CloudWatchAction
    {
        public static async Task<double> GetFreeableMemoryAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_FreeableMemory",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "FreeableMemory",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Average"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };
                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);
                double averageValue = response.MetricDataResults[0].Values.Average();
                Console.WriteLine(averageValue.ToString());
                return averageValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }
        public static async Task<double> GetFreeStorageSpaceAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_FreeStorageSpace",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "FreeStorageSpace",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Minimum"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };

                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);

                double minValue = response.MetricDataResults[0].Values.Min();

                return minValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }

        public static async Task<double> GetConnectionNumberAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_DatabaseConnections",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "DatabaseConnections",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Average"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };

                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);

                double averageValue = response.MetricDataResults[0].Values.Average();

                return averageValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }

        public static async Task<double> GetCPUUtilizationAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_CPUUtilization",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "CPUUtilization",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Maximum"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };

                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);

                double maxValue = response.MetricDataResults[0].Values.Max();

                return maxValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }

        public static async Task<double> GetReadIOPSAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_ReadIOPS",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "ReadIOPS",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Maximum"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };

                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);

                double maxValue = response.MetricDataResults[0].Values.Max();

                return maxValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }

        public static async Task<double> GetWriteIOPSAsync(string rdsIdentifier)
        {
            try
            {
                DateTime startTime = CIUtil.GetJapanDateTimeNow() - TimeSpan.FromSeconds(300 * 12);

                var cloudWatch = new AmazonCloudWatchClient();
                var metricDataQueries = new MetricDataQuery
                {
                    Id = "fetching_WriteIOPS",
                    MetricStat = new MetricStat
                    {
                        Metric = new Metric
                        {
                            Namespace = "AWS/RDS",
                            MetricName = "WriteIOPS",
                            Dimensions = new List<Dimension>
                        {
                            new Dimension
                            {
                                Name = "DBInstanceIdentifier",
                                Value = rdsIdentifier
                            }
                        }
                        },
                        Period = 300,
                        Stat = "Maximum"
                    }
                };

                var getMetricDataRequest = new GetMetricDataRequest
                {
                    MetricDataQueries = new List<MetricDataQuery> { metricDataQueries },
                    StartTime = startTime,
                    EndTime = CIUtil.GetJapanDateTimeNow(),
                    ScanBy = "TimestampDescending"
                };

                var response = await cloudWatch.GetMetricDataAsync(getMetricDataRequest);

                double maxValue = response.MetricDataResults[0].Values.Max();

                return maxValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0.0;
            }
        }

        public static async Task<Dictionary<string, Dictionary<string, string>>> GetSummaryCardAsync()
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> card = new Dictionary<string, Dictionary<string, string>>();
                var rdsInformation = await RDSAction.GetRDSInformation();
                foreach (var entry in rdsInformation)
                {
                    string key = entry.Key;
                    var connectionNumber = await GetConnectionNumberAsync(key);
                    var freeStorageSpace = await GetFreeStorageSpaceAsync(key);
                    var freeableMemory = await GetFreeableMemoryAsync(key);
                    var cPUUtilization = await GetCPUUtilizationAsync(key);
                    var readIOPS = await GetReadIOPSAsync(key);
                    var writeIOPS = await GetWriteIOPSAsync(key);

                    double avgConn = connectionNumber / 1000 * 100;
                    double avgFreeStorage = freeStorageSpace / (1024 * 1000) / 100000 * 100;
                    double avgFreeMem = freeableMemory / (1024 * 1000) / 1024 * 100;
                    double avgCPU = cPUUtilization;
                    double avgReadIOPS = readIOPS / 300 * 100;
                    double avgWriteIOPS = writeIOPS / 300 * 100;

                    string cAvailable = (avgConn > 80 || avgFreeStorage < 80
                                         || avgFreeMem < 40 || avgCPU < 40
                                         || avgReadIOPS < 80 || avgWriteIOPS < 80) ? "no" : "yes";

                    Dictionary<string, string> rdsCard = new Dictionary<string, string>
                {
                    { "available", cAvailable }
                };

                    card[key] = rdsCard;
                }

                return card;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}