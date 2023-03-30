using Entity.Tenant;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class KaService : IKaService
    {
        private List<KaMst> _kaInfoList = new();
        //private readonly string _cacheKey;
        private readonly ITenantProvider _tenantProvider;
        //private readonly IMemoryCache _memoryCache;
        public KaService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
            //_memoryCache = memoryCache;
            //_cacheKey = "UserInfo-" + tenantProvider.GetClinicID();
            //if (!memoryCache.TryGetValue(_cacheKey, out _userInfoList))
            //{
            Reload();
            //}
        }

        public string GetNameById(int id)
        {
            var kaInfo = _kaInfoList.FirstOrDefault(u => u.KaId == id);
            if (kaInfo == null)
            {
                return string.Empty;
            }
            return kaInfo.KaName ?? string.Empty;
        }

        public void Reload()
        {
            _kaInfoList = _tenantProvider.GetNoTrackingDataContext().KaMsts.ToList();
            //_memoryCache.Set(_cacheKey, _userInfoList);
        }
    }
}
