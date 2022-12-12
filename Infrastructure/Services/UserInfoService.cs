using Entity.Tenant;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class UserInfoService : IUserInfoService
    {
        private List<UserMst> _userInfoList;
        //private readonly string _cacheKey;
        private readonly ITenantProvider _tenantProvider;
        //private readonly IMemoryCache _memoryCache;
        public UserInfoService(ITenantProvider tenantProvider)
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
            var userInfo = _userInfoList.FirstOrDefault(u => u.UserId == id);
            if (userInfo == null)
            {
                return string.Empty;
            }
            return userInfo.Sname ?? string.Empty;
        }

        public void Reload()
        {
            _userInfoList = _tenantProvider.GetNoTrackingDataContext().UserMsts.Where(u => u.IsDeleted == 0).ToList();
            //_memoryCache.Set(_cacheKey, _userInfoList);
        }
    }
}
