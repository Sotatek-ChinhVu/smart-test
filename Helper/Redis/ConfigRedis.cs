using Microsoft.Extensions.Configuration;
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
                return ConnectionMultiplexer.Connect(RedisHost);
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
