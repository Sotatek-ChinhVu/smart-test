using Domain.Models.UserConf;
using Entity.Tenant;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UserConfRepository : RepositoryBase, IUserConfRepository
{
    private const int ADOPTED_CONFIRM_CD = 100005;
    public static Dictionary<int, Dictionary<int, int>> ConfigGroupDefault = new();

    public UserConfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
        InitConfigDefaultValue();
    }

    public List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd)
    {
        return NoTrackingDataContext.UserConfs
            .Where(u => u.UserId == userId && u.GrpCd >= fromGrpCd && u.GrpCd <= toGrpCd)
            .AsEnumerable().Select(u => ToModel(u)).ToList();
    }

    public int Sagaku(bool fromRece)
    {
        if (fromRece)
        {
            return NoTrackingDataContext.UserConfs.FirstOrDefault(p => p.GrpCd == 923 && p.GrpItemCd == 0 && p.GrpItemEdaNo == 0)?.Val ?? 0;
        }
        return NoTrackingDataContext.UserConfs.FirstOrDefault(p => p.GrpCd == 922 && p.GrpItemCd == 0 && p.GrpItemEdaNo == 0)?.Val ?? 0;
    }

    public Dictionary<string, int> GetList(int userId)
    {
        var result = new Dictionary<string, int>();

        var displaySetName = NoTrackingDataContext.UserConfs
            .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 2 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 2);
        result.Add("DisplaySetName", displaySetName);
        var displayUserInput = NoTrackingDataContext.UserConfs
          .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 3 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 3);
        result.Add("DisplayUserInput", displayUserInput);
        var displayTimeInput = NoTrackingDataContext.UserConfs
    .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 4 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 4);
        result.Add("DisplayTimeInput", displayTimeInput);
        var displayDrugPrice = NoTrackingDataContext.UserConfs
   .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 5 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 5);
        result.Add("DisplayDrugPrice", displayDrugPrice);
        var adoptedConfirmCD = NoTrackingDataContext.UserConfs
   .FirstOrDefault(u => u.UserId == userId && u.GrpCd == ADOPTED_CONFIRM_CD)?.Val ?? GetDefaultValue(ADOPTED_CONFIRM_CD);
        result.Add("AdoptedConfirmCD", adoptedConfirmCD);
        var confirmEditByomei = NoTrackingDataContext.UserConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 100006 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(100006);
        result.Add("ConfirmEditByomei", confirmEditByomei);
        var isLockSuperSetDisplay = NoTrackingDataContext.UserConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 906 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(906);
        result.Add("IsLockSuperSetDisplay", isLockSuperSetDisplay);
        var displayByomeiDateType = NoTrackingDataContext.UserConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 100001 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(100001);
        result.Add("DisplayByomeiDateType", displayByomeiDateType);

        string paramSaveMedical = NoTrackingDataContext.UserConfs
            .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 5)?.Param ?? "11111";
        var isByomeiCheckTempSave = paramSaveMedical[0].AsInteger();
        var isByomeiCheckKeisanSave = paramSaveMedical[1].AsInteger();
        var isByomeiCheckCheckPrint = paramSaveMedical[4].AsInteger();
        var isByomeiCheckTrialCalc = paramSaveMedical[3].AsInteger();
        var isByomeiCheckNormalSave = paramSaveMedical[2].AsInteger();
        result.Add("IsByomeiCheckTempSave", isByomeiCheckTempSave);
        result.Add("IsByomeiCheckKeisanSave", isByomeiCheckKeisanSave);
        result.Add("IsByomeiCheckCheckPrint", isByomeiCheckCheckPrint);
        result.Add("IsByomeiCheckTrialCalc", isByomeiCheckTrialCalc);
        result.Add("IsByomeiCheckNormalSave", isByomeiCheckNormalSave);

        string santeiCheckSaveParam = NoTrackingDataContext.UserConfs
         .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 1)?.Param ?? "10100";
        var isSanteiCheckNormalSave = santeiCheckSaveParam[0].AsInteger();
        var isSanteiCheckTempSave = santeiCheckSaveParam[2].AsInteger();
        var isSanteiCheckKeisanSave = santeiCheckSaveParam[1].AsInteger();
        var isSanteiCheckTrialCalc = santeiCheckSaveParam[3].AsInteger();
        var isSanteiCheckPrint = santeiCheckSaveParam[4].AsInteger();
        result.Add("IsSanteiCheckNormalSave", isSanteiCheckNormalSave);
        result.Add("IsSanteiCheckTempSave", isSanteiCheckTempSave);
        result.Add("IsSanteiCheckKeisanSave", isSanteiCheckKeisanSave);
        result.Add("IsSanteiCheckTrialCalc", isSanteiCheckTrialCalc);
        result.Add("IsSanteiCheckPrint", isSanteiCheckPrint);

        string inputCheckSaveParam = NoTrackingDataContext.UserConfs
         .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 2)?.Param ?? "10100";
        var isInputCheckNormalSave = inputCheckSaveParam[0].AsInteger();
        var isInputCheckTempSave = inputCheckSaveParam[2].AsInteger();
        var isInputCheckKeisanSave = inputCheckSaveParam[1].AsInteger();
        var isInputCheckTrialCalc = inputCheckSaveParam[3].AsInteger();
        var isInputCheckPrint = inputCheckSaveParam[4].AsInteger();
        result.Add("IsInputCheckNormalSave", isInputCheckNormalSave);
        result.Add("IsInputCheckTempSave", isInputCheckTempSave);
        result.Add("IsInputCheckKeisanSave", isInputCheckKeisanSave);
        result.Add("IsInputCheckTrialCalc", isInputCheckTrialCalc);
        result.Add("IsInputCheckPrint", isInputCheckPrint);

        string commentCheckSaveParam = NoTrackingDataContext.UserConfs
        .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 0)?.Param ?? "00000";
        var isCmtCheckNormalSave = inputCheckSaveParam[0].AsInteger();
        var isCmtCheckTempSave = inputCheckSaveParam[2].AsInteger();
        var isCmtCheckKeisanSave = inputCheckSaveParam[1].AsInteger();
        var isCmtCheckTrialCalc = inputCheckSaveParam[3].AsInteger();
        var isCmtCheckPrint = inputCheckSaveParam[4].AsInteger();
        result.Add("IsCmtCheckNormalSave", isCmtCheckNormalSave);
        result.Add("IsCmtCheckTempSave", isCmtCheckTempSave);
        result.Add("IsCmtCheckKeisanSave", isCmtCheckKeisanSave);
        result.Add("IsCmtCheckTrialCalc", isCmtCheckTrialCalc);
        result.Add("IsCmtCheckPrint", isCmtCheckPrint);

        string kubunCheckSaveParam = NoTrackingDataContext.UserConfs
        .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 3)?.Param ?? "10100";
        var isKubunCheckNormalSave = inputCheckSaveParam[0].AsInteger();
        var isKubunCheckTempSave = inputCheckSaveParam[2].AsInteger();
        var isKubunCheckKeisanSave = inputCheckSaveParam[1].AsInteger();
        result.Add("IsKubunCheckNormalSave", isKubunCheckNormalSave);
        result.Add("IsKubunCheckTempSave", isKubunCheckTempSave);
        result.Add("IsKubunCheckKeisanSave", isKubunCheckKeisanSave);

        return result;
    }

    public void UpdateAdoptedByomeiConfig(int hpId, int userId, int adoptedValue)
    {
        var userConfig = TrackingDataContext.UserConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == ADOPTED_CONFIRM_CD && p.UserId == userId);
        if (userConfig == null)
        {
            userConfig = new UserConf()
            {
                HpId = hpId,
                GrpCd = ADOPTED_CONFIRM_CD,
                UserId = userId,
                CreateId = userId,
                CreateDate = DateTime.Now
            };
            TrackingDataContext.UserConfs.Add(userConfig);
        }
        userConfig.UpdateId = userId;
        userConfig.UpdateDate = DateTime.UtcNow;
        userConfig.Val = adoptedValue;

        TrackingDataContext.SaveChanges();
    }

    public void UpdateUserConf(int hpId, int userId, int grpCd, int value)
    {
        var userConfig = TrackingDataContext.UserConfs.FirstOrDefault(p => p.HpId == hpId && p.UserId == userId && p.GrpCd == grpCd);
        if (userConfig == null)
        {
            userConfig = new UserConf()
            {
                HpId = hpId,
                GrpCd = grpCd,
                UserId = userId,
                CreateId = userId,
                CreateDate = DateTime.UtcNow
            };
            TrackingDataContext.UserConfs.Add(userConfig);
        }
        userConfig.UpdateId = userId;
        userConfig.UpdateDate = DateTime.UtcNow;
        userConfig.Val = value;
        TrackingDataContext.SaveChanges();
    }

    private UserConfModel ToModel(UserConf u)
    {
        return new UserConfModel(u.UserId, u.GrpCd,
            u.GrpItemCd, u.GrpItemEdaNo, u.Val, u.Param ?? String.Empty);
    }

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
    }

    private static void AddDefaultValue(int groupCd, int groupItem = 0, int defaultValue = 0)
    {
        if (ConfigGroupDefault.ContainsKey(groupCd))
        {
            var ConfigItemDefault = ConfigGroupDefault[groupCd];

            if (ConfigItemDefault.ContainsKey(groupItem))
            {
                ConfigItemDefault[groupItem] = defaultValue;
            }
            else
            {
                ConfigItemDefault.Add(groupItem, defaultValue);
            }
        }
        else
        {
            ConfigGroupDefault.Add(groupCd, new Dictionary<int, int>() { { groupItem, defaultValue } });
        }
    }

    private int GetDefaultValue(int groupCd, int groupItemCd = 0)
    {
        if (ConfigGroupDefault.ContainsKey(groupCd))
        {
            var ConfigItemDefault = ConfigGroupDefault[groupCd];
            if (ConfigItemDefault.ContainsKey(groupItemCd))
            {
                return ConfigItemDefault[groupItemCd];
            }
            return 0;
        }
        return 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
