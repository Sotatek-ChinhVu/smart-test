﻿using Domain.Constant;
using Domain.Models.Accounting;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.Receipt.ReceiptCreation;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReceiptRepository : RepositoryBase, IReceiptRepository
{
    private readonly IMstItemRepository _mstItemRepository;
    public ReceiptRepository(ITenantProvider tenantProvider, IMstItemRepository mstItemRepository) : base(tenantProvider)
    {
        _mstItemRepository = mstItemRepository;
    }

    #region Rece check list
    public List<ReceiptListModel> GetReceiptList(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel)
    {
        if (seikyuYm == 0)
        {
            return new();
        }
        int fromDay = seikyuYm * 100 + 1;
        int toDay = seikyuYm * 100 + DateTime.DaysInMonth(seikyuYm / 100, seikyuYm % 100);

        List<ReceiptListModel> result = ActionGetReceiptList(hpId, fromDay, toDay, seikyuYm, searchModel);

        return result;
    }

    private List<ReceiptListModel> ActionGetReceiptList(int hpId, int fromDay, int toDay, int seikyuYm, ReceiptListAdvancedSearchInput searchModel)
    {
        List<ItemSumModel> sinYmPtIdForFilterList = new();

        #region Simple query 
        // Rece
        var receInfs = NoTrackingDataContext.ReceInfs.Where(item => item.SeikyuYm == seikyuYm
                                                                    && item.HpId == hpId);

        // 練習患者を表示しない
        if (!searchModel.IsTestPatientSearch)
        {
            receInfs = receInfs.Where(item => item.IsTester != 1);
        }

        List<ReceInf> receInfFilters = new();

        var listPtIds = receInfs.Select(item => item.PtId).Distinct().ToList();
        var minSinYM = receInfs.Select(item => item.SinYm).DefaultIfEmpty().Min();
        var minDay = minSinYM * 100 + 1;

        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId
                                                                        && item.SinDate >= minDay
                                                                        && item.SinDate <= toDay
                                                                        && listPtIds.Contains(item.PtId))
                                                         .Select(item => new { item.PtId, item.HokenId, item.SinDate });

        var receInfEdits = NoTrackingDataContext.ReceInfEdits.Where(item => item.SeikyuYm == seikyuYm
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && listPtIds.Contains(item.PtId))
                                                             .AsEnumerable()
                                                             .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm, item.SeikyuYm })
                                                             .Select(grp => grp.FirstOrDefault() ?? new ReceInfEdit())
                                                             .Select(item => new { item.SinYm, item.SeikyuYm, item.HpId, item.HokenId, item.PtId });

        var receStatuses = NoTrackingDataContext.ReceStatuses.Where(item => item.SeikyuYm == seikyuYm
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && listPtIds.Contains(item.PtId))
                                                            .Select(item => new { item.SinYm, item.SeikyuYm, item.HpId, item.HokenId, item.PtId, item.StatusKbn, item.FusenKbn, item.IsPaperRece, item.Output });

        var receCheckCmts = NoTrackingDataContext.ReceCheckCmts.Where(item => item.IsDeleted == 0
                                                                              && item.HpId == hpId
                                                                              && item.IsChecked == 0
                                                                              && item.SinYm >= minSinYM
                                                                              && item.SinYm <= seikyuYm
                                                                              && listPtIds.Contains(item.PtId))
                                                                .AsEnumerable()
                                                                .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                                .Select(grp => grp.OrderByDescending(item => item.SortNo).FirstOrDefault() ?? new ReceCheckCmt());

        var receCheckErrors = NoTrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                                && item.IsChecked == 0
                                                                                && item.SinYm >= minSinYM
                                                                                && item.SinYm <= seikyuYm
                                                                                && listPtIds.Contains(item.PtId))
                                                                  .AsEnumerable()
                                                                  .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                                  .Select(grp => grp.OrderBy(item => item.ErrCd).FirstOrDefault() ?? new ReceCheckErr());

        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.IsDeleted == 0
                                                                    && item.HpId == hpId
                                                                    && item.SinYm >= minSinYM
                                                                    && item.SinYm <= seikyuYm
                                                                    && listPtIds.Contains(item.PtId))
                                                     .AsEnumerable()
                                                     .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                     .Select(grp => grp.FirstOrDefault() ?? new ReceCmt())
                                                     .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId });

        var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.SeikyuYm == seikyuYm
                                                                          && item.IsDeleted == 0
                                                                          && item.HpId == hpId
                                                                          && listPtIds.Contains(item.PtId));

        var syoukiInfs = NoTrackingDataContext.SyoukiInfs.Where(item => item.IsDeleted == 0
                                                                        && item.HpId == hpId
                                                                        && item.SinYm >= minSinYM
                                                                        && item.SinYm <= seikyuYm
                                                                        && listPtIds.Contains(item.PtId))
                                                         .AsEnumerable()
                                                         .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                         .Select(grp => grp.FirstOrDefault() ?? new SyoukiInf())
                                                         .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId });

        var syobyokeikas = NoTrackingDataContext.SyobyoKeikas.Where(item => item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && !string.IsNullOrEmpty(item.Keika)
                                                                            && item.SinYm >= minSinYM
                                                                            && item.SinYm <= seikyuYm
                                                                            && listPtIds.Contains(item.PtId)).AsEnumerable()
                                                    .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                    .Select(item => new { item.Key.SinYm, item.Key.HpId, item.Key.HokenId, item.Key.PtId });

        // Patient
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && listPtIds.Contains(item.PtId));

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                          && item.IsDeleted == 0
                                                                          && listPtIds.Contains(item.PtId));

        var ptLastVisitDates = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == 0
                                                                             && listPtIds.Contains(item.PtId)
                                                                             && item.Status > RaiinState.TempSave)
                                                              .Select(item => new { item.PtId, item.HpId, item.SinDate, item.RaiinNo });

        var ptKyuseis = NoTrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId
                                                                      && item.IsDeleted == 0
                                                                      && listPtIds.Contains(item.PtId))
                                                       .OrderBy(item => item.EndDate);

        var ptKohis = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId
                                                                  && item.IsDeleted == 0
                                                                  && listPtIds.Contains(item.PtId));

        // Master
        var kaMsts = NoTrackingDataContext.KaMsts.Where(u => u.IsDeleted == 0
                                                             && u.HpId == hpId);

        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.StartDate <= fromDay
                                                                 && toDay <= u.EndDate
                                                                 && u.JobCd == 1
                                                                 && u.IsDeleted == 0
                                                                 && u.HpId == hpId);
        #endregion

        #region AdvancedSearch query for receInfs
        if (searchModel.IsAdvanceSearch)
        {
            var listSinYm = receInfs.Select(item => item.SinYm).Distinct().ToList();

            // レセプト種別
            if (searchModel.HokenSbts != null && searchModel.HokenSbts.Count > 0)
            {
                receInfs = receInfs.Where(item => ((searchModel.HokenSbts.Contains(1) && item.HokenKbn == 1 && item.ReceSbt != null && item.ReceSbt.StartsWith("11"))
                                                || (searchModel.HokenSbts.Contains(2) && item.HokenKbn == 2 && item.ReceSbt != null && item.ReceSbt.StartsWith("11"))
                                                || (searchModel.HokenSbts.Contains(5) && item.ReceSbt != null && item.ReceSbt.StartsWith("12"))
                                                || (searchModel.HokenSbts.Contains(3) && item.ReceSbt != null && item.ReceSbt.StartsWith("13"))
                                                || (searchModel.HokenSbts.Contains(4) && item.ReceSbt != null && item.ReceSbt.StartsWith("14"))
                                                || (searchModel.HokenSbts.Contains(0) && item.HokenKbn == 0)
                                                || (searchModel.HokenSbts.Contains(11) && (item.HokenKbn == 11 || item.HokenKbn == 12 || item.HokenKbn == 13))
                                                || (searchModel.HokenSbts.Contains(14) && (item.HokenKbn == 14))));
                if (searchModel.ReceSbtCenter >= 0)
                {
                    receInfs = receInfs.Where(item => ((searchModel.HokenSbts.Contains(1)
                                                     || searchModel.HokenSbts.Contains(2)
                                                     || searchModel.HokenSbts.Contains(3)
                                                     || searchModel.HokenSbts.Contains(4)
                                                     || searchModel.HokenSbts.Contains(5))
                                                    && item.ReceSbt != null
                                                    && item.ReceSbt.Substring(2, 1) == searchModel.ReceSbtCenter.ToString())
                                                || (searchModel.HokenSbts.Contains(0) && item.HokenKbn == 0)
                                                || (searchModel.HokenSbts.Contains(11) && (item.HokenKbn == 11 || item.HokenKbn == 12 || item.HokenKbn == 13))
                                                || (searchModel.HokenSbts.Contains(14) && (item.HokenKbn == 14)));
                }
                if (searchModel.ReceSbtRight >= 0)
                {
                    receInfs = receInfs.Where(item => ((searchModel.HokenSbts.Contains(1)
                                                     || searchModel.HokenSbts.Contains(2)
                                                     || searchModel.HokenSbts.Contains(3)
                                                     || searchModel.HokenSbts.Contains(4)
                                                     || searchModel.HokenSbts.Contains(5))
                                                     && item.ReceSbt != null
                                                     && item.ReceSbt.Substring(3, 1) == searchModel.ReceSbtRight.ToString())
                                                || (searchModel.HokenSbts.Contains(0) && item.HokenKbn == 0)
                                                || (searchModel.HokenSbts.Contains(11) && (item.HokenKbn == 11 || item.HokenKbn == 12 || item.HokenKbn == 13))
                                                || (searchModel.HokenSbts.Contains(14) && (item.HokenKbn == 14)));
                }
            }
            else
            {
                if (searchModel.ReceSbtCenter >= 0)
                {
                    receInfs = receInfs.Where(item => item.ReceSbt != null && item.ReceSbt.Substring(2, 1) == searchModel.ReceSbtCenter.ToString());
                }
                if (searchModel.ReceSbtRight >= 0)
                {
                    receInfs = receInfs.Where(item => item.ReceSbt != null && item.ReceSbt.Substring(3, 1) == searchModel.ReceSbtRight.ToString());
                }

            }

            // 法別番号
            if (!string.IsNullOrEmpty(searchModel.HokenHoubetu))
            {
                receInfs = receInfs.Where(item => item.Houbetu == searchModel.HokenHoubetu.ToString());
            }

            if (searchModel.Kohi1Houbetu != 0)
            {
                receInfs = receInfs.Where(item => (item.Kohi1Houbetu != null && item.Kohi1Houbetu.Equals(searchModel.Kohi1Houbetu.ToString()))
                                                  || (item.Kohi3Houbetu != null && item.Kohi3Houbetu.Equals(searchModel.Kohi1Houbetu.ToString()))
                                                  || (item.Kohi2Houbetu != null && item.Kohi2Houbetu.Equals(searchModel.Kohi1Houbetu.ToString()))
                                                  || (item.Kohi4Houbetu != null && item.Kohi4Houbetu.Equals(searchModel.Kohi1Houbetu.ToString())));
            }

            if (searchModel.Kohi2Houbetu != 0)
            {
                receInfs = receInfs.Where(item => (item.Kohi1Houbetu != null && item.Kohi1Houbetu.Equals(searchModel.Kohi2Houbetu.ToString()))
                                                || (item.Kohi2Houbetu != null && item.Kohi2Houbetu.Equals(searchModel.Kohi2Houbetu.ToString()))
                                                || (item.Kohi3Houbetu != null && item.Kohi3Houbetu.Equals(searchModel.Kohi2Houbetu.ToString()))
                                                || (item.Kohi4Houbetu != null && item.Kohi4Houbetu.Equals(searchModel.Kohi2Houbetu.ToString())));
            }

            if (searchModel.Kohi3Houbetu != 0)
            {
                receInfs = receInfs.Where(item => (item.Kohi1Houbetu != null && item.Kohi1Houbetu.Equals(searchModel.Kohi3Houbetu.ToString()))
                                                || (item.Kohi2Houbetu != null && item.Kohi2Houbetu.Equals(searchModel.Kohi3Houbetu.ToString()))
                                                || (item.Kohi3Houbetu != null && item.Kohi3Houbetu.Equals(searchModel.Kohi3Houbetu.ToString()))
                                                || (item.Kohi4Houbetu != null && item.Kohi4Houbetu.Equals(searchModel.Kohi3Houbetu.ToString())));
            }

            if (searchModel.Kohi4Houbetu != 0)
            {
                receInfs = receInfs.Where(item => (item.Kohi1Houbetu != null && item.Kohi1Houbetu.Equals(searchModel.Kohi4Houbetu.ToString()))
                                                || (item.Kohi2Houbetu != null && item.Kohi2Houbetu.Equals(searchModel.Kohi4Houbetu.ToString()))
                                                || (item.Kohi3Houbetu != null && item.Kohi3Houbetu.Equals(searchModel.Kohi4Houbetu.ToString()))
                                                || (item.Kohi4Houbetu != null && item.Kohi4Houbetu.Equals(searchModel.Kohi4Houbetu.ToString())));
            }

            // 単独扱いを含む
            if ((searchModel.Kohi1Houbetu != 0
                || searchModel.Kohi2Houbetu != 0
                || searchModel.Kohi3Houbetu != 0
                || searchModel.Kohi4Houbetu != 0)
                && !searchModel.IsIncludeSingle)
            {
                receInfs = receInfs.Where(item => (!string.IsNullOrEmpty(item.Kohi1Houbetu) && item.Kohi1ReceKisai == 1)
                                                 || (!string.IsNullOrEmpty(item.Kohi2Houbetu) && item.Kohi2ReceKisai == 1)
                                                 || (!string.IsNullOrEmpty(item.Kohi3Houbetu) && item.Kohi3ReceKisai == 1)
                                                 || (!string.IsNullOrEmpty(item.Kohi4Houbetu) && item.Kohi4ReceKisai == 1));
            }

            // 保険者番号
            var tempPtHokenInfs = ptHokenInfs.AsEnumerable();
            if (searchModel.HokensyaNoFromLong > 0)
            {
                tempPtHokenInfs = tempPtHokenInfs.Where(item => item.HokensyaNo != null && item.HokensyaNo.AsLong() >= searchModel.HokensyaNoFromLong);
            }
            if (searchModel.HokensyaNoToLong > 0)
            {
                tempPtHokenInfs = tempPtHokenInfs.Where(item => item.HokensyaNo != null && item.HokensyaNo.AsLong() <= searchModel.HokensyaNoToLong);
            }

            if (searchModel.HokensyaNoFromLong > 0 || searchModel.HokensyaNoToLong > 0)
            {
                var listPtHokenInfs = tempPtHokenInfs.Select(pthk => pthk.HokensyaNo).Distinct().ToList();
                receInfs = receInfs.Where(item => listPtHokenInfs.Contains(item.HokensyaNo));
            }

            // 点数
            if (searchModel.TensuFrom > 0)
            {
                receInfs = receInfs.Where(item => item.Tensu >= searchModel.TensuFrom);
            }
            if (searchModel.TensuTo > 0)
            {
                receInfs = receInfs.Where(item => item.Tensu <= searchModel.TensuTo);
            }

            // 最終来院日
            if (searchModel.LastRaiinDateFrom > 0 || searchModel.LastRaiinDateTo > 0)
            {

                ptLastVisitDates = ptLastVisitDates.Where(p => (p.SinDate >= searchModel.LastRaiinDateFrom && p.SinDate <= searchModel.LastRaiinDateTo)
                                                               || (searchModel.LastRaiinDateTo == 0 && p.SinDate >= searchModel.LastRaiinDateFrom));
                var ptIds = ptLastVisitDates.Select(p => p.PtId).Distinct().ToList();
                receInfs = receInfs.Where(item => ptIds.Contains(item.PtId));
            }

            // 患者番号
            if (searchModel.PtSearchOption == PtIdSearchOptionEnum.RangeSearch
                && (searchModel.PtIdFrom > 0 || searchModel.PtIdTo > 0))
            {
                if (searchModel.PtIdFrom > 0)
                {
                    ptInfs = ptInfs.Where(item => item.PtNum >= searchModel.PtIdFrom);
                }
                if (searchModel.PtIdTo > 0)
                {
                    ptInfs = ptInfs.Where(item => item.PtNum <= searchModel.PtIdTo);
                }
                listPtIds = ptInfs.Select(pt => pt.PtId).Distinct().ToList();
                receInfs = receInfs.Where(item => listPtIds.Contains(item.PtId));
            }
            else if (searchModel.PtSearchOption == PtIdSearchOptionEnum.IndividualSearch && !string.IsNullOrEmpty(searchModel.PtId))
            {
                List<long> ptIdList = searchModel.PtId.Split(',').Select(item => item.AsLong()).ToList();
                ptInfs = ptInfs.Where(item => ptIdList.Contains(item.PtNum));
                listPtIds = ptInfs.Select(pt => pt.PtId).Distinct().ToList();
                receInfs = receInfs.Where(item => listPtIds.Contains(item.PtId));
            }

            // 診療科 + 担当医 + SYSTEM_CONFIG 6002
            if (searchModel.KaId > 0
                && searchModel.DoctorId > 0
                && GetSettingValue(6002, 0, 0) == 1
                && GetSettingValue(6002, 1, 0) == 1)
            {
                var raiinInfKaIdTantoIds = NoTrackingDataContext.RaiinInfs.Where(item => (item.SinDate >= fromDay || item.SinDate >= minSinYM)
                                                                                      && item.SinDate <= toDay
                                                                                      && item.IsDeleted == 0
                                                                                      && item.KaId == searchModel.KaId
                                                                                      && item.TantoId == searchModel.DoctorId
                                                                                      && listSinYm.Contains(item.SinDate / 100)
                                                                                      && item.Status >= RaiinState.Calculate
                                                                                      && listPtIds.Contains(item.PtId))
                                                                           .AsEnumerable()
                                                                           .Select(item => new { item.HpId, item.PtId, SinYm = item.SinDate / 100, item.KaId, item.TantoId })
                                                                           .GroupBy(item => new { item.HpId, item.PtId, item.SinYm, item.KaId, item.TantoId })
                                                                           .Select(grp => grp.FirstOrDefault());

                var receInfFilter = from receInf in receInfs.AsEnumerable()
                                    join raiinInf in raiinInfKaIdTantoIds on receInf.PtId equals raiinInf.PtId
                                    where receInf.SinYm == raiinInf.SinYm
                                    select receInf;

                receInfs = receInfFilter.AsQueryable();
            }
            else
            {
                // 診療科
                if (searchModel.KaId > 0)
                {
                    if (GetSettingValue(6002, 0, 0) == 0)
                    {
                        receInfs = receInfs.Where(item => item.KaId == searchModel.KaId);
                    }
                    else
                    {
                        var raiinInfKaIds = NoTrackingDataContext.RaiinInfs.Where(item => (item.SinDate >= fromDay || item.SinDate >= minSinYM)
                                                                                           && item.SinDate <= toDay
                                                                                           && item.IsDeleted == 0
                                                                                           && item.KaId == searchModel.KaId
                                                                                           && item.Status >= RaiinState.Calculate
                                                                                           && listSinYm.Contains(item.SinDate / 100)
                                                                                           && listPtIds.Contains(item.PtId))
                                                    .AsEnumerable()
                                                    .Select(item => new { item.HpId, item.PtId, SinYm = item.SinDate / 100, item.KaId })
                                                    .GroupBy(item => new { item.HpId, item.PtId, item.SinYm, item.KaId })
                                                    .Select(grp => grp.FirstOrDefault());

                        var receInfFilter = from receInf in receInfs.AsEnumerable()
                                            join raiinInf in raiinInfKaIds on receInf.PtId equals raiinInf.PtId
                                            where receInf.SinYm == raiinInf.SinYm
                                            select receInf;

                        receInfs = receInfFilter.AsQueryable();
                    }
                }

                // 担当医
                if (searchModel.DoctorId > 0)
                {
                    if (GetSettingValue(6002, 1, 0) == 0)
                    {
                        receInfs = receInfs.Where(item => item.TantoId == searchModel.DoctorId);
                    }
                    else
                    {
                        var raiinInfTantoIds = NoTrackingDataContext.RaiinInfs.Where(item => (item.SinDate >= fromDay || item.SinDate >= minSinYM)
                                                                                       && item.SinDate <= toDay
                                                                                       && item.IsDeleted == 0
                                                                                       && searchModel.DoctorId == item.TantoId
                                                                                       && listSinYm.Contains(item.SinDate / 100)
                                                                                       && item.Status >= RaiinState.Calculate
                                                                                       && listPtIds.Contains(item.PtId))
                                                                               .AsEnumerable()
                                                                               .Select(item => new { item.HpId, item.PtId, SinYm = item.SinDate / 100, item.TantoId })
                                                                               .GroupBy(item => new { item.HpId, item.PtId, item.SinYm, item.TantoId })
                                                                               .Select(grp => grp.FirstOrDefault());
                        var receInfFilter = from receInf in receInfs.AsEnumerable()
                                            join raiinInf in raiinInfTantoIds on receInf.PtId equals raiinInf.PtId
                                            where receInf.SinYm == raiinInf.SinYm
                                            select receInf;

                        receInfs = receInfFilter.AsQueryable();
                    }
                }
            }

            // 氏名
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                ptInfs = ptInfs.Where(item => (item.Name != null && item.Name.Contains(searchModel.Name))
                                            || (item.KanaName != null && item.KanaName.StartsWith(searchModel.Name))
                                            || (item.Name != null && item.Name.Replace(" ", string.Empty).Replace("　", string.Empty).Contains(searchModel.Name))
                                            || (item.KanaName != null && item.KanaName.Replace(" ", string.Empty).Replace("　", string.Empty).StartsWith(searchModel.Name)));
            }

            // 生年月日
            if (searchModel.BirthDayFrom > 0 || searchModel.BirthDayTo > 0)
            {
                if (searchModel.BirthDayFrom > 0)
                {
                    ptInfs = ptInfs.Where(item => item.Birthday >= searchModel.BirthDayFrom);
                }
                if (searchModel.BirthDayTo > 0)
                {
                    ptInfs = ptInfs.Where(item => item.Birthday <= searchModel.BirthDayTo);
                }
                listPtIds = ptInfs.Select(pt => pt.PtId).Distinct().ToList();
                receInfs = receInfs.Where(item => listPtIds.Contains(item.PtId));
            }

            // 印刷済を表示しない
            if (searchModel.IsNotDisplayPrinted)
            {
                receInfs = receInfs.Where(item => receStatuses.FirstOrDefault(rcs => rcs.PtId == item.PtId
                                                                            && rcs.HokenId == item.HokenId
                                                                            && rcs.SeikyuYm == item.SeikyuYm
                                                                            && rcs.SinYm == item.SinYm)!.Output != 1);
            }

            // グループ
            if (searchModel.GroupSearchModels.Any(item => !string.IsNullOrEmpty(item.Value)))
            {
                var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(ptGrpInf => ptGrpInf.IsDeleted == 0
                                                                                  && listPtIds.Contains(ptGrpInf.PtId))
                                                               .ToList();

                foreach (var group in searchModel.GroupSearchModels)
                {
                    if (string.IsNullOrEmpty(group.Value))
                    {
                        continue;
                    }
                    listPtIds = NoTrackingDataContext.PtGrpInfs.Where(ptGr => ptGr.GroupId == group.Key
                                                                              && ptGr.GroupCode == group.Value
                                                                              && ptGr.IsDeleted == 0
                                                                              && listPtIds.Contains(ptGr.PtId))
                                                               .Select(item => item.PtId)
                                                               .Distinct()
                                                               .ToList();

                    listPtIds = ptGrpInfs.Where(ptGr => listPtIds.Contains(ptGr.PtId))
                                         .Select(item => item.PtId)
                                         .Distinct()
                                         .ToList();

                    receInfs = receInfs.Where(pt => listPtIds.Contains(pt.PtId));
                }
            }

            // 項目
            // Group item by sinYm and ptId for query
            var sinYmPtIdList = receInfs.GroupBy(item => new { item.SinYm, item.PtId, item.HokenId }).Select(item => new { item.Key.PtId, item.Key.SinYm, item.Key.HokenId }).ToList();
            List<int> sinYmGroup = sinYmPtIdList.GroupBy(item => item.SinYm).Select(item => item.Key).ToList();

            var listTenMstSearch = searchModel.ItemList;
            if (listTenMstSearch?.Count > 0 && listTenMstSearch.Find(item => !string.IsNullOrEmpty(item.InputName)) != null
                && sinYmPtIdList != null
                && sinYmPtIdList.Any())
            {
                #region ItemList
                var originItemOrderList = listTenMstSearch.Where(item => item.OrderStatus == 1).Select(item => item.ItemCd).ToList();
                var originItemSanteiList = listTenMstSearch.Where(item => item.OrderStatus == 0).Select(item => item.ItemCd).ToList();
                var listFreeComment = listTenMstSearch.Where(item => (string.IsNullOrEmpty(item.ItemCd)
                                                                      && item.IsComment
                                                                      && !string.IsNullOrEmpty(item.InputName))
                                                                      || item.ItemCd.StartsWith("CO"))
                                                      .Select(item => item.InputName).ToList();
                var tenMstSanteis = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                                && item.SanteiItemCd != "9999999999"
                                                                                && !string.IsNullOrEmpty(item.SanteiItemCd)
                                                                                && originItemSanteiList.Contains(item.ItemCd));
                var santeiItemList = tenMstSanteis.GroupBy(item => item.SanteiItemCd).Select(item => item.Key).ToList();
                var santeiItemListWithItemCd = tenMstSanteis.GroupBy(item => new { item.ItemCd, SanteiItemCd = item.SanteiItemCd ?? string.Empty })
                                                            .Select(item => new { item.Key.ItemCd, item.Key.SanteiItemCd }).ToList();

                IEnumerable<ItemSumModel> enumOdrDetailItemSum = Enumerable.Empty<ItemSumModel>();
                List<ItemSumModel> santeiItemSum = new();
                #endregion

                if (originItemOrderList != null && originItemOrderList.Any())
                {
                    #region Count and sum item from order
                    int maxSinYm = (sinYmGroup.DefaultIfEmpty(0).Max() * 100) + 31;
                    int minSinYm = (sinYmGroup.DefaultIfEmpty(0).Min() * 100) + 1;

                    var hokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                                            && item.IsDeleted == 0
                                                                                            && listPtIds.Contains(item.PtId))
                                                                             .Select(item => new { item.HpId, item.HokenId, item.HokenPid, item.PtId });

                    var tenMstOdrs = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                                 && originItemOrderList.Contains(item.ItemCd)
                                                                                 && item.ItemCd != item.SanteiItemCd);

                    var listHokenPId = hokenPatterns.Select(item => item.HokenPid).Distinct().ToList();
                    var odrInfs = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                              && item.SinDate <= maxSinYm
                                                                              && item.SinDate >= minSinYm
                                                                              && item.IsDeleted == 0
                                                                              && listHokenPId.Contains(item.HokenPid)
                                                                              && listSinYm.Contains(item.SinDate / 100)
                                                                              && listPtIds.Contains(item.PtId))
                                                        .Select(item => new { item.HpId, item.PtId, item.SinDate, item.RaiinNo, item.RpEdaNo, item.RpNo, item.HokenPid });

                    // Add santei item to orderItemList for query
                    var santeiItemCdList = tenMstOdrs.GroupBy(item => item.SanteiItemCd).Select(item => item.Key).ToList();
                    var orderItemList = originItemOrderList;
                    foreach (var itemCd in santeiItemCdList)
                    {
                        if (itemCd == null || orderItemList.Contains(itemCd)) { continue; }
                        orderItemList.Add(itemCd);
                    }

                    var odrDetails = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                       && item.SinDate <= maxSinYm
                                                                                       && item.SinDate >= minSinYm
                                                                                       && listPtIds.Contains(item.PtId)
                                                                                       && listSinYm.Contains(item.SinDate / 100)
                                                                                       && (item.ItemCd != null && orderItemList.Contains(item.ItemCd)) // For normal item
                                                                                       || (listFreeComment.Any() // For free comment
                                                                                       && item.ItemCd == null
                                                                                       && item.ItemName != null
                                                                                       ))
                                                                        .Select(item => new OrdInfDetailModel(
                                                                            item.HpId,
                                                                            item.SinDate,
                                                                            item.RaiinNo,
                                                                            item.RpEdaNo,
                                                                            item.RpNo,
                                                                            item.PtId,
                                                                            item.ItemCd,
                                                                            item.Suryo, item.ItemName));

                    enumOdrDetailItemSum = (from odrDetail in odrDetails.AsEnumerable()
                                            join rece in receInfs on new { odrDetail.HpId, odrDetail.PtId, odrDetail.SinYm } equals new { rece.HpId, rece.PtId, rece.SinYm }
                                            join odr in odrInfs on new { odrDetail.HpId, odrDetail.PtId, odrDetail.SinDate, odrDetail.RaiinNo, odrDetail.RpEdaNo, odrDetail.RpNo }
                                                                equals new { odr.HpId, odr.PtId, odr.SinDate, odr.RaiinNo, odr.RpEdaNo, odr.RpNo }
                                            join hokenPattern in hokenPatterns on new { odr.HpId, odr.PtId, odr.HokenPid }
                                                                            equals new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.HokenPid } into hokenPatternLefts
                                            from hokenPatternLeft in hokenPatternLefts.DefaultIfEmpty()
                                            join tenMst in tenMstOdrs on new { odrDetail.ItemCd } equals new { ItemCd = tenMst.SanteiItemCd } into tenMstLeft
                                            from tenMst in tenMstLeft.Where(item => item.StartDate <= odrDetail.SinDate && item.EndDate >= odrDetail.SinDate).DefaultIfEmpty()
                                            where odrDetail.SinDate <= maxSinYm
                                                  && odrDetail.SinDate >= minSinYm
                                                  && (odrDetail.ItemCd != null && orderItemList.Contains(odrDetail.ItemCd)) // For normal item
                                                  || (listFreeComment.Any() // For free comment
                                                  && odrDetail.ItemCd == null
                                                  && odrDetail.ItemName != null
                                                  && listFreeComment.Any(str => odrDetail.ItemName.Contains(str)))
                                            select new
                                            {
                                                odrDetail.PtId,
                                                ItemCd = tenMst != null ? tenMst.ItemCd : odrDetail.ItemCd,
                                                Suryo = odrDetail.Suryo > 0 ? odrDetail.Suryo : 1,
                                                odrDetail.ItemName,
                                                odrDetail.SinYm,
                                                HokenId = hokenPatternLeft?.HokenId ?? 0
                                            })
                                        .GroupBy(item => new { item.PtId, item.ItemCd, item.ItemName, item.SinYm, item.HokenId })
                                        .Select(item => new ItemSumModel(item.Key.PtId, item.Key.ItemCd, item.Key.ItemName, item.Sum(x => x.Suryo), item.Key.SinYm, item.Key.HokenId));

                    var ptIds = sinYmPtIdList.Select(r => r.PtId).Distinct().ToList();
                    var sinYms = sinYmPtIdList.Select(r => r.SinYm).Distinct().ToList();
                    enumOdrDetailItemSum = enumOdrDetailItemSum.Where(item => sinYmPtIdList != null
                                                                              && sinYmPtIdList.Any(r => ptIds.Contains(r.PtId) && sinYms.Contains(r.SinYm))
                                                                ).ToList();
                    #endregion
                }

                if (originItemSanteiList != null && originItemSanteiList.Any())
                {
                    #region Count and sum item from santei
                    int maxSinYm = sinYmGroup.DefaultIfEmpty(0).Max();
                    int minSinYm = sinYmGroup.DefaultIfEmpty(0).Min();

                    var sinkouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                            && item.IsDeleted == 0
                                                                                            && listPtIds.Contains(item.PtId)
                                                                                            && item.SinYm <= maxSinYm
                                                                                            && item.SinYm >= minSinYm
                                                                                            && (santeiItemList.Contains(item.ItemCd) && !string.IsNullOrEmpty(item.ItemCd)) // for santei item
                                                                                            || (listFreeComment.Any() // For free comment
                                                                                                && (item.ItemCd == null || ItemCdConst.CommentFree == item.ItemCd)
                                                                                                && item.ItemName != null
                                                                                                && listFreeComment.Any(str => item.ItemName.Contains(str))
                                                                               ));

                    var sinKouis = NoTrackingDataContext.SinKouis.Where(item => item.HpId == hpId
                                                                                && item.IsDeleted == 0
                                                                                && listSinYm.Contains(item.SinYm)
                                                                                && listPtIds.Contains(item.PtId))
                                                                 .Select(item => new { item.HpId, item.PtId, item.SinYm, item.RpNo, item.SeqNo, item.HokenId, item.InoutKbn });


                    var sinkouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                                          && listPtIds.Contains(item.PtId)
                                                                                          && listSinYm.Contains(item.SinYm))
                                                                           .Select(item => new { item.HpId, item.PtId, item.SinYm, item.SeqNo, item.RpNo, item.Count });

                    var santeiItemSumQuery = (from detail in sinkouiDetails.AsEnumerable()
                                              join rece in receInfs on new { detail.PtId, detail.SinYm } equals new { rece.PtId, rece.SinYm }
                                              join sinkoui in sinKouis on new { detail.HpId, detail.PtId, detail.SinYm, detail.SeqNo, detail.RpNo }
                                                                      equals new { sinkoui.HpId, sinkoui.PtId, sinkoui.SinYm, sinkoui.SeqNo, sinkoui.RpNo }
                                              join count in sinkouiCounts on new { detail.HpId, detail.PtId, detail.SinYm, detail.SeqNo, detail.RpNo }
                                                                          equals new { count.HpId, count.PtId, count.SinYm, count.SeqNo, count.RpNo }
                                              join tenMst in tenMstSanteis on new { detail.ItemCd } equals new { ItemCd = tenMst.SanteiItemCd } into tenMstLeft
                                              from tenMst in tenMstLeft.Where(item => item.StartDate / 100 <= detail.SinYm && item.EndDate / 100 >= detail.SinYm).DefaultIfEmpty()
                                              where detail.SinYm <= maxSinYm
                                                  && detail.SinYm >= minSinYm
                                                  && ((sinkoui.InoutKbn != 1 && tenMst != null) || detail.ItemCd == ItemCdConst.CommentFree)
                                              select new
                                              {
                                                  detail.HpId,
                                                  detail.SinYm,
                                                  detail.PtId,
                                                  sinkoui.HokenId,
                                                  ItemCd = tenMst != null ? tenMst.ItemCd : detail.ItemCd,
                                                  Count = count.Count > 0 ? count.Count : 1,
                                                  detail.ItemName,
                                              })
                                    .GroupBy(item => new { item.HpId, item.ItemCd, item.ItemName, item.PtId, item.SinYm, item.HokenId })
                                    .Select(item => new { item.Key.SinYm, item.Key.PtId, item.Key.HokenId, item.Key.ItemCd, item.Key.ItemName, Sum = item.Sum(c => c.Count) })
                                    .Select(item => new ItemSumModel(item.PtId, item.ItemCd, item.ItemName, item.Sum, item.SinYm, item.HokenId));

                    var ptIds = sinYmPtIdList.Select(r => r.PtId).Distinct().ToList();
                    var sinYms = sinYmPtIdList.Select(r => r.SinYm).Distinct().ToList();
                    santeiItemSum = santeiItemSumQuery.Where(item => sinYmPtIdList != null && sinYmPtIdList.Any(r => ptIds.Contains(r.PtId) && sinYms.Contains(r.SinYm))).ToList();
                    #endregion
                }

                // Search item by condition (Santei-order, and-or, compare)
                int index = 0;
                sinYmPtIdList.Clear();
                foreach (var model in listTenMstSearch)
                {
                    var itemCd = model.ItemCd;
                    var itemName = model.InputName;
                    IEnumerable<ItemSumModel> itemSumList;

                    // Search by santei item
                    if (model.OrderStatus == 0)
                    {
                        itemSumList = santeiItemSum;
                        // Get santei item
                        var itemSantei = santeiItemListWithItemCd.FirstOrDefault(item => item.ItemCd == model.ItemCd);
                        if (!string.IsNullOrEmpty(itemCd) && !itemCd.StartsWith("CO"))
                        {
                            if (itemSantei == null) { continue; }
                            itemCd = itemSantei.SanteiItemCd;
                        }
                    }

                    // Search by order item
                    else
                    {
                        itemSumList = enumOdrDetailItemSum;
                    }

                    // Normal item. Filter by itemcd
                    if (!string.IsNullOrEmpty(itemCd) && !ItemCdConst.CommentFree.Equals(itemCd) && !itemCd.StartsWith("CO"))
                    {
                        itemSumList = itemSumList.Where(item => item.ItemCd == itemCd || (model.OrderStatus == 0 && item.ItemCd == model.ItemCd)).ToList();
                    }

                    // In case free comment. Filter by item name
                    else
                    {
                        itemSumList = itemSumList.Where(item => item.ItemName.Contains(itemName)).ToList();
                    }

                    // Process next item if query = OR and list-item empty
                    if (searchModel.ItemQuery == QuerySearchEnum.OR && itemSumList?.Count() == 0) continue;

                    // Search by range
                    if (!string.IsNullOrEmpty(model.RangeSeach))
                    {
                        switch (model.RangeSeach)
                        {
                            case "=":
                                itemSumList = itemSumList!.Where(item => item.ItemCd == itemCd && item.Sum == model.Amount).ToList();
                                break;
                            case ">":
                                itemSumList = itemSumList!.Where(item => item.ItemCd == itemCd && item.Sum > model.Amount).ToList();
                                break;
                            case "<":
                                itemSumList = itemSumList!.Where(item => item.ItemCd == itemCd && item.Sum < model.Amount).ToList();
                                break;
                            case ">=":
                                itemSumList = itemSumList!.Where(item => item.ItemCd == itemCd && item.Sum >= model.Amount).ToList();
                                break;
                            case "<=":
                                itemSumList = itemSumList!.Where(item => item.ItemCd == itemCd && item.Sum <= model.Amount).ToList();
                                break;
                        }
                    }

                    // Search OR AND condition
                    if (searchModel.ItemQuery == QuerySearchEnum.OR)
                    {
                        sinYmPtIdList.AddRange(itemSumList!.Select(sum => new { sum.PtId, sum.SinYm, sum.HokenId }).ToList());
                        sinYmPtIdList = sinYmPtIdList.Distinct().ToList();
                    }
                    else
                    {
                        sinYmPtIdList.AddRange(itemSumList!.Select(sum => new { sum.PtId, sum.SinYm, sum.HokenId }).ToList());
                        if (index > 0)
                        {
                            sinYmPtIdList = sinYmPtIdList.GroupBy(l => l)
                                                         .Where(g => g.Count() > 1)
                                                         .Select(g => g.Key).ToList();
                        }
                        if (sinYmPtIdList.Count == 0)
                        {
                            return new List<ReceiptListModel>();
                        }
                    }
                    index++;
                }

                // Add correct list for filter later
                sinYmPtIdForFilterList.AddRange(sinYmPtIdList.Select(s => new ItemSumModel(s.PtId, s.SinYm, s.HokenId)).ToList());
            }

            // 病名
            var listByoMstSearch = searchModel.ByomeiCdList;
            if (listByoMstSearch.Any(item => !string.IsNullOrEmpty(item.InputName)))
            {
                string valueCheck = ByomeiConstant.SuspectedCode;
                int index = 0;
                IQueryable<PtByomei> ptByomeiTemp = null!;
                foreach (var item in listByoMstSearch)
                {
                    bool isFreeByomei = item.IsComment;
                    var ptByoNextQuery = NoTrackingDataContext.PtByomeis.Where(x => x.HpId == hpId
                                                                                    && (isFreeByomei ? x.ByomeiCd.Trim() == ByomeiConstant.FreeWordCode : x.ByomeiCd.Trim() == item.ByomeiCd.Trim())
                                                                                    && (!isFreeByomei || x.Byomei.Trim().Contains(item.InputName.Trim()))
                                                                                    && x.IsDeleted == 0);

                    // 疑い病名のみ
                    if (searchModel.IsOnlySuspectedDisease)
                    {
                        ptByoNextQuery = ptByoNextQuery.Where(x => (x.SyusyokuCd1.Trim() == valueCheck
                                                                || x.SyusyokuCd2.Trim() == valueCheck
                                                                || x.SyusyokuCd3.Trim() == valueCheck
                                                                || x.SyusyokuCd4.Trim() == valueCheck
                                                                || x.SyusyokuCd5.Trim() == valueCheck
                                                                || x.SyusyokuCd6.Trim() == valueCheck
                                                                || x.SyusyokuCd7.Trim() == valueCheck
                                                                || x.SyusyokuCd8.Trim() == valueCheck
                                                                || x.SyusyokuCd9.Trim() == valueCheck
                                                                || x.SyusyokuCd10.Trim() == valueCheck
                                                                || x.SyusyokuCd11.Trim() == valueCheck
                                                                || x.SyusyokuCd12.Trim() == valueCheck
                                                                || x.SyusyokuCd13.Trim() == valueCheck
                                                                || x.SyusyokuCd14.Trim() == valueCheck
                                                                || x.SyusyokuCd15.Trim() == valueCheck
                                                                || x.SyusyokuCd16.Trim() == valueCheck
                                                                || x.SyusyokuCd11.Trim() == valueCheck
                                                                || x.SyusyokuCd18.Trim() == valueCheck
                                                                || x.SyusyokuCd19.Trim() == valueCheck
                                                                || x.SyusyokuCd20.Trim() == valueCheck
                                                                || x.SyusyokuCd21.Trim() == valueCheck)
                                                                );
                    }

                    if (index == 0)
                    {
                        ptByomeiTemp = ptByoNextQuery;
                    }
                    else
                    {
                        if (searchModel.ByomeiQuery == QuerySearchEnum.AND)
                        {
                            ptByomeiTemp = ptByomeiTemp!.Where(pb => ptByoNextQuery.Select(item => item.PtId).ToList().Contains(pb.PtId));
                        }
                        else
                        {
                            ptByomeiTemp = ptByomeiTemp!.Union(ptByoNextQuery).Distinct();
                        }
                    }
                    index++;
                }

                var ptByomeiTempList = ptByomeiTemp.ToList();
                var lockObj = new object();
                var receInfToList = receInfs.ToList();
                Parallel.ForEach(ptByomeiTempList, byomei =>
                {
                    var receInf = receInfToList.FirstOrDefault(item => byomei.PtId == item.PtId
                                                                     && (byomei.HokenPid == item.HokenId || byomei.HokenPid == 0)
                                                                     && byomei.StartDate / 100 <= item.SinYm
                                                                     && (byomei.TenkiKbn == TenkiKbnConst.Continued
                                                                         || (byomei.TenkiDate / 100 >= item.SinYm)));
                    if (receInf != null)
                    {
                        lock (lockObj)
                        {
                            receInfFilters.Add(receInf);
                        }
                    }
                });
            }
        }
        #endregion

        #region main query
        var listReceInfs = receInfFilters.Any() ? receInfFilters : receInfs.ToList();
        var query = from receInf in listReceInfs
                    join receInfEdit in receInfEdits on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receInfEdit.HpId, receInfEdit.SeikyuYm, receInfEdit.PtId, receInfEdit.HokenId, receInfEdit.SinYm } into receInfEditLeft
                    from receInfEdit in receInfEditLeft.DefaultIfEmpty()

                    join receStatus in receStatuses on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receStatus.HpId, receStatus.SeikyuYm, receStatus.PtId, receStatus.HokenId, receStatus.SinYm } into receStatusLeft
                    from receStatus in receStatusLeft.DefaultIfEmpty()

                    join receCheckCmt in receCheckCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCheckCmt.HpId, receCheckCmt.PtId, receCheckCmt.HokenId, receCheckCmt.SinYm } into receCheckCmtLeft
                    from receCheckCmt in receCheckCmtLeft.DefaultIfEmpty()

                    join receCheckErr in receCheckErrors on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                               equals new { receCheckErr.HpId, receCheckErr.PtId, receCheckErr.HokenId, receCheckErr.SinYm } into receCheckErrLeft
                    from receCheckErr in receCheckErrLeft.DefaultIfEmpty()

                    join receCmt in receCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCmt.HpId, receCmt.PtId, receCmt.HokenId, receCmt.SinYm } into receCmtLeft
                    from receCmt in receCmtLeft.DefaultIfEmpty()

                    join receSeikyu in receSeikyus on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuLeft
                    from receSeikyu in receSeikyuLeft.DefaultIfEmpty()

                    join syoukiInf in syoukiInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syoukiInf.HpId, syoukiInf.PtId, syoukiInf.HokenId, syoukiInf.SinYm } into syoukiInfLeft
                    from syoukiInf in syoukiInfLeft.DefaultIfEmpty()

                    join syobyokeika in syobyokeikas on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syobyokeika.HpId, syobyokeika.PtId, syobyokeika.HokenId, syobyokeika.SinYm } into syobyokeikaLeft
                    from syobyokeika in syobyokeikaLeft.Take(1).DefaultIfEmpty()

                    join ptInf in ptInfs on new { receInf.HpId, receInf.PtId }
                                                equals new { ptInf.HpId, ptInf.PtId }

                    join ptHokenInf in ptHokenInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId }
                                                equals new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfLeft
                    from ptHokenInf in ptHokenInfLeft.DefaultIfEmpty()

                    join ptLastVisitDate in ptLastVisitDates on new { receInf.HpId, receInf.PtId }
                                               equals new { ptLastVisitDate.HpId, ptLastVisitDate.PtId } into ptLastVisitDateInf
                    from ptLastVisitDate in ptLastVisitDateInf.OrderByDescending(p => p.SinDate).ThenByDescending(p => p.RaiinNo).Take(1).DefaultIfEmpty()

                    join kaikeiInf in kaikeiInfs on new { receInf.PtId, receInf.HokenId }
                                               equals new { kaikeiInf.PtId, kaikeiInf.HokenId } into kaikeiInfLeft
                    from kaikeiInf in kaikeiInfLeft.OrderByDescending(item => item.SinDate).Take(1).DefaultIfEmpty()

                    join ptKyusei in ptKyuseis on new { receInf.HpId, receInf.PtId }
                                              equals new { ptKyusei.HpId, ptKyusei.PtId } into ptKyuseiLeft
                    from ptKyusei in ptKyuseiLeft.Where(item => kaikeiInf != null && item.EndDate >= kaikeiInf.SinDate && item.PtId == kaikeiInf.PtId).Take(1).DefaultIfEmpty()

                    join kaMst in kaMsts on new { receInf.HpId, receInf.KaId }
                                               equals new { kaMst.HpId, kaMst.KaId } into kaMstLeft
                    from kaMst in kaMstLeft.DefaultIfEmpty()

                    join userMst in userMsts on new { receInf.HpId, receInf.TantoId }
                                               equals new { userMst.HpId, TantoId = userMst.UserId } into userMstLeft
                    from userMst in userMstLeft.DefaultIfEmpty()

                    join ptKohi1 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi1Id }
                                              equals new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1Left
                    from ptKohi1 in ptKohi1Left.DefaultIfEmpty()

                    join ptKohi2 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi2Id }
                                              equals new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2Left
                    from ptKohi2 in ptKohi2Left.DefaultIfEmpty()

                    join ptKohi3 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi3Id }
                                              equals new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3Left
                    from ptKohi3 in ptKohi3Left.DefaultIfEmpty()

                    join ptKohi4 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi4Id }
                                              equals new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4Left
                    from ptKohi4 in ptKohi4Left.DefaultIfEmpty()
                    select new
                    {
                        IsReceInfDetailExist = receInfEdit != null ? 1 : 0,
                        IsPaperRece = receStatus != null ? receStatus.IsPaperRece : 0,
                        Output = receStatus != null ? receStatus.Output : 0,
                        FusenKbn = receStatus != null ? receStatus.FusenKbn : 0,
                        StatusKbn = receStatus != null ? receStatus.StatusKbn : 0,
                        ReceCheckCmt = receCheckCmt != null ? receCheckCmt.Cmt : (receCheckErr != null ? receCheckErr?.Message1 ?? string.Empty + receCheckErr?.Message2 ?? string.Empty : string.Empty),
                        IsPending = receCheckCmt != null ? receCheckCmt.IsPending : -1,
                        PtNum = ptInf != null ? ptInf.PtNum : 0,
                        Name = ptKyusei != null ? ptKyusei.Name : ptInf.Name,
                        KanaName = ptKyusei != null ? ptKyusei.KanaName : ptInf.KanaName,
                        Sex = ptInf != null ? ptInf.Sex : 0,
                        Birthday = ptInf != null ? ptInf.Birthday : 0,
                        receInf.IsTester,
                        HokensyaNo = ptHokenInf?.HokensyaNo ?? string.Empty,
                        IsSyoukiInfExist = syoukiInf != null ? 1 : 0,
                        IsReceCmtExist = receCmt != null ? 1 : 0,
                        IsSyobyoKeikaExist = syobyokeika != null ? 1 : 0,
                        SeikyuCmt = receSeikyu != null ? receSeikyu.Cmt : string.Empty,
                        LastVisitDate = ptLastVisitDate != null ? ptLastVisitDate.SinDate : 0,
                        KaName = kaMst != null ? kaMst.KaName : string.Empty,
                        UserName = userMst?.Name ?? string.Empty,
                        IsPtKyuseiExist = ptKyusei != null ? 1 : 0,
                        FutansyaNoKohi1 = ptKohi1 != null ? ptKohi1.FutansyaNo : string.Empty,
                        FutansyaNoKohi2 = ptKohi2 != null ? ptKohi2.FutansyaNo : string.Empty,
                        FutansyaNoKohi3 = ptKohi3 != null ? ptKohi3.FutansyaNo : string.Empty,
                        FutansyaNoKohi4 = ptKohi4 != null ? ptKohi4.FutansyaNo : string.Empty,
                        IsReceStatusExists = receStatus != null,
                        IsPtInfExists = ptInf != null,
                        IsPtHokenInfExists = ptHokenInf != null,
                        IsKohi1Exists = ptKohi1 != null,
                        IsKohi2Exists = ptKohi2 != null,
                        IsKohi3Exists = ptKohi3 != null,
                        IsKohi4Exists = ptKohi4 != null,
                        IsPtLastVisitDateExist = ptLastVisitDate != null,
                        IsKaMstExists = kaMst != null,
                        IsUserMstExist = userMst != null,
                        receInf.SeikyuYm,
                        receInf.PtId,
                        receInf.SeikyuKbn,
                        receInf.SinYm,
                        receInf.ReceSbt,
                        receInf.HokenSbtCd,
                        receInf.HokenKbn,
                        receInf.HokenId,
                        receInf.Tensu,
                        receInf.HokenNissu,
                        receInf.Kohi1Nissu,
                        receInf.HpId,
                        receInf.Kohi1ReceKisai,
                        receInf.Kohi2ReceKisai,
                        receInf.Kohi3ReceKisai,
                        receInf.Kohi4ReceKisai,
                        receInf.Tokki,
                        LastSinDateByHokenId = kaikeiInf?.SinDate ?? 0
                    };
        #endregion

        #region Search after query
        // 請求区分
        if (!searchModel.SeikyuKbnAll)
        {
            // 労災のレセプト電算
            var rousaiRecedenValue = GetSettingValue(hpId, 100003, 0);
            var rousaiRecedenParam = GetSettingParam(hpId, 100003, 0).AsInteger();
            // アフターケア電算
            var aftercareDensanValue = GetSettingValue(hpId, 100003, 1);
            var aftercareDensanParam = GetSettingParam(hpId, 100003, 1).AsInteger();
            if (searchModel.SeikyuKbnDenshi)
            {
                query = query.Where(item => !(item.IsPaperRece == 1
                                               || item.SeikyuKbn == 2
                                               || item.HokenKbn == 0
                                               || item.HokenKbn == 14
                                               || ((rousaiRecedenValue != 1 || (rousaiRecedenValue == 1 && item.SeikyuYm < rousaiRecedenParam))
                                                   && (item.HokenKbn == 11 || item.HokenKbn == 12))
                                               || ((aftercareDensanValue != 1 || (aftercareDensanValue == 1 && item.SeikyuYm < aftercareDensanParam))
                                                   && item.HokenKbn == 13)));
            }
            else if (searchModel.SeikyuKbnPaper)
            {
                query = query.Where(item => item.IsPaperRece == 1
                                               || item.SeikyuKbn == 2
                                               || item.HokenKbn == 0
                                               || item.HokenKbn == 14
                                               || ((rousaiRecedenValue != 1 || (rousaiRecedenValue == 1 && item.SeikyuYm < rousaiRecedenParam))
                                                   && (item.HokenKbn == 11 || item.HokenKbn == 12))
                                               || ((aftercareDensanValue != 1 || (aftercareDensanValue == 1 && item.SeikyuYm < aftercareDensanParam))
                                                   && item.HokenKbn == 13));
            }
        }
        #endregion

        #region Filter after query
        if (searchModel.IsAdvanceSearch)
        {
            // 確認
            if (!searchModel.IsAll)
            {
                List<int> statusKbnList = new List<int>();

                if (searchModel.IsSystemSave)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.SystemPending);
                }

                if (searchModel.IsSave1)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.Keep1);
                }

                if (searchModel.IsSave2)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.Keep2);
                }

                if (searchModel.IsSave3)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.Keep3);
                }

                if (searchModel.IsTempSave)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.TempComfirmed);
                }

                if (searchModel.IsDone)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.Confirmed);
                }

                if (searchModel.IsNoSetting)
                {
                    statusKbnList.Add((int)ReceCheckStatusEnum.UnConfirmed);
                    query = query.Where(item => statusKbnList.Contains(item.StatusKbn) || !item.IsReceStatusExists);
                }
                else
                {
                    query = query.Where(item => statusKbnList.Contains(item.StatusKbn) && item.IsReceStatusExists);
                }
            }

            if (!string.IsNullOrEmpty(searchModel.HokenHoubetu)
                || searchModel.HokensyaNoFromLong > 0
                || searchModel.HokensyaNoToLong > 0)
            {
                query = query.Where(item => item.IsPtHokenInfExists);
            }

            // 生年月日
            if (searchModel.BirthDayFrom > 0
                || searchModel.BirthDayTo > 0
                 || !string.IsNullOrEmpty(searchModel.Name)
                 || searchModel.PtIdFrom > 0
                 || searchModel.PtIdTo > 0
                 || string.IsNullOrEmpty(searchModel.PtId))
            {
                query = query.Where(item => item.IsPtInfExists);
            }

            // 最終来院日
            if (searchModel.LastRaiinDateFrom > 0
                || searchModel.LastRaiinDateTo > 0)
            {
                query = query.Where(item => item.IsPtLastVisitDateExist);
            }
        }
        #endregion

        #region Convert to list model
        var result = query.Select(
                        data => new ReceiptListModel(
                                data.SeikyuKbn,
                                data.SinYm,
                                data.IsReceInfDetailExist,
                                data.IsPaperRece,
                                data.HokenId,
                                data.HokenKbn,
                                data.Output,
                                data.FusenKbn,
                                data.StatusKbn,
                                data.IsPending,
                                data.PtId,
                                data.PtNum,
                                data.KanaName,
                                data.Name,
                                data.Sex,
                                data.LastSinDateByHokenId != 0 ? data.LastSinDateByHokenId : data.SinYm * 100 + 1,
                                data.Birthday,
                                data.ReceSbt,
                                data.HokensyaNo,
                                data.Tensu,
                                data.HokenSbtCd,
                                data.Kohi1Nissu ?? 0,
                                data.IsSyoukiInfExist,
                                data.IsReceCmtExist,
                                data.IsSyobyoKeikaExist,
                                data.SeikyuCmt,
                                data.LastVisitDate,
                                data.KaName,
                                data.UserName,
                                data.IsPtKyuseiExist,
                                data.FutansyaNoKohi1,
                                data.FutansyaNoKohi2,
                                data.FutansyaNoKohi3,
                                data.FutansyaNoKohi4,
                                data.IsTester == 1,
                                data.Kohi1ReceKisai,
                                data.Kohi2ReceKisai,
                                data.Kohi3ReceKisai,
                                data.Kohi4ReceKisai,
                                data.Tokki,
                                data.HokenNissu ?? 0,
                                data.ReceCheckCmt
                            ))
                    .OrderBy(item => item.SinYm)
                    .ThenBy(item => item.PtNum)
                    .ToList();
        #endregion

        if (searchModel.IsAdvanceSearch)
        {
            return FilterAfterConvertModel(result, searchModel, sinYmPtIdForFilterList);
        }
        return result;
    }

    private List<ReceiptListModel> FilterAfterConvertModel(List<ReceiptListModel> result, ReceiptListAdvancedSearchInput searchModel, List<ItemSumModel> sinYmPtIdForFilterList)
    {
        // 負担者番号
        if (searchModel.FutansyaNoFromLong > 0 && searchModel.FutansyaNoToLong > 0)
        {
            result = result.Where(item => (!string.IsNullOrEmpty(item.FutansyaNoKohi1)
                                                && item.FutansyaNoKohi1.AsLong() >= searchModel.FutansyaNoFromLong
                                                && item.FutansyaNoKohi1.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi1ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi2)
                                                && item.FutansyaNoKohi2.AsLong() >= searchModel.FutansyaNoFromLong
                                                && item.FutansyaNoKohi2.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi2ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi3)
                                                && item.FutansyaNoKohi3.AsLong() >= searchModel.FutansyaNoFromLong
                                                && item.FutansyaNoKohi3.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi3ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi4)
                                                && item.FutansyaNoKohi4.AsLong() >= searchModel.FutansyaNoFromLong
                                                && item.FutansyaNoKohi4.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi4ReceKisai == 1))).ToList();
        }
        else
        {
            if (searchModel.FutansyaNoFromLong > 0)
            {
                result = result.Where(item => (!string.IsNullOrEmpty(item.FutansyaNoKohi1)
                                                && item.FutansyaNoKohi1.AsLong() >= searchModel.FutansyaNoFromLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi1ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi2)
                                                && item.FutansyaNoKohi2.AsLong() >= searchModel.FutansyaNoFromLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi2ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi3)
                                                && item.FutansyaNoKohi3.AsLong() >= searchModel.FutansyaNoFromLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi3ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi4)
                                                && item.FutansyaNoKohi4.AsLong() >= searchModel.FutansyaNoFromLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi4ReceKisai == 1))).ToList();
            }
            else if (searchModel.FutansyaNoToLong > 0)
            {
                result = result.Where(item => (!string.IsNullOrEmpty(item.FutansyaNoKohi1)
                                                && item.FutansyaNoKohi1.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi1ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi2)
                                                && item.FutansyaNoKohi2.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi2ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi3)
                                                && item.FutansyaNoKohi3.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi3ReceKisai == 1))
                                            || (!string.IsNullOrEmpty(item.FutansyaNoKohi4)
                                                && item.FutansyaNoKohi4.AsLong() <= searchModel.FutansyaNoToLong
                                                && (searchModel.IsFutanIncludeSingle || item.Kohi4ReceKisai == 1))).ToList();
            }
        }

        // 項目
        if (searchModel.ItemList?.Count > 0)
        {
            result = result.Where(item => sinYmPtIdForFilterList.Any(s => s.PtId == item.PtId && s.SinYm == item.SinYm && s.HokenId == item.HokenId)).ToList();
        }

        // 特記事項
        if (!string.IsNullOrEmpty(searchModel.Tokki))
        {
            IEnumerable<string> Split(string str, int chunkSize)
            {
                return str == null ? Enumerable.Empty<string>() : Enumerable.Range(0, str.Length / chunkSize)
                    .Select(i => str.Substring(i * chunkSize, chunkSize));
            }

            result = result.Where(item => Split(item.Tokki, 2).Contains(searchModel.Tokki)).ToList();
        }
        return result;
    }
    #endregion

    #region Rece check screeen
    public List<ReceCmtModel> GetReceCmtList(int hpId, int sinYm, long ptId, int hokenId, int sinDate)
    {
        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                    && (sinYm == 0 || item.SinYm == sinYm)
                                                                    && item.PtId == ptId
                                                                    && (hokenId == 0 || item.HokenId == hokenId)
                                                                    && item.IsDeleted == DeleteTypes.None)
                                                     .ToList();

        var itemCdList = receCmts.Select(item => item.ItemCd).Distinct().ToList();
        var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                     && item.StartDate <= sinDate
                                                                     && item.EndDate >= sinDate
                                                                     && itemCdList.Contains(item.ItemCd))
                                                      .ToList();


        var result = receCmts.Select(item => ConvertToReceCmtModel(item, sinDate, tenMstList)).ToList();
        return result;
    }

    public bool SaveReceCmtList(int hpId, int userId, List<ReceCmtModel> receCmtList)
    {
        if (!receCmtList.Any())
        {
            return true;
        }
        var receCmtUpdateList = receCmtList.Where(item => item.Id > 0).ToList();
        var receCmtUpdateDBList = TrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && receCmtUpdateList.Select(item => item.Id).Contains(item.Id))
                                                              .ToList();

        var receCmtAddNewList = receCmtList.Where(item => item.Id == 0 && !item.IsDeleted)
                                           .Select(item => ConvertToNewReceCmt(hpId, userId, item))
                                           .ToList();
        TrackingDataContext.ReceCmts.AddRange(receCmtAddNewList);
        foreach (var model in receCmtUpdateList)
        {
            var entity = receCmtUpdateDBList.FirstOrDefault(item => item.Id == model.Id);
            if (entity == null)
            {
                continue;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            if (model.IsDeleted)
            {
                entity.IsDeleted = 1;
                continue;
            }
            entity.SeqNo = model.SeqNo;
            entity.CmtData = model.CmtData;
            entity.Cmt = model.Cmt;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<SyoukiInfModel> GetSyoukiInfList(int hpId, int sinYm, long ptId, int hokenId)
    {
        var syoukiInfList = NoTrackingDataContext.SyoukiInfs.Where(item => item.HpId == hpId
                                                                           && (sinYm == 0 || item.SinYm == sinYm)
                                                                           && item.PtId == ptId
                                                                           && (hokenId == 0 || item.HokenId == hokenId)
                                                                           && item.IsDeleted == DeleteTypes.None)
                                                            .OrderBy(item => item.SortNo)
                                                            .ToList();

        var result = syoukiInfList.Select(item => ConvertToSyoukiInfModel(item)).ToList();
        return result;
    }

    public List<SyoukiKbnMstModel> GetSyoukiKbnMstList(int sinYm)
    {
        var syoukiKbnMstList = NoTrackingDataContext.SyoukiKbnMsts.Where(item => item.StartYm <= sinYm && item.EndYm >= sinYm)
                                                                  .OrderBy(p => p.SyoukiKbn)
                                                                  .ToList();

        var result = syoukiKbnMstList.Select(item => ConvertToSyoukiKbnMstModel(item)).ToList();
        return result;
    }

    public List<SyobyoKeikaModel> GetSyobyoKeikaList(int hpId, int sinYm, long ptId, int hokenId)
    {
        var hokenKbnList = new List<int> { 11, 12, 13 };
        var ptHokenInf = NoTrackingDataContext.PtHokenInfs.FirstOrDefault(item => item.HpId == hpId && item.HokenId == hokenId && item.PtId == ptId);
        var hokenKbn = ptHokenInf?.HokenKbn ?? 0;
        if (!hokenKbnList.Contains(hokenKbn) && hokenId != 0)
        {
            return new();
        }

        var syobyoKeikaList = NoTrackingDataContext.SyobyoKeikas.Where(item => item.HpId == hpId
                                                                               && (sinYm == 0 || item.SinYm == sinYm)
                                                                               && item.PtId == ptId
                                                                               && (hokenId == 0 || item.HokenId == hokenId)
                                                                               && item.IsDeleted == DeleteTypes.None
                                                                               && (hokenId == 0
                                                                                   || (hokenKbn == 13 && item.SinDay > 0)
                                                                                   || (hokenKbn != 13 && item.SinDay == 0)))
                                                                .OrderBy(item => item.SinDay)
                                                                .ThenByDescending(item => item.SeqNo)
                                                                .ToList();

        var result = syobyoKeikaList.Select(item => ConvertToSyobyoKeikaModel(item)).ToList();
        return result;
    }

    public List<SyobyoKeikaModel> GetSyobyoKeikaList(int hpId, List<int> sinYmList, List<long> ptIdList, List<int> hokenIdList)
    {
        sinYmList = sinYmList.Distinct().ToList();
        ptIdList = ptIdList.Distinct().ToList();
        hokenIdList = hokenIdList.Distinct().ToList();
        var syobyoKeikaList = NoTrackingDataContext.SyobyoKeikas.Where(item => item.HpId == hpId
                                                                               && sinYmList.Contains(item.SinYm)
                                                                               && ptIdList.Contains(item.PtId)
                                                                               && hokenIdList.Contains(item.HokenId)
                                                                               && item.IsDeleted == DeleteTypes.None)
                                                                .ToList();

        var result = syobyoKeikaList.Select(item => ConvertToSyobyoKeikaModel(item)).ToList();
        return result;
    }

    public bool SaveSyoukiInfList(int hpId, int userId, List<SyoukiInfModel> syoukiInfList)
    {
        var syoukiInfUpdateList = syoukiInfList.Where(item => item.SeqNo > 0).ToList();
        var syoukiInfUpdateDBList = TrackingDataContext.SyoukiInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && syoukiInfUpdateList.Select(item => item.SeqNo).Contains(item.SeqNo))
                                                              .ToList();

        var syoukiInfAddNewList = syoukiInfList.Where(item => item.SeqNo == 0 && !item.IsDeleted)
                                               .Select(item => ConvertToNewSyoukiInf(hpId, userId, item))
                                               .ToList();
        TrackingDataContext.SyoukiInfs.AddRange(syoukiInfAddNewList);
        foreach (var model in syoukiInfUpdateList)
        {
            var entity = syoukiInfUpdateDBList.FirstOrDefault(item => item.SeqNo == model.SeqNo);
            if (entity == null)
            {
                continue;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            if (model.IsDeleted)
            {
                entity.IsDeleted = 1;
                continue;
            }
            entity.Syouki = model.Syouki;
            entity.SyoukiKbn = model.SyoukiKbn;
            entity.SortNo = model.SortNo;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistSyoukiKbn(int sinYm, List<SyoukiKbnMstModel> syoukiKbnList)
    {
        var countSyoukiKbn = NoTrackingDataContext.SyoukiKbnMsts.AsEnumerable().Count(entity => entity.StartYm <= sinYm
                                                                                                && entity.EndYm >= sinYm
                                                                                                && syoukiKbnList.Any(input => input.SyoukiKbn == entity.SyoukiKbn && input.StartYm == entity.StartYm));
        return countSyoukiKbn == syoukiKbnList.GroupBy(item => new { item.SyoukiKbn, item.StartYm }).Count();
    }

    public bool SaveSyobyoKeikaList(int hpId, int userId, List<SyobyoKeikaModel> syoukiInfList)
    {
        var syobyoKeikaUpdateList = syoukiInfList.Where(item => item.SeqNo > 0).ToList();
        var syobyoKeikaUpdateDBList = TrackingDataContext.SyobyoKeikas.Where(item => item.HpId == hpId
                                                                                   && item.IsDeleted == DeleteTypes.None
                                                                                   && syobyoKeikaUpdateList.Select(item => item.SeqNo).Contains(item.SeqNo))
                                                                      .ToList();

        var syobyoKeikaAddNewList = syoukiInfList.Where(item => item.SeqNo == 0 && !item.IsDeleted)
                                                 .Select(item => ConvertToNewSyobyoKeika(hpId, userId, item))
                                                 .ToList();
        TrackingDataContext.SyobyoKeikas.AddRange(syobyoKeikaAddNewList);
        foreach (var model in syobyoKeikaUpdateList)
        {
            var entity = syobyoKeikaUpdateDBList.FirstOrDefault(item => item.SeqNo == model.SeqNo);
            if (entity == null)
            {
                continue;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            if (model.IsDeleted)
            {
                entity.IsDeleted = 1;
                continue;
            }
            entity.SinDay = model.SinDay;
            entity.Keika = model.Keika;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<ReceReasonModel> GetReceReasonList(int hpId, int seikyuYm, int sinDate, long ptId, int hokenId)
    {
        int sinYm = sinDate / 100;
        var receInfList = NoTrackingDataContext.ReceInfs.Where(item => item.HpId == hpId
                                                                       && item.SinYm == sinYm
                                                                       && item.SeikyuYm == seikyuYm
                                                                       && item.PtId == ptId
                                                                       && item.HokenId == hokenId);

        var renJiyuuList = NoTrackingDataContext.RecedenHenJiyuus.Where(item => item.HpId == hpId
                                                                                && item.SinYm == sinYm
                                                                                && item.PtId == ptId
                                                                                && item.HokenId == hokenId
                                                                                && item.IsDeleted == 0);

        var query = from receItem in receInfList
                    join henJiyuuItem in renJiyuuList
                    on new { receItem.HpId, receItem.PtId } equals new { henJiyuuItem.HpId, henJiyuuItem.PtId }
                    select new ReceReasonModel(
                                                    hokenId,
                                                    sinYm,
                                                    henJiyuuItem.HenreiJiyuuCd ?? string.Empty,
                                                    henJiyuuItem.HenreiJiyuu ?? string.Empty,
                                                    henJiyuuItem.Hosoku ?? string.Empty
                                               );
        return query.ToList();
    }

    public List<ReceCheckCmtModel> GetReceCheckCmtList(int hpId, int sinYm, long ptId, int hokenId)
    {
        var receCheckCmts = NoTrackingDataContext.ReceCheckCmts.Where(item => item.HpId == hpId
                                                                              && item.SinYm == sinYm
                                                                              && item.PtId == ptId
                                                                              && item.HokenId == hokenId
                                                                              && item.IsDeleted == DeleteTypes.None)
                                                                .OrderByDescending(item => item.SortNo)
                                                                .ToList();
        return receCheckCmts.Select(item => ConvertToReceCheckCmtModel(item)).ToList();
    }

    public List<ReceCheckErrModel> GetReceCheckErrList(int hpId, int sinYm, long ptId, int hokenId)
    {
        var receCheckErrs = NoTrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                              && item.SinYm == sinYm
                                                                              && item.PtId == ptId
                                                                              && item.HokenId == hokenId)
                                                                .OrderBy(item => item.ErrCd)
                                                                .ToList();
        return receCheckErrs.Select(item => ConvertToReceCheckErrModel(item)).ToList();
    }

    public List<ReceCheckErrModel> GetReceCheckErrList(int hpId, List<int> sinYmList, List<long> ptIdList, List<int> hokenIdList)
    {
        hokenIdList = hokenIdList.Distinct().ToList();
        ptIdList = ptIdList.Distinct().ToList();
        var receCheckErrs = NoTrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                              && sinYmList.Contains(item.SinYm)
                                                                              && ptIdList.Contains(item.PtId)
                                                                              && hokenIdList.Contains(item.HokenId))
                                                                .OrderBy(item => item.ErrCd)
                                                                .ToList();
        return receCheckErrs.Select(item => ConvertToReceCheckErrModel(item)).ToList();
    }

    public bool SaveReceCheckCmtList(int hpId, int userId, int hokenId, int sinYm, long ptId, List<ReceCheckCmtModel> receCheckCmtList)
    {
        var receCheckCmtUpdateList = receCheckCmtList.Where(item => item.SeqNo > 0).ToList();
        var receCheckCmtUpdateDBList = TrackingDataContext.ReceCheckCmts.Where(item => item.HpId == hpId
                                                                                       && item.SinYm == sinYm
                                                                                       && item.PtId == ptId
                                                                                       && item.HokenId == hokenId
                                                                                       && item.IsDeleted == DeleteTypes.None
                                                                                       && receCheckCmtUpdateList.Select(item => item.SeqNo).Contains(item.SeqNo))
                                                                        .ToList();

        var receCheckAddNewList = receCheckCmtList.Where(item => item.SeqNo == 0 && !item.IsDeleted)
                                                  .Select(item => ConvertToNewReceCheckCmt(hpId, userId, hokenId, sinYm, ptId, item))
                                                  .ToList();
        TrackingDataContext.ReceCheckCmts.AddRange(receCheckAddNewList);
        foreach (var model in receCheckCmtUpdateList)
        {
            var entity = receCheckCmtUpdateDBList.FirstOrDefault(item => item.SeqNo == model.SeqNo);
            if (entity == null)
            {
                continue;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            if (model.IsDeleted)
            {
                entity.IsDeleted = 1;
                continue;
            }
            entity.Cmt = model.Cmt;
            entity.IsPending = model.IsPending;
            entity.IsChecked = model.IsChecked;
            entity.SortNo = model.SortNo;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistSeqNoReceCheckCmtList(int hpId, int hokenId, int sinYm, long ptId, List<int> seqNoList)
    {
        var seqNoListQuery = seqNoList.Distinct().ToList();
        var receCheckCmtCount = NoTrackingDataContext.ReceCheckCmts.Count(item => item.HpId == hpId
                                                                                  && item.SinYm == sinYm
                                                                                  && item.PtId == ptId
                                                                                  && item.HokenId == hokenId
                                                                                  && seqNoListQuery.Contains(item.SeqNo)
                                                                                  && item.IsDeleted == DeleteTypes.None);
        return receCheckCmtCount == seqNoList.Count;
    }

    public InsuranceReceInfModel GetInsuranceReceInfList(int hpId, int seikyuYm, int hokenId, int sinYm, long ptId)
    {
        var receInf = NoTrackingDataContext.ReceInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.SeikyuYm == seikyuYm
                                                                            && item.PtId == ptId
                                                                            && item.SinYm == sinYm
                                                                            && item.HokenId == hokenId);

        if (receInf == null)
        {
            return new();
        }

        var hokenInf = NoTrackingDataContext.PtHokenInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.HokenId == hokenId
                                                                                && item.IsDeleted == 0);

        var kohiRepoList = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId
                                                                       && item.PtId == ptId
                                                                       && item.IsDeleted == 0
                                                                       && (item.HokenId == receInf.Kohi1Id
                                                                           || item.HokenId == receInf.Kohi2Id
                                                                           || item.HokenId == receInf.Kohi3Id
                                                                           || item.HokenId == receInf.Kohi4Id)
                                                        ).ToList();


        var ptKohi1 = kohiRepoList.FirstOrDefault(item => item.HokenId == receInf.Kohi1Id);
        var ptKohi2 = kohiRepoList.FirstOrDefault(item => item.HokenId == receInf.Kohi2Id);
        var ptKohi3 = kohiRepoList.FirstOrDefault(item => item.HokenId == receInf.Kohi3Id);
        var ptKohi4 = kohiRepoList.FirstOrDefault(item => item.HokenId == receInf.Kohi4Id);
        return ConvertToInsuranceReceInfModel(receInf, hokenInf ?? new(), ptKohi1 ?? new(), ptKohi2 ?? new(), ptKohi3 ?? new(), ptKohi4 ?? new());
    }

    public bool SaveReceCheckOpt(int hpId, int userId, List<ReceCheckOptModel> receCheckOptList)
    {
        var errCdList = receCheckOptList.Select(item => item.ErrCd).Distinct().ToList();
        if (errCdList.Count == 0)
        {
            return true;
        }
        var receCheckOptDBList = TrackingDataContext.ReceCheckOpts.Where(item => item.HpId == hpId && errCdList.Contains(item.ErrCd)).ToList();
        foreach (var model in receCheckOptList)
        {
            var receOptItem = receCheckOptDBList.FirstOrDefault(item => item.ErrCd == model.ErrCd);
            if (receOptItem != null)
            {
                receOptItem.CheckOpt = model.CheckOpt;
                receOptItem.Biko = model.Biko;
                receOptItem.IsInvalid = model.IsInvalid;
                receOptItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                receOptItem.UpdateId = userId;
                continue;
            }
            receOptItem = new ReceCheckOpt();
            receOptItem.HpId = hpId;
            receOptItem.ErrCd = model.ErrCd;
            receOptItem.CheckOpt = model.CheckOpt;
            receOptItem.Biko = model.Biko;
            receOptItem.IsInvalid = model.IsInvalid;
            receOptItem.CreateDate = CIUtil.GetJapanDateTimeNow();
            receOptItem.CreateId = userId;
            receOptItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            receOptItem.UpdateId = userId;
            TrackingDataContext.ReceCheckOpts.Add(receOptItem);
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<ReceInfModel> GetReceInf(int hpId, ReceiptPreviewModeEnum receiptPreviewType, long ptId)
    {
        var receInfList = NoTrackingDataContext.ReceInfs.Where(item => item.HpId == hpId
                                                                       && item.PtId == ptId
                                                                       && item.SeikyuYm != 999999)
                                                        .ToList();

        if (receiptPreviewType == ReceiptPreviewModeEnum.ReceiptCheckInputSyoujoSyouki && receInfList.Any())
        {
            var hokenIdList = receInfList.Select(item => item.HokenId).Distinct().ToList();
            var sinYmList = receInfList.Select(item => item.SinYm).Distinct().ToList();
            var syoukiInfList = NoTrackingDataContext.SyoukiInfs.Where(item => item.HpId == hpId
                                                                               && item.PtId == ptId
                                                                               && item.IsDeleted == 0
                                                                               && hokenIdList.Contains(item.HokenId)
                                                                               && sinYmList.Contains(item.SinYm))
                                                                .ToList();

            receInfList = receInfList.Where(receInf => syoukiInfList.Any(item => item.SinYm == receInf.SinYm && item.HokenId == receInf.HokenId)).ToList();
        }

        return receInfList.Select(item => ConvertToReceInfModel(item))
                          .OrderByDescending(item => item.SeikyuYm)
                          .ToList();
    }

    public ReceInfModel GetReceInf(int hpId, int seikyuYm, long ptId, int sinYm, int hokenId)
    {
        var receInf = NoTrackingDataContext.ReceInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.SeikyuYm == seikyuYm
                                                                            && item.PtId == ptId
                                                                            && item.SinYm == sinYm
                                                                            && item.HokenId == hokenId);

        if (receInf == null)
        {
            return new();
        }
        return ConvertToReceInfModel(receInf);
    }

    public ReceiptEditModel GetReceInfEdit(int hpId, int seikyuYm, long ptId, int sinYm, int hokenId)
    {
        var receInfEdit = NoTrackingDataContext.ReceInfEdits.Where(item => item.HpId == hpId
                                                                           && item.SeikyuYm == seikyuYm
                                                                           && item.PtId == ptId
                                                                           && item.SinYm == sinYm
                                                                           && item.HokenId == hokenId
                                                                           && item.IsDeleted == 0)
                                                             .ToList()
                                                             ?.MaxBy(item => item.SeqNo);

        if (receInfEdit == null)
        {
            return new();
        }
        return ConvertToReceiptEditModel(receInfEdit);
    }

    public ReceiptEditModel GetReceInfPreEdit(int hpId, int seikyuYm, long ptId, int sinYm, int hokenId)
    {
        var receInfPreEdit = NoTrackingDataContext.ReceInfPreEdits.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.SeikyuYm == seikyuYm
                                                                                  && item.PtId == ptId
                                                                                  && item.SinYm == sinYm
                                                                                  && item.HokenId == hokenId);
        if (receInfPreEdit == null)
        {
            return new();
        }
        return ConvertToReceiptEditModel(receInfPreEdit);
    }

    public List<ReceCmtModel> GetLastMonthReceCmt(int hpId, int sinDate, long ptId)
    {
        var result = new List<ReceCmtModel>();
        int lastYM = 0;
        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                    && item.SinYm < (sinDate / 100)
                                                                    && item.PtId == ptId
                                                                    && item.IsDeleted == DeleteTypes.None)
                                                     .OrderByDescending(item => item.SinYm)
                                                     .ToList();

        lastYM = receCmts.FirstOrDefault()?.SinYm ?? 0;
        if (lastYM == 0)
        {
            return result;
        }
        receCmts = receCmts.Where(item => item.SinYm == lastYM).ToList();
        if (!receCmts.Any())
        {
            receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                    && item.SinYm < lastYM
                                                                    && item.IsDeleted == DeleteTypes.None
                                                                    && item.PtId == ptId)
                                                     .ToList();
        }

        var itemCdList = receCmts.Select(item => item.ItemCd).Distinct().ToList();
        var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId &&
                                                                     item.StartDate <= sinDate &&
                                                                     item.EndDate >= sinDate &&
                                                                     itemCdList.Contains(item.ItemCd))
                                                      .ToList();

        foreach (var receCmt in receCmts)
        {
            result.Add(ConvertToReceCmtModel(receCmt, sinDate, tenMstList));
        }
        return result;
    }

    public Dictionary<string, string> GetTokkiMstDictionary(int hpId, int sinDate = 0)
    {
        Dictionary<string, string> result = new();
        var tokkiMstList = NoTrackingDataContext.TokkiMsts.Where(item => item.HpId == hpId
                                                                   && ((item.StartDate == 0 && item.EndDate == 0) ||
                                                                       (item.StartDate <= sinDate && sinDate <= item.EndDate)))
                                                    .OrderBy(x => x.TokkiCd)
                                                    .ToList();
        foreach (var item in tokkiMstList)
        {
            result.Add(item.TokkiCd, item.TokkiName ?? string.Empty);
        }
        return result;
    }

    public List<int> GetSinDateRaiinInfList(int hpId, long ptId, int sinYm, int hokenId)
    {
        var hokenPidList = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                               && item.PtId == ptId
                                                                               && item.HokenId == hokenId
                                                                               && item.IsDeleted == DeleteTypes.None)
                                                                .Select(item => item.HokenPid)
                                                                .Distinct()
                                                                .ToList();

        var sinDateList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.SinDate / 100 == sinYm
                                                                        && item.Status >= RaiinState.Calculate
                                                                        && hokenPidList.Contains(item.HokenPid)
                                                                        && item.IsDeleted == DeleteTypes.None)
                                                         .Select(item => item.SinDate)
                                                         .Distinct()
                                                         .OrderBy(item => item)
                                                         .ToList();
        return sinDateList;
    }

    public bool SaveReceiptEdit(int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId, ReceiptEditModel model)
    {
        var receiptEditDB = TrackingDataContext.ReceInfEdits.FirstOrDefault(item => item.HpId == hpId
                                                                                    && item.SeikyuYm == seikyuYm
                                                                                    && item.PtId == ptId
                                                                                    && item.SinYm == sinYm
                                                                                    && item.HokenId == hokenId
                                                                                    && item.IsDeleted == 0
                                                                                    && item.SeqNo == model.SeqNo);
        if (receiptEditDB == null)
        {
            TrackingDataContext.ReceInfEdits.Add(ConvertToNewReceInfEdit(hpId, userId, seikyuYm, ptId, sinYm, hokenId, model));
        }
        else
        {
            if (model.IsDeleted)
            {
                receiptEditDB.IsDeleted = 1;
            }
            else
            {
                receiptEditDB.ReceSbt = string.IsNullOrEmpty(model.ReceSbt) ? null : model.ReceSbt;
                receiptEditDB.Houbetu = string.IsNullOrEmpty(model.Houbetu) ? null : model.Houbetu;
                receiptEditDB.HokenReceTensu = model.HokenReceTensu >= 0 ? model.HokenReceTensu : null;
                receiptEditDB.HokenReceFutan = model.HokenReceFutan >= 0 ? model.HokenReceFutan : null;
                receiptEditDB.Kohi1ReceTensu = model.Kohi1ReceTensu >= 0 ? model.Kohi1ReceTensu : null;
                receiptEditDB.Kohi1ReceFutan = model.Kohi1ReceFutan >= 0 ? model.Kohi1ReceFutan : null;
                receiptEditDB.Kohi2ReceTensu = model.Kohi2ReceTensu >= 0 ? model.Kohi2ReceTensu : null;
                receiptEditDB.Kohi2ReceFutan = model.Kohi2ReceFutan >= 0 ? model.Kohi2ReceFutan : null;
                receiptEditDB.Kohi3ReceTensu = model.Kohi3ReceTensu >= 0 ? model.Kohi3ReceTensu : null;
                receiptEditDB.Kohi3ReceFutan = model.Kohi3ReceFutan >= 0 ? model.Kohi3ReceFutan : null;
                receiptEditDB.Kohi4ReceTensu = model.Kohi4ReceTensu >= 0 ? model.Kohi4ReceTensu : null;
                receiptEditDB.Kohi4ReceFutan = model.Kohi4ReceFutan >= 0 ? model.Kohi4ReceFutan : null;
                receiptEditDB.Kohi1ReceKyufu = model.Kohi1ReceKyufu >= 0 ? model.Kohi1ReceKyufu : null;
                receiptEditDB.Kohi2ReceKyufu = model.Kohi2ReceKyufu >= 0 ? model.Kohi2ReceKyufu : null;
                receiptEditDB.Kohi3ReceKyufu = model.Kohi3ReceKyufu >= 0 ? model.Kohi3ReceKyufu : null;
                receiptEditDB.Kohi4ReceKyufu = model.Kohi4ReceKyufu >= 0 ? model.Kohi4ReceKyufu : null;
                receiptEditDB.HokenNissu = model.HokenNissu >= 0 ? model.HokenNissu : null;
                receiptEditDB.Kohi1Nissu = model.Kohi1Nissu >= 0 ? model.Kohi1Nissu : null;
                receiptEditDB.Kohi2Nissu = model.Kohi2Nissu >= 0 ? model.Kohi2Nissu : null;
                receiptEditDB.Kohi3Nissu = model.Kohi3Nissu >= 0 ? model.Kohi3Nissu : null;
                receiptEditDB.Kohi4Nissu = model.Kohi4Nissu >= 0 ? model.Kohi4Nissu : null;
                receiptEditDB.Tokki = string.IsNullOrEmpty(model.Tokki) ? null : model.Tokki;
                receiptEditDB.Tokki1 = string.IsNullOrEmpty(model.Tokki1) ? null : model.Tokki1;
                receiptEditDB.Tokki2 = string.IsNullOrEmpty(model.Tokki2) ? null : model.Tokki2;
                receiptEditDB.Tokki3 = string.IsNullOrEmpty(model.Tokki3) ? null : model.Tokki3;
                receiptEditDB.Tokki4 = string.IsNullOrEmpty(model.Tokki4) ? null : model.Tokki4;
                receiptEditDB.Tokki5 = string.IsNullOrEmpty(model.Tokki5) ? null : model.Tokki5;
                receiptEditDB.IsDeleted = 0;
            }
            receiptEditDB.UpdateDate = CIUtil.GetJapanDateTimeNow();
            receiptEditDB.UpdateId = userId;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistReceiptEdit(int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo)
    {
        var receInf = NoTrackingDataContext.ReceInfEdits.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.SeikyuYm == seikyuYm
                                                                                && item.PtId == ptId
                                                                                && item.SinYm == sinYm
                                                                                && item.HokenId == hokenId
                                                                                && item.IsDeleted == 0);
        int seqNoCheck = receInf?.SeqNo ?? 0;
        return seqNoCheck == seqNo;
    }

    public bool SaveReceStatus(int hpId, int userId, ReceStatusModel receStatus)
    {
        bool isAddNew = false;
        var entity = TrackingDataContext.ReceStatuses.FirstOrDefault(item => item.HpId == hpId
                                                                               && item.PtId == receStatus.PtId
                                                                               && item.SeikyuYm == receStatus.SeikyuYm
                                                                               && item.SinYm == receStatus.SinYm
                                                                               && item.HokenId == receStatus.HokenId);
        if (entity == null)
        {
            isAddNew = true;
            entity = new ReceStatus();
            entity.CreateDate = CIUtil.GetJapanDateTimeNow();
            entity.CreateId = userId;
            entity.HpId = hpId;
            entity.PtId = receStatus.PtId;
            entity.SeikyuYm = receStatus.SeikyuYm;
            entity.SinYm = receStatus.SinYm;
            entity.HokenId = receStatus.HokenId;
        }
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        entity.FusenKbn = receStatus.FusenKbn;
        entity.IsPaperRece = receStatus.IsPaperRece ? 1 : 0;
        entity.StatusKbn = receStatus.StatusKbn;
        entity.IsPrechecked = receStatus.IsPrechecked ? 1 : 0;
        if (isAddNew)
        {
            TrackingDataContext.ReceStatuses.Add(entity);
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public ReceStatusModel GetReceStatus(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
    {
        var receStatus = NoTrackingDataContext.ReceStatuses.FirstOrDefault(item => item.HpId == hpId
                                                                                   && item.PtId == ptId
                                                                                   && item.SeikyuYm == seikyuYm
                                                                                   && item.SinYm == sinYm
                                                                                   && item.HokenId == hokenId);
        return receStatus != null ? ConvertToReceStatusModel(receStatus) : new ReceStatusModel();
    }
    #endregion

    #region Recalculation Check
    public List<ReceRecalculationModel> GetReceRecalculationList(int hpId, int sinYm, List<long> ptIdList)
    {
        ptIdList = ptIdList.Distinct().ToList();
        List<ReceRecalculationModel> receRecalculationList = new();
        var receInfList = NoTrackingDataContext.ReceInfs.Where(item => item.HpId == hpId
                                                                       && item.SeikyuYm == sinYm
                                                                       && (ptIdList.Count <= 0 || ptIdList.Contains(item.PtId)))
                                                        .ToList();

        ptIdList = receInfList.Select(item => item.PtId).ToList();

        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                   && item.IsDelete == DeleteTypes.None
                                                                   && ptIdList.Contains(item.PtId))
                                                    .ToList();

        var receStateList = NoTrackingDataContext.ReceStatuses.Where(item => item.HpId == hpId
                                                                             && item.SeikyuYm == sinYm
                                                                             && ptIdList.Contains(item.PtId))
                                                              .ToList();

        var ptHokenInfList = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && ptIdList.Contains(item.PtId))
                                                              .ToList();

        var ptKohiInfList = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId
                                                                        && item.IsDeleted == DeleteTypes.None
                                                                        && ptIdList.Contains(item.PtId))
                                                         .ToList();

        var hokenCheckList = NoTrackingDataContext.PtHokenChecks.Where(item => item.HpId == hpId
                                                                               && item.IsDeleted == DeleteTypes.None
                                                                               && (item.HokenGrp == 1 || item.HokenGrp == 2)
                                                                               && ptIdList.Contains(item.PtID))
                                                                .ToList();

        foreach (var receInf in receInfList)
        {
            var ptInf = ptInfList.FirstOrDefault(item => item.PtId == receInf.PtId);
            if (ptInf == null)
            {
                continue;
            }
            var ptHokenInf = ptHokenInfList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.HokenId == item.HokenId);
            var ptKohi1Inf = ptKohiInfList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.Kohi1Id == item.HokenId);
            var ptKohi2Inf = ptKohiInfList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.Kohi2Id == item.HokenId);
            var ptKohi3Inf = ptKohiInfList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.Kohi3Id == item.HokenId);
            var ptKohi4Inf = ptKohiInfList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.Kohi4Id == item.HokenId);
            var hokenChecks = hokenCheckList.Where(item => item.PtID == receInf.PtId && receInf.HokenId == item.HokenId && item.HokenGrp == 1).ToList();
            var kohi1Checks = hokenCheckList.Where(item => item.PtID == receInf.PtId && receInf.Kohi1Id == item.HokenId && item.HokenGrp == 2).ToList();
            var kohi2Checks = hokenCheckList.Where(item => item.PtID == receInf.PtId && receInf.Kohi2Id == item.HokenId && item.HokenGrp == 2).ToList();
            var kohi3Checks = hokenCheckList.Where(item => item.PtID == receInf.PtId && receInf.Kohi3Id == item.HokenId && item.HokenGrp == 2).ToList();
            var kohi4Checks = hokenCheckList.Where(item => item.PtID == receInf.PtId && receInf.Kohi4Id == item.HokenId && item.HokenGrp == 2).ToList();
            var receStatus = receStateList.FirstOrDefault(item => item.PtId == receInf.PtId && receInf.HokenId == item.HokenId);

            bool isNashi = receInf.Houbetu == HokenConstant.HOUBETU_NASHI;
            bool isJihi = receInf.HokenKbn == 0 && (receInf.Houbetu == HokenConstant.HOUBETU_JIHI_108 || receInf.Houbetu == HokenConstant.HOUBETU_JIHI_109);
            bool isHokenConfirmed = isJihi || isNashi || hokenChecks.Any(item => CIUtil.DateTimeToInt(item.CheckDate) / 100 == sinYm);
            bool isRosai = receInf.HokenKbn == 11 || receInf.HokenKbn == 12 || receInf.HokenKbn == 13 || (receInf.HokenKbn == 14 && GetSettingValue(hpId, 3001, 0) == 1);

            receRecalculationList.Add(
                new ReceRecalculationModel(
                    receInf.SeikyuYm,
                    ptHokenInf?.RousaiSaigaiKbn ?? 0,
                    receStatus?.IsPaperRece ?? 0,
                    ptInf.Birthday,
                    receInf.PtId,
                    ptInf.PtNum,
                    receInf.SinYm,
                    receInf.Houbetu ?? string.Empty,
                    receInf.Kohi1Houbetu ?? string.Empty,
                    receInf.Kohi2Houbetu ?? string.Empty,
                    receInf.Kohi3Houbetu ?? string.Empty,
                    receInf.Kohi4Houbetu ?? string.Empty,
                    receInf.HokenKbn,
                    receInf.HokenId,
                    receInf.Kohi1Id,
                    receInf.Kohi2Id,
                    receInf.Kohi3Id,
                    receInf.Kohi4Id,
                    ptHokenInf?.StartDate ?? 0,
                    ptKohi1Inf?.StartDate ?? 0,
                    ptKohi2Inf?.StartDate ?? 0,
                    ptKohi3Inf?.StartDate ?? 0,
                    ptKohi4Inf?.StartDate ?? 0,
                    ptHokenInf?.EndDate ?? 0,
                    ptKohi1Inf?.EndDate ?? 0,
                    ptKohi2Inf?.EndDate ?? 0,
                    ptKohi3Inf?.EndDate ?? 0,
                    ptKohi4Inf?.EndDate ?? 0,
                    isHokenConfirmed,
                    kohi1Checks.Any(item => CIUtil.DateTimeToInt(item.CheckDate) / 100 == sinYm),
                    kohi2Checks.Any(item => CIUtil.DateTimeToInt(item.CheckDate) / 100 == sinYm),
                    kohi3Checks.Any(item => CIUtil.DateTimeToInt(item.CheckDate) / 100 == sinYm),
                    kohi4Checks.Any(item => CIUtil.DateTimeToInt(item.CheckDate) / 100 == sinYm),
                    ptHokenInf?.RousaiSyobyoDate ?? 0,
                    isRosai,
                    receInf.IsTester,
                    hokenChecks.Any() ? CIUtil.DateTimeToInt(hokenChecks.Max(item => item.CheckDate)) : 0,
                    kohi1Checks.Any() ? CIUtil.DateTimeToInt(kohi1Checks.Max(item => item.CheckDate)) : 0,
                    kohi2Checks.Any() ? CIUtil.DateTimeToInt(kohi2Checks.Max(item => item.CheckDate)) : 0,
                    kohi3Checks.Any() ? CIUtil.DateTimeToInt(kohi3Checks.Max(item => item.CheckDate)) : 0,
                    kohi4Checks.Any() ? CIUtil.DateTimeToInt(kohi4Checks.Max(item => item.CheckDate)) : 0
                ));
        }
        return receRecalculationList;
    }

    public List<ReceSinKouiCountModel> GetSinKouiCountList(int hpId, int sinYm, long ptId, int hokenId)
    {
        var result = new List<ReceSinKouiCountModel>();

        var sinKouiList = NoTrackingDataContext.SinKouis.Where(item => item.HpId == hpId
                                                                       && item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.IsNodspRece == 0
                                                                       && item.InoutKbn == 0
                                                                       && item.IsDeleted == DeleteTypes.None)
                                                        .ToList();

        var hokenPidList = sinKouiList.Select(item => item.HokenPid).Distinct().ToList();
        var rpNoList = sinKouiList.Select(item => item.RpNo).Distinct().ToList();
        var seqNoList = sinKouiList.Select(item => item.SeqNo).Distinct().ToList();

        var ptHokenPatternList = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                                     && item.PtId == ptId
                                                                                     && hokenPidList.Contains(item.HokenPid))
                                                                      .ToList();

        var sinKouiCountList = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                                 && item.PtId == ptId
                                                                                 && item.SinYm == sinYm
                                                                                 && rpNoList.Contains(item.RpNo)
                                                                                 && seqNoList.Contains(item.SeqNo))
                                                                  .OrderBy(p => p.SinDate)
                                                                  .ToList();

        var sinKouiDetailList = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                   && item.PtId == ptId
                                                                                   && item.SinYm == sinYm
                                                                                   && item.IsDeleted == DeleteTypes.None
                                                                                   && rpNoList.Contains(item.RpNo)
                                                                                   && seqNoList.Contains(item.SeqNo))
                                                                    .ToList();

        var listItemCd = sinKouiDetailList.Select(item => item.ItemCd).Distinct().ToList();
        var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                     && listItemCd.Contains(item.ItemCd))
                                                      .ToList();

        var sinKouiJoinPatternQuery = from sinKoui in sinKouiList
                                      join ptHokenPattern in ptHokenPatternList
                                      on sinKoui.HokenPid equals ptHokenPattern.HokenPid
                                      select new
                                      {
                                          sinKoui,
                                          ptHokenPattern
                                      };

        var sinKouiJoinSinKouiCountquery = from sinKouiJoinPattern in sinKouiJoinPatternQuery
                                           join sinKouiCount in sinKouiCountList
                                           on new { sinKouiJoinPattern.sinKoui.RpNo, sinKouiJoinPattern.sinKoui.SeqNo } equals new { sinKouiCount.RpNo, sinKouiCount.SeqNo }
                                           select new
                                           {
                                               sinKouiJoinPattern.ptHokenPattern,
                                               SinKouiCount = sinKouiCount
                                           };

        var sinKouiCountJoinDetailQuery = from sinKouiJoinSinKouiCount in sinKouiJoinSinKouiCountquery
                                          join sinKouiDetail in sinKouiDetailList
                                          on new { sinKouiJoinSinKouiCount.SinKouiCount.RpNo, sinKouiJoinSinKouiCount.SinKouiCount.SeqNo }
                                          equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                                          select new
                                          {
                                              sinKouiJoinSinKouiCount.ptHokenPattern,
                                              SinKouiCount = sinKouiJoinSinKouiCount.SinKouiCount,
                                              SinKouiDetail = sinKouiDetail
                                          };

        var joinTenMstQuery = from sinKouiCountJoinDetail in sinKouiCountJoinDetailQuery
                              join tenMst in tenMstList
                              on sinKouiCountJoinDetail.SinKouiDetail.ItemCd equals tenMst.ItemCd into tempTenMstList
                              select new
                              {
                                  PtId = sinKouiCountJoinDetail.SinKouiCount.PtId,
                                  SinDate = sinKouiCountJoinDetail.SinKouiCount.SinDate,
                                  RaiinNo = sinKouiCountJoinDetail.SinKouiCount.RaiinNo,
                                  SinKouiCount = sinKouiCountJoinDetail.SinKouiCount,
                                  SinKouiDetail = sinKouiCountJoinDetail.SinKouiDetail,
                                  sinKouiCountJoinDetail.ptHokenPattern,
                                  TenMst = tempTenMstList.OrderByDescending(p => p.StartDate).FirstOrDefault(p => p.StartDate <= sinKouiCountJoinDetail.SinKouiCount.SinDate)
                              };

        var groupKeys = joinTenMstQuery.GroupBy(item => new { item.PtId, item.SinDate, item.RaiinNo })
                                       .Select(p => p.FirstOrDefault())
                                       .ToList();

        foreach (var groupKey in groupKeys)
        {
            var entities = joinTenMstQuery.Where(p => p.PtId == groupKey!.PtId && p.SinDate == groupKey.SinDate && p.RaiinNo == groupKey.RaiinNo);
            var sinKouiDetailModelList = entities.Select(item => new SinKouiDetailModel(
                                                                     item.PtId,
                                                                     item.SinKouiDetail.SinYm,
                                                                     item.SinDate,
                                                                     item.SinKouiDetail.PtId,
                                                                     item.TenMst.MaxAge ?? string.Empty,
                                                                     item.TenMst.MinAge ?? string.Empty,
                                                                     item.SinKouiDetail.ItemCd ?? string.Empty,
                                                                     item.SinKouiDetail.CmtOpt ?? string.Empty,
                                                                     item.SinKouiDetail.ItemName ?? string.Empty,
                                                                     item.TenMst.ReceName ?? string.Empty,
                                                                     item.SinKouiDetail.Suryo,
                                                                     item.SinKouiDetail.IsNodspPaperRece,
                                                                     item.TenMst.MasterSbt ?? string.Empty,
                                                                     item.TenMst != null))
                                                .ToList();

            var ptHokenPatternModelList = entities.Select(item => new PtHokenPatternModel(
                                                                      item.ptHokenPattern.PtId,
                                                                      item.ptHokenPattern.HokenPid,
                                                                      item.ptHokenPattern.SeqNo,
                                                                      item.ptHokenPattern.HokenKbn,
                                                                      item.ptHokenPattern.HokenSbtCd,
                                                                      item.ptHokenPattern.HokenId,
                                                                      item.ptHokenPattern.Kohi1Id,
                                                                      item.ptHokenPattern.Kohi2Id,
                                                                      item.ptHokenPattern.Kohi3Id,
                                                                      item.ptHokenPattern.Kohi4Id,
                                                                      item.ptHokenPattern.HokenMemo ?? string.Empty,
                                                                      item.ptHokenPattern.StartDate,
                                                                      item.ptHokenPattern.EndDate))
                                                    .Distinct()
                                                    .ToList();

            result.Add(new ReceSinKouiCountModel(
                            ptHokenPatternModelList,
                            sinKouiDetailModelList,
                            groupKey!.PtId,
                            groupKey.SinDate,
                            groupKey.RaiinNo,
                            sinKouiDetailModelList.Any(item => ReceErrCdConst.IsFirstVisitCd.Contains(item.ItemCd)),
                            sinKouiDetailModelList.Any(item => ReceErrCdConst.IsReVisitCd.Contains(item.ItemCd)),
                            sinKouiDetailModelList.Any(item => item.ItemCd == "101110040" || item.ItemCd == "111011810")
                       ));
        }
        return result;
    }

    public List<ReceCheckOptModel> GetReceCheckOptList(int hpId, List<string> errCdList)
    {
        var receCheckOption = NoTrackingDataContext.ReceCheckOpts.Where(item => item.HpId == hpId
                                                                                && (errCdList.Count == 0 || errCdList.Contains(item.ErrCd)))
                                                                 .Select(item => new ReceCheckOptModel(
                                                                                     item.ErrCd,
                                                                                     item.CheckOpt,
                                                                                     item.Biko ?? string.Empty,
                                                                                     item.IsInvalid))
                                                                 .ToList();
        return receCheckOption;
    }

    public List<BuiOdrItemMstModel> GetBuiOdrItemMstList(int hpId)
    {
        var result = NoTrackingDataContext.BuiOdrItemMsts.Where(item => item.HpId == hpId).Select(item => new BuiOdrItemMstModel(item.ItemCd)).ToList();
        return result;
    }

    public List<BuiOdrItemByomeiMstModel> GetBuiOdrItemByomeiMstList(int hpId)
    {
        var result = NoTrackingDataContext.BuiOdrItemByomeiMsts.Where(item => item.HpId == hpId)
                                                               .Select(item => new BuiOdrItemByomeiMstModel(
                                                                                   item.ItemCd,
                                                                                   item.ByomeiBui,
                                                                                   item.LrKbn,
                                                                                   item.BothKbn))
                                                               .ToList();
        return result;
    }

    public List<BuiOdrMstModel> GetBuiOdrMstList(int hpId)
    {
        var result = NoTrackingDataContext.BuiOdrMsts.Where(item => item.HpId == hpId)
                                                     .Select(item => new BuiOdrMstModel(
                                                                         item.BuiId,
                                                                         item.OdrBui ?? string.Empty,
                                                                         item.LrKbn,
                                                                         item.MustLrKbn,
                                                                         item.BothKbn,
                                                                         item.Koui30,
                                                                         item.Koui40,
                                                                         item.Koui50,
                                                                         item.Koui60,
                                                                         item.Koui70,
                                                                         item.Koui80))
                                                     .ToList();
        return result;
    }

    public List<BuiOdrByomeiMstModel> GetBuiOdrByomeiMstList(int hpId)
    {
        var result = NoTrackingDataContext.BuiOdrByomeiMsts.Where(item => item.HpId == hpId)
                                                           .Select(item => new BuiOdrByomeiMstModel(
                                                                                item.BuiId,
                                                                                item.ByomeiBui))
                                                           .ToList();
        return result;
    }

    public string GetSanteiItemCd(int hpId, string itemCd, int sinDate)
    {
        var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(item => item.HpId == hpId
                                                                          && item.ItemCd == itemCd
                                                                          && item.StartDate <= sinDate
                                                                          && item.EndDate >= sinDate);
        return tenMst?.SanteiItemCd ?? string.Empty;
    }

    public List<string> GetTekiouByomei(int hpId, List<string> itemCdList)
    {
        itemCdList = itemCdList.Distinct().ToList();
        return NoTrackingDataContext.TekiouByomeiMsts.Where(item => item.HpId == hpId
                                                                    && itemCdList.Contains(item.ItemCd)
                                                                    && item.IsInvalid == 0)
                                                     .Select(p => p.ByomeiCd)
                                                     .ToList();
    }

    public double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns)
    {
        int startYm = startDate / 100;
        int endYm = endDate / 100;

        itemCds = itemCds.Distinct().ToList();
        List<int> checkHokenKbn = new();

        if (hokenKbns != null)
        {
            checkHokenKbn = hokenKbns.Distinct().ToList();
        }

        List<int> checkSanteiKbn = new();

        if (santeiKbns != null)
        {
            checkSanteiKbn = santeiKbns.Distinct().ToList();
        }

        var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(item => item.HpId == hpId
                                                                      && item.PtId == ptId
                                                                      && item.SinYm >= startYm
                                                                      && item.SinYm <= endYm
                                                                      && checkHokenKbn.Contains(item.HokenKbn)
                                                                      && checkSanteiKbn.Contains(item.SanteiKbn))
                                                       .ToList();

        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.SinDate >= startDate
                                                                              && item.SinDate <= endDate
                                                                              && item.RaiinNo != raiinNo)
                                                                .ToList();

        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.SinYm >= startYm
                                                                                && item.SinYm <= endYm
                                                                                && item.ItemCd != null
                                                                                && itemCds.Contains(item.ItemCd)
                                                                                && item.FmtKbn != 10  // 在がん医総のダミー項目を除く
                                                                 ).ToList();

        var joinQuery = (from sinKouiDetail in sinKouiDetails
                         join sinKouiCount in sinKouiCounts on
                             new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                             new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                         join sinRpInf in sinRpInfs on
                             new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                             new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                         where
                             sinKouiDetail.HpId == hpId
                             && sinKouiDetail.PtId == ptId
                             && sinKouiDetail.SinYm >= startYm
                             && sinKouiDetail.SinYm <= endYm
                             && sinKouiDetail.ItemCd != null
                             && itemCds.Contains(sinKouiDetail.ItemCd)
                             && sinKouiCount.SinDate >= startDate
                             && sinKouiCount.SinDate <= endDate
                             && sinKouiCount.RaiinNo != raiinNo
                         group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                         select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 || (a.sinKouiDetail.ItemCd != null && ItemCdConst.ZaitakuTokushu.Contains(a.sinKouiDetail.ItemCd)) ? 1 : a.sinKouiDetail.Suryo)) }
        );

        return joinQuery.FirstOrDefault()?.sum ?? 0;
    }

    public List<SinKouiMstModel> GetListSinKoui(int hpId, long ptId, int sinYm, int hokenId)
    {
        List<SinKouiMstModel> result = new();
        List<SinKoui> listSinKoui = NoTrackingDataContext.SinKouis.Where(item => item.HpId == hpId
                                                                                 && item.PtId == ptId
                                                                                 && item.SinYm == sinYm
                                                                                 && item.HokenId == hokenId
                                                                                 && item.IsNodspRece == 0)
                                                                  .ToList();

        var sinKouiCountList = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                                 && item.PtId == ptId
                                                                                 && item.SinYm == sinYm)
                                                                  .ToList();

        var rpNoList = listSinKoui.Select(item => item.RpNo).Distinct().ToList();
        var seqNoList = listSinKoui.Select(item => item.SeqNo).Distinct().ToList();
        var allSinKouiDetailList = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                      && item.PtId == ptId
                                                                                      && item.SinYm == sinYm
                                                                                      && item.IsNodspRece == 0
                                                                                      && rpNoList.Contains(item.RpNo)
                                                                                      && seqNoList.Contains(item.SeqNo))
                                                                       .ToList();

        listSinKoui.ForEach((sinKoui) =>
        {
            var sinKouiDetailList = allSinKouiDetailList.Where(item => item.RpNo == sinKoui.RpNo && item.SeqNo == sinKoui.SeqNo).ToList();
            var listSinKouiDetailEntity = (from sinKouiDetail in sinKouiDetailList
                                           join sinKouiCount in sinKouiCountList
                                           on new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals new { sinKouiCount.RpNo, sinKouiCount.SeqNo }
                                           select new
                                           {
                                               sinKouiDetail,
                                               sinKouiCount
                                           })
                                          .ToList();

            List<SinKouiDetailModel> listSinKouiDetailModel = new();
            listSinKouiDetailEntity.ForEach((sinKouiDetail) =>
            {
                string itemCd = sinKouiDetail.sinKouiDetail?.ItemCd ?? string.Empty;
                int sinDate = sinKouiDetail.sinKouiCount.SinDate;
                var listCmtSelect = _mstItemRepository.GetSelectiveComment(hpId, new List<string> { itemCd }, sinDate, new List<int> { 0 }, true);
                listSinKouiDetailModel.Add(new SinKouiDetailModel(sinKouiDetail.sinKouiDetail?.PtId ?? 0,
                                                                     sinKouiDetail.sinKouiDetail?.SinYm ?? 0,
                                                                     sinKouiDetail.sinKouiCount?.SinDate ?? 0,
                                                                     sinKouiDetail.sinKouiDetail?.PtId ?? 0,
                                                                     string.Empty,
                                                                     string.Empty,
                                                                     sinKouiDetail.sinKouiDetail?.ItemCd ?? string.Empty,
                                                                     sinKouiDetail.sinKouiDetail?.CmtOpt ?? string.Empty,
                                                                     sinKouiDetail.sinKouiDetail?.ItemName ?? string.Empty,
                                                                     string.Empty,
                                                                     sinKouiDetail.sinKouiDetail?.Suryo ?? 0,
                                                                     sinKouiDetail.sinKouiDetail?.IsNodspPaperRece ?? 0,
                                                                     string.Empty,
                                                                     false,
                                                                     listCmtSelect));
            });
            result.Add(AddNewSinKouiMstModel(sinKoui, listSinKouiDetailModel));
        });

        return result;
    }

    public List<string> GetListReceCmtItemCode(int hpId, long ptId, int sinYm, int hokenId)
    {
        return NoTrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                            && item.SinYm == sinYm
                                                            && item.PtId == ptId
                                                            && item.HokenId == hokenId
                                                            && item.IsDeleted == DeleteTypes.None)
                                             .Select(item => item.ItemCd ?? string.Empty)
                                             .ToList();
    }

    public List<CalcLogModel> GetAddtionItems(int hpId, long ptId, int sinYm, int hokenId)
    {
        return NoTrackingDataContext.CalcLogs.Where(item => item.HpId == hpId
                                                            && item.DelSbt == 13
                                                            && item.PtId == ptId
                                                            && item.SinDate / 100 == sinYm
                                                            && item.HokenId == hokenId)
                                             .Select(item => new CalcLogModel(
                                                                 item.RaiinNo,
                                                                 item.LogSbt,
                                                                 item.Text ?? string.Empty,
                                                                 item.PtId,
                                                                 item.SinDate,
                                                                 item.SeqNo,
                                                                 item.HokenId,
                                                                 item.ItemCd ?? string.Empty,
                                                                 item.DelItemCd ?? string.Empty,
                                                                 item.DelSbt,
                                                                 item.IsWarning,
                                                                 item.TermCnt,
                                                                 item.TermSbt))
                                             .ToList();
    }

    public bool SaveNewReceCheckErrList(int hpId, int userId, List<ReceCheckErrModel> receCheckErrList)
    {
        if (!receCheckErrList.Any())
        {
            return true;
        }
        var ptIdList = receCheckErrList.Select(item => item.PtId).Distinct().ToList();
        var hokenIdList = receCheckErrList.Select(item => item.HokenId).Distinct().ToList();
        var sinYmList = receCheckErrList.Select(item => item.SinYm).Distinct().ToList();
        var errorCdList = receCheckErrList.Select(item => item.ErrCd).Distinct().ToList();
        var oldReceCheckErrList = TrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                                  && ptIdList.Contains(item.PtId)
                                                                                  && hokenIdList.Contains(item.HokenId)
                                                                                  && sinYmList.Contains(item.SinYm)
                                                                                  && errorCdList.Contains(item.ErrCd))
                                                                    .ToList();

        foreach (var receErr in receCheckErrList)
        {
            var receErrUpdate = oldReceCheckErrList.FirstOrDefault(item => item.PtId == receErr.PtId
                                                                           && item.HokenId == receErr.HokenId
                                                                           && item.SinYm == receErr.SinYm
                                                                           && item.ErrCd == receErr.ErrCd
                                                                           && item.SinDate == receErr.SinDate
                                                                           && item.ACd == receErr.ACd
                                                                           && item.BCd == receErr.BCd);

            if (receErrUpdate != null)
            {
                receErrUpdate.Message1 = receErr.Message1;
                receErrUpdate.Message2 = receErr.Message2;
                receErrUpdate.IsChecked = receErr.IsChecked;
                receErrUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                receErrUpdate.UpdateId = userId;
                continue;
            }
            TrackingDataContext.ReceCheckErrs.Add(new ReceCheckErr()
            {
                HpId = hpId,
                PtId = receErr.PtId,
                HokenId = receErr.HokenId,
                SinYm = receErr.SinYm,
                ErrCd = receErr.ErrCd,
                SinDate = receErr.SinDate,
                ACd = receErr.ACd,
                BCd = receErr.BCd,
                Message1 = receErr.Message1,
                Message2 = receErr.Message2,
                IsChecked = receErr.IsChecked,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            });
        }

        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<SinKouiDetailModel> GetKouiDetailToCheckSantei(int hpId, List<long> ptIdList, int seikyuYm, List<string> zaiganIsoItemCds, bool isCheckPartOfNextMonth)
    {
        List<SinKouiDetailModel> result = new();
        int firstDateOfWeek = 0;
        int lastDateOfWeek = 0;
        int checkMonth = 0;

        ptIdList = ptIdList.Distinct().ToList();
        zaiganIsoItemCds = zaiganIsoItemCds.Distinct().ToList();
        if (isCheckPartOfNextMonth)
        {
            var firstDateOfNextMonth = new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddMonths(1);
            checkMonth = firstDateOfNextMonth.ToString("yyyyMM").AsInteger();
            firstDateOfWeek = CIUtil.DateTimeToInt(firstDateOfNextMonth);
            lastDateOfWeek = CIUtil.DateTimeToInt(firstDateOfNextMonth.AddDays(6 - (int)firstDateOfNextMonth.DayOfWeek));
        }
        else
        {
            var endDateOfLastMonth = new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddDays(-1);
            checkMonth = endDateOfLastMonth.ToString("yyyyMM").AsInteger();
            firstDateOfWeek = CIUtil.DateTimeToInt(endDateOfLastMonth.AddDays(-(int)endDateOfLastMonth.DayOfWeek));
            lastDateOfWeek = CIUtil.DateTimeToInt(endDateOfLastMonth);
        }
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                              && item.SinYm == checkMonth
                                                                              && item.SinDate >= firstDateOfWeek
                                                                              && item.SinDate <= lastDateOfWeek
                                                                              && (ptIdList.Count <= 0 || ptIdList.Contains(item.PtId)))
                                                               .ToList();

        ptIdList = sinKouiCounts.Select(item => item.PtId).Distinct().ToList();
        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                && item.SinYm == checkMonth
                                                                                && item.ItemCd != null
                                                                                && zaiganIsoItemCds.Contains(item.ItemCd)
                                                                                && ptIdList.Contains(item.PtId))
                                                                 .ToList();

        zaiganIsoItemCds = sinKouiDetails.Select(item => item.ItemCd ?? string.Empty).Distinct().ToList();
        var tenMsts = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                  && item.IsDeleted == DeleteTypes.None
                                                                  && zaiganIsoItemCds.Contains(item.ItemCd))
                                                    .ToList();

        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && item.IsDelete == DeleteTypes.None
                                                                && ptIdList.Contains(item.PtId))
                                                 .ToList();

        var joinKouiCountWithDetailQuery = from sinKouiCount in sinKouiCounts
                                           join sinKouiDetail in sinKouiDetails
                                           on new { sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                                           select new
                                           {
                                               SinKouiCount = sinKouiCount,
                                               SinKouiDetail = sinKouiDetail
                                           };

        var joinTenMstQuery = from kouiCountDetail in joinKouiCountWithDetailQuery
                              join tenMst in tenMsts
                              on kouiCountDetail.SinKouiDetail.ItemCd equals tenMst.ItemCd
                              where tenMst.StartDate <= kouiCountDetail.SinKouiCount.SinDate &&
                              tenMst.EndDate >= kouiCountDetail.SinKouiCount.SinDate
                              select new
                              {
                                  SinKouiDetail = kouiCountDetail.SinKouiDetail,
                                  TenMst = tenMst
                              };

        var joinWithPtInf = from joinTenMst in joinTenMstQuery
                            join ptInf in ptInfs
                            on joinTenMst.SinKouiDetail.PtId equals ptInf.PtId
                            select new
                            {
                                SinKouiDetail = joinTenMst.SinKouiDetail,
                                TenMst = joinTenMst.TenMst,
                                PtInf = ptInf
                            };

        foreach (var entity in joinWithPtInf)
        {
            result.Add(new SinKouiDetailModel(
                           entity.SinKouiDetail?.PtId ?? 0,
                           entity.SinKouiDetail?.SinYm ?? 0,
                           0,
                           entity.PtInf.PtNum,
                           entity.TenMst?.MaxAge ?? string.Empty,
                           entity.TenMst?.MinAge ?? string.Empty,
                           entity.SinKouiDetail?.ItemCd ?? string.Empty,
                           entity.SinKouiDetail?.CmtOpt ?? string.Empty,
                           entity.SinKouiDetail?.ItemName ?? string.Empty,
                           entity.TenMst?.ReceName ?? string.Empty,
                           entity.SinKouiDetail?.Suryo ?? 0,
                           entity.SinKouiDetail?.IsNodspRece ?? 0,
                           entity.TenMst?.MasterSbt ?? string.Empty,
                           entity.TenMst != null
                       ));
        }
        return result;
    }

    public Dictionary<long, int> GetSanteiStartDateList(int hpId, List<long> ptIdList, int seikyuYm)
    {
        Dictionary<long, int> result = new();
        ptIdList = ptIdList.Distinct().ToList();
        var sinKouiCountList = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                                 && ptIdList.Contains(item.PtId)
                                                                                 && item.SinYm == seikyuYm)
                                                                  .ToList();

        ptIdList = sinKouiCountList.Select(item => item.PtId).Distinct().ToList();
        var rpNoList = sinKouiCountList.Select(item => item.RpNo).Distinct().ToList();
        var seqNoList = sinKouiCountList.Select(item => item.SeqNo).Distinct().ToList();

        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                && ptIdList.Contains(item.PtId)
                                                                                && item.SinYm == seikyuYm
                                                                                && item.ItemCd == "X00013"
                                                                                && rpNoList.Contains(item.RpNo)
                                                                                && seqNoList.Contains(item.SeqNo))
                                                                 .ToList();

        foreach (var sinKouiCount in sinKouiCountList)
        {
            if (sinKouiDetails.Any(item => item.RpNo == sinKouiCount.RpNo && item.SeqNo == sinKouiCount.SeqNo))
            {
                if (result.Any(item => item.Key == sinKouiCount.PtId))
                {
                    result[sinKouiCount.PtId] = sinKouiCount.SinDate;
                    continue;
                }
                result.Add(sinKouiCount.PtId, sinKouiCount.SinDate);
                continue;
            }
            else if (result.Any(item => item.Key == sinKouiCount.PtId))
            {
                continue;
            }
            result.Add(sinKouiCount.PtId, seikyuYm * 100 + 1);
        }
        return result;
    }

    public Dictionary<long, int> GetSanteiEndDateList(int hpId, List<long> ptIdList, int seikyuYm)
    {
        Dictionary<long, int> result = new();
        ptIdList = ptIdList.Distinct().ToList();
        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId && ptIdList.Contains(item.PtId)).ToList();
        foreach (var ptId in ptIdList)
        {
            var ptInf = ptInfList.FirstOrDefault(item => item.PtId == ptId);
            if (ptInf != null && ptInf.DeathDate > 0 && ptInf.DeathDate < seikyuYm * 100 + 1)
            {
                result.Add(ptId, ptInf.DeathDate);
                continue;
            }
            result.Add(ptId, CIUtil.DateTimeToInt(new DateTime(seikyuYm / 100, seikyuYm % 100, 1).AddMonths(1).AddDays(-1)));
        }
        return result;
    }

    public List<HasErrorWithSanteiModel> GetHasErrorWithSanteiByStartDateList(int hpId, int seikyuYm, List<HasErrorWithSanteiModel> hasErrorList)
    {
        List<HasErrorWithSanteiModel> result = new();
        var ptIdList = hasErrorList.Select(item => item.PtId).Distinct().ToList();
        var itemCdList = hasErrorList.Select(item => item.ItemCd).Distinct().ToList();
        int minSindate = hasErrorList.Select(item => item.Sindate)?.DefaultIfEmpty(0).Min() ?? 0;
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                              && ptIdList.Contains(item.PtId)
                                                                              && item.SinYm == seikyuYm
                                                                              && item.SinDate >= minSindate)
                                                               .ToList();

        ptIdList = sinKouiCounts.Select(item => item.PtId).Distinct().ToList();
        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                && ptIdList.Contains(item.PtId)
                                                                                && item.SinYm == seikyuYm
                                                                                && item.ItemCd != null
                                                                                && itemCdList.Contains(item.ItemCd))
                                                                 .ToList();

        foreach (var model in hasErrorList)
        {
            var sinKouiCountItems = sinKouiCounts.Where(item => item.PtId == model.PtId && item.SinDate >= model.Sindate).ToList();
            var sinKouiDetailItems = sinKouiDetails.Where(item => item.PtId == model.PtId && item.ItemCd == model.ItemCd).ToList();
            var query = from sinKouiCount in sinKouiCountItems
                        join sinKouiDetail in sinKouiDetailItems
                        on new { sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        select new
                        {
                            SinKouiCount = sinKouiCount
                        };

            result.Add(new HasErrorWithSanteiModel(
                           model.PtId,
                           model.ItemCd,
                           model.Sindate,
                           query?.Count() == 0));
        }
        return result;
    }

    public List<HasErrorWithSanteiModel> GetHasErrorWithSanteiByEndDateList(int hpId, int seikyuYm, List<HasErrorWithSanteiModel> hasErrorList)
    {
        List<HasErrorWithSanteiModel> result = new();
        var ptIdList = hasErrorList.Select(item => item.PtId).Distinct().ToList();
        var itemCdList = hasErrorList.Select(item => item.ItemCd).Distinct().ToList();
        int maxSindate = hasErrorList.Select(item => item.Sindate)?.DefaultIfEmpty(0).Max() ?? 0;
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                              && ptIdList.Contains(item.PtId)
                                                                              && item.SinYm == seikyuYm
                                                                              && item.SinDate <= maxSindate)
                                                               .ToList();

        ptIdList = sinKouiCounts.Select(item => item.PtId).Distinct().ToList();
        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(item => item.HpId == hpId
                                                                                && ptIdList.Contains(item.PtId)
                                                                                && item.SinYm == seikyuYm
                                                                                && item.ItemCd != null
                                                                                && itemCdList.Contains(item.ItemCd))
                                                                 .ToList();

        foreach (var model in hasErrorList)
        {
            var sinKouiCountItems = sinKouiCounts.Where(item => item.PtId == model.PtId && item.SinDate <= model.Sindate).ToList();
            var sinKouiDetailItems = sinKouiDetails.Where(item => item.PtId == model.PtId && item.ItemCd == model.ItemCd).ToList();
            var query = from sinKouiCount in sinKouiCountItems
                        join sinKouiDetail in sinKouiDetailItems
                        on new { sinKouiCount.RpNo, sinKouiCount.SeqNo } equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        select new
                        {
                            SinKouiCount = sinKouiCount
                        };

            result.Add(new HasErrorWithSanteiModel(
                           model.PtId,
                           model.ItemCd,
                           model.Sindate,
                           query?.Count() == 0));
        }
        return result;
    }

    public int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm)
    {
        return NoTrackingDataContext.ReceInfs.Count(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count == 0 || ptIds.Contains(p.PtId)));
    }

    public void ResetStatusAfterPendingShukei(int hpId, int userId, List<ReceInfo> receInfos)
    {
        if (receInfos == null || receInfos.Count == 0) return;

        List<ReceStatus> newReceStatus = new List<ReceStatus>();
        foreach (var receInfo in receInfos)
        {
            var receStatus = TrackingDataContext.ReceStatuses.Where(p => p.HpId == hpId &&
                                                                         p.PtId == receInfo.PtId &&
                                                                         p.SeikyuYm == receInfo.SeikyuYm &&
                                                                         p.SinYm == receInfo.SinYm &&
                                                                         p.HokenId == receInfo.HokenId &&
                                                                         p.IsDeleted == DeleteTypes.None)
                                .OrderByDescending(u => u.CreateDate).ThenByDescending(u => u.UpdateDate).FirstOrDefault();

            bool hasError = TrackingDataContext.ReceCheckErrs.Any(p => p.SinYm == receInfo.SinYm && p.PtId == receInfo.PtId && p.HokenId == receInfo.HokenId);

            if (receStatus == null)
            {
                if (hasError)
                {
                    newReceStatus.Add(new ReceStatus()
                    {
                        HpId = Session.HospitalID,
                        PtId = receInfo.PtId,
                        SeikyuYm = receInfo.SeikyuYm,
                        SinYm = receInfo.SinYm,
                        HokenId = receInfo.HokenId,
                        StatusKbn = (int)ReceCheckStatusEnum.SystemPending,
                        CreateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow()
                    });
                }
                continue;
            }

            if (hasError)
            {
                if (receStatus.StatusKbn == (int)ReceCheckStatusEnum.UnConfirmed || receStatus.StatusKbn == (int)ReceCheckStatusEnum.TempComfirmed)
                {
                    receStatus.StatusKbn = (int)ReceCheckStatusEnum.SystemPending;
                }
            }
            else
            {
                if (receStatus.StatusKbn == (int)ReceCheckStatusEnum.SystemPending)
                {
                    receStatus.StatusKbn = (int)ReceCheckStatusEnum.UnConfirmed;
                }
            }
        }
        TrackingDataContext.ReceStatuses.AddRange(newReceStatus);
        TrackingDataContext.SaveChanges();
    }

    #endregion

    #region Private function
    private double GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int defaultValue = 0)
    {
        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.HpId == hpId
                                                                               && p.GrpCd == groupCd
                                                                               && p.GrpEdaNo == grpEdaNo);
        return systemConf != null ? systemConf.Val : defaultValue;
    }

    private string GetSettingParam(int hpId, int groupCd, int grpEdaNo = 0)
    {
        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.HpId == hpId
                                                                               && p.GrpCd == groupCd
                                                                               && p.GrpEdaNo == grpEdaNo);
        return systemConf?.Param ?? string.Empty;
    }

    private ReceCmtModel ConvertToReceCmtModel(ReceCmt receCmt, int sinDate, List<TenMst> tenMstList)
    {
        var tenMst = tenMstList.FirstOrDefault(item => item.ItemCd == receCmt.ItemCd
                                                       && item.StartDate <= sinDate
                                                       && item.EndDate >= sinDate);
        return new ReceCmtModel(
                receCmt.Id,
                receCmt.PtId,
                receCmt.SeqNo,
                receCmt.SinYm,
                receCmt.HokenId,
                receCmt.CmtKbn,
                receCmt.CmtSbt,
                receCmt.Cmt ?? string.Empty,
                receCmt.CmtData ?? string.Empty,
                receCmt.ItemCd ?? string.Empty,
                tenMst?.CmtCol1 ?? 0,
                tenMst?.CmtCol2 ?? 0,
                tenMst?.CmtCol3 ?? 0,
                tenMst?.CmtCol4 ?? 0,
                tenMst?.CmtColKeta1 ?? 0,
                tenMst?.CmtColKeta2 ?? 0,
                tenMst?.CmtColKeta3 ?? 0,
                tenMst?.CmtColKeta4 ?? 0
            );
    }

    private ReceCmt ConvertToNewReceCmt(int hpId, int userId, ReceCmtModel model)
    {
        ReceCmt entity = new();
        entity.HpId = hpId;
        entity.PtId = model.PtId;
        entity.SinYm = model.SinYm;
        entity.HokenId = model.HokenId;
        entity.CmtKbn = model.CmtKbn;
        entity.CmtSbt = model.CmtSbt;
        entity.Id = 0;
        entity.SeqNo = model.SeqNo;
        entity.ItemCd = model.ItemCd;
        entity.Cmt = model.Cmt;
        entity.CmtData = model.CmtData;
        entity.IsDeleted = 0;
        entity.CreateDate = CIUtil.GetJapanDateTimeNow();
        entity.CreateId = userId;
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        return entity;
    }

    private SyoukiInfModel ConvertToSyoukiInfModel(SyoukiInf syoukiInf)
    {
        return new SyoukiInfModel(
                                    syoukiInf.PtId,
                                    syoukiInf.SinYm,
                                    syoukiInf.HokenId,
                                    syoukiInf.SeqNo,
                                    syoukiInf.SortNo,
                                    syoukiInf.SyoukiKbn,
                                    syoukiInf.Syouki ?? string.Empty
                                );
    }

    private SyoukiKbnMstModel ConvertToSyoukiKbnMstModel(SyoukiKbnMst item)
    {
        return new SyoukiKbnMstModel(
                                        item.SyoukiKbn,
                                        item.StartYm,
                                        item.EndYm,
                                        item.Name ?? string.Empty
                                    );
    }

    private SyoukiInf ConvertToNewSyoukiInf(int hpId, int userId, SyoukiInfModel item)
    {
        SyoukiInf entity = new();
        entity.HpId = hpId;
        entity.PtId = item.PtId;
        entity.SinYm = item.SinYm;
        entity.HokenId = item.HokenId;
        entity.SeqNo = 0;
        entity.SortNo = item.SortNo;
        entity.SyoukiKbn = item.SyoukiKbn;
        entity.Syouki = item.Syouki;
        entity.IsDeleted = 0;
        entity.CreateDate = CIUtil.GetJapanDateTimeNow();
        entity.CreateId = userId;
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        return entity;
    }

    private SyobyoKeikaModel ConvertToSyobyoKeikaModel(SyobyoKeika syobyoKeika)
    {
        return new SyobyoKeikaModel(
                                        syobyoKeika.PtId,
                                        syobyoKeika.SinYm,
                                        syobyoKeika.SinDay,
                                        syobyoKeika.HokenId,
                                        syobyoKeika.SeqNo,
                                        syobyoKeika.Keika ?? string.Empty
                                    );
    }

    private SyobyoKeika ConvertToNewSyobyoKeika(int hpId, int userId, SyobyoKeikaModel item)
    {
        SyobyoKeika entity = new();
        entity.HpId = hpId;
        entity.PtId = item.PtId;
        entity.SinYm = item.SinYm;
        entity.HokenId = item.HokenId;
        entity.SeqNo = 0;
        entity.SinDay = item.SinDay;
        entity.Keika = item.Keika;
        entity.IsDeleted = 0;
        entity.CreateDate = CIUtil.GetJapanDateTimeNow();
        entity.CreateId = userId;
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        return entity;
    }

    private ReceCheckCmtModel ConvertToReceCheckCmtModel(ReceCheckCmt receCheckCmt)
    {
        return new ReceCheckCmtModel(
                    receCheckCmt.PtId,
                    receCheckCmt.SeqNo,
                    receCheckCmt.SinYm,
                    receCheckCmt.HokenId,
                    receCheckCmt.IsPending,
                    receCheckCmt.Cmt ?? string.Empty,
                    receCheckCmt.IsChecked,
                    receCheckCmt.SortNo
                   );
    }

    private ReceCheckErrModel ConvertToReceCheckErrModel(ReceCheckErr receCheckErr)
    {
        return new ReceCheckErrModel(
                    receCheckErr.PtId,
                    receCheckErr.SinYm,
                    receCheckErr.HokenId,
                    receCheckErr.ErrCd,
                    receCheckErr.SinDate,
                    receCheckErr.ACd,
                    receCheckErr.BCd,
                    receCheckErr.Message1 ?? string.Empty,
                    receCheckErr.Message2 ?? string.Empty,
                    receCheckErr.IsChecked);
    }

    private ReceCheckCmt ConvertToNewReceCheckCmt(int hpId, int userId, int hokenId, int sinYm, long ptId, ReceCheckCmtModel item)
    {
        ReceCheckCmt entity = new();
        entity.HpId = hpId;
        entity.PtId = ptId;
        entity.SinYm = sinYm;
        entity.HokenId = hokenId;
        entity.SeqNo = GetLastSeqNo(hpId, ptId, hokenId, sinYm) + 1;
        entity.IsPending = item.IsPending;
        entity.Cmt = item.Cmt;
        entity.IsChecked = item.IsChecked;
        entity.SortNo = item.SortNo;
        entity.IsDeleted = 0;
        entity.CreateDate = CIUtil.GetJapanDateTimeNow();
        entity.CreateId = userId;
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        return entity;
    }

    private int GetLastSeqNo(int hpId, long ptId, int hokenId, int sinYm)
    {
        var receCheckCmtList = NoTrackingDataContext.ReceCheckCmts.Where(item => item.HpId == hpId
                                                                                 && item.SinYm == sinYm
                                                                                 && item.PtId == ptId
                                                                                 && item.HokenId == hokenId)
                                                                  .ToList();
        return receCheckCmtList.Any() ? receCheckCmtList.Max(item => item.SeqNo) : 0;
    }

    private InsuranceReceInfModel ConvertToInsuranceReceInfModel(ReceInf receInf, PtHokenInf hokenInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
    {
        return new InsuranceReceInfModel(
                receInf.SeikyuYm,
                receInf.PtId,
                receInf.SinYm,
                receInf.HokenId,
                receInf.HokenId2,
                receInf.Kohi1Id,
                receInf.Kohi2Id,
                receInf.Kohi3Id,
                receInf.Kohi4Id,
                receInf.HokenKbn,
                receInf.ReceSbt ?? string.Empty,
                receInf.HokensyaNo ?? string.Empty,
                receInf.HokenReceTensu ?? 0,
                receInf.HokenReceFutan ?? 0,
                receInf.Kohi1ReceTensu ?? 0,
                receInf.Kohi1ReceFutan ?? 0,
                receInf.Kohi1ReceKyufu ?? 0,
                receInf.Kohi2ReceTensu ?? 0,
                receInf.Kohi2ReceFutan ?? 0,
                receInf.Kohi2ReceKyufu ?? 0,
                receInf.Kohi3ReceTensu ?? 0,
                receInf.Kohi3ReceFutan ?? 0,
                receInf.Kohi3ReceKyufu ?? 0,
                receInf.Kohi4ReceTensu ?? 0,
                receInf.Kohi4ReceFutan ?? 0,
                receInf.Kohi4ReceKyufu ?? 0,
                receInf.HokenNissu ?? 0,
                receInf.Kohi1Nissu ?? 0,
                receInf.Kohi2Nissu ?? 0,
                receInf.Kohi3Nissu ?? 0,
                receInf.Kohi4Nissu ?? 0,
                receInf.Kohi1ReceKisai,
                receInf.Kohi2ReceKisai,
                receInf.Kohi3ReceKisai,
                receInf.Kohi4ReceKisai,
                receInf.Tokki1 ?? string.Empty,
                receInf.Tokki2 ?? string.Empty,
                receInf.Tokki3 ?? string.Empty,
                receInf.Tokki4 ?? string.Empty,
                receInf.Tokki5 ?? string.Empty,
                receInf.RousaiIFutan,
                receInf.RousaiRoFutan,
                receInf.JibaiITensu,
                receInf.JibaiRoTensu,
                receInf.JibaiHaFutan,
                receInf.JibaiNiFutan,
                receInf.JibaiHoSindan,
                receInf.JibaiHeMeisai,
                receInf.JibaiAFutan,
                receInf.JibaiBFutan,
                receInf.JibaiCFutan,
                receInf.JibaiDFutan,
                receInf.JibaiKenpoFutan,
                ptKohi1?.FutansyaNo ?? string.Empty,
                ptKohi2?.FutansyaNo ?? string.Empty,
                ptKohi3?.FutansyaNo ?? string.Empty,
                ptKohi4?.FutansyaNo ?? string.Empty,
                ptKohi1?.JyukyusyaNo ?? string.Empty,
                ptKohi2?.JyukyusyaNo ?? string.Empty,
                ptKohi3?.JyukyusyaNo ?? string.Empty,
                ptKohi4?.JyukyusyaNo ?? string.Empty,
                hokenInf?.RousaiKofuNo ?? string.Empty,
                hokenInf?.Kigo ?? string.Empty,
                hokenInf?.Bango ?? string.Empty,
                hokenInf?.EdaNo ?? string.Empty
            );
    }

    private SinKouiMstModel AddNewSinKouiMstModel(SinKoui sinKoui, List<SinKouiDetailModel> listSinKouiDetailModel)
    {
        bool existItemWithCommentSelect = false;
        if (listSinKouiDetailModel == null || listSinKouiDetailModel.Count == 0)
        {
            existItemWithCommentSelect = false;
        }
        existItemWithCommentSelect = listSinKouiDetailModel?.Any(item => item.CmtSelectList != null && item.CmtSelectList.Count > 0) ?? false;
        return new SinKouiMstModel(
                sinKoui.PtId,
                sinKoui.SinYm,
                sinKoui.RpNo,
                sinKoui.SeqNo,
                sinKoui.HokenPid,
                sinKoui.HokenId,
                sinKoui.SyukeiSaki ?? string.Empty,
                sinKoui.HokatuKensa,
                sinKoui.TotalTen,
                sinKoui.Ten,
                sinKoui.Zei,
                sinKoui.Count,
                sinKoui.TenCount ?? string.Empty,
                sinKoui.TenColCount,
                sinKoui.IsNodspRece,
                sinKoui.IsNodspPaperRece,
                sinKoui.InoutKbn,
                sinKoui.EntenKbn,
                sinKoui.CdKbn ?? string.Empty,
                sinKoui.RecId ?? string.Empty,
                sinKoui.JihiSbt,
                sinKoui.KazeiKbn,
                sinKoui.DetailData ?? string.Empty,
                sinKoui.Day1,
                sinKoui.Day2,
                sinKoui.Day3,
                sinKoui.Day4,
                sinKoui.Day5,
                sinKoui.Day6,
                sinKoui.Day7,
                sinKoui.Day8,
                sinKoui.Day9,
                sinKoui.Day10,
                sinKoui.Day11,
                sinKoui.Day12,
                sinKoui.Day13,
                sinKoui.Day14,
                sinKoui.Day15,
                sinKoui.Day16,
                sinKoui.Day17,
                sinKoui.Day18,
                sinKoui.Day19,
                sinKoui.Day20,
                sinKoui.Day21,
                sinKoui.Day22,
                sinKoui.Day23,
                sinKoui.Day24,
                sinKoui.Day25,
                sinKoui.Day26,
                sinKoui.Day27,
                sinKoui.Day28,
                sinKoui.Day29,
                sinKoui.Day30,
                sinKoui.Day31,
                listSinKouiDetailModel ?? new(),
                existItemWithCommentSelect
            );
    }

    private ReceInfModel ConvertToReceInfModel(ReceInf receInf)
    {
        return new ReceInfModel(
                   receInf.SeikyuYm,
                   receInf.PtId,
                   receInf.SinYm,
                   receInf.HokenId,
                   receInf.HokenId2,
                   receInf.KaId,
                   receInf.TantoId,
                   receInf.ReceSbt ?? string.Empty,
                   receInf.HokenKbn,
                   receInf.HokenSbtCd,
                   receInf.Houbetu ?? string.Empty,
                   receInf.Kohi1Id,
                   receInf.Kohi2Id,
                   receInf.Kohi3Id,
                   receInf.Kohi4Id,
                   receInf.Kohi1Houbetu ?? string.Empty,
                   receInf.Kohi2Houbetu ?? string.Empty,
                   receInf.Kohi3Houbetu ?? string.Empty,
                   receInf.Kohi4Houbetu ?? string.Empty,
                   receInf.HonkeKbn,
                   receInf.Tokki1 ?? string.Empty,
                   receInf.Tokki2 ?? string.Empty,
                   receInf.Tokki3 ?? string.Empty,
                   receInf.Tokki4 ?? string.Empty,
                   receInf.Tokki5 ?? string.Empty,
                   receInf?.HokenNissu ?? -1,
                   receInf?.Kohi1Nissu ?? -1,
                   receInf?.Kohi2Nissu ?? -1,
                   receInf?.Kohi3Nissu ?? -1,
                   receInf?.Kohi4Nissu ?? -1,
                   receInf?.Kohi1ReceKyufu ?? -1,
                   receInf?.Kohi2ReceKyufu ?? -1,
                   receInf?.Kohi3ReceKyufu ?? -1,
                   receInf?.Kohi4ReceKyufu ?? -1,
                   receInf?.HokenReceTensu ?? -1,
                   receInf?.HokenReceFutan ?? -1,
                   receInf?.Kohi1ReceTensu ?? -1,
                   receInf?.Kohi1ReceFutan ?? -1,
                   receInf?.Kohi2ReceTensu ?? -1,
                   receInf?.Kohi2ReceFutan ?? -1,
                   receInf?.Kohi3ReceTensu ?? -1,
                   receInf?.Kohi3ReceFutan ?? -1,
                   receInf?.Kohi4ReceTensu ?? -1,
                   receInf?.Kohi4ReceFutan ?? -1
            );
    }

    private ReceiptEditModel ConvertToReceiptEditModel(ReceInfEdit receInfEdit)
    {
        return new ReceiptEditModel(
                   receInfEdit.SeqNo,
                   receInfEdit.Tokki1 ?? string.Empty,
                   receInfEdit.Tokki2 ?? string.Empty,
                   receInfEdit.Tokki3 ?? string.Empty,
                   receInfEdit.Tokki4 ?? string.Empty,
                   receInfEdit.Tokki5 ?? string.Empty,
                   receInfEdit?.HokenNissu ?? -1,
                   receInfEdit?.Kohi1Nissu ?? -1,
                   receInfEdit?.Kohi2Nissu ?? -1,
                   receInfEdit?.Kohi3Nissu ?? -1,
                   receInfEdit?.Kohi4Nissu ?? -1,
                   receInfEdit?.Kohi1ReceKyufu ?? -1,
                   receInfEdit?.Kohi2ReceKyufu ?? -1,
                   receInfEdit?.Kohi3ReceKyufu ?? -1,
                   receInfEdit?.Kohi4ReceKyufu ?? -1,
                   receInfEdit?.HokenReceTensu ?? -1,
                   receInfEdit?.HokenReceFutan ?? -1,
                   receInfEdit?.Kohi1ReceTensu ?? -1,
                   receInfEdit?.Kohi1ReceFutan ?? -1,
                   receInfEdit?.Kohi2ReceTensu ?? -1,
                   receInfEdit?.Kohi2ReceFutan ?? -1,
                   receInfEdit?.Kohi3ReceTensu ?? -1,
                   receInfEdit?.Kohi3ReceFutan ?? -1,
                   receInfEdit?.Kohi4ReceTensu ?? -1,
                   receInfEdit?.Kohi4ReceFutan ?? -1
            );
    }

    private ReceiptEditModel ConvertToReceiptEditModel(ReceInfPreEdit recePreInfEdit)
    {
        return new ReceiptEditModel(
                   int.MaxValue,
                   recePreInfEdit.Tokki1 ?? string.Empty,
                   recePreInfEdit.Tokki2 ?? string.Empty,
                   recePreInfEdit.Tokki3 ?? string.Empty,
                   recePreInfEdit.Tokki4 ?? string.Empty,
                   recePreInfEdit.Tokki5 ?? string.Empty,
                   recePreInfEdit?.HokenNissu ?? -1,
                   recePreInfEdit?.Kohi1Nissu ?? -1,
                   recePreInfEdit?.Kohi2Nissu ?? -1,
                   recePreInfEdit?.Kohi3Nissu ?? -1,
                   recePreInfEdit?.Kohi4Nissu ?? -1,
                   recePreInfEdit?.Kohi1ReceKyufu ?? -1,
                   recePreInfEdit?.Kohi2ReceKyufu ?? -1,
                   recePreInfEdit?.Kohi3ReceKyufu ?? -1,
                   recePreInfEdit?.Kohi4ReceKyufu ?? -1,
                   recePreInfEdit?.HokenReceTensu ?? -1,
                   recePreInfEdit?.HokenReceFutan ?? -1,
                   recePreInfEdit?.Kohi1ReceTensu ?? -1,
                   recePreInfEdit?.Kohi1ReceFutan ?? -1,
                   recePreInfEdit?.Kohi2ReceTensu ?? -1,
                   recePreInfEdit?.Kohi2ReceFutan ?? -1,
                   recePreInfEdit?.Kohi3ReceTensu ?? -1,
                   recePreInfEdit?.Kohi3ReceFutan ?? -1,
                   recePreInfEdit?.Kohi4ReceTensu ?? -1,
                   recePreInfEdit?.Kohi4ReceFutan ?? -1
            );
    }

    private ReceInfEdit ConvertToNewReceInfEdit(int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId, ReceiptEditModel model)
    {
        return new ReceInfEdit()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            SeqNo = 0,
            ReceSbt = string.IsNullOrEmpty(model.ReceSbt) ? null : model.ReceSbt,
            Houbetu = string.IsNullOrEmpty(model.Houbetu) ? null : model.Houbetu,
            HokenReceTensu = model.HokenReceTensu >= 0 ? model.HokenReceTensu : null,
            HokenReceFutan = model.HokenReceFutan >= 0 ? model.HokenReceFutan : null,
            Kohi1ReceTensu = model.Kohi1ReceTensu >= 0 ? model.Kohi1ReceTensu : null,
            Kohi1ReceFutan = model.Kohi1ReceFutan >= 0 ? model.Kohi1ReceFutan : null,
            Kohi2ReceTensu = model.Kohi2ReceTensu >= 0 ? model.Kohi2ReceTensu : null,
            Kohi2ReceFutan = model.Kohi2ReceFutan >= 0 ? model.Kohi2ReceFutan : null,
            Kohi3ReceTensu = model.Kohi3ReceTensu >= 0 ? model.Kohi3ReceTensu : null,
            Kohi3ReceFutan = model.Kohi3ReceFutan >= 0 ? model.Kohi3ReceFutan : null,
            Kohi4ReceTensu = model.Kohi4ReceTensu >= 0 ? model.Kohi4ReceTensu : null,
            Kohi4ReceFutan = model.Kohi4ReceFutan >= 0 ? model.Kohi4ReceFutan : null,
            Kohi1ReceKyufu = model.Kohi1ReceKyufu >= 0 ? model.Kohi1ReceKyufu : null,
            Kohi2ReceKyufu = model.Kohi2ReceKyufu >= 0 ? model.Kohi2ReceKyufu : null,
            Kohi3ReceKyufu = model.Kohi3ReceKyufu >= 0 ? model.Kohi3ReceKyufu : null,
            Kohi4ReceKyufu = model.Kohi4ReceKyufu >= 0 ? model.Kohi4ReceKyufu : null,
            HokenNissu = model.HokenNissu >= 0 ? model.HokenNissu : null,
            Kohi1Nissu = model.Kohi1Nissu >= 0 ? model.Kohi1Nissu : null,
            Kohi2Nissu = model.Kohi2Nissu >= 0 ? model.Kohi2Nissu : null,
            Kohi3Nissu = model.Kohi3Nissu >= 0 ? model.Kohi3Nissu : null,
            Kohi4Nissu = model.Kohi4Nissu >= 0 ? model.Kohi4Nissu : null,
            Tokki = string.IsNullOrEmpty(model.Tokki) ? null : model.Tokki,
            Tokki1 = string.IsNullOrEmpty(model.Tokki1) ? null : model.Tokki1,
            Tokki2 = string.IsNullOrEmpty(model.Tokki2) ? null : model.Tokki2,
            Tokki3 = string.IsNullOrEmpty(model.Tokki3) ? null : model.Tokki3,
            Tokki4 = string.IsNullOrEmpty(model.Tokki4) ? null : model.Tokki4,
            Tokki5 = string.IsNullOrEmpty(model.Tokki5) ? null : model.Tokki5,
            IsDeleted = 0,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateId = userId,
        };
    }

    private ReceStatusModel ConvertToReceStatusModel(ReceStatus receStatus)
    {
        return new ReceStatusModel(
                   receStatus.PtId,
                   receStatus.SeikyuYm,
                   receStatus.HokenId,
                   receStatus.SinYm,
                   receStatus.FusenKbn,
                   receStatus.IsPaperRece == 1,
                   receStatus.Output == 1,
                   receStatus.StatusKbn,
                   receStatus.IsPrechecked == 1
               );
    }
    #endregion

    public List<ReceInfValidateModel> GetReceValidateReceiptCreation(int hpId, List<long> ptIds, int sinYm)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);

        var receInfs = NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

        var receStates = NoTrackingDataContext.ReceStatuses.Where(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

        var receInfJoinPtInfQuery = from receInf in receInfs
                                    join ptInf in ptInfs
                                    on receInf.PtId equals ptInf.PtId
                                    join receState in receStates
                                    on new { receInf.PtId, receInf.HokenId } equals new { receState.PtId, receState.HokenId } into tempReceStates
                                    from tempRcStt in tempReceStates.DefaultIfEmpty()
                                    select new
                                    {
                                        PtInf = ptInf,
                                        ReceInf = receInf,
                                        RcStatus = tempRcStt ?? null
                                    };

        var query = from receInfJoinPtInf in receInfJoinPtInfQuery
                    join ptHokenInf in ptHokenInfs
                    on new { receInfJoinPtInf.ReceInf.PtId, receInfJoinPtInf.ReceInf.HokenId } equals new { ptHokenInf.PtId, ptHokenInf.HokenId } into tempPtHokenInfs
                    from tempPtHokenInf in tempPtHokenInfs.DefaultIfEmpty()
                    where receInfJoinPtInf.ReceInf.HokenId != 0
                    select new
                    {
                        PtInf = receInfJoinPtInf.PtInf,
                        ReceInf = receInfJoinPtInf.ReceInf,
                        PtHokenInf = tempPtHokenInf,
                        RcStatus = receInfJoinPtInf.RcStatus
                    };

        return query.Select(x => new ReceInfValidateModel(x.PtInf.PtId,
                                                         x.PtInf.PtNum,
                                                         x.ReceInf.SinYm,
                                                         x.ReceInf.IsTester,
                                                         x.ReceInf.HokenKbn,
                                                         x.RcStatus == null ? 0 : x.RcStatus.IsPaperRece,
                                                         x.PtHokenInf == null ? 0 : x.PtHokenInf.RousaiSaigaiKbn,
                                                         x.ReceInf.HokenId,
                                                         x.PtHokenInf == null ? 0 : x.PtHokenInf.RousaiSyobyoDate)).ToList();
    }

    public bool ExistSyobyoKeikaData(int hpId, long ptId, int sinYm, int hokenId)
    {
        var syobyoKeika = NoTrackingDataContext.SyobyoKeikas.FirstOrDefault(p => p.HpId == hpId &&
                                                                                 p.IsDeleted == DeleteTypes.None &&
                                                                                 p.PtId == ptId &&
                                                                                 p.SinYm == sinYm &&
                                                                                 p.HokenId == hokenId);
        return syobyoKeika != null && !string.IsNullOrEmpty(syobyoKeika.Keika);
    }

    public List<ReceInfForCheckErrSwapHokenModel> GetReceInforCheckErrForCalculateSwapHoken(int hpId, List<long> ptIds, int sinYM)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None && ptIds.Contains(p.PtId));
        var receInfs = NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

        var query = from rc in receInfs
                    join pt in ptInfs
                    on rc.PtId equals pt.PtId
                    select new
                    {
                        PtId = pt.PtId,
                        PtNum = pt.PtNum,
                        HokenId = rc.HokenId,
                        SinYm = rc.SinYm
                    };

        return query.AsEnumerable().Select(x => new ReceInfForCheckErrSwapHokenModel(x.PtId, x.PtNum, x.SinYm, x.HokenId)).ToList();
    }

    public bool HasErrorCheck(int sinYm, long ptId, int hokenId)
    {
        return NoTrackingDataContext.ReceCheckErrs.Any(p => p.SinYm == sinYm && p.PtId == ptId && p.HokenId == hokenId);
    }

    public bool SaveReceStatusCalc(List<ReceStatusModel> newReceStatus, List<ReceStatusModel> updateList, int userId, int hpId)
    {
        foreach (var item in updateList)
        {
            var update = TrackingDataContext.ReceStatuses.FirstOrDefault(x => x.HpId == hpId && x.PtId == item.PtId
                                                                            && x.SeikyuYm == item.SeikyuYm
                                                                            && x.SinYm == item.SinYm
                                                                            && x.HokenId == item.HokenId);
            if (update != null)
            {
                update.StatusKbn = item.StatusKbn;
            }
        }

        TrackingDataContext.ReceStatuses.AddRange(Mapper.Map<ReceStatusModel, ReceStatus>(newReceStatus, (src, dest) =>
        {
            dest.HpId = hpId;
            dest.CreateDate = CIUtil.GetJapanDateTimeNow();
            dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dest.CreateId = userId;
            return dest;
        }));
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<int> GetListKaikeiInf(int hpId, long ptId)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs
                                                .Where(x => x.HpId == hpId && x.PtId == ptId)
                                                .AsEnumerable()
                                                .Select(x => new KaikeiInfModel(x.PtId, x.SinDate))
                                                .OrderByDescending(x => x.SinDate);
        return kaikeiInfs.Select(x => x.SinYm).Distinct().ToList();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
