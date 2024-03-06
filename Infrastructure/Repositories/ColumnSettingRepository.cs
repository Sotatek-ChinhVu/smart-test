using Domain.Models.ColumnSetting;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class ColumnSettingRepository : RepositoryBase, IColumnSettingRepository
{
    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;
    public ColumnSettingRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetCacheKey() + CacheKeyConstant.ColumnSetting;
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
    public List<ColumnSettingModel> GetList(int hpId, int userId, string tableName)
    {
        return NoTrackingDataContext.ColumnSettings
            .Where(c => c.HpId == hpId && c.UserId == userId && c.TableName == tableName)
            .AsEnumerable().Select(c => ToModel(c)).ToList();
    }

    public Dictionary<string, List<ColumnSettingModel>> GetList(int hpId, int userId, List<string> tableNameList)
    {
        var finalKey = key + "_" + hpId + "_" + userId;
        tableNameList = tableNameList.Distinct().ToList();
        IEnumerable<ColumnSetting> columnSettingList;
        if (!_cache.KeyExists(finalKey))
        {
            columnSettingList = ReloadCache(hpId, userId);
        }
        else
        {
            columnSettingList = ReadCache(hpId, userId);
        }
        columnSettingList = columnSettingList!.Where(item => tableNameList.Contains(item.TableName))
                                                                    .ToList();
        var result = tableNameList.ToDictionary(tableName => tableName, tableName => columnSettingList
                                                                    .Where(item => item.TableName == tableName)
                                                                    .Select(item => ToModel(item))
                                                                    .ToList());
        return result;
    }
    private IEnumerable<ColumnSetting> ReloadCache(int hpId, int userId)
    {
        var finalKey = key + "_" + hpId + "_" + userId;
        var columnSettingList = NoTrackingDataContext.ColumnSettings.Where(item => item.HpId == hpId && item.UserId == userId).ToList();
        var json = JsonSerializer.Serialize(columnSettingList);
        _cache.StringSet(finalKey, json);

        return columnSettingList;
    }
    private IEnumerable<ColumnSetting> ReadCache(int hpId, int userId)
    {
        var finalKey = key + "_" + hpId + "_" + userId;
        var results = _cache.StringGet(finalKey);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<ColumnSetting>>(json) : new();
        return datas ?? new();
    }

    public bool SaveList(List<ColumnSettingModel> settingModels)
    {
        if (settingModels.Count == 0)
        {
            return true;
        }

        var hpId = settingModels.First().HpId;
        var userId = settingModels.First().UserId;
        var tableName = settingModels.First().TableName;

        var unrelatedSetting = settingModels.FirstOrDefault(m => m.UserId != userId || m.TableName != tableName);
        if (unrelatedSetting is not null)
        {
            return false;
        }

        var existingSettings = TrackingDataContext.ColumnSettings
            .Where(c => c.HpId == hpId && c.UserId == userId && c.TableName == tableName).ToList();
        TrackingDataContext.ColumnSettings.RemoveRange(existingSettings);

        var newSettings = settingModels.Select(m => ToEntity(m));
        TrackingDataContext.ColumnSettings.AddRange(newSettings);

        TrackingDataContext.SaveChanges();
        ReloadCache(hpId, userId);
        return true;
    }

    private ColumnSettingModel ToModel(ColumnSetting c)
    {
        return new ColumnSettingModel(c.HpId, c.UserId, c.TableName,
            c.ColumnName, c.DisplayOrder, c.IsPinned, c.IsHidden, c.Width, c.OrderBy);
    }

    private ColumnSetting ToEntity(ColumnSettingModel model)
    {
        return new ColumnSetting
        {
            HpId = model.HpId,
            UserId = model.UserId,
            TableName = model.TableName,
            ColumnName = model.ColumnName,
            DisplayOrder = model.DisplayOrder,
            IsPinned = model.IsPinned,
            IsHidden = model.IsHidden,
            Width = model.Width,
            OrderBy = model.OrderBy
        };
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
