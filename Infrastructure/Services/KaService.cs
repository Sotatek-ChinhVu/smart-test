using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;

public class KaService : RepositoryBase, IKaService
{
    private List<KaMst> _kaInfoList = new();
    private readonly ITenantProvider _tenantProvider;
    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;

    public KaService(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        _tenantProvider = tenantProvider;
        key = GetDomainKey();
        _configuration = configuration;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
        Reload();
    }

    public void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    public string GetNameById(int id)
    {
        var kaInfo = _kaInfoList.FirstOrDefault(u => u.KaId == id);
        if (kaInfo == null)
        {
            return string.Empty;
        }
        return kaInfo.KaSname ?? string.Empty;
    }

    public void Reload()
    {
        // check if cache exists, load data from cache
        string finalKey = key + CacheKeyConstant.KaCacheService;
        if (_cache.KeyExists(finalKey))
        {
            var stringJson = _cache.StringGet(finalKey).AsString();
            if (!string.IsNullOrEmpty(stringJson))
            {
                _kaInfoList = JsonSerializer.Deserialize<List<KaMst>>(stringJson) ?? new();
                return;
            }
        }

        // if cache does not exists, get data from database then set to cache
        _kaInfoList = _tenantProvider.GetNoTrackingDataContext().KaMsts.ToList();
        var jsonKaList = JsonSerializer.Serialize(_kaInfoList);
        _cache.StringSet(finalKey, jsonKaList);
    }

    public List<KaMst> AllKaMstList()
    {
        return _kaInfoList;
    }

    public void DisposeSource()
    {
        _tenantProvider.DisposeDataContext();
    }
}
