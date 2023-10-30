using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Collections;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class SystemConfRepository : RepositoryBase, ISystemConfRepository
{
    private readonly IDatabase _cache;
    private readonly string key;
    private readonly IConfiguration _configuration;

    public SystemConfRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetCacheKey() + "SystemConf";
        if (key.StartsWith("-"))
        {
            key = "ClinicID-SystemConfRepository" + "SystemConf";
        }
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

    private List<SystemConf> ReloadCache(int hpId)
    {
        var result = NoTrackingDataContext.SystemConfs
                                    .Where(item => item.HpId == hpId)
                                    .ToList();
        var json = JsonSerializer.Serialize(result);
        _cache.StringSet(key, json);

        return result;
    }

    private List<SystemConf> ReadCache(int hpId)
    {
        var results = _cache.StringGet(key);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<SystemConf>>(json) : new();
        return datas ?? new();
    }

    private List<SystemConf> GetData(int hpId)
    {
        var result = new List<SystemConf>();
        if (!_cache.KeyExists(key))
        {
            result = ReloadCache(hpId);
        }
        else
        {
            result = ReadCache(hpId);
        }

        return result;
    }

    public List<SystemConfModel> GetList(int hpId, int fromGrpCd, int toGrpCd)
    {
        var result = GetData(hpId);
        return result.Where(s => s.GrpCd >= fromGrpCd && s.GrpCd <= toGrpCd).AsEnumerable().Select(s => ToModel(s)).ToList();
    }

    public List<SystemConfModel> GetList(int hpId, List<int> grpCodeList)
    {
        grpCodeList = grpCodeList.Distinct().ToList();
        var result = GetData(hpId);
        var systemConfigList = result.Where(item => grpCodeList.Contains(item.GrpCd)).ToList();
        return systemConfigList.Select(item => ToModel(item)).ToList();
    }

    public SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo)
    {
        var result = GetData(hpId);
        var data = result.FirstOrDefault(s => s.GrpCd == grpCd && s.GrpEdaNo == grpEdaNo);
        if (data == null) return new SystemConfModel();
        return new SystemConfModel(data.GrpCd, data.GrpEdaNo, data.Val, data?.Param ?? string.Empty, data?.Biko ?? string.Empty);
    }

    public bool SaveSystemConfigList(int hpId, int userId, List<SystemConfModel> systemConfigList)
    {
        var grpCdList = systemConfigList.Select(item => item.GrpCd).Distinct().ToList();
        var systemConfigDBList = TrackingDataContext.SystemConfs.Where(item => item.HpId == hpId && grpCdList.Contains(item.GrpCd)).ToList();
        foreach (var model in systemConfigList)
        {
            bool addNew = false;
            var systemConfigItem = systemConfigDBList.FirstOrDefault(item => item.GrpCd == model.GrpCd && item.GrpEdaNo == model.GrpEdaNo);
            if (systemConfigItem == null)
            {
                addNew = true;
                systemConfigItem = new SystemConf();
                systemConfigItem.GrpEdaNo = model.GrpEdaNo;
                systemConfigItem.GrpCd = model.GrpCd;
                systemConfigItem.HpId = hpId;
                systemConfigItem.CreateDate = CIUtil.GetJapanDateTimeNow();
                systemConfigItem.CreateId = userId;
            }
            systemConfigItem.Param = model.Param;
            systemConfigItem.Val = model.Val;
            systemConfigItem.Biko = model.Biko;
            systemConfigItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            systemConfigItem.UpdateId = userId;
            if (addNew)
            {
                TrackingDataContext.SystemConfs.Add(systemConfigItem);
            }
        }

        var result = TrackingDataContext.SaveChanges();
        if (result > 0)
        {
            ReloadCache(hpId);
        }

        return true;
    }

    public List<SystemConfModel> GetAllSystemConfig(int hpId)
    {
        var data = GetData(hpId);

        var result = data.Select(item => new SystemConfModel(
                                                         item.GrpCd,
                                                         item.GrpEdaNo,
                                                         item.Val,
                                                         item.Param ??
                                                         string.Empty,
                                                         item.Biko ??
                                                         string.Empty))
                    .ToList();
        return result;
    }

    public double GetSettingValue(int groupCd, int grpEdaNo, int hpId)
    {
        var result = GetData(hpId);
        var systemConf = result.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
        return systemConf != null ? systemConf.Val : 0;
    }

    public string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "")
    {
        var result = GetData(hpId);

        var systemConf = result.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);

        //Fix comment 894 (duong.vu)
        //Return value in DB if and only if Param is not null or white space
        if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
        {
            return systemConf.Param;
        }

        return defaultParam;
    }

    public Hashtable GetConfigForPrintFunction(int hpId)
    {
        Hashtable config = new Hashtable();
        config.Add("OrderLabelCheckMachineParam", GetSettingParams(92001, 9, hpId, "KrtRenkei,TKImport"));
        config.Add("InnaishohosenCheckMachineParam", GetSettingParams(92002, 2, hpId, "KrtRenkei,TKImport"));
        config.Add("IngaiShohosenCheckMachineParam", GetSettingParams(92003, 8, hpId, "KrtRenkei,TKImport"));
        config.Add("KusurijoCheckMachineParam", GetSettingParams(92004, 16, hpId, "KrtRenkei,TKImport"));
        config.Add("PrintDrgLabelCheckMachineParam", GetSettingParams(92005, 30, hpId, "KrtRenkei,TKImport"));
        config.Add("PrintDrgNoteCheckMachineParam", GetSettingParams(92006, 1, hpId, "KrtRenkei,TKImport"));
        config.Add("SijisenCheckMachineParam", GetSettingParams(92008, 5, hpId, "KrtRenkei,TKImport"));
        config.Add("OrderLabelCheckMachine", GetSettingValue(92001, 9, hpId));
        config.Add("InnaishohosenCheckMachine", GetSettingValue(92002, 2, hpId));
        config.Add("IngaiShohosenCheckMachine", GetSettingValue(92003, 8, hpId));
        config.Add("KusurijoCheckMachine", GetSettingValue(92004, 16, hpId));
        config.Add("PrintDrgLabelCheckMachine", GetSettingValue(92005, 50, hpId));
        config.Add("PrintDrgNoteCheckMachine", GetSettingValue(92006, 1, hpId));
        config.Add("SijisenCheckMachine", GetSettingValue(92008, 5, hpId));

        return config;
    }

    private SystemConfModel ToModel(SystemConf s)
    {
        return new SystemConfModel(s.GrpCd, s.GrpEdaNo, s.Val, s?.Param ?? string.Empty, s?.Biko ?? string.Empty);
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public List<SystemConfMenuModel> GetListSystemConfMenuWithGeneration(int hpId, List<int> menuGrp)
    {
        var systemConfMenus = NoTrackingDataContext.SystemConfMenu.Where(u => u.HpId == hpId && menuGrp.Contains(u.MenuGrp));
        var systemConfItems = NoTrackingDataContext.SystemConfItem.Where(u => u.HpId == hpId).OrderBy(u => u.Val);
        var systemGenerationConfs = NoTrackingDataContext.SystemGenerationConfs.Where(u => u.HpId == hpId).OrderBy(u => u.StartDate);
        var query = from menu in systemConfMenus.AsEnumerable()
                    join item in systemConfItems on menu.MenuId equals item.MenuId into items
                    join generation in systemGenerationConfs on
                    new { menu.GrpCd, menu.GrpEdaNo } equals
                    new { generation.GrpCd, generation.GrpEdaNo } into generations
                    select ConvertToSystemConfModel(menu, items.ToList(), generations.ToList());

        return query.ToList();

    }

    private SystemConfMenuModel ConvertToSystemConfModel(SystemConfMenu systemConfMenu, List<SystemConfItem> systemConfItem, List<SystemGenerationConf> systemGeneration)
    {
        return new SystemConfMenuModel
            (
                 systemConfMenu.HpId,
                 systemConfMenu.MenuId,
                 systemConfMenu.MenuGrp,
                 systemConfMenu.SortNo,
                 systemConfMenu.MenuName ?? string.Empty,
                 systemConfMenu.GrpCd,
                 systemConfMenu.GrpEdaNo,
                 systemConfMenu.PathGrpCd,
                 systemConfMenu.IsParam,
                 systemConfMenu.ParamMask,
                 systemConfMenu.ParamType,
                 systemConfMenu.ParamHint ?? string.Empty,
                 systemConfMenu.ValMin,
                 systemConfMenu.ValMax,
                 systemConfMenu.ParamMin,
                 systemConfMenu.ParamMax,
                 systemConfMenu.ItemCd ?? string.Empty,
                 systemConfMenu.PrefNo,
                 systemConfMenu.IsVisible,
                 systemConfMenu.ManagerKbn,
                 systemConfMenu.IsValue,
                 systemConfMenu.ParamMaxLength,

                 !systemConfItem.Any() ? new() :
                 systemConfItem.Select(x =>
                                        new SystemConfItemModel(
                                            x.HpId,
                                            x.MenuId,
                                            x.SeqNo,
                                            x.SortNo,
                                            x.ItemName ?? string.Empty,
                                            x.Val,
                                            x.ParamMin,
                                            x.ParamMax
                                            )).ToList(),

                 !systemGeneration.Any() ? new() :
                 systemGeneration.Select(y =>
                                          new SystemGenerationConfModel(
                                              y.Id,
                                              y.HpId,
                                              y.GrpCd,
                                              y.GrpEdaNo,
                                              y.StartDate,
                                              y.EndDate,
                                              y.Val,
                                              y.Param ?? string.Empty,
                                              y.Biko ?? string.Empty
                                            )).ToList()
            );
    }

    //Key: RoudouCd, Value: RoudouName
    public Dictionary<string, string> GetRoudouMst()
    {
        var result = new Dictionary<string, string>();
        List<RoudouMst> RoudouMsts = NoTrackingDataContext.RoudouMsts.ToList();
        foreach (var item in RoudouMsts)
        {
            result.Add(item.RoudouCd, item.RoudouName ?? string.Empty);
        }

        return result;
    }

    public List<SystemConfMenuModel> GetListSystemConfMenu(int hpId, List<int> menuGrp)
    {
        var systemConfMenus = NoTrackingDataContext.SystemConfMenu.Where(u => u.HpId == hpId && menuGrp.Contains(u.MenuGrp) && u.IsVisible == 1);
        var systemConfItems = NoTrackingDataContext.SystemConfItem.Where(u => u.HpId == hpId).OrderBy(u => u.SortNo);
        var systemSettings = GetData(hpId);
        var systemConfs = (from menu in systemConfMenus.AsEnumerable()
                           join item in systemConfItems on menu.MenuId equals item.MenuId into items
                           join setting in systemSettings on new { menu.GrpCd, menu.GrpEdaNo } equals new { setting.GrpCd, setting.GrpEdaNo }
                           into settingList
                           select ConvertToSystemConfModel(menu, items.ToList(), settingList.FirstOrDefault() ?? new())
                          ).ToList();

        var hpInfs = NoTrackingDataContext.HpInfs.Where(p => p.HpId == hpId).OrderByDescending(p => p.StartDate).FirstOrDefault();
        var prefCD = 0;
        if (hpInfs != null) prefCD = hpInfs.PrefNo;

        systemConfs.RemoveAll(x => x.PrefNo != 0 && prefCD != x.PrefNo);

        return systemConfs;
    }

    private SystemConfMenuModel ConvertToSystemConfModel(SystemConfMenu systemConfMenu, List<SystemConfItem> systemConfItem, SystemConf systemConf)
    {
        return new SystemConfMenuModel
            (
                 systemConfMenu.HpId,
                 systemConfMenu.MenuId,
                 systemConfMenu.MenuGrp,
                 systemConfMenu.SortNo,
                 systemConfMenu.MenuName ?? string.Empty,
                 systemConfMenu.GrpCd,
                 systemConfMenu.GrpEdaNo,
                 systemConfMenu.PathGrpCd,
                 systemConfMenu.IsParam,
                 systemConfMenu.ParamMask,
                 systemConfMenu.ParamType,
                 systemConfMenu.ParamHint ?? string.Empty,
                 systemConfMenu.ValMin,
                 systemConfMenu.ValMax,
                 systemConfMenu.ParamMin,
                 systemConfMenu.ParamMax,
                 systemConfMenu.ItemCd ?? string.Empty,
                 systemConfMenu.PrefNo,
                 systemConfMenu.IsVisible,
                 systemConfMenu.ManagerKbn,
                 systemConfMenu.IsValue,
                 systemConfMenu.ParamMaxLength,

                 !systemConfItem.Any() ? new() :
                 systemConfItem.Select(x =>
                                        new SystemConfItemModel(
                                            x.HpId,
                                            x.MenuId,
                                            x.SeqNo,
                                            x.SortNo,
                                            x.ItemName ?? string.Empty,
                                            x.Val,
                                            x.ParamMin,
                                            x.ParamMax
                                            )).ToList(),

                 new SystemConfModel(systemConf.GrpCd,
                                     systemConf.GrpEdaNo,
                                     systemConf.Val,
                                     systemConf.Param ?? string.Empty,
                                     systemConf.Biko ?? string.Empty)

            );
    }

    public List<SystemConfMenuModel> GetListSystemConfMenuOnly(int hpId, int menuGrp)
    {
        var systemConfMenus = NoTrackingDataContext.SystemConfMenu
            .Where(u => u.HpId == hpId && u.MenuGrp == menuGrp)
            .OrderBy(u => u.SortNo)
            .ToList();
        return systemConfMenus.Select(x => new SystemConfMenuModel(
                                             x.HpId,
                                             x.MenuId,
                                             x.MenuGrp,
                                             x.SortNo,
                                             x.MenuName ?? string.Empty,
                                             x.GrpCd,
                                             x.GrpEdaNo,
                                             x.PathGrpCd,
                                             x.IsParam,
                                             x.ParamMask,
                                             x.ParamType,
                                             x.ParamHint ?? string.Empty,
                                             x.ValMin,
                                             x.ValMax,
                                             x.ParamMin,
                                             x.ParamMax,
                                             x.ItemCd ?? string.Empty,
                                             x.PrefNo,
                                             x.IsVisible,
                                             x.ManagerKbn,
                                             x.IsValue,
                                             x.ParamMaxLength
                               )).ToList();
    }

    public List<string> GetListCenterCd(int hpId)
    {
        var centerCds = NoTrackingDataContext.KensaInfs.Where(u => u.HpId == hpId)
                                                             .Select(item => item.CenterCd ?? string.Empty)
                                                             .Distinct()
                                                             .ToList();
        if (!centerCds.Any())
        {
            return new();
        }
        return centerCds;
    }

    public bool SaveSystemGenerationConf(int userId, List<SystemConfMenuModel> systemConfMenuModels)
    {
        var addedGenModels = new List<SystemGenerationConfModel>();
        var updatedGenModels = new List<SystemGenerationConfModel>();
        var deletedGenModels = new List<SystemGenerationConfModel>();
        var listSystemConfMenuModels = systemConfMenuModels.Where(u => u.SystemGenerationConfs != null);

        foreach (var modelVal in listSystemConfMenuModels)
        {
            foreach (var modelGenVal in modelVal.SystemGenerationConfs)
            {
                if (modelGenVal.SystemGenerationConfStatus == ModelStatus.Added && !modelGenVal.CheckDefaultValue())
                {
                    addedGenModels.Add(modelGenVal);
                }
                if (modelGenVal.SystemGenerationConfStatus == ModelStatus.Modified)
                {
                    updatedGenModels.Add(modelGenVal);
                }
                if (modelGenVal.SystemGenerationConfStatus == ModelStatus.Deleted)
                {
                    deletedGenModels.Add(modelGenVal);
                }
            }
        }

        if (!addedGenModels.Any() && !updatedGenModels.Any() && !deletedGenModels.Any()) return true;

        if (deletedGenModels.Any())
        {
            var modelsToDelete = TrackingDataContext.SystemGenerationConfs.AsEnumerable().Where(x => deletedGenModels.Any(d => d.HpId == x.HpId && d.GrpCd == x.GrpCd && d.GrpEdaNo == x.GrpEdaNo && d.Id == x.Id)).ToList();
            TrackingDataContext.SystemGenerationConfs.RemoveRange(modelsToDelete);
        }

        if (updatedGenModels.Any())
        {
            foreach (var model in updatedGenModels)
            {
                var geneTracking = TrackingDataContext.SystemGenerationConfs
                    .FirstOrDefault(x =>
                                        x.HpId == model.HpId &&
                                        x.GrpCd == model.GrpCd &&
                                        x.GrpEdaNo == model.GrpEdaNo &&
                                        x.Id == model.Id);

                if (geneTracking != null)
                {
                    geneTracking.StartDate = model.StartDate;
                    geneTracking.EndDate = model.EndDate;
                    geneTracking.Val = model.Val;
                    geneTracking.Param = model.Param;
                    geneTracking.Biko = model.Biko;
                    geneTracking.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    geneTracking.UpdateId = userId;
                }
            }
        }

        foreach (var model in addedGenModels)
        {
            TrackingDataContext.SystemGenerationConfs.Add(new SystemGenerationConf()
            {
                HpId = model.HpId,
                GrpCd = model.GrpCd,
                GrpEdaNo = model.GrpEdaNo,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Val = model.Val,
                Param = model.Param,
                Biko = model.Biko,
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            });
        }

        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool SaveSystemSetting(int hpId, int userId, List<SystemConfMenuModel> SystemConfMenuModels)
    {
        var systemSettingModels = SystemConfMenuModels.Select(u => u.SystemConf);

        var addedSettingModels = systemSettingModels.Where(k => k.SystemSettingModelStatus == ModelStatus.Added).ToList();
        var updatedSettingModels = systemSettingModels.Where(k => k.SystemSettingModelStatus == ModelStatus.Modified).ToList();

        if (!addedSettingModels.Any() && !updatedSettingModels.Any()) return true;

        if (updatedSettingModels.Any())
        {
            foreach (var model in updatedSettingModels)
            {
                var settingTracking = TrackingDataContext.SystemConfs
                    .FirstOrDefault(x =>
                                        x.HpId == model.HpId &&
                                        x.GrpCd == model.GrpCd &&
                                        x.GrpEdaNo == model.GrpEdaNo);
                if (settingTracking != null)
                {
                    settingTracking.Val = model.Val;
                    settingTracking.Param = model.Param;
                    settingTracking.Biko = model.Biko;
                    settingTracking.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    settingTracking.UpdateId = userId;
                }

                if (model.GrpCd == 93002 &&
                    model.GrpEdaNo == 0 &&
                    model.IsUpdatePtRyosyo) // Check seting 明細書
                {
                    UpdatePtRyosyoDetail(userId, model);
                }
            }
        }

        if (addedSettingModels.Any())
        {
            foreach (var model in addedSettingModels)
            {
                TrackingDataContext.SystemConfs.Add(new SystemConf()
                {
                    HpId = hpId,
                    GrpCd = model.GrpCd,
                    GrpEdaNo = model.GrpEdaNo,
                    Val = model.Val,
                    Param = model.Param,
                    Biko = model.Biko,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                });

                if (model.GrpCd == 93002 &&
                    model.GrpEdaNo == 0 &&
                    model.IsUpdatePtRyosyo) // Check seting 明細書
                {
                    UpdatePtRyosyoDetail(userId, model);
                }
            }
        }

        var result = TrackingDataContext.SaveChanges() > 0;
        if (result)
        {
            ReloadCache(hpId);
        }

        return result;
    }

    private void UpdatePtRyosyoDetail(int userId, SystemConfModel model)
    {
        var query = from PtInfs in TrackingDataContext.PtInfs
                    where PtInfs.HpId == model.HpId && PtInfs.IsRyosyoDetail != model.Val.AsInteger()
                    select PtInfs;

        if (!query.Any()) return;

        foreach (var item in query)
        {
            item.IsRyosyoDetail = model.Val.AsInteger();
            item.UpdateDate = CIUtil.GetJapanDateTimeNow();
            item.UpdateId = userId;
        }
    }

    public List<SystemConfListXmlPathModel> GetSystemConfListXmlPath(int hpId, int grpCd, string machine, bool isKesaIrai)
    {
        List<PathConf> pathConf;

        if (isKesaIrai)
        {
            pathConf = NoTrackingDataContext.PathConfs.Where(item => item.HpId == hpId
                                                                           && item.GrpCd == grpCd
                                                                           && item.IsInvalid == 0).ToList();
        }
        else
        {
            pathConf = NoTrackingDataContext.PathConfs.Where(item => item.HpId == hpId
                                                                           && item.GrpCd == grpCd
                                                                           && item.Machine == machine)
                                                                           .ToList();
            if (pathConf == null || !pathConf.Any())
            {
                pathConf = NoTrackingDataContext.PathConfs.Where(item => item.HpId == hpId
                                                                               && item.GrpCd == grpCd
                                                                               && (item.Machine == string.Empty || item.Machine == null))
                                                                               .ToList();
            }
        }

        return pathConf.Select(item => ToModel(item)).ToList();
    }

    public List<SystemConfListXmlPathModel> GetAllPathConf(int hpId)
    {
        var pathConfs = NoTrackingDataContext.PathConfs.Where(item => item.HpId == hpId).ToList();

        return pathConfs.Select(item => ToModel(item)).ToList();
    }

    public bool SavePathConfOnline(int hpId, int userId, List<SystemConfListXmlPathModel> systemConfListXmlPathModels)
    {
        foreach (var systemConfListXmlPathModel in systemConfListXmlPathModels)
        {
            var entity = TrackingDataContext.PathConfs.FirstOrDefault(p => p.HpId == systemConfListXmlPathModel.HpId && p.SeqNo == systemConfListXmlPathModel.SeqNo && p.GrpCd == systemConfListXmlPathModel.GrpCd && p.GrpEdaNo == systemConfListXmlPathModel.GrpEdaNo);

            var newEntity = new PathConf();
            newEntity.HpId = hpId;
            newEntity.Machine = systemConfListXmlPathModel.Machine;
            newEntity.Path = systemConfListXmlPathModel.Path;
            newEntity.IsInvalid = 1;
            newEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            newEntity.UpdateId = userId;
            newEntity.Biko = systemConfListXmlPathModel.Biko;
            newEntity.GrpCd = systemConfListXmlPathModel.GrpCd;
            if (entity != null)
            {
                newEntity.SeqNo = systemConfListXmlPathModel.SeqNo;
                TrackingDataContext.PathConfs.Remove(entity);
                newEntity.CreateDate = TimeZoneInfo.ConvertTimeToUtc(systemConfListXmlPathModel.CreateDate);
                newEntity.CreateId = systemConfListXmlPathModel.CreateId;
                newEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                newEntity.UpdateId = userId;
            }
            else
            {
                newEntity.CreateDate = CIUtil.GetJapanDateTimeNow();
                newEntity.CreateId = userId;
                newEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                newEntity.UpdateId = userId;
            }

            TrackingDataContext.Add(newEntity);
        }
        var first = systemConfListXmlPathModels.FirstOrDefault();
        var grpCd = first?.GrpCd ?? 0;
        var grpCdEdaNo = first?.GrpEdaNo ?? 0;

        var itemDeleted = NoTrackingDataContext.PathConfs.AsEnumerable().Where(p => p.HpId == hpId && p.GrpCd == grpCd && p.GrpEdaNo == grpCdEdaNo && !systemConfListXmlPathModels.Any(p1 => p.HpId == p1.HpId && p.GrpCd == p1.GrpCd && p.GrpEdaNo == p1.GrpEdaNo && p.SeqNo == p1.SeqNo));
        TrackingDataContext.RemoveRange(itemDeleted);

        return TrackingDataContext.SaveChanges() > 0;
    }


    private SystemConfListXmlPathModel ToModel(PathConf pathConf)
    {
        return new SystemConfListXmlPathModel(pathConf.HpId, pathConf.GrpCd, pathConf.GrpEdaNo, pathConf.SeqNo, pathConf.Machine ?? string.Empty, pathConf.Path ?? string.Empty, pathConf.Param ?? string.Empty, pathConf.Biko ?? string.Empty, pathConf.CharCd, pathConf.IsInvalid, pathConf.CreateId, pathConf.CreateDate);
    }
}