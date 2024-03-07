using Domain.Models.SystemGenerationConf;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class SystemGenerationConfRepository : RepositoryBase, ISystemGenerationConfRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public SystemGenerationConfRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
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

        public (int, string) GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int presentDate = 0, int defaultValue = 0, string defaultParam = "", bool fromLastestDb = false)
        {
            SystemGenerationConf? systemConf;
            if (!fromLastestDb)
            {
                systemConf = GetAllSystemGenerationConf(hpId).FirstOrDefault(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate);
            }
            else
            {
                systemConf = GetAllSystemGenerationConf(hpId).Where(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate)?.FirstOrDefault();
            }
            return systemConf != null ? (systemConf.Val, systemConf.Param ?? string.Empty) : (defaultValue, defaultParam);
        }

        public List<SystemGenerationConfModel> GetList(int hpId)
        {
            var result = GetAllSystemGenerationConf(hpId);
            return result.Select(r => new SystemGenerationConfModel(r.Id, r.HpId, r.GrpCd, r.GrpEdaNo, r.StartDate, r.EndDate, r.Val, r.Param ?? string.Empty, r.Biko ?? string.Empty)).ToList();
        }


        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        /// <summary>
        /// if cache exists, get data from cache, if cache does not exist, get data from database
        /// </summary>
        /// <param name="hpId"></param>
        /// <returns></returns>
        private List<SystemGenerationConf> GetAllSystemGenerationConf(int hpId)
        {
            List<SystemGenerationConf> result;
            // check if cache exists, load data from cache
            var finalKey = key + CacheKeyConstant.SystemGenerationConf + hpId;
            if (_cache.KeyExists(finalKey))
            {
                var stringJson = _cache.StringGet(finalKey).AsString();
                if (!string.IsNullOrEmpty(stringJson))
                {
                    result = JsonSerializer.Deserialize<List<SystemGenerationConf>>(stringJson) ?? new();
                    return result;
                }
            }

            // if cache does not exists, get data from database then set to cache
            result = NoTrackingDataContext.SystemGenerationConfs.Where(p => p.HpId == hpId).ToList();
            var jsonKaList = JsonSerializer.Serialize(result);
            _cache.StringSet(finalKey, jsonKaList);
            return result;
        }
    }
}
