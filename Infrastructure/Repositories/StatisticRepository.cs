using Domain.Models.MainMenu;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
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
