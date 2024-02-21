using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Common.Interface;

namespace Reporting.CommonMasters.Common;

public class UserMstCache : RepositoryBase, IUserMstCache
{
    public delegate void UserMstCacheDelegate();
    private static readonly object _threadsafelock = new object();

    private List<UserMst> _allOfUserMst = new();

    public UserMstCache(ITenantProvider tenantProvider) : base(tenantProvider)
    {
        RefreshData();
    }

    public void RefreshData(int hpId = 0)
    {
        lock (_threadsafelock)
        {
            if (hpId <= 0)
            {
                _allOfUserMst = NoTrackingDataContext.UserMsts.ToList();
            }
            else
            {
                _allOfUserMst = NoTrackingDataContext.UserMsts.Where(x => x.HpId == hpId).ToList();
            }
        }
    }

    public string GetUserSNameIncludedDeleted(int hpId, int userId, bool fromLastestDb = false)
    {
        if (fromLastestDb) RefreshData(hpId);

        UserMst userMst = _allOfUserMst?.FirstOrDefault(p => p.HpId == hpId && p.UserId == userId) ?? new();
        return userMst == null ? string.Empty : userMst.Sname ?? string.Empty;
    }

}
