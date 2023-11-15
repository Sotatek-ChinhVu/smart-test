using Helper.Extension;
using StackExchange.Redis;

namespace Helper.Redis
{
    public static class RedisConnectorHelper
    {
        public static string RedisHost = string.Empty;

        static RedisConnectorHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var strs = RedisHost.Split(":");
                string host = string.Empty;
                int port = -1;
                var config = new ConfigurationOptions
                {
                    AbortOnConnectFail = false
                };
                if (strs.Count() == 2)
                {
                    host = strs.FirstOrDefault()?.ToString() ?? string.Empty;
                    port = strs.LastOrDefault()?.AsInteger() ?? 0;
                }
                config.EndPoints.Add(host, port);

                var taskConnection = ConnectionMultiplexer.ConnectAsync(config);
                Task.WaitAll(taskConnection);
                var connection = taskConnection.Result;

                connection.ConnectionFailed += (_, e) =>
                {
                    Console.WriteLine("Connection to Redis failed in help.");
                };

                if (!connection.IsConnected)
                {
                    Console.WriteLine("Did not connect to Redis in help.");
                }
                else
                {
                    Console.WriteLine("Connected to Redis in help.");
                }
                return connection;
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
