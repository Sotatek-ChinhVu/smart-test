using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class UserInfoService : RepositoryBase, IUserInfoService
    {
        private List<UserMst> _userInfoList = new();
        private readonly ITenantProvider _tenantProvider;
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public UserInfoService(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            _tenantProvider = tenantProvider;
            key = GetCacheKey();
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
            var userInfo = _userInfoList.FirstOrDefault(u => u.UserId == id);
            if (userInfo == null)
            {
                return string.Empty;
            }
            return userInfo.Sname ?? string.Empty;
        }

        public string GetFullNameById(int id)
        {
            var userInfo = _userInfoList.FirstOrDefault(u => u.UserId == id);
            if (userInfo == null)
            {
                return string.Empty;
            }
            return userInfo.Name ?? string.Empty;
        }

        public void Reload()
        {
            // check if cache exists, load data from cache
            string finalKey = key + CacheKeyConstant.UserInfoCacheService;
            if (_cache.KeyExists(finalKey))
            {
                var stringJson = _cache.StringGet(finalKey).AsString();
                if (!string.IsNullOrEmpty(stringJson))
                {
                    _userInfoList = JsonSerializer.Deserialize<List<UserMst>>(stringJson) ?? new();
                    return;
                }
            }

            // if cache does not exists, get data from database then set to cache
            _userInfoList = _tenantProvider.GetNoTrackingDataContext().UserMsts.ToList();
            var jsonUserList = JsonSerializer.Serialize(_userInfoList);
            _cache.StringSet(finalKey, jsonUserList);
        }

        public void DisposeSource()
        {
            _tenantProvider.DisposeDataContext();
        }
    }
}
