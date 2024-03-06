using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Common;
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

public class PtCmtInfRepository : RepositoryBase, IPtCmtInfRepository
{

    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;
    public PtCmtInfRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetDomainKey();
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

    public List<PtCmtInfModel> GetList(long ptId, int hpId)
    {
        List<PtCmtInfModel> ptCmts;

        // If exist cache, get data from cache then return data
        var finalKey = key + CacheKeyConstant.PtCmtInfGetList + "_" + hpId + "_" + ptId;
        if (_cache.KeyExists(finalKey))
        {
            var cacheString = _cache.StringGet(finalKey).AsString();
            ptCmts = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtCmtInfModel>>(cacheString) ?? new() : new();
        }
        else
        {
            // If not, get data from database
            ptCmts = NoTrackingDataContext.PtCmtInfs.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0)
                                                    .OrderByDescending(p => p.UpdateDate)
                                                    .Select(x => new PtCmtInfModel(
                                                        x.HpId,
                                                        x.PtId,
                                                        x.SeqNo,
                                                        x.Text ?? String.Empty,
                                                        x.IsDeleted,
                                                        x.Id
                                                    )).ToList();
            // Set data to new cache
            var json = JsonSerializer.Serialize(ptCmts);
            _cache.StringSet(finalKey, json);
        }
        return ptCmts;
    }

    public PtCmtInfModel GetPtCmtInfo(int hpId, long ptId)
    {
        var result = NoTrackingDataContext.PtCmtInfs
                           .Where(u => u.HpId == hpId && u.PtId == ptId && u.IsDeleted == 0)
                           .OrderByDescending(u => u.UpdateDate)
                           .AsEnumerable()
                           .Select(u => new PtCmtInfModel(
                                u.HpId,
                                u.PtId,
                                u.SeqNo,
                                u.Text ?? string.Empty,
                                u.IsDeleted,
                                u.Id
                               ))
                           .FirstOrDefault();
        return result ?? new PtCmtInfModel();
    }

    /// <summary>
    /// using hpId to get data
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="text"></param>
    /// <param name="userId"></param>
    public void Upsert(int hpId, long ptId, string text, int userId)
    {
        var ptCmtList = TrackingDataContext.PtCmtInfs.AsTracking()
            .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
            .ToList();

        if (ptCmtList.Count != 1)
        {
            foreach (var ptCmt in ptCmtList)
            {
                ptCmt.IsDeleted = 1;
            }

            TrackingDataContext.PtCmtInfs.Add(new PtCmtInf
            {
                HpId = hpId,
                PtId = ptId,
                Text = text,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                CreateId = userId
            });
        }
        else
        {
            var ptCmt = ptCmtList[0];

            ptCmt.Text = text;
            ptCmt.UpdateDate = CIUtil.GetJapanDateTimeNow();
            ptCmt.UpdateId = userId;
        }

        // delete cache key when save SavePtCmtInfItems
        string finalKey = key + CacheKeyConstant.PtCmtInfGetList + "_" + hpId + "_" + ptId;
        if (_cache.KeyExists(finalKey))
        {
            _cache.KeyDelete(finalKey);
        }
        TrackingDataContext.SaveChanges();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
