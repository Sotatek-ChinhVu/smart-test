using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Entity.Tenant;
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

    public SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo)
    {
        var data = NoTrackingDataContext.SystemConfs
            .FirstOrDefault(s => s.HpId == hpId && s.GrpCd == grpCd && s.GrpEdaNo == grpEdaNo);
        if (data == null) return new SystemConfModel();
        return new SystemConfModel(data.GrpCd, data.GrpEdaNo, data.Val, data?.Param ?? string.Empty, data?.Biko ?? string.Empty);
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

    public List<SystemConfMenuModel> GetListSystemConfMenuWithGeneration(int hpId, int menuGrp)
    {
        var systemConfMenus = NoTrackingDataContext.SystemConfMenu.Where(u => u.HpId == hpId && u.MenuGrp == menuGrp);
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
                 systemConfMenu.GrpCd,
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

                 systemConfItem == null ? new List<SystemConfItemModel>() :
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

                 systemGeneration == null ? new List<SystemGenerationConfModel>() :
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

}
