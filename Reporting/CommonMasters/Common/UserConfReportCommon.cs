using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Common.Interface;

namespace Reporting.CommonMasters.Common;

public class UserConfReportCommon : RepositoryBase, IUserConfReportCommon
{
    private static Dictionary<int, Dictionary<int, int>> ConfigGroupDefault = new();
    private List<UserConf> _userConfigs = new();
    private static readonly object _threadsafelock = new object();

    public enum DateTimeFormart
    {
        JapaneseCalendar = 0,
        WesternCalendar = 1,
        JapAndWestCalendar = 2,
    }

    public static class GroupCodes
    {
        /// <summary>
        /// Group code that contains font name and font size settings
        /// </summary>
        public const int Font = 2001;
        public const int AutoRefresh = 2002;
        public const int MouseWheel = 2003;
        public const int KanFocus = 2004;
        public const int SelectTodoSetting = 2005;
    }

    public UserConfReportCommon(ITenantProvider tenantProvider) : base(tenantProvider)
    {
        RefreshData();
        InitConfigDefaultValue();
    }

    public void RefreshData(int hpId = 0)
    {
        lock (_threadsafelock)
        {
            if(hpId <= 0)
            {
                _userConfigs = NoTrackingDataContext.UserConfs.Where(p => p.UserId == Session.UserID).ToList();
            }
            else
            {
                _userConfigs = NoTrackingDataContext.UserConfs.Where(p => p.HpId == hpId && p.UserId == Session.UserID).ToList();
            }
            ChangedData?.Invoke();
        }
    }

    public delegate void UserConfCommonDelegate();
    public UserConfCommonDelegate? ChangedData { get; set; }

    private static void InitConfigDefaultValue()
    {
        AddDefaultValue(15, defaultValue: 180);
        AddDefaultValue(201, defaultValue: 13);
        AddDefaultValue(13, defaultValue: 0);
        AddDefaultValue(2001, defaultValue: 13);
        AddDefaultValue(2002, defaultValue: 60);
        AddDefaultValue(2003, defaultValue: 3);
        AddDefaultValue(2004, defaultValue: 0);
        AddDefaultValue(2005, defaultValue: 0);
        AddDefaultValue(101, defaultValue: 13);
        AddDefaultValue(12, defaultValue: 1);
        AddDefaultValue(11, defaultValue: 3);
        AddDefaultValue(10, 1, defaultValue: 4);
        AddDefaultValue(301, defaultValue: 1);
        AddDefaultValue(8, 1, 1);
        AddDefaultValue(8, 2, 1);
        AddDefaultValue(8, 3, 1);
        AddDefaultValue(8, 4, 1);
        AddDefaultValue(8, 5, 1);
        AddDefaultValue(8, 6, 1);
        AddDefaultValue(8, 7, 1);
        AddDefaultValue(8, 8, 1);
        AddDefaultValue(8, 9, 1);
        AddDefaultValue(8, 10, 1);
        AddDefaultValue(8, 11, 1);
        AddDefaultValue(8, 12, 1);
        AddDefaultValue(8, 99, 1);
        AddDefaultValue(5, defaultValue: 10);
        AddDefaultValue(16, defaultValue: 1);
        AddDefaultValue(103, defaultValue: 1);
        AddDefaultValue(104, 1, 1);
        AddDefaultValue(104, 2, 200);
        AddDefaultValue(99, defaultValue: 12345);
        AddDefaultValue(4, defaultValue: 0);
        AddDefaultValue(16, defaultValue: 1);
        AddDefaultValue(203, defaultValue: 0);
        AddDefaultValue(100006, defaultValue: 0);
        AddDefaultValue(15, defaultValue: 180);

        // Reservation  screen config
        AddDefaultValue(100010, 1, 0);
        AddDefaultValue(100010, 3, 13);
        AddDefaultValue(100010, 4, 120);
        AddDefaultValue(100010, 5, 96);
        AddDefaultValue(100010, 6, 24);
        AddDefaultValue(100010, 7, 48);
        AddDefaultValue(100010, 8, 24);
        AddDefaultValue(100010, 10, 90);
        AddDefaultValue(100010, 11, 14);

        AddDefaultValue(922, 0, 1);
        AddDefaultValue(901, 0, 13);
        AddDefaultValue(927, 0, 1);
        AddDefaultValue(208, 0, 3);

        //Medical's Layout config
        AddDefaultValue(11, 0, 1);
        AddDefaultValue(12, 0, 3);
        AddDefaultValue(2, 0, 10);
        AddDefaultValue(6, 0, 1);

        //Medical'Odr Config
        AddDefaultValue(201, 0, 13);
        AddDefaultValue(202, 2, 1);
        AddDefaultValue(202, 3, 1);
        AddDefaultValue(202, 4, 1);
        AddDefaultValue(202, 5, 1);
        AddDefaultValue(206, 1, 1);
        AddDefaultValue(206, 2, 1);
        AddDefaultValue(206, 3, 1);
        AddDefaultValue(206, 4, 1);
        AddDefaultValue(206, 5, 1);
        AddDefaultValue(206, 6, 1);
        AddDefaultValue(206, 7, 1);
        AddDefaultValue(206, 8, 1);
        AddDefaultValue(206, 9, 1);
        AddDefaultValue(206, 10, 1);
        AddDefaultValue(206, 11, 1);
        AddDefaultValue(206, 12, 1);
        AddDefaultValue(206, 13, 1);
        AddDefaultValue(206, 14, 1);
        AddDefaultValue(206, 15, 1);
        AddDefaultValue(206, 16, 1);
        AddDefaultValue(206, 17, 1);
        AddDefaultValue(206, 18, 1);
        AddDefaultValue(206, 19, 1);
        AddDefaultValue(206, 20, 1);
        AddDefaultValue(206, 21, 1);
        AddDefaultValue(208, 0, 3);

        //Medical'Karte Config
        AddDefaultValue(103, 0, 0);

        //Medical'SuperSet Config
        AddDefaultValue(301, 0, 2);


        AddDefaultValue(100013, 0, 0);

        AddDefaultValue(8, 13, 1);
        AddDefaultValue(304, 0, 1);
        AddDefaultValue(304, 1, 1);
        AddDefaultValue(308, 0, 1);
        AddDefaultValue(308, 1, 1);
        AddDefaultValue(308, 2, 1);
        AddDefaultValue(309, 1, 2);
        AddDefaultValue(309, 2, 33);
        AddDefaultValue(309, 3, 5);
        AddDefaultValue(309, 4, 13);

        AddDefaultValue(3001, 0, 13);

        AddDefaultValue(1001, 5, 200);

        AddDefaultValue(929, 0, 0);
    }

    private static void AddDefaultValue(int groupCd, int groupItem = 0, int defaultValue = 0)
    {
        if (ConfigGroupDefault.ContainsKey(groupCd))
        {
            var configItemDefault = ConfigGroupDefault[groupCd];

            if (configItemDefault.ContainsKey(groupItem))
            {
                configItemDefault[groupItem] = defaultValue;
            }
            else
            {
                configItemDefault.Add(groupItem, defaultValue);
            }
        }
        else
        {
            ConfigGroupDefault.Add(groupCd, new Dictionary<int, int>() { { groupItem, defaultValue } });
        }
    }

    public int GetDefaultValue(int groupCd, int groupItemCd = 0)
    {
        if (ConfigGroupDefault.ContainsKey(groupCd))
        {
            var configItemDefault = ConfigGroupDefault[groupCd];
            if (configItemDefault.ContainsKey(groupItemCd))
            {
                return configItemDefault[groupItemCd];
            }
            return 0;
        }
        return 0;
    }

    public int GetSettingValue(int hpId, int groupCd, int grpItemCd = 0, bool fromLastestDb = false, int grpItemEdaNo = 0)
    {
        lock (_threadsafelock)
        {
            UserConf userConf;
            if (!fromLastestDb)
            {
                userConf = _userConfigs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == groupCd && p.GrpItemCd == grpItemCd && p.GrpItemEdaNo == grpItemEdaNo) ?? new();
            }
            else
            {
                userConf = NoTrackingDataContext.UserConfs.FirstOrDefault(p =>
                    p.HpId == hpId && p.GrpCd == groupCd && p.GrpItemCd == grpItemCd && p.GrpItemEdaNo == grpItemEdaNo && p.UserId == Session.UserID) ?? new();
            }
            return userConf != null ? userConf.Val : GetDefaultValue(groupCd, grpItemCd);
        }
    }

    public int DisplaySetName(int hpId) { return GetSettingValue(hpId, 202, 2); }

    public int DisplayUserInput(int hpId) { return GetSettingValue(hpId, 202, 3); }

    public int DisplayTimeInput(int hpId) { return GetSettingValue(hpId, 202, 4); }

    public int DisplayDrugPrice(int hpId) { return GetSettingValue(hpId, 202, 5); }
}
