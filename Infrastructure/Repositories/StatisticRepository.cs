using Domain.Models.MainMenu;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Helper.Util;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class StatisticRepository : RepositoryBase, IStatisticRepository
{
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;
    public StatisticRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
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
    public List<StatisticMenuModel> GetStatisticMenu(int hpId, int grpId)
    {
        var staMenuList = NoTrackingDataContext.StaMenus.Where(item => item.HpId == hpId
                                                                       && (grpId == 0 || item.GrpId == grpId)
                                                                       && item.IsDeleted == 0)
                                                        .OrderBy(item => item.SortNo)
                                                        .ToList();

        var menuIdList = staMenuList.Select(item => item.MenuId).Distinct().ToList();

        var staConfigList = NoTrackingDataContext.StaConfs.Where(item => item.HpId == hpId
                                                                         && menuIdList.Contains(item.MenuId))
                                                          .ToList();

        return ConvertToStatisticList(staMenuList, staConfigList);
    }

    public (int menuIdTemp, bool success) SaveStatisticMenu(int hpId, int userId, List<StatisticMenuModel> statisticMenuModelList)
    {
        int menuIdTemp = 0;
        bool success = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        menuIdTemp = SaveDailyStatisticMenuAction(hpId, userId, statisticMenuModelList);
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });
        return (menuIdTemp, success);
    }

    public List<StaGrpModel> GetStaGrp(int hpId, int grpId)
    {
        var finalKey = GetCacheKey() + CacheKeyConstant.StaGrpModel + "_" + hpId;
        IEnumerable<StaGrpModel> staGrpModel;
        if (!_cache.KeyExists(finalKey))
        {
            staGrpModel = ReloadCacheStaGrpModel(hpId);
        }
        else
        {
            staGrpModel = ReadCacheStaGrpModel(hpId);
        }
        var result = staGrpModel.Where(i => i.GrpId == grpId).ToList();
        return result;
    }

    public List<StaCsvMstModel> GetStaCsvMstModels(int hpId)
    {
        string getDisplayColumnName(List<StaCsvModel> staCsvTemplateModels, string columns)
        {
            var staCsvModel = staCsvTemplateModels.FirstOrDefault(x => x.Columns == columns);
            if (staCsvModel != null)
            {
                return staCsvModel.ColumnsDisplay;
            }
            return columns;
        }

        string revertJpNameToSaveName(List<StaCsvModel> staCsvTemplateModels, string dataColumn)
        {
            var staCsvModel = staCsvTemplateModels.FirstOrDefault(x => x.ColumnsDisplay == dataColumn);
            if (staCsvModel != null)
            {
                return staCsvModel.Columns;
            }
            return dataColumn;
        }

        List<PtGrpNameMst> _ptGrpNameMsts = NoTrackingDataContext.PtGrpNameMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).ToList();
        List<RaiinKbnMst> _raiinKbnMsts = NoTrackingDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).ToList();

        var result = new List<StaCsvMstModel>();
        for (int i = 1; i <= 8; i++)
        {
            List<ConfigObject> template = PtManagementUtil.GetStaCsvTemplate(i);
            List<StaCsvModel> staCsvTemplateModels = template.Select(
                    (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.SaveName, x.IsSelected, x.OutputColumnName)
                ).ToList();
            if (i == 4)
            {
                foreach (var raiinKbn in _raiinKbnMsts)
                {
                    staCsvTemplateModels.Add(new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, i, string.Format("RaiinKbn_{0}", raiinKbn.GrpCd), false, string.Format("来院区分({0})", raiinKbn.GrpName)));
                }
            }

            if (i != 1)
            {
                List<StaCsvModel> staCsvSubTemplateModels = StaCsvConfigTemplate.PtInfSubConfig.Select(
                   (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.SaveName, x.IsSelected, x.OutputColumnName)
               ).ToList();
                staCsvTemplateModels.AddRange(staCsvSubTemplateModels);
            }

            foreach (var ptGrpName in _ptGrpNameMsts)
            {
                staCsvTemplateModels.Add(new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, i, string.Format("PtGrpCd_{0}", ptGrpName.GrpId), false, string.Format("グループ{0}({1}) 区分コード", ptGrpName.GrpId, ptGrpName.GrpName)));
                staCsvTemplateModels.Add(new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, i, string.Format("PtGrpCdName_{0}", ptGrpName.GrpId), false, string.Format("グループ{0}({1}) 区分名称", ptGrpName.GrpId, ptGrpName.GrpName)));
            }

            result.Add(new StaCsvMstModel(
                PtManagementUtil.GetConfName(i),
                i,
                staCsvTemplateModels,
                staCsvTemplateModels.Where(x => x.IsSelected).Select(
                    (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.Columns,
                            x.IsSelected,
                            x.ColumnsDisplay)).ToList(),
                i - 1,
                true));
        }

        var staCsvModels = NoTrackingDataContext.StaCsvs.Where(x => x.HpId == hpId && x.ReportId == 9000);
        var groupStaCsvModels = staCsvModels.GroupBy(x => new { x.DataSbt, x.ConfName, x.RowNo });
        foreach (var group in groupStaCsvModels)
        {
            List<ConfigObject> template = PtManagementUtil.GetStaCsvTemplate(group.Key.DataSbt);
            List<StaCsvModel> staCsvTemplateModels = template.Select(
                    (x, idx) => new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, idx, 9000, group.Key.DataSbt, x.SaveName, x.IsSelected, x.OutputColumnName)
                ).ToList();
            if (group.Key.DataSbt == 4)
            {
                foreach (var raiinKbn in _raiinKbnMsts)
                {
                    staCsvTemplateModels.Add(new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, group.Key.DataSbt, string.Format("RaiinKbn_{0}", raiinKbn.GrpCd), false, string.Format("来院区分({0})", raiinKbn.GrpName)));
                }
            }

            if (group.Key.DataSbt != 1)
            {
                List<StaCsvModel> staCsvSubTemplateModels = StaCsvConfigTemplate.PtInfSubConfig.Select(
                    (x, idx) => new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(u => u.SortKbn) + 1, 9000, group.Key.DataSbt, x.SaveName, x.IsSelected, x.OutputColumnName)
                ).ToList();
                staCsvTemplateModels.AddRange(staCsvSubTemplateModels);
            }

            foreach (var ptGrpName in _ptGrpNameMsts)
            {
                staCsvTemplateModels.Add(new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, group.Key.DataSbt, string.Format("PtGrpCd_{0}", ptGrpName.GrpId), false, string.Format("グループ{0}({1}) 区分コード", ptGrpName.GrpId, ptGrpName.GrpName)));
                staCsvTemplateModels.Add(new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, group.Key.DataSbt, string.Format("PtGrpCdName_{0}", ptGrpName.GrpId), false,
                 string.Format("グループ{0}({1}) 区分名称", ptGrpName.GrpId, ptGrpName.GrpName)));
            }

            result.Add(new StaCsvMstModel(
                group.Key.ConfName ?? string.Empty,
                group.Key.DataSbt,
                staCsvTemplateModels,
                group.AsEnumerable().Select(x => new StaCsvModel(x.Id, x.HpId, x.ConfName ?? string.Empty, x.RowNo, x.SortKbn, x.ReportId, x.DataSbt, revertJpNameToSaveName(staCsvTemplateModels, x.Columns ?? string.Empty), true, getDisplayColumnName(staCsvTemplateModels, x.Columns ?? string.Empty))).ToList(),
                group.Key.RowNo,
                false
                ));
        }

        foreach (var item in result)
        {
            foreach (var staCsv in item.StaCsvModels)
            {
                staCsv.ChangeIsModified(false);
            }
        }

        return result;
    }

    public void SaveStaCsvMst(int hpId, int userId, List<StaCsvMstModel> staCsvMstModels)
    {
        var addStaCsvMsts = new List<StaCsv>();
        var deleteStaCsvMsts = new List<StaCsv>();
        foreach (var (item, staCsv) in from staCsvMstModel in staCsvMstModels
                                       let ids = staCsvMstModel.StaCsvModelsSelected.Select(s => s.Id).ToList()
                                       let staCsvs = TrackingDataContext.StaCsvs.Where(s => s.HpId == hpId).AsEnumerable().Where(s => ids.Contains(s.Id)).ToList()
                                       from item in staCsvMstModel.StaCsvModelsSelected
                                       where item.IsModified
                                       let staCsv = staCsvs.FirstOrDefault(s => s.Id == item.Id)
                                       select (item, staCsv))
        {
            if (staCsv != null)
            {
                staCsv.ReportId = item.ReportId;
                staCsv.RowNo = item.RowNo;
                staCsv.ConfName = item.ConfName;
                staCsv.DataSbt = item.DataSbt;
                staCsv.Columns = item.Columns;
                staCsv.SortKbn = item.SortKbn;
                staCsv.UpdateDate = CIUtil.GetJapanDateTimeNow();
                staCsv.UpdateId = userId;
            }

            if (item.Id > 0 && item.IsDeleted && staCsv != null)
            {
                deleteStaCsvMsts.Add(staCsv);
            }
            else if (item.Id == 0 && !item.IsDeleted && item.IsSelected)
            {
                var newStaCsv = ConvertStaCsvModelToStaCsv(hpId, userId, item);
                addStaCsvMsts.Add(newStaCsv);
            }
            else if (item.Id > 0 && !item.IsSelected && staCsv != null)
            {
                deleteStaCsvMsts.Add(staCsv);
            }
        }

        TrackingDataContext.StaCsvs.AddRange(addStaCsvMsts);
        TrackingDataContext.StaCsvs.RemoveRange(deleteStaCsvMsts);
        TrackingDataContext.SaveChanges();

    }



    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private function

    private IEnumerable<StaGrpModel> ReloadCacheStaGrpModel(int hpId)
    {
        var finalKey = GetCacheKey() + CacheKeyConstant.StaGrpModel + "_" + hpId;
        var staGrpList = NoTrackingDataContext.StaGrps.Where(item => item.HpId == hpId).ToList();
        var staGrpMstList = staGrpList.Select(item => item.ReportId).Distinct().ToList();
        var starMstList = NoTrackingDataContext.StaMsts.Where(item => item.HpId == hpId && staGrpMstList.Contains(item.ReportId)).ToList();
        var result = staGrpList.Select(grp => new StaGrpModel(
                                                  grp.GrpId,
                                                  grp.ReportId,
                                                  starMstList.FirstOrDefault(mst => mst.ReportId == grp.ReportId)?.ReportName ?? string.Empty,
                                                  grp.SortNo
                               )).ToList();
        var json = JsonSerializer.Serialize(result);
        _cache.StringSet(finalKey, json);
        return result;
    }
    private IEnumerable<StaGrpModel> ReadCacheStaGrpModel(int hpId)
    {
        var finalKey = GetCacheKey() + CacheKeyConstant.StaGrpModel + "_" + hpId;
        var results = _cache.StringGet(finalKey);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<StaGrpModel>>(json) : new();
        return datas ?? new();
    }

    private List<StatisticMenuModel> ConvertToStatisticList(List<StaMenu> staMenuList, List<StaConf> staConfigList)
    {
        List<StatisticMenuModel> result = new();
        foreach (StaMenu staMenu in staMenuList)
        {
            var staConfigItem = staConfigList.Where(item => item.MenuId == staMenu.MenuId)
                                             .Select(item => new StaConfModel(
                                                                 item.MenuId,
                                                                 item.ConfId,
                                                                 item.Val ?? string.Empty))
                                             .ToList();

            result.Add(new StatisticMenuModel(
                           staMenu.MenuId,
                           staMenu.GrpId,
                           staMenu.ReportId,
                           staMenu.SortNo,
                           staMenu.MenuName ?? string.Empty,
                           staMenu.IsPrint,
                           staConfigItem));
        }

        return result;
    }

    private int SaveDailyStatisticMenuAction(int hpId, int userId, List<StatisticMenuModel> statisticMenuModelList)
    {
        int menuIdTemp = 0;
        var menuIdList = statisticMenuModelList.Select(item => item.MenuId).Distinct().ToList();
        var staMenuDBList = TrackingDataContext.StaMenus.Where(item => item.HpId == hpId
                                                                       && menuIdList.Contains(item.MenuId)
                                                                       && item.IsDeleted == 0)
                                                        .ToList();

        var staMenuConfigDBList = TrackingDataContext.StaConfs.Where(item => item.HpId == hpId
                                                                             && menuIdList.Contains(item.MenuId))
                                                              .ToList();

        StaMenu? staMenuTempDb = null;
        if (statisticMenuModelList.Any(item => item.IsSaveTemp))
        {
            staMenuTempDb = TrackingDataContext.StaMenus.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.CreateId == userId
                                                                                && item.IsDeleted == 2);
            if (staMenuTempDb != null)
            {
                staMenuConfigDBList.AddRange(TrackingDataContext.StaConfs.Where(item => item.HpId == hpId
                                                                                        && staMenuTempDb.MenuId == item.MenuId)
                                                                         .ToList());
            }
        }

        foreach (var model in statisticMenuModelList)
        {
            var staMenu = staMenuDBList.FirstOrDefault(item => item.MenuId == model.MenuId);
            if (staMenu == null && !model.IsSaveTemp)
            {
                staMenu = new StaMenu();
                staMenu.HpId = hpId;
                staMenu.MenuId = 0;
                staMenu.CreateDate = CIUtil.GetJapanDateTimeNow();
                staMenu.CreateId = userId;
            }

            if (model.IsSaveTemp)
            {
                if (staMenuTempDb == null)
                {
                    staMenu = new StaMenu();
                    staMenu.HpId = hpId;
                    staMenu.MenuId = 0;
                    staMenu.CreateDate = CIUtil.GetJapanDateTimeNow();
                    staMenu.CreateId = userId;
                }
                else
                {
                    staMenu = staMenuTempDb;

                    // if item is saveTemp item, remove all staConfig temp
                    var allStaConfigDeleted = TrackingDataContext.StaConfs.Where(item => item.MenuId == staMenu.MenuId).ToList();
                    TrackingDataContext.StaConfs.RemoveRange(allStaConfigDeleted);
                    TrackingDataContext.SaveChanges();
                    staMenuConfigDBList = staMenuConfigDBList.Where(menu => !allStaConfigDeleted.Any(config => config.ConfId == menu.ConfId 
                                                                                                               && config.MenuId == menu.MenuId))
                                                             .ToList();
                }
                // if save temp, isDeleted = 2
                staMenu.IsDeleted = 2;
            }
            staMenu!.UpdateDate = CIUtil.GetJapanDateTimeNow();
            staMenu.UpdateId = userId;
            if (model.IsDeleted)
            {
                staMenu.IsDeleted = 1;
                continue;
            }
            staMenu.GrpId = model.GrpId;
            staMenu.ReportId = model.ReportId;
            staMenu.MenuName = model.MenuName;
            staMenu.IsPrint = model.IsPrint;
            staMenu.SortNo = model.SortNo;
            if (staMenu.MenuId == 0)
            {
                TrackingDataContext.StaMenus.Add(staMenu);
                TrackingDataContext.SaveChanges();
            }
            int menuId = staMenu.MenuId;
            if (model.IsSaveTemp)
            {
                menuIdTemp = menuId;
            }
            SaveStaConfig(hpId, userId, menuId, model.StaConfigList, ref staMenuConfigDBList);
        }
        return menuIdTemp;
    }

    private void SaveStaConfig(int hpId, int userId, int menuId, List<StaConfModel> staConfModelList, ref List<StaConf> staConfDBList)
    {
        foreach (var model in staConfModelList)
        {
            bool isAddNew = false;
            var staConf = staConfDBList.FirstOrDefault(item => item.MenuId == menuId && item.ConfId == model.ConfId);
            if (staConf == null)
            {
                isAddNew = true;
                staConf = new StaConf();
                staConf.HpId = hpId;
                staConf.CreateDate = CIUtil.GetJapanDateTimeNow();
                staConf.CreateId = userId;
                staConf.MenuId = menuId;
                staConf.ConfId = model.ConfId;
            }
            staConf.Val = model.Val;
            staConf.UpdateId = userId;
            staConf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            if (isAddNew)
            {
                TrackingDataContext.StaConfs.Add(staConf);
            }
        }
    }

    #endregion

    public bool SaveStaConfMenu(int hpId, int userId, StatisticMenuModel statisticMenu)
    {
        var staMenu = TrackingDataContext.StaMenus.FirstOrDefault(item => item.HpId == hpId && item.MenuId == statisticMenu.MenuId);
        if (staMenu == null && statisticMenu.MenuId > 0)
        {
            return false;
        }
        if (statisticMenu.IsDeleted && statisticMenu.MenuId > 0)
        {
            staMenu!.IsDeleted = 1;
            staMenu.UpdateDate = CIUtil.GetJapanDateTimeNow();
            staMenu.UpdateId = userId;
            return TrackingDataContext.SaveChanges() > 0;
        }

        if (statisticMenu.MenuId == 0 && staMenu == null)
        {
            staMenu = new();
            staMenu.MenuId = 0;
            staMenu.HpId = hpId;
            staMenu.GrpId = 9;
            staMenu.SortNo = statisticMenu.SortNo;
            staMenu.ReportId = 9000;
            staMenu.MenuName = statisticMenu.MenuName;
            staMenu.IsDeleted = 0;
            staMenu.CreateDate = CIUtil.GetJapanDateTimeNow();
            staMenu.CreateId = userId;
        }

        staMenu!.MenuName = statisticMenu.MenuName;
        staMenu.UpdateDate = CIUtil.GetJapanDateTimeNow();
        staMenu.UpdateId = userId;
        if (staMenu.MenuId == 0)
        {
            TrackingDataContext.StaMenus.Add(staMenu);
        }
        TrackingDataContext.SaveChanges();

        return SavePtManagementConf(hpId, userId, staMenu.MenuId, statisticMenu.PatientManagement, statisticMenu.IsDeleted);
    }

    public bool SavePtManagementConf(int hpId, int userId, int menuId, PatientManagementModel patientManagementModel, bool isDeleted)
    {
        if (menuId == 0 || patientManagementModel == null) return true;
        var addStaConfs = new List<StaConf>();
        var staConfs = TrackingDataContext.StaConfs.AsEnumerable().Where(x => x.MenuId == menuId && x.HpId == hpId).ToList();
        TrackingDataContext.StaConfs.RemoveRange(staConfs);

        if (isDeleted)
        {
            return TrackingDataContext.SaveChanges() > 0;
        }

        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.OutputOrder, patientManagementModel.OutputOrder.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.OutputOrder2, patientManagementModel.OutputOrder2.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.OutputOrder3, patientManagementModel.OutputOrder3.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ReportType, patientManagementModel.ReportType.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.PtNumFrom, patientManagementModel.PtNumFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.PtNumTo, patientManagementModel.PtNumTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KanaName, patientManagementModel.KanaName.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Name, patientManagementModel.Name.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.BirthDayFrom, patientManagementModel.BirthDayFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.BirthDayTo, patientManagementModel.BirthDayTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.AgeFrom, patientManagementModel.AgeFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.AgeTo, patientManagementModel.AgeTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.AgeRefDate, patientManagementModel.AgeRefDate.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Sex, patientManagementModel.Sex.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.HomePost, patientManagementModel.ZipCD1 + patientManagementModel.ZipCD2));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Address, patientManagementModel.Address.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.PhoneNumber, patientManagementModel.PhoneNumber.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.RegistrationDateFrom, patientManagementModel.RegistrationDateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.RegistrationDateTo, patientManagementModel.RegistrationDateTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.IncludeTestPt, patientManagementModel.IncludeTestPt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.GroupSelected, patientManagementModel.GroupSelected.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.HokensyaNoFrom, patientManagementModel.HokensyaNoFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.HokensyaNoTo, patientManagementModel.HokensyaNoTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Kigo, patientManagementModel.Kigo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Bango, patientManagementModel.Bango.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.HokenKbn, patientManagementModel.HokenKbn.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiFutansyaNoFrom, patientManagementModel.KohiFutansyaNoFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiFutansyaNoTo, patientManagementModel.KohiFutansyaNoTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiTokusyuNoFrom, patientManagementModel.KohiTokusyuNoFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiTokusyuNoTo, patientManagementModel.KohiTokusyuNoTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ExpireDateFrom, patientManagementModel.ExpireDateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ExpireDateTo, patientManagementModel.ExpireDateTo.AsString()));
        if (!string.IsNullOrEmpty(patientManagementModel.HokenSbtStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.HokenSbt, patientManagementModel.HokenSbtStr.AsString()));
        }
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Houbetu1, patientManagementModel.Houbetu1.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Houbetu2, patientManagementModel.Houbetu2.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Houbetu3, patientManagementModel.Houbetu3.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Houbetu4, patientManagementModel.Houbetu4.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Houbetu5, patientManagementModel.Houbetu5.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Kogaku, patientManagementModel.Kogaku.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiHokenNoFrom, patientManagementModel.KohiHokenNoFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiHokenEdaNoFrom, patientManagementModel.KohiHokenEdaNoFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiHokenNoTo, patientManagementModel.KohiHokenNoTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KohiHokenEdaNoTo, patientManagementModel.KohiHokenEdaNoTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ValidOrExpired, patientManagementModel.ValidOrExpired.AsString())); // 有効/期限切れ
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.StartDateFrom, patientManagementModel.StartDateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.StartDateTo, patientManagementModel.StartDateTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.TenkiDateFrom, patientManagementModel.TenkiDateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.TenkiDateTo, patientManagementModel.TenkiDateTo.AsString()));
        if (!string.IsNullOrEmpty(patientManagementModel.TenkiKbnStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.TenkiKbn, patientManagementModel.TenkiKbnStr.AsString()));
        }
        if (!string.IsNullOrEmpty(patientManagementModel.SikkanKbnStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.SikkanKbn, patientManagementModel.SikkanKbnStr.AsString()));
        }
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.IsDoubt, patientManagementModel.IsDoubt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.SearchWord, patientManagementModel.SearchWord.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.SearchWordMode, patientManagementModel.SearchWordMode.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ByomeiCd, patientManagementModel.ByomeiCdStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ByomeiCdOpt, patientManagementModel.ByomeiCdOpt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.FreeByomei, patientManagementModel.FreeByomeiStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.SindateFrom, patientManagementModel.SindateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.SindateTo, patientManagementModel.SindateTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.LastVisitDateFrom, patientManagementModel.LastVisitDateFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.LastVisitDateTo, patientManagementModel.LastVisitDateTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.NanbyoCds, patientManagementModel.NanbyoCdsStr.AsString()));
        if (!string.IsNullOrEmpty(patientManagementModel.StatuseStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.Status, patientManagementModel.StatuseStr.AsString()));
        }
        if (!string.IsNullOrEmpty(patientManagementModel.UketukeSbtStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.UketukeSbtId, patientManagementModel.UketukeSbtStr.AsString()));
        }
        if (!string.IsNullOrEmpty(patientManagementModel.KaMstStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KaMstId, patientManagementModel.KaMstStr.AsString()));
        }
        if (!string.IsNullOrEmpty(patientManagementModel.UserMstStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.UserMstId, patientManagementModel.UserMstStr.AsString()));
        }
        if (!string.IsNullOrEmpty(patientManagementModel.JikanKbnStr.AsString()))
        {
            addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.JikanKbn, patientManagementModel.JikanKbnStr.AsString()));
        }
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.IsSinkan, patientManagementModel.IsSinkan.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.RaiinAgeFrom, patientManagementModel.RaiinAgeFrom.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.RaiinAgeTo, patientManagementModel.RaiinAgeTo.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.DataKind, patientManagementModel.DataKind.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ItemCds, patientManagementModel.ItemCdStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ItemCdOpt, patientManagementModel.ItemCdOpt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.MedicalSearchWord, patientManagementModel.MedicalSearchWord.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.WordOpt, patientManagementModel.WordOpt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.ItemCmt, patientManagementModel.ItemCmtStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KarteKbns, patientManagementModel.KarteKbnsStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KarteSearchWords, patientManagementModel.KarteSearchWordsStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KarteWordOpt, patientManagementModel.KarteWordOpt.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.StartIraiDate, patientManagementModel.StartIraiDate.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.EndIraiDate, patientManagementModel.EndIraiDate.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KensaItemCds, patientManagementModel.KensaItemCdsStr.AsString()));
        addStaConfs.Add(CreateStaConf(hpId, userId, menuId, StaConfId.KensaItemCdOpt, patientManagementModel.KensaItemCdOpt.AsString()));
        TrackingDataContext.StaConfs.AddRange(addStaConfs);
        return TrackingDataContext.SaveChanges() > 0;
    }

    private StaConf CreateStaConf(int hpId, int userId, int menuId, int configId, string val)
    {
        return new StaConf()
        {
            HpId = hpId,
            ConfId = configId,
            Val = val,
            MenuId = menuId,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateId = userId,
        };
    }

    public List<StatisticMenuModel> GetStatisticMenuModels(int hpId)
    {
        var staconfs = NoTrackingDataContext.StaConfs.Where(x => x.HpId == hpId).ToList();

        var result = NoTrackingDataContext.StaMenus.Where(x => x.HpId == hpId && x.IsDeleted == 0 && x.GrpId == 9)
                                                   .AsEnumerable()
                                                   .Select(x => new StatisticMenuModel(
                                                                                       x.MenuId,
                                                                                       x.GrpId,
                                                                                       x.ReportId,
                                                                                       x.MenuName ?? string.Empty,
                                                                                       x.SortNo,
                                                                                       x.IsDeleted == 1,
                                                                                       false,
                                                                                       GetPatientManagement(hpId, x.MenuId, staconfs)
                                                                                       ))
                                                   .ToList();

        return result;
    }

    public PatientManagementModel GetPatientManagement(int hpId, int menuId, List<StaConf> staconfs)
    {
        if (!staconfs.Any()) return new();

        string outputOrder = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder && x.MenuId == menuId)?.Val ?? string.Empty;
        string outputOrder2 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder2 && x.MenuId == menuId)?.Val ?? string.Empty;
        string outputOrder3 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder3 && x.MenuId == menuId)?.Val ?? string.Empty;
        string reportType = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ReportType && x.MenuId == menuId)?.Val ?? string.Empty;
        string ptNumFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.PtNumFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string ptNumTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.PtNumTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string kanaName = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KanaName && x.MenuId == menuId)?.Val ?? string.Empty;
        string name = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Name && x.MenuId == menuId)?.Val ?? string.Empty;
        string birthDayFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.BirthDayFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string birthDayTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.BirthDayTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string ageFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.AgeFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string ageTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.AgeTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string ageRefDate = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.AgeRefDate && x.MenuId == menuId)?.Val ?? string.Empty;
        string sex = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Sex && x.MenuId == menuId)?.Val ?? string.Empty;
        string homePost = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.HomePost && x.MenuId == menuId)?.Val ?? string.Empty;
        string zipCd1 = string.Empty;
        string zipCd2 = string.Empty;
        if (!string.IsNullOrEmpty(homePost))
        {
            homePost = homePost.Replace("-", string.Empty);
            if (homePost.Length > 3)
            {
                zipCd1 = homePost.Substring(0, 3);
                zipCd2 = homePost.Substring(3);
            }
        }
        string address = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Address && x.MenuId == menuId)?.Val ?? string.Empty;
        string phoneNumber = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.PhoneNumber && x.MenuId == menuId)?.Val ?? string.Empty;
        string registrationDateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.RegistrationDateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string registrationDateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.RegistrationDateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string includeTestPt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.IncludeTestPt && x.MenuId == menuId)?.Val ?? string.Empty;
        string groupSelected = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.GroupSelected && x.MenuId == menuId)?.Val ?? string.Empty;
        string hokensyaNoFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.HokensyaNoFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string hokensyaNoTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.HokensyaNoTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string kigo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Kigo && x.MenuId == menuId)?.Val ?? string.Empty;
        string bango = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Bango && x.MenuId == menuId)?.Val ?? string.Empty;
        string hokenKbn = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.HokenKbn && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiFutansyaNoFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiFutansyaNoFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiFutansyaNoTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiFutansyaNoTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiTokusyuNoFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiTokusyuNoFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiTokusyuNoTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiTokusyuNoTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string expireDateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ExpireDateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string expireDateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ExpireDateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string hokenSbt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.HokenSbt && x.MenuId == menuId)?.Val ?? string.Empty;
        string houbetu1 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu1 && x.MenuId == menuId)?.Val ?? string.Empty;
        string houbetu2 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu2 && x.MenuId == menuId)?.Val ?? string.Empty;
        string houbetu3 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu3 && x.MenuId == menuId)?.Val ?? string.Empty;
        string houbetu4 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu4 && x.MenuId == menuId)?.Val ?? string.Empty;
        string houbetu5 = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu5 && x.MenuId == menuId)?.Val ?? string.Empty;
        string kogaku = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Kogaku && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiHokenNoFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenNoFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiHokenEdaNoFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenEdaNoFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiHokenNoTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenNoTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string kohiHokenEdaNoTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenEdaNoTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string validOrExpired = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ValidOrExpired && x.MenuId == menuId)?.Val ?? string.Empty; // 有効/期限切れ
        string startDateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.StartDateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string startDateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.StartDateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string tenkiDateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.TenkiDateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string tenkiDateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.TenkiDateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string tenkiKbnStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.TenkiKbn && x.MenuId == menuId)?.Val ?? string.Empty;
        string sikkanKbnStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.SikkanKbn && x.MenuId == menuId)?.Val ?? string.Empty;
        string nanbyoCdStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.NanbyoCds && x.MenuId == menuId)?.Val ?? string.Empty;
        string isDoubt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.IsDoubt && x.MenuId == menuId)?.Val ?? string.Empty;
        string searchWord = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.SearchWord && x.MenuId == menuId)?.Val ?? string.Empty;
        string searchWordMode = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.SearchWordMode && x.MenuId == menuId)?.Val ?? string.Empty;
        string byomeiCdStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ByomeiCd && x.MenuId == menuId)?.Val ?? string.Empty;
        string byomeiCdOpt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ByomeiCdOpt && x.MenuId == menuId)?.Val ?? string.Empty;
        string freeByomeiStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.FreeByomei && x.MenuId == menuId)?.Val ?? string.Empty;
        string sindateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.SindateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string sindateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.SindateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string lastVisitDateFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.LastVisitDateFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string lastVisitDateTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.LastVisitDateTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string statuses = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.Status && x.MenuId == menuId)?.Val ?? string.Empty;
        string uketukeSbts = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.UketukeSbtId && x.MenuId == menuId)?.Val ?? string.Empty;
        string kaMsts = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KaMstId && x.MenuId == menuId)?.Val ?? string.Empty;
        string userMsts = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.UserMstId && x.MenuId == menuId)?.Val ?? string.Empty;
        string isSinkan = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.IsSinkan && x.MenuId == menuId)?.Val ?? string.Empty;
        string raiinAgeFrom = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.RaiinAgeFrom && x.MenuId == menuId)?.Val ?? string.Empty;
        string raiinAgeTo = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.RaiinAgeTo && x.MenuId == menuId)?.Val ?? string.Empty;
        string jikbanKbns = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.JikanKbn && x.MenuId == menuId)?.Val ?? string.Empty;
        string dataKind = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.DataKind && x.MenuId == menuId)?.Val ?? string.Empty;
        string itemCds = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ItemCds && x.MenuId == menuId)?.Val ?? string.Empty;
        string itemCdOpt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ItemCdOpt && x.MenuId == menuId)?.Val ?? string.Empty;
        string itemCmtStr = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.ItemCmt && x.MenuId == menuId)?.Val ?? string.Empty;
        string medicalSearchWord = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.MedicalSearchWord && x.MenuId == menuId)?.Val ?? string.Empty;
        string wordOpt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.WordOpt && x.MenuId == menuId)?.Val ?? string.Empty;
        string kartekbn = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KarteKbns && x.MenuId == menuId)?.Val ?? string.Empty;
        string karteSearchWords = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KarteSearchWords && x.MenuId == menuId)?.Val ?? string.Empty;
        string karteWordOpt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KarteWordOpt && x.MenuId == menuId)?.Val ?? string.Empty;
        string startIraiDate = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.StartIraiDate && x.MenuId == menuId)?.Val ?? string.Empty;
        string endIraiDate = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.EndIraiDate && x.MenuId == menuId)?.Val ?? string.Empty;
        string kensaItemCds = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KensaItemCds && x.MenuId == menuId)?.Val ?? string.Empty;
        string kensaItemCdOpt = staconfs.FirstOrDefault(x => x.ConfId == StaConfId.KensaItemCdOpt && x.MenuId == menuId)?.Val ?? string.Empty;

        var result = new PatientManagementModel(
                                              !string.IsNullOrEmpty(outputOrder) ? outputOrder.AsInteger() : 0,
                                              !string.IsNullOrEmpty(outputOrder2) ? outputOrder2.AsInteger() : 0,
                                              !string.IsNullOrEmpty(outputOrder3) ? outputOrder3.AsInteger() : 0,
                                              !string.IsNullOrEmpty(reportType) ? reportType.AsInteger() : 0,
                                              !string.IsNullOrEmpty(ptNumFrom) ? ptNumFrom.AsInteger() : 0,
                                              !string.IsNullOrEmpty(ptNumTo) ? ptNumTo.AsInteger() : 0,
                                              kanaName.AsString(),
                                              name.AsString(),
                                              birthDayFrom.AsInteger(),
                                              birthDayTo.AsInteger(),
                                              ageFrom,
                                              ageTo,
                                              ageRefDate,
                                              sex.AsInteger(),
                                              homePost,
                                              zipCd1,
                                              zipCd2,
                                              address,
                                              phoneNumber,
                                              includeTestPt.AsInteger(),
                                              new(),
                                              registrationDateFrom.AsInteger(),
                                              registrationDateTo.AsInteger(),
                                              groupSelected.AsString(),
                                              hokensyaNoFrom.AsString(),
                                              hokensyaNoTo.AsString(),
                                              kigo.AsString(),
                                              bango.AsString(),
                                              string.Empty,
                                              hokenKbn.AsInteger(),
                                              kohiFutansyaNoFrom,
                                              kohiFutansyaNoTo,
                                              kohiTokusyuNoFrom,
                                              kohiTokusyuNoTo,
                                              expireDateFrom.AsInteger(),
                                              expireDateTo.AsInteger(),
                                              string.IsNullOrEmpty(hokenSbt) ? new List<int>() : hokenSbt.Split(',').Select(x => x.AsInteger()).ToList(),
                                              houbetu1,
                                              houbetu2,
                                              houbetu3,
                                              houbetu4,
                                              houbetu5,
                                              kogaku,
                                              kohiHokenNoFrom.AsInteger(),
                                              kohiHokenEdaNoFrom.AsInteger(),
                                              kohiHokenNoTo.AsInteger(),
                                              kohiHokenEdaNoTo.AsInteger(),
                                              validOrExpired.AsInteger(), // 有効/期限切れ
                                              startDateFrom.AsInteger(),
                                              startDateTo.AsInteger(),
                                              tenkiDateFrom.AsInteger(),
                                              tenkiDateTo.AsInteger(),
                                              isDoubt.AsInteger(),
                                              searchWord,
                                              searchWordMode.AsInteger(),
                                              byomeiCdOpt.AsInteger(),
                                              sindateFrom.AsInteger(),
                                              sindateTo.AsInteger(),
                                              lastVisitDateFrom.AsInteger(),
                                              lastVisitDateTo.AsInteger(),
                                              isSinkan.AsInteger(),
                                              raiinAgeFrom,
                                              raiinAgeTo,
                                              dataKind.AsInteger(),
                                              itemCdOpt.AsInteger(),
                                              medicalSearchWord,
                                              wordOpt.AsInteger(),
                                              karteWordOpt.AsInteger(),
                                              startIraiDate.AsInteger(),
                                              endIraiDate.AsInteger(),
                                              kensaItemCdOpt.AsInteger(),
                                              string.IsNullOrEmpty(tenkiKbnStr) ? new List<int>() : tenkiKbnStr.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(sikkanKbnStr) ? new List<int>() : sikkanKbnStr.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(byomeiCdStr) ? new List<string>() : byomeiCdStr.Split(',').ToList(),
                                              string.IsNullOrEmpty(freeByomeiStr) ? new List<string>() : freeByomeiStr.Split(',').ToList(),
                                              string.IsNullOrEmpty(nanbyoCdStr) ? new List<int>() : nanbyoCdStr.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(statuses) ? new List<int>() : statuses.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(uketukeSbts) ? new List<int>() : uketukeSbts.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(itemCds) ? new List<string>() : itemCds.Split(',').ToList(),
                                              string.IsNullOrEmpty(kaMsts) ? new List<int>() : kaMsts.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(userMsts) ? new List<int>() : userMsts.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(jikbanKbns) ? new List<int>() : jikbanKbns.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(itemCmtStr) ? new List<string>() : itemCmtStr.Split(',').ToList(),
                                              string.IsNullOrEmpty(kartekbn) ? new List<int>() : kartekbn.Split(',').Select(x => x.AsInteger()).ToList(),
                                              string.IsNullOrEmpty(karteSearchWords) ? new List<string>() : karteSearchWords.Split(',').ToList(),
                                              string.IsNullOrEmpty(kensaItemCds) ? new List<string>() : kensaItemCds.Split(',').ToList()
                                              );

        return result;
    }

    private StaCsv ConvertStaCsvModelToStaCsv(int hpId, int userId, StaCsvModel staCsv)
    {
        return new StaCsv
        {
            HpId = hpId,
            Id = staCsv.Id,
            ReportId = staCsv.ReportId,
            RowNo = staCsv.RowNo,
            ConfName = staCsv.ConfName,
            DataSbt = staCsv.DataSbt,
            Columns = staCsv.Columns,
            SortKbn = staCsv.SortKbn,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateId = userId
        };
    }
}
