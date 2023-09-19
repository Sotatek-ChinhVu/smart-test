using Domain.Models.MainMenu;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Util;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StatisticRepository : RepositoryBase, IStatisticRepository
{
    public StatisticRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
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
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            });
        return (menuIdTemp, success);
    }

    public List<StaGrpModel> GetStaGrp(int hpId, int grpId)
    {
        var staGrpList = NoTrackingDataContext.StaGrps.Where(item => item.HpId == hpId && item.GrpId == grpId).ToList();
        var staGrpMstList = staGrpList.Select(item => item.ReportId).Distinct().ToList();
        var starMstList = NoTrackingDataContext.StaMsts.Where(item => item.HpId == hpId && staGrpMstList.Contains(item.ReportId)).ToList();
        var result = staGrpList.Select(grp => new StaGrpModel(
                                                  grp.GrpId,
                                                  grp.ReportId,
                                                  starMstList.FirstOrDefault(mst => mst.ReportId == grp.ReportId)?.ReportName ?? string.Empty,
                                                  grp.SortNo
                               )).ToList();
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
            List<(bool isSelected, string outputColumnName, string saveName)> template = PtManagementUtil.GetStaCsvTemplate(i);
            List<StaCsvModel> staCsvTemplateModels = template.Select(
                    (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.saveName, x.isSelected, x.outputColumnName)
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
                   (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.saveName, x.isSelected, x.outputColumnName)
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
                    (x, idx) => new StaCsvModel(hpId, PtManagementUtil.GetConfName(i), i, idx, 9000, i, x.Columns ,
                            x.IsSelected,
                            x.ColumnsDisplay)).ToList(),
                i - 1,
                true));
        }

        var staCsvModels = NoTrackingDataContext.StaCsvs.Where(x => x.HpId == hpId && x.ReportId == 9000);
        var groupStaCsvModels = staCsvModels.GroupBy(x => new { x.DataSbt, x.ConfName, x.RowNo });
        foreach (var group in groupStaCsvModels)
        {
            List<(bool isSelected, string outputColumnName, string saveName)> template = PtManagementUtil.GetStaCsvTemplate(group.Key.DataSbt);
            List<StaCsvModel> staCsvTemplateModels = template.Select(
                    (x, idx) => new StaCsvModel( hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, idx, 9000, group.Key.DataSbt, x.saveName,
                        x.isSelected,
                        x.outputColumnName)
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
                    (x, idx) => new StaCsvModel( hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(u => u.SortKbn) + 1, 9000, group.Key.DataSbt, x.saveName, x.isSelected, x.outputColumnName)
                ).ToList();
                staCsvTemplateModels.AddRange(staCsvSubTemplateModels);
            }

            foreach (var ptGrpName in _ptGrpNameMsts)
            {
                staCsvTemplateModels.Add(new StaCsvModel( hpId,  group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, group.Key.DataSbt, string.Format("PtGrpCd_{0}", ptGrpName.GrpId), false, string.Format("グループ{0}({1}) 区分コード", ptGrpName.GrpId, ptGrpName.GrpName)));
                staCsvTemplateModels.Add(new StaCsvModel(hpId, group.Key.ConfName ?? string.Empty, group.Key.RowNo, staCsvTemplateModels.Max(x => x.SortKbn) + 1, 9000, group.Key.DataSbt, string.Format("PtGrpCdName_{0}", ptGrpName.GrpId), false,
                 string.Format("グループ{0}({1}) 区分名称", ptGrpName.GrpId, ptGrpName.GrpName)));
            }

            result.Add(new StaCsvMstModel(
                group.Key.ConfName ?? string.Empty,
                group.Key.DataSbt,
                staCsvTemplateModels,
                group.ToList().Select(x => new StaCsvModel(x.Id, x.HpId, x.ConfName ?? string.Empty, x.RowNo, x.SortKbn, x.ReportId, x.DataSbt, revertJpNameToSaveName(staCsvTemplateModels, x.Columns ?? string.Empty), true, getDisplayColumnName(staCsvTemplateModels, x.Columns ?? string.Empty))).ToList(),
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

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private function
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
}
