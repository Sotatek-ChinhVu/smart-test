using Domain.Models.JsonSetting;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class JsonSettingRepository : RepositoryBase, IJsonSettingRepository
{
    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;
    public JsonSettingRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetCacheKey() + CacheKeyConstant.JsonSettings;
        _configuration = configuration;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    public void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    public JsonSettingModel? Get(int userId, string key)
    {
        var entity = NoTrackingDataContext.JsonSettings.FirstOrDefault(e => e.UserId == userId && e.Key == key);
        return entity is null ? null : ToModel(entity);
    }

    public List<JsonSettingModel> GetListFollowUserId(int userId)
    {
        var finalKey = key + "_" + userId;
        IEnumerable<JsonSetting> entities;
        if (!_cache.KeyExists(finalKey))
        {
            entities = ReloadCache(userId);
        }
        else
        {
            entities = ReadCache(userId);
        }
        return entities?.Select(e => ToModel(e)).ToList() ?? new();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void Upsert(JsonSettingModel model)
    {
        var existingEntity = TrackingDataContext.JsonSettings.AsTracking()
            .FirstOrDefault(e => e.UserId == model.UserId && e.Key == model.Key);
        if (existingEntity is null)
        {
            TrackingDataContext.JsonSettings.Add(new JsonSetting
            {
                UserId = model.UserId,
                Key = model.Key,
                Value = model.Value
            });
        }
        else
        {
            existingEntity.Value = model.Value;
        }

        TrackingDataContext.SaveChanges();
        ReloadCache(model.UserId);
    }

    private JsonSettingModel ToModel(JsonSetting entity)
    {
        return new JsonSettingModel(entity.UserId, entity.Key, entity.Value);
    }

    private IEnumerable<JsonSetting> ReloadCache(int userId)
    {
        var finalKey = key + "_" + userId;
        var entities = NoTrackingDataContext.JsonSettings.Where(e => e.UserId == userId).ToList();
        var json = JsonSerializer.Serialize(entities);
        _cache.StringSet(finalKey, json);

        return entities;
    }

    private IEnumerable<JsonSetting> ReadCache(int userId)
    {
        var finalKey = key + "_" + userId;
        var results = _cache.StringGet(finalKey);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<JsonSetting>>(json) : new();
        return datas ?? new();
    }
}
