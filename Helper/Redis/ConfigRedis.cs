using StackExchange.Redis;

namespace Helper.Redis
{
    public class RedisConnectorHelper
    {
        public static string RedisHost = string.Empty;

        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var connection = ConnectionMultiplexer.Connect(RedisHost);
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
