using Domain.Models.Cacche;
using Helper.Redis;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class CacheRepository : IRemoveCacheRepository
{

    private readonly IDatabase _cache;
    public CacheRepository()
    {
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    public void RemoveAllCache()
    {
        _cache.Execute("FLUSHDB");
    }

    public bool RemoveCache(string keyCache)
    {
        bool result = false;
        if (string.IsNullOrEmpty(keyCache) && _cache.KeyExists(keyCache))
        {
            _cache.KeyDelete(keyCache);
            result = true;
        }

        return result;
    }
}
