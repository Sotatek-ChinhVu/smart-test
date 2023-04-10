using Domain.Models.HpInf;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Collections;

namespace Infrastructure.Repositories;

public class SystemConfRepository : RepositoryBase, ISystemConfRepository
{
    public SystemConfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {

    }

    public List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd)
    {
        return NoTrackingDataContext.SystemConfs
            .Where(s => s.GrpCd >= fromGrpCd && s.GrpCd <= toGrpCd)
            .AsEnumerable().Select(s => ToModel(s)).ToList();
    }

    public List<SystemConfModel> GetList(int hpId, List<int> grpCodeList)
    {
        grpCodeList = grpCodeList.Distinct().ToList();
        var systemConfigList = NoTrackingDataContext.SystemConfs.Where(item => item.HpId == hpId
                                                                               && grpCodeList.Contains(item.GrpCd))
                                                                .ToList();
        return systemConfigList.Select(item => ToModel(item)).ToList();
    }

    public SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo)
    {
        var data = NoTrackingDataContext.SystemConfs
            .FirstOrDefault(s => s.HpId == hpId && s.GrpCd == grpCd && s.GrpEdaNo == grpEdaNo);
        if (data == null) return new SystemConfModel();
        return new SystemConfModel(data.GrpCd, data.GrpEdaNo, data.Val, data?.Param ?? string.Empty, data?.Biko ?? string.Empty);
    }

    public bool SaveSystemConfigList(int hpId, int userId, List<SystemConfModel> systemConfigList)
    {
        var grpCdList = systemConfigList.Select(item => item.GrpCd).Distinct().ToList();
        var systemConfigDBList = TrackingDataContext.SystemConfs.Where(item => item.HpId == hpId
                                                                               && grpCdList.Contains(item.GrpCd))
                                                                .ToList();
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
        TrackingDataContext.SaveChanges();
        return true;
    }

    public List<SystemConfModel> GetAllSystemConfig(int hpId)
    {
        var result = NoTrackingDataContext.SystemConfs.Where(item => item.HpId == hpId)
                                                      .Select(item => new SystemConfModel(
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
        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo && p.HpId == hpId);
        return systemConf != null ? systemConf.Val : 0;
    }

    public string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "")
    {

        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo && p.HpId == hpId);

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
        var systemSettings = NoTrackingDataContext.SystemConfs.Where(u => u.HpId == hpId);
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
        if (centerCds.Any())
        {
            return new();
        }
        return centerCds;
    }
}
