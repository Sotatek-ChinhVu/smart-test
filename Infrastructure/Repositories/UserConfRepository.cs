using Domain.Models.UserConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class UserConfRepository : RepositoryBase, IUserConfRepository
{
    private const int ADOPTED_CONFIRM_CD = 100005;
    private static List<ConfigGroupDefault> configGroupDefault = new();
    private readonly IDatabase _cache;
    private readonly string key;
    private readonly IConfiguration _configuration;

    public UserConfRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetCacheKey() + "UserMst";
        InitConfigDefaultValue();
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

    public List<UserConfModel> GetList(int hpId, int userId, int fromGrpCd, int toGrpCd)
    {
        return NoTrackingDataContext.UserConfs
            .Where(u => u.HpId == hpId && u.UserId == userId && u.GrpCd >= fromGrpCd && u.GrpCd <= toGrpCd)
            .AsEnumerable().Select(u => ToModel(u)).ToList();
    }

    public List<UserConfModel> GetList(int hpId, int userId, List<int> grpCodes)
    {
        var result = GetData(hpId, userId);

        return result!.Select(r => ToModel(r)).ToList();
    }

    public List<UserConfModel> GetList(int hpId, int userId)
    {
        var userConfs = GetData(hpId, userId);
        var entities = userConfs.Where(u => u.HpId == hpId && u.UserId == userId).ToList();
        var result = entities.Select(e => new UserConfModel(e.UserId, e.GrpCd, e.GrpItemCd, e.GrpItemEdaNo, e.Val, e.Param ?? string.Empty)).ToList();
        return result;
    }

    public Dictionary<string, int> GetDic(int hpId, int userId)
    {
        var result = new Dictionary<string, int>();
        var userConfs = GetData(hpId, userId);

        var displaySetName = userConfs
            .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 2 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 2);
        result.Add("DisplaySetName", displaySetName);
        var displayUserInput = userConfs
          .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 3 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 3);
        result.Add("DisplayUserInput", displayUserInput);
        var displayTimeInput = userConfs
    .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 4 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 4);
        result.Add("DisplayTimeInput", displayTimeInput);
        var displayDrugPrice = userConfs
   .FirstOrDefault(u => u.UserId == userId && u.GrpCd == 202 && u.GrpItemCd == 5 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(202, 5);
        result.Add("DisplayDrugPrice", displayDrugPrice);
        var adoptedConfirmCD = userConfs
   .FirstOrDefault(u => u.UserId == userId && u.GrpCd == ADOPTED_CONFIRM_CD)?.Val ?? GetDefaultValue(ADOPTED_CONFIRM_CD);
        result.Add("AdoptedConfirmCD", adoptedConfirmCD);
        var confirmEditByomei = userConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 100006 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(100006);
        result.Add("ConfirmEditByomei", confirmEditByomei);
        var isLockSuperSetDisplay = userConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 906 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(906);
        result.Add("IsLockSuperSetDisplay", isLockSuperSetDisplay);
        var displayByomeiDateType = userConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 100001 && u.GrpItemCd == 0 && u.GrpItemEdaNo == 0)?.Val ?? GetDefaultValue(100001);
        result.Add("DisplayByomeiDateType", displayByomeiDateType);

        string paramSaveMedical = userConfs
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

        string santeiCheckSaveParam = userConfs
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

        string inputCheckSaveParam = userConfs
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

        string reportCheckSaveParam = userConfs.FirstOrDefault(u => u.UserId == userId && u.GrpCd == 921 && u.GrpItemCd == 4)?.Param ?? "10101";
        var isReportCheckKeisanSave = reportCheckSaveParam[1].AsInteger();
        var isReportCheckNormalSave = reportCheckSaveParam[0].AsInteger();
        var isReportCheckTempSave = reportCheckSaveParam[2].AsInteger();
        result.Add("IsReportCheckKeisanSave", isReportCheckKeisanSave);
        result.Add("IsReportCheckNormalSave", isReportCheckNormalSave);
        result.Add("IsReportCheckTempSave", isReportCheckTempSave);

        return result;
    }

    public string GetSettingParam(int hpId, int userId, int groupCd, int grpItemCd = 0, string defaultValue = "")
    {
        var userConfs = GetData(hpId, userId);
        var userConf = userConfs.FirstOrDefault(p =>
             p.HpId == hpId && p.GrpCd == groupCd && p.GrpItemCd == grpItemCd && p.UserId == userId);
        return userConf != null ? userConf.Param ?? string.Empty : defaultValue;
    }

    public List<UserConfModel> GetListSettingParam(int hpId, int userId, List<Tuple<int, int>> groupCode, string defaultValue = "")
    {
        var result = new List<UserConfModel>();
        var userConfs = GetData(hpId, userId);
        foreach (var cd in groupCode)
        {
            var userConf = userConfs.FirstOrDefault(p =>
             p.HpId == hpId && p.GrpCd == cd.Item1 && p.GrpItemCd == cd.Item2 && p.UserId == userId);

            result.Add(new UserConfModel(
                userId,
                cd.Item1,
                cd.Item2,
                userConf != null ? userConf.GrpItemEdaNo : 0,
                userConf != null ? userConf.Val : 0,
                userConf != null ? userConf.Param ?? string.Empty : defaultValue));
        }

        return result;
    }

    private List<UserConf> ReloadCache(int hpId, int userId)
    {
        var result = NoTrackingDataContext.UserConfs
                                    .Where(item => item.UserId == userId
                                                   && item.HpId == hpId)
                                    .ToList();
        var json = JsonSerializer.Serialize(result);
        _cache.StringSet(key + userId + hpId, json);

        return result;
    }

    private List<UserConf> ReadCache(int userId, int hpId)
    {
        var results = _cache.StringGet(key + userId + hpId);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<UserConf>>(json) : new();
        return datas ?? new();
    }

    private List<UserConf> GetData(int hpId, int userId)
    {
        List<UserConf> result;
        if (!_cache.KeyExists(key + userId + hpId))
        {
            result = ReloadCache(hpId, userId);
        }
        else
        {
            result = ReadCache(userId, hpId);
        }

        return result;
    }

    public List<UserConfModel> GetListUserConf(int hpId, int userId, int groupCd)
    {
        var userConfs = GetData(hpId, userId);
        return userConfs.Where(p =>
            p.HpId == hpId && p.GrpCd == groupCd && p.UserId == userId).AsEnumerable().Select(u => ToModel(u)).ToList();
    }

    public int Sagaku(int hpId, int userId, bool fromRece)
    {
        var userConfs = GetData(hpId, userId);
        if (fromRece)
        {
            return userConfs.FirstOrDefault(p => p.HpId == hpId && p.UserId == userId && p.GrpCd == 923 && p.GrpItemCd == 0 && p.GrpItemEdaNo == 0)?.Val ?? 0;
        }
        return userConfs.FirstOrDefault(p => p.HpId == hpId && p.UserId == userId && p.GrpCd == 922 && p.GrpItemCd == 0 && p.GrpItemEdaNo == 0)?.Val ?? 0;
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
                CreateDate = CIUtil.GetJapanDateTimeNow(),
            };
            TrackingDataContext.UserConfs.Add(userConfig);
        }
        userConfig.UpdateId = userId;
        userConfig.UpdateDate = CIUtil.GetJapanDateTimeNow();
        userConfig.Val = adoptedValue;
        var check = TrackingDataContext.SaveChanges();
        if (check > 0)
        {
            ReloadCache(hpId, userId);
        }
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
                CreateDate = CIUtil.GetJapanDateTimeNow()
            };
            TrackingDataContext.UserConfs.Add(userConfig);
        }
        userConfig.UpdateId = userId;
        userConfig.UpdateDate = CIUtil.GetJapanDateTimeNow();
        userConfig.Val = value;
        var check = TrackingDataContext.SaveChanges();
        if (check > 0)
        {
            ReloadCache(hpId, userId);
        }
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
        var configItemDefault = configGroupDefault.FirstOrDefault(c => c.GroupCd == groupCd && c.GroupItemCd == groupItem);

        if (configItemDefault != null)
        {
            configItemDefault.DefaultValue = defaultValue;
        }
        else
        {
            configGroupDefault.Add(new ConfigGroupDefault { GroupCd = groupCd, GroupItemCd = groupItem, DefaultValue = defaultValue });
        }
    }

    public int GetDefaultValue(int groupCd, int groupItemCd = 0)
    {
        var configItemDefault = configGroupDefault.FirstOrDefault(c => c.GroupCd == groupCd && c.GroupItemCd == groupItemCd);

        if (configItemDefault != null)
        {
            return configItemDefault.DefaultValue;
        }
        return 0;
    }

    public bool UpsertUserConfs(int hpId, int userId, List<UserConfModel> userConfs)
    {
        foreach (var model in userConfs)
        {
            var checkEntity = TrackingDataContext.UserConfs.FirstOrDefault(u => u.GrpCd == model.GrpCd && u.GrpItemCd == model.GrpItemCd && u.GrpItemEdaNo == model.GrpItemEdaNo && u.HpId == hpId && u.UserId == userId);
            if (checkEntity == null)
            {
                var entity = ConvertToEntity(userId, hpId, model);
                entity.HpId = hpId;
                entity.CreateId = userId;
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                TrackingDataContext.UserConfs.Add(entity);
            }
            else
            {
                checkEntity.Val = model.Val;
                checkEntity.Param = model.Param;
                checkEntity.UpdateId = userId;
                checkEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            }
        }

        var result = TrackingDataContext.SaveChanges() > 0;
        if (result)
            ReloadCache(hpId, userId);
        return result;
    }

    private static UserConf ConvertToEntity(int userId, int hpId, UserConfModel userConfModel)
    {
        var userConf = new UserConf();
        userConf.HpId = hpId;
        userConf.UserId = userId;
        userConf.GrpCd = userConfModel.GrpCd;
        userConf.GrpItemCd = userConfModel.GrpItemCd;
        userConf.GrpItemEdaNo = userConfModel.GrpItemEdaNo;
        userConf.Val = userConfModel.Val;
        userConf.Param = userConfModel.Param;

        return userConf;
    }

    public int GetSettingValue(int hpId, int userId, int groupCd, int grpItemCd = 0, int grpItemEdaNo = 0)
    {
        var userConfs = GetData(hpId, userId);
        var userConf = userConfs.FirstOrDefault(p => p.HpId == hpId
                                                                           && p.GrpCd == groupCd
                                                                           && p.GrpItemCd == grpItemCd
                                                                           && p.GrpItemEdaNo == grpItemEdaNo
                                                                           && p.UserId == userId);
        return userConf != null ? userConf.Val : GetDefaultValue(groupCd, grpItemCd);
    }

    public List<(int groupItemCd, int value)> GetSettingValues(int hpId, int userId, int groupCd, int fromGroupItemCd, int toGroupItemCd)
    {
        var userConfs = GetData(hpId, userId);
        userConfs = userConfs.Where(p => p.HpId == hpId
                                                                           && p.GrpCd == groupCd
                                                                           && p.UserId == userId).ToList();
        List<(int groupItemCd, int value)> values = new();
        for (int i = fromGroupItemCd; i <= toGroupItemCd; i++)
        {
            var userConf = userConfs.FirstOrDefault(u => u.GrpItemCd == i);
            if (userConf != null)
            {
                values.Add(new(userConf.GrpItemCd, userConf.Val));
            }
            else
            {
                values.Add(new(i, GetDefaultValue(groupCd, i)));
            }
        }

        return values;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}

public class ConfigGroupDefault
{
    public int GroupCd { get; set; }

    public int GroupItemCd { get; set; }

    public int DefaultValue { get; set; }
}
