using Domain.Models.MainMenu;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
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

    public bool SaveStaConfMenu(int hpId, int userId, StatisticMenuModel statisticMenu)
    {
        var addStamenus = new List<StaMenu>();
        if (!statisticMenu.IsDeleted && statisticMenu.IsModified && statisticMenu.MenuId == 0)
        {
            TrackingDataContext.Add(new StaMenu()
            {
                HpId = hpId,
                GrpId = 9,
                ReportId = 9000,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
            });
        }
        else if (statisticMenu.IsModified)
        {
            var staMenuUpdate = TrackingDataContext.StaMenus.FirstOrDefault(x => x.HpId == hpId && x.MenuId == statisticMenu.MenuId);
            if (staMenuUpdate != null)
            {
                staMenuUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                staMenuUpdate.UpdateId = userId;
            }
        }

        TrackingDataContext.SaveChanges();

        return SavePtManagementConf(hpId, userId, statisticMenu.MenuId, statisticMenu.PatientManagement, statisticMenu.IsDeleted);
    }

    public bool SavePtManagementConf(int hpId, int userId, int menuId, PatientManagementItem patientManagementModel, bool isDeleted)
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
}
