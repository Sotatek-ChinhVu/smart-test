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
    private List<UserMst> _listUserMst
    {
        get
        {
            if (_allOfUserMst == null) return new List<UserMst>();

            return _allOfUserMst.Where(p => p.IsDeleted == DeleteTypes.None).ToList();
        }
    }

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

    public List<UserMst> GetListUserBySinDate(int sinDate)
    {
        lock (_threadsafelock)
        {
            return _listUserMst.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate).OrderBy(p => p.SortNo).ToList();
        }
    }

    public List<UserMst> GetListDoctorBySinDate(int sinDate)
    {
        lock (_threadsafelock)
        {
            return _listUserMst.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate && p.JobCd == 1).OrderBy(p => p.SortNo).ToList();
        }
    }

    public string GetUserSNameByUserId(int hpId, int userId, bool fromLastestDb = false)
    {
        UserMst userMst = GetUserMst(hpId, userId, 0, fromLastestDb);
        return userMst == null ? string.Empty : userMst.Sname ?? string.Empty;
    }

    public string GetUserSNameIncludedDeleted(int hpId, int userId, bool fromLastestDb = false)
    {
        if (fromLastestDb) RefreshData(hpId);

        UserMst userMst = _allOfUserMst?.FirstOrDefault(p => p.HpId == hpId && p.UserId == userId) ?? new();
        return userMst == null ? string.Empty : userMst.Sname ?? string.Empty;
    }

    /// <summary>
    /// In some case It didn't have sindate, so need to ignore sindate condition to get usermst => Is will pass sindate by 0
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="sinDate"></param>
    /// <param name="fromLastestDb"></param>
    /// <returns></returns>
    public UserMst GetUserMst(int hpId, int userId, int sinDate, bool fromLastestDb = false)
    {
        lock (_threadsafelock)
        {
            if (fromLastestDb) RefreshData(hpId);
            return _listUserMst.FirstOrDefault(p => p.UserId == userId && (sinDate <= 0 || p.StartDate <= sinDate && p.EndDate >= sinDate)) ?? new();
        }
    }

    public string GetUserNameByUserID(int userID)
    {
        lock (_threadsafelock)
        {
            var userInfo = _listUserMst.FirstOrDefault(p => p.UserId == userID);
            return userInfo == null ? string.Empty : userInfo.Name ?? string.Empty;
        }
    }

    public string GetUserSNameByUserID(int userID)
    {
        lock (_threadsafelock)
        {
            var userInfo = _listUserMst.FirstOrDefault(p => p.UserId == userID);
            return userInfo == null ? string.Empty : userInfo.Sname ?? string.Empty;
        }
    }
}
