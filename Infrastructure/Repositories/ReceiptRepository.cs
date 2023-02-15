using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.Receipt;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReceiptRepository : RepositoryBase, IReceiptRepository
{
    public ReceiptRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<ReceiptListModel> GetListReceipt(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel)
    {
        if (seikyuYm == 0)
        {
            return new();
        }
        int fromDay = (seikyuYm.AsString() + "01").AsInteger();
        int toDay = (seikyuYm.AsString() + DateTime.DaysInMonth(seikyuYm / 100, seikyuYm % 100)).AsInteger();

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
                                                                .Select(grp => grp.OrderByDescending(item => item.SortNo).FirstOrDefault() ?? new ReceCheckCmt())
                                                                .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId, item.Cmt, item.IsPending });

        var receCheckErrors = NoTrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                                && item.IsChecked == 0
                                                                                && item.SinYm >= minSinYM
                                                                                && item.SinYm <= seikyuYm
                                                                                && listPtIds.Contains(item.PtId))
                                                                  .AsEnumerable()
                                                                  .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                                  .Select(grp => grp.OrderBy(item => item.ErrCd).FirstOrDefault() ?? new ReceCheckErr())
                                                                  .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId, item.Message1, item.Message2 });

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

            // 練習患者を表示しない
            if (!searchModel.IsTestPatientSearch)
            {
                receInfs = receInfs.Where(item => item.IsTester != 1);
            }

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
                if (searchModel.ReceSbtCenter != -1)
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
                if (searchModel.ReceSbtRight != -1)
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
                if (searchModel.ReceSbtCenter != -1)
                {
                    receInfs = receInfs.Where(item => item.ReceSbt != null && item.ReceSbt.Substring(2, 1) == searchModel.ReceSbtCenter.ToString());
                }
                if (searchModel.ReceSbtRight != -1)
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
                                                                            && rcs.SinYm == item.SinYm).Output != 1);
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
                var santeiItemListWithItemCd = tenMstSanteis.GroupBy(item => new { item.ItemCd, item.SanteiItemCd })
                                                            .Select(item => new { item.Key.ItemCd, item.Key.SanteiItemCd }).ToList();

                IEnumerable<ItemSumModel> enumOdrDetailItemSum = Enumerable.Empty<ItemSumModel>();
                List<ItemSumModel> santeiItemSum = new();
                #endregion

                if (originItemOrderList != null && originItemOrderList.Any())
                {
                    #region Count and sum item from order
                    int maxSinYm = (sinYmGroup.Max() * 100) + 31;
                    int minSinYm = (sinYmGroup.Min() * 100) + 1;

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
                    int maxSinYm = sinYmGroup.Max();
                    int minSinYm = sinYmGroup.Min();

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
                    from ptKyusei in ptKyuseiLeft.Where(item => item.EndDate >= kaikeiInf.SinDate && item.PtId == kaikeiInf.PtId).Take(1).DefaultIfEmpty()

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
                        ReceCheckCmt = receCheckCmt != null ? receCheckCmt.Cmt : (receCheckErr != null ? receCheckErr.Message1 + receCheckErr.Message2 : string.Empty),
                        IsPending = receCheckCmt != null ? receCheckCmt.IsPending : -1,
                        ptInf.PtNum,
                        Name = ptKyusei != null ? ptKyusei.Name : ptInf.Name,
                        KanaName = ptKyusei != null ? ptKyusei.KanaName : ptInf.KanaName,
                        ptInf.Sex,
                        ptInf.Birthday,
                        receInf.IsTester,
                        HokensyaNo = ptHokenInf?.HokensyaNo ?? string.Empty,
                        IsSyoukiInfExist = syoukiInf != null ? 1 : 0,
                        IsReceCmtExist = receCmt != null ? 1 : 0,
                        IsSyobyoKeikaExist = syobyokeika != null ? 1 : 0,
                        SeikyuCmt = receSeikyu != null ? receSeikyu.Cmt : string.Empty,
                        LastVisitDate = ptLastVisitDate != null ? ptLastVisitDate.SinDate : 0,
                        kaMst.KaName,
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
                                data.Tokki
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

    #region Rece check screeen
    public List<ReceCmtModel> GetListReceCmt(int hpId, int sinYm, long ptId, int hokenId)
    {
        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                 && item.SinYm == sinYm
                                                                 && item.PtId == ptId
                                                                 && item.HokenId == hokenId
                                                                 && item.IsDeleted == DeleteTypes.None)
                                                     .ToList();

        var result = receCmts.Select(item => ConvertToReceCmtModel(item)).ToList();
        return result;
    }

    public bool SaveListReceCmt(int hpId, int userId, List<ReceCmtModel> listReceCmt)
    {
        var listReceCmtUpdate = listReceCmt.Where(item => item.Id > 0).ToList();
        var listReceCmtUpdateDB = TrackingDataContext.ReceCmts.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && listReceCmtUpdate.Select(item => item.Id).Contains(item.Id))
                                                              .ToList();

        var listReceCmtAddNew = listReceCmt.Where(item => item.Id == 0 && !item.IsDeleted)
                                           .Select(item => ConvertToNewReceCmt(hpId, userId, item))
                                           .ToList();
        TrackingDataContext.ReceCmts.AddRange(listReceCmtAddNew);
        foreach (var model in listReceCmtUpdate)
        {
            var entity = listReceCmtUpdateDB.FirstOrDefault(item => item.Id == model.Id);
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

    public List<SyoukiInfModel> GetListSyoukiInf(int hpId, int sinYm, long ptId, int hokenId)
    {
        var listSyoukiInf = NoTrackingDataContext.SyoukiInfs.Where(item => item.HpId == hpId
                                                                           && item.SinYm == sinYm
                                                                           && item.PtId == ptId
                                                                           && item.HokenId == hokenId
                                                                           && item.IsDeleted == DeleteTypes.None)
                                                            .OrderBy(item => item.SortNo)
                                                            .ToList();

        var result = listSyoukiInf.Select(item => ConvertToSyoukiInfModel(item)).ToList();
        return result;
    }

    public List<SyoukiKbnMstModel> GetListSyoukiKbnMst(int sinYm)
    {
        var listSyoukiKbnMst = NoTrackingDataContext.SyoukiKbnMsts.Where(item => item.StartYm <= sinYm && item.EndYm >= sinYm)
                                                                  .OrderBy(p => p.SyoukiKbn)
                                                                  .ToList();

        var result = listSyoukiKbnMst.Select(item => ConvertToSyoukiKbnMstModel(item)).ToList();
        return result;
    }

    public List<SyobyoKeikaModel> GetListSyobyoKeika(int hpId, int sinYm, long ptId, int hokenId)
    {
        var listSyobyoKeika = NoTrackingDataContext.SyobyoKeikas.Where(item => item.HpId == hpId
                                                                               && item.SinYm == sinYm
                                                                               && item.PtId == ptId
                                                                               && item.HokenId == hokenId
                                                                               && item.IsDeleted == DeleteTypes.None)
                                                                .OrderBy(item => item.SinDay)
                                                                .ThenByDescending(item => item.SeqNo)
                                                                .ToList();

        var result = listSyobyoKeika.Select(item => ConvertToSyobyoKeikaModel(item)).ToList();
        return result;
    }

    public bool SaveListSyoukiInf(int hpId, int userId, List<SyoukiInfModel> listSyoukiInf)
    {
        var listSyoukiInfUpdate = listSyoukiInf.Where(item => item.SeqNo > 0).ToList();
        var listSyoukiInfUpdateDB = TrackingDataContext.SyoukiInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && listSyoukiInfUpdate.Select(item => item.SeqNo).Contains(item.SeqNo))
                                                              .ToList();

        var listSyoukiInfAddNew = listSyoukiInf.Where(item => item.SeqNo == 0 && !item.IsDeleted)
                                               .Select(item => ConvertToNewSyoukiInf(hpId, userId, item))
                                               .ToList();
        TrackingDataContext.SyoukiInfs.AddRange(listSyoukiInfAddNew);
        foreach (var model in listSyoukiInfUpdate)
        {
            var entity = listSyoukiInfUpdateDB.FirstOrDefault(item => item.SeqNo == model.SeqNo);
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

    public bool CheckExistSyoukiKbn(int sinYm, List<SyoukiKbnMstModel> listSyoukiKbn)
    {
        var countSyoukiKbn = NoTrackingDataContext.SyoukiKbnMsts.AsEnumerable().Count(entity => entity.StartYm <= sinYm
                                                                               && entity.EndYm >= sinYm
                                                                               && listSyoukiKbn.Any(input => input.SyoukiKbn == entity.SyoukiKbn && input.StartYm == entity.StartYm));
        return countSyoukiKbn == listSyoukiKbn.Count;
    }
    #endregion

    #region Private function
    private ReceCmtModel ConvertToReceCmtModel(ReceCmt receCmt)
    {
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
                receCmt.ItemCd ?? string.Empty
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
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
