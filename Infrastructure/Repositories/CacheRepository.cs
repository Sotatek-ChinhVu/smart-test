using Domain.Models.Cacche;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class CacheRepository : RepositoryBase, IRemoveCacheRepository
{

    private readonly IDatabase _cache;
    public CacheRepository(IConfiguration configuration, ITenantProvider tenantProvider) : base(tenantProvider)
    {
        _cache = RedisConnectorHelper.Connection.GetDatabase();
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

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
