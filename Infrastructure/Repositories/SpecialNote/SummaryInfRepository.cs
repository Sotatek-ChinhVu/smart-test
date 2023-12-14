using Domain.Models.SpecialNote.SummaryInf;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories.SpecialNote
{
    public class SummaryInfRepository : RepositoryBase, ISummaryInfRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;
        public SummaryInfRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
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

        public SummaryInfModel Get(int hpId, long ptId)
        {
            List<SummaryInfModel> summaryInfs;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.SummaryInfGetList + "_" + hpId + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                summaryInfs = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<SummaryInfModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                summaryInfs = NoTrackingDataContext.SummaryInfs.Where(x => x.PtId == ptId && x.HpId == hpId)
                                                               .OrderByDescending(u => u.UpdateDate)
                                                               .Select(item => new SummaryInfModel(
                                                                                   item.Id,
                                                                                   item.HpId,
                                                                                   item.PtId,
                                                                                   item.SeqNo,
                                                                                   item.Text ?? string.Empty,
                                                                                   item.Rtext == null ? string.Empty : Encoding.UTF8.GetString(item.Rtext),
                                                                                   item.CreateDate,
                                                                                   item.UpdateDate
                                                                )).ToList();

                // Set data to new cache
                var json = JsonSerializer.Serialize(summaryInfs);
                _cache.StringSet(finalKey, json);
            }
            return summaryInfs.Any() ? summaryInfs.First() : new SummaryInfModel();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
