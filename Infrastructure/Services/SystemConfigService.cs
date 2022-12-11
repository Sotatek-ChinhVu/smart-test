using Entity.Tenant;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class SystemConfigService : ISystemConfigService
    {
        private List<SystemConf> _systemConfigList;
        //private readonly string _cacheKey;
        private readonly ITenantProvider _tenantProvider;
        //private readonly IMemoryCache _memoryCache;
        public SystemConfigService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
            //_memoryCache = memoryCache;
            //_cacheKey = "SystemConfig-" + tenantProvider.GetClinicID();
            //if (!memoryCache.TryGetValue(_cacheKey, out _systemConfigList))
            //{
            Reload();
            //}
        }

        public void Reload()
        {
            _systemConfigList = _tenantProvider.GetNoTrackingDataContext().SystemConfs.ToList();
            //_memoryCache.Set(_cacheKey, _systemConfigList);
        }

        public double GetConfigValue(int grpCd, int grpEdaNo)
        {
            var userInfo = _systemConfigList.FirstOrDefault(u => u.GrpCd == grpCd && u.GrpEdaNo == grpEdaNo);
            if (userInfo == null)
            {
                return 0;
            }
            return userInfo.Val;
        }
    }
}
