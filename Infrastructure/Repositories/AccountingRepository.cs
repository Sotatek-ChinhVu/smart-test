using Domain.Constant;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.Reception;
using Domain.Models.ReceptionSameVisit;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        public AccountingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public List<ReceptionDto> GetListRaiinInf(int hpId, long ptId, int sinDate, long raiinNo, bool isGetHeader = false, bool getAll = true)
        {
            var listRaiinInf = new List<RaiinInf>();
            if (getAll)
            {
                var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item => item.RaiinNo == raiinNo && item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted == 0);

                if (oyaRaiinNo == null || oyaRaiinNo.Status <= 3)
                {
                    return new List<ReceptionDto>();
                }

                listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(
                  item => item.OyaRaiinNo == oyaRaiinNo.OyaRaiinNo && item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate && item.IsDeleted == 0).ToList();
            }
            else
            {
                listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(
                  item => item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate && item.RaiinNo == raiinNo && item.IsDeleted == 0).ToList();
            }


            var listKaId = listRaiinInf.Select(item => item.KaId).Distinct().ToList();

            var listHokenPid = listRaiinInf.Select(item => item.HokenPid).Distinct().ToList();

            var listKaikeiInf = NoTrackingDataContext.KaikeiInfs.Where(item =>
                item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate);

            var listKaMst = NoTrackingDataContext.KaMsts.Where(item =>
            item.HpId == hpId && item.IsDeleted == 0 && listKaId.Contains(item.KaId));

            var listHokenPattern = FindPtHokenPatternList(hpId, ptId, sinDate, listHokenPid);
            var listRaiin = from raiinInf in listRaiinInf
                            join kaikeiInf in listKaikeiInf on
                                raiinInf.RaiinNo equals kaikeiInf.RaiinNo into listKaikei
                            select new
                            {
                                ListKaikeiInf = listKaikei,
                                RaiinInf = raiinInf
                            };

            #region GetPersonNumber
            var countAcc = 0;
            if (isGetHeader)
            {
                countAcc = NoTrackingDataContext.RaiinInfs.Count(item =>
               item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted == DeleteTypes.None &&
               (item.Status == RaiinState.Calculate || item.Status == RaiinState.Waiting));
                var lastRaiinInf = listRaiinInf.Last();

                if (lastRaiinInf.Status < RaiinState.Settled)
                {
                    if (listRaiinInf.Count > 1)
                    {
                        countAcc = countAcc - listRaiinInf.Count();
                    }
                    else
                    {
                        countAcc--;
                    }
                }
            }
            #endregion

            return listRaiin
            .Select(
                item => new ReceptionDto(item.RaiinInf.RaiinNo, item.RaiinInf.UketukeNo,
                    listKaMst.FirstOrDefault(itemKaMst => itemKaMst.KaId == item.RaiinInf.KaId)?.KaSname ?? string.Empty,
                    countAcc,
                    listHokenPattern.FirstOrDefault(itemPattern => itemPattern.HokenPid == item.RaiinInf.HokenPid) ?? new(),
                    item.ListKaikeiInf.Select(k => new KaikeiInfModel(
                                                k.HpId,
                                                k.PtId,
                                                k.SinDate,
                                                k.RaiinNo,
                                                k.HokenId,
                                                k.Kohi1Id,
                                                k.Kohi2Id,
                                                k.Kohi3Id,
                                                k.Kohi4Id,
                                                k.HokenKbn,
                                                k.HokenSbtCd,
                                                k.ReceSbt ?? string.Empty,
                                                k.Houbetu ?? string.Empty,
                                                k.Kohi1Houbetu ?? string.Empty,
                                                k.Kohi2Houbetu ?? string.Empty,
                                                k.Kohi3Houbetu ?? string.Empty,
                                                k.Kohi4Houbetu ?? string.Empty,
                                                k.HonkeKbn,
                                                k.HokenRate,
                                                k.PtRate,
                                                k.DispRate,
                                                k.Tensu,
                                                k.TotalIryohi,
                                                k.PtFutan,
                                                k.JihiFutan,
                                                k.JihiTax,
                                                k.JihiOuttax,
                                                k.JihiFutanTaxfree,
                                                k.JihiFutanTaxNr,
                                                k.JihiFutanTaxGen,
                                                k.JihiFutanOuttaxNr,
                                                k.JihiFutanOuttaxGen,
                                                k.JihiTaxNr,
                                                k.JihiTaxGen,
                                                k.JihiOuttaxNr,
                                                k.JihiOuttaxGen,
                                                k.AdjustFutan,
                                                k.AdjustRound,
                                                k.TotalPtFutan,
                                                k.AdjustFutanVal,
                                                k.AdjustFutanRange,
                                                k.AdjustRateVal,
                                                k.AdjustRateRange)).ToList()
                                            ))
            .ToList();

        }

        public List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, List<int> listPatternId)
        {
            List<HokenPatternModel> ptHokenPatternList = new List<HokenPatternModel>();

            // PtInf
            var ptInf = NoTrackingDataContext.PtInfs
                .FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == 0);

            if (ptInf is null) return ptHokenPatternList;

            var listPtHokenPattern = NoTrackingDataContext.PtHokenPatterns
                .Where(pattern => pattern.HpId == hpId
                                                     && pattern.PtId == ptId
                                                     && pattern.IsDeleted == 0
                                                     && listPatternId.Contains(pattern.HokenPid)).ToList();

            if (!listPtHokenPattern.Any()) return ptHokenPatternList;

            var ptHokenInfRepos = NoTrackingDataContext.PtHokenInfs
                .Where(hoken => hoken.HpId == hpId
                                                   && hoken.PtId == ptId
                                                   && hoken.IsDeleted == 0);

            var predicateHokenInf = CreatePtHokenInfExpression(listPtHokenPattern.Select(item => item.HokenId).ToList());

            List<PtHokenInf> listPtHokenInf = new List<PtHokenInf>();
            if (predicateHokenInf != null)
            {
                listPtHokenInf = ptHokenInfRepos.Where(predicateHokenInf).ToList();
            }

            var ptKohiRepos = NoTrackingDataContext.PtKohis
                .Where(kohi => kohi.HpId == hpId
                                                  && kohi.PtId == ptId
                                                  && kohi.IsDeleted == 0);

            var predicateKohi = CreatePtKohiExpression(listPtHokenPattern);

            List<PtKohi> listPtKohi = new List<PtKohi>();
            if (predicateKohi != null)
            {
                listPtKohi = ptKohiRepos.Where(predicateKohi).ToList();
            }

            var predicateHokenMst = CreateHokenMstExpression(listPtHokenInf, listPtKohi);

            if (predicateHokenMst == null) return ptHokenPatternList;

            var hokenMstListRepo = NoTrackingDataContext.HokenMsts
                .Where(
                    entity => entity.HpId == hpId
                              && (entity.PrefNo == 0
                              || entity.PrefNo == 0
                              || entity.IsOtherPrefValid == 1))
                .OrderBy(e => e.HpId)
                .ThenBy(e => e.HokenNo)
                .ThenBy(e => e.HokenEdaNo)
                .ThenByDescending(e => e.StartDate)
                .ThenBy(e => e.HokenSbtKbn)
                .ThenBy(e => e.SortNo);

            var hokenMstList = hokenMstListRepo.Where(predicateHokenMst).ToList();

            var predicatePtHokenCheck = CreatePtHokenCheckExpression(listPtHokenPattern);

            if (predicatePtHokenCheck == null) return ptHokenPatternList;

            var ptHokenCheckRepos = NoTrackingDataContext.PtHokenChecks.Where(item =>
                item.HpId == hpId && item.PtID == ptId && item.IsDeleted == 0);

            var ptHokenCheckList = ptHokenCheckRepos.Where(predicatePtHokenCheck).ToList();

            foreach (var ptHokenPattern in listPtHokenPattern)
            {
                var ptHokenInf = listPtHokenInf.FirstOrDefault(hk => hk.HokenId == ptHokenPattern.HokenId);
                var ptKohi1 = listPtKohi.FirstOrDefault(kohi => kohi.HokenId == ptHokenPattern.Kohi1Id);
                var ptKohi2 = listPtKohi.FirstOrDefault(kohi => kohi.HokenId == ptHokenPattern.Kohi2Id);
                var ptKohi3 = listPtKohi.FirstOrDefault(kohi => kohi.HokenId == ptHokenPattern.Kohi3Id);
                var ptKohi4 = listPtKohi.FirstOrDefault(kohi => kohi.HokenId == ptHokenPattern.Kohi4Id);

                ptHokenPatternList.Add(new HokenPatternModel(
                    ptHokenPattern.PtId, ptHokenPattern.HokenPid, ptHokenPattern.HokenId, ptHokenPattern.StartDate, ptHokenPattern.EndDate, ptHokenPattern.HokenSbtCd, ptHokenPattern.HokenKbn, ptHokenPattern.Kohi1Id, ptHokenPattern.Kohi2Id, ptHokenPattern.Kohi3Id, ptHokenPattern.Kohi4Id,
                    ptHokenInf == null
                        ? new()
                        : CreateHokenInfModel(ptHokenInf,
                            hokenMstList.Where(item =>
                                item.HokenNo == ptHokenInf.HokenNo &&
                                item.HokenEdaNo == ptHokenInf.HokenEdaNo).ToList(),
                            ptHokenCheckList.Where(item =>
                                item.HokenGrp == 1 &&
                                item.HokenId == ptHokenInf.HokenId)
                                .Select(item => new ConfirmDateModel(
                                     item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDay),
                    ptKohi1 == null
                        ? new()
                        : CreatePtKohiModel(ptKohi1,
                            hokenMstList.Where(item =>
                                item.HokenNo == ptKohi1.HokenNo &&
                                item.HokenEdaNo == ptKohi1.HokenEdaNo).ToList(),
                            ptHokenCheckList.Where(item =>
                                item.HokenGrp == 2 &&
                                item.HokenId == ptKohi1.HokenId)
                                .Select(item => new ConfirmDateModel(
                                     item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDay),
                    ptKohi2 == null
                        ? new()
                        : CreatePtKohiModel(ptKohi2,
                            hokenMstList.Where(item =>
                                item.HokenNo == ptKohi2.HokenNo &&
                                item.HokenEdaNo == ptKohi2.HokenEdaNo).ToList(),
                            ptHokenCheckList.Where(item =>
                                item.HokenGrp == 2 &&
                                item.HokenId == ptKohi2.HokenId)
                                .Select(item => new ConfirmDateModel(
                                    item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDay),
                    ptKohi3 == null
                        ? new()
                        : CreatePtKohiModel(ptKohi3,
                            hokenMstList.Where(item =>
                                item.HokenNo == ptKohi3.HokenNo &&
                                item.HokenEdaNo == ptKohi3.HokenEdaNo).ToList(),
                            ptHokenCheckList.Where(item =>
                                item.HokenGrp == 2 &&
                                item.HokenId == ptKohi3.HokenId)
                                .Select(item => new ConfirmDateModel(
                                    item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDay),
                    ptKohi4 == null
                        ? new()
                        : CreatePtKohiModel(ptKohi4,
                            hokenMstList.Where(item =>
                                item.HokenNo == ptKohi4.HokenNo &&
                                item.HokenEdaNo == ptKohi4.HokenEdaNo).ToList(),
                            ptHokenCheckList.Where(item =>
                                item.HokenGrp == 2 &&
                                item.HokenId == ptKohi4.HokenId)
                                .Select(item => new ConfirmDateModel(
                                   item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDay)
                ));
            }

            return ptHokenPatternList;
        }

        private Expression<Func<PtHokenInf, bool>> CreatePtHokenInfExpression(List<int> listHokenId)
        {
            var param = Expression.Parameter(typeof(PtHokenInf));
            Expression expression = null;

            if (listHokenId != null && listHokenId.Count > 0)
            {
                foreach (var hokenId in listHokenId)
                {
                    if (hokenId > 0)
                    {
                        var valHokenId = Expression.Constant(hokenId);
                        var memberHokenId = Expression.Property(param, nameof(PtHokenInf.HokenId));
                        Expression expressionHokenId = Expression.Equal(valHokenId, memberHokenId);

                        expression = expression == null ? expressionHokenId : Expression.Or(expression, expressionHokenId);
                    }
                }
            }

            return expression != null
                ? Expression.Lambda<Func<PtHokenInf, bool>>(body: expression, parameters: param)
                : null;
        }

        private Expression<Func<PtKohi, bool>> CreatePtKohiExpression(List<PtHokenPattern> listPtHokenPattern)
        {
            var param = Expression.Parameter(typeof(PtKohi));
            Expression expression = null;

            if (listPtHokenPattern != null && listPtHokenPattern.Count > 0)
            {
                foreach (var pattern in listPtHokenPattern)
                {
                    if (pattern.PtId > 0)
                    {
                        CreatePtKohiExpression(new List<int>()
                        {
                            pattern.Kohi1Id,
                            pattern.Kohi2Id,
                            pattern.Kohi3Id,
                            pattern.Kohi4Id
                        }, ref expression, ref param);
                    }
                }
            }

            return expression != null
                ? Expression.Lambda<Func<PtKohi, bool>>(body: expression, parameters: param)
                : null;
        }

        private void CreatePtKohiExpression(List<int> listKohiId, ref Expression expression, ref ParameterExpression param)
        {
            if (listKohiId != null && listKohiId.Count > 0)
            {
                foreach (var kohiId in listKohiId)
                {
                    if (kohiId > 0)
                    {
                        var valHokenId = Expression.Constant(kohiId);
                        var memberHokenId = Expression.Property(param, nameof(PtKohi.HokenId));
                        Expression expressionHokenId = Expression.Equal(valHokenId, memberHokenId);

                        expression = expression == null ? expressionHokenId : Expression.Or(expression, expressionHokenId);
                    }
                }
            }
        }

        private Expression<Func<PtHokenCheck, bool>> CreatePtHokenCheckExpression(List<PtHokenPattern> listPtHokenPattern)
        {
            var param = Expression.Parameter(typeof(PtHokenCheck));
            Expression expression = null;

            if (listPtHokenPattern != null && listPtHokenPattern.Count > 0)
            {
                CreatePtHokenCheckExpression(listPtHokenPattern.Select(item => item.HokenId).ToList(), 1, ref expression,
                    ref param);

                foreach (var pattern in listPtHokenPattern)
                {
                    if (pattern.PtId > 0)
                    {
                        CreatePtHokenCheckExpression(new List<int>()
                        {
                            pattern.Kohi1Id,
                            pattern.Kohi2Id,
                            pattern.Kohi3Id,
                            pattern.Kohi4Id
                        }, 2, ref expression, ref param);
                    }
                }
            }

            return expression != null
                ? Expression.Lambda<Func<PtHokenCheck, bool>>(body: expression, parameters: param)
                : null;
        }

        private void CreatePtHokenCheckExpression(List<int> listHokenId, int hokenGrp, ref Expression expression, ref ParameterExpression param)
        {
            if (listHokenId != null && listHokenId.Count > 0)
            {
                foreach (var hokenId in listHokenId)
                {
                    if (hokenId > 0)
                    {
                        var valHokenId = Expression.Constant(hokenId);
                        var memberHokenId = Expression.Property(param, nameof(PtHokenCheck.HokenId));

                        var valHokenGrp = Expression.Constant(hokenGrp);
                        var memberHokenGrp = Expression.Property(param, nameof(PtHokenCheck.HokenGrp));

                        var expressionHokenCheck = Expression.And(Expression.Equal(valHokenId, memberHokenId),
                            Expression.Equal(valHokenGrp, memberHokenGrp));

                        expression = expression == null ? expressionHokenCheck : Expression.Or(expression, expressionHokenCheck);
                    }
                }
            }
        }

        private Expression<Func<HokenMst, bool>>? CreateHokenMstExpression(List<PtHokenInf> listPtHokenInf,
            List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(HokenMst));
            Expression expression = null;

            CreateHokenMstExpression(listPtHokenInf, ref expression, ref param);
            CreateHokenMstExpression(listPtKohi, ref expression, ref param);

            return expression != null
                ? Expression.Lambda<Func<HokenMst, bool>>(body: expression, parameters: param)
                : null;
        }

        private void CreateHokenMstExpression(List<PtHokenInf> listPtHokenInf, ref Expression expression, ref ParameterExpression param)
        {
            if (listPtHokenInf != null)
            {
                foreach (var item in listPtHokenInf)
                {
                    if (item != null)
                    {
                        var valHokenNo = Expression.Constant(item.HokenNo);
                        var memberHokenNo = Expression.Property(param, nameof(HokenMst.HokenNo));

                        var valHokenEdaNo = Expression.Constant(item.HokenEdaNo);
                        var memberHokenEdaNo = Expression.Property(param, nameof(HokenMst.HokenEdaNo));

                        var expressionHoken = Expression.And(Expression.Equal(valHokenNo, memberHokenNo),
                            Expression.Equal(valHokenEdaNo, memberHokenEdaNo));

                        expression = expression == null ? expressionHoken : Expression.Or(expression, expressionHoken);
                    }
                }
            }
        }

        private void CreateHokenMstExpression(List<PtKohi> listPtKohi, ref Expression expression, ref ParameterExpression param)
        {
            if (listPtKohi != null && listPtKohi.Count > 0)
            {
                foreach (var item in listPtKohi)
                {
                    if (item != null)
                    {
                        var valHokenNo = Expression.Constant(item.HokenNo);
                        var memberHokenNo = Expression.Property(param, nameof(HokenMst.HokenNo));

                        var valHokenEdaNo = Expression.Constant(item.HokenEdaNo);
                        var memberHokenEdaNo = Expression.Property(param, nameof(HokenMst.HokenEdaNo));

                        var expressionKohi = Expression.And(Expression.Equal(valHokenNo, memberHokenNo),
                            Expression.Equal(valHokenEdaNo, memberHokenEdaNo));

                        expression = expression == null ? expressionKohi : Expression.Or(expression, expressionKohi);
                    }
                }
            }
        }

        private HokenInfModel CreateHokenInfModel(PtHokenInf ePtHokenInf, List<HokenMst> hokenMstLists, List<ConfirmDateModel> ConfirmDateModelList, int sinDay)
        {
            HokenInfModel hokenInfModel = new();
            if (ePtHokenInf != null)
            {
                HokenMst? hokenMst;
                var hokMstMapped = hokenMstLists
                   .FindAll(hk =>
                   hk.HokenNo == ePtHokenInf.HokenNo
                   && hk.HokenEdaNo == ePtHokenInf.HokenEdaNo)
                   .OrderByDescending(hk => hk.StartDate);

                if (hokMstMapped.Count() > 1)
                {
                    // pick one newest within startDate <= sinday
                    var firstMapped = hokMstMapped.FirstOrDefault(hokMst => hokMst.StartDate <= sinDay);
                    if (firstMapped == null)
                    {
                        // does not exist any hoken master with startDate <= sinday, pick lastest hoken mst (with min start date)
                        // pick last cause by all hoken master is order by start date descending
                        hokenMst = hokMstMapped?.LastOrDefault();
                    }
                    else
                    {
                        hokenMst = firstMapped;
                    }
                }
                else
                {
                    // have just one hoken mst with HokenNo and HokenEdaNo
                    hokenMst = hokMstMapped.FirstOrDefault();
                }

                HokenMstModel? hokenMstModel = null;
                if (hokenMst != null)
                {
                    hokenMstModel = new HokenMstModel();
                }
                hokenInfModel = new HokenInfModel(ePtHokenInf.HpId, ePtHokenInf.PtId, ePtHokenInf.HokenId, ePtHokenInf.HokenKbn, ePtHokenInf.Houbetu ?? string.Empty, ePtHokenInf.StartDate, ePtHokenInf.EndDate, sinDay, new(), ConfirmDateModelList.Select(p => new ConfirmDateModel(p.HokenGrp, p.HokenId,p.SeqNo, p.CheckId, p.CheckMachine, p.CheckComment, p.ConfirmDate)).ToList());
            }

            return hokenInfModel;
        }

        private KohiInfModel CreatePtKohiModel(PtKohi eKohiInf, List<HokenMst> hokenMstLists, List<ConfirmDateModel> ConfirmDateModelList, int sinDay)
        {
            KohiInfModel kohiInfModel = new();
            if (eKohiInf != null)
            {
                HokenMst hokenMst;
                var hokMstMapped = hokenMstLists
                   .FindAll(hk =>
                   hk.HokenNo == eKohiInf.HokenNo
                   && hk.HokenEdaNo == eKohiInf.HokenEdaNo)
                   .OrderByDescending(hk => hk.StartDate);
                if (hokMstMapped.Count() > 1)
                {
                    // pick one newest within startDate <= sinday
                    var firstMapped = hokMstMapped.FirstOrDefault(hokMst => hokMst.StartDate <= sinDay);
                    if (firstMapped == null)
                    {
                        // does not exist any hoken master with startDate <= sinday, pick lastest hoken mst (with min start date)
                        // pick last cause by all hoken master is order by start date descending
                        hokenMst = hokMstMapped.LastOrDefault() ?? new();
                    }
                    else
                    {
                        hokenMst = firstMapped;
                    }
                }
                else
                {
                    // have just one hoken mst with HokenNo and HokenEdaNo
                    hokenMst = hokMstMapped.FirstOrDefault() ?? new();
                }

                HokenMstModel hokenMstModel = new();
                if (hokenMst != null)
                {
                    hokenMstModel = new HokenMstModel(
                        hokenMst.FutanKbn,
                        hokenMst.FutanRate,
                        hokenMst.StartDate,
                        hokenMst.EndDate,
                        hokenMst.HokenNo,
                        hokenMst.HokenEdaNo,
                        hokenMst.HokenSname ?? string.Empty,
                        hokenMst.Houbetu ?? string.Empty,
                        hokenMst.HokenSbtKbn,
                        hokenMst.CheckDigit,
                        hokenMst.AgeStart,
                        hokenMst.AgeEnd,
                        hokenMst.IsFutansyaNoCheck,
                        hokenMst.IsJyukyusyaNoCheck,
                        hokenMst.JyukyuCheckDigit,
                        hokenMst.IsTokusyuNoCheck,
                        hokenMst.HokenName ?? string.Empty,
                        hokenMst.HokenNameCd ?? string.Empty,
                        hokenMst.HokenKohiKbn,
                        hokenMst.IsOtherPrefValid,
                        hokenMst.ReceKisai,
                        hokenMst.IsLimitList,
                        hokenMst.IsLimitListSum,
                        hokenMst.EnTen,
                        hokenMst.KaiLimitFutan,
                        hokenMst.DayLimitFutan,
                        hokenMst.MonthLimitFutan,
                        hokenMst.MonthLimitCount,
                        hokenMst.LimitKbn,
                        hokenMst.CountKbn,
                        hokenMst.FutanYusen,
                        hokenMst.CalcSpKbn,
                        hokenMst.MonthSpLimit,
                        hokenMst.KogakuTekiyo,
                        hokenMst.KogakuTotalKbn,
                        hokenMst.KogakuHairyoKbn,
                        hokenMst.ReceSeikyuKbn,
                        hokenMst.ReceKisaiKokho,
                        hokenMst.ReceKisai2,
                        hokenMst.ReceTenKisai,
                        hokenMst.ReceFutanRound,
                        hokenMst.ReceZeroKisai,
                        hokenMst.ReceSpKbn,
                        string.Empty,
                        hokenMst.PrefNo,
                        hokenMst.SortNo,
                        hokenMst.SeikyuYm,
                        hokenMst.ReceFutanHide,
                        hokenMst.ReceFutanKbn,
                        hokenMst.KogakuTotalAll,
                        true,
                        hokenMst.DayLimitCount,
                        new List<ExceptHokensyaModel>());
                }
                kohiInfModel = new KohiInfModel(eKohiInf.HokenId, eKohiInf.PrefNo, eKohiInf.HokenNo, eKohiInf.HokenEdaNo, eKohiInf.FutansyaNo ?? string.Empty, eKohiInf.StartDate, eKohiInf.EndDate, sinDay, hokenMstModel, ConfirmDateModelList.Select(p => new ConfirmDateModel(p.HokenGrp, p.HokenId, p.ConfirmDate, p.CheckId, p.CheckMachine, p.CheckComment, p.IsDeleted)).ToList());
            }

            return kohiInfModel;
        }

        public List<SyunoSeikyuModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> listRaiinNo, bool getAll = false)
        {
            IEnumerable<SyunoSeikyu> syunoSeikyuRepo;

            if (getAll == true)
            {
                syunoSeikyuRepo = NoTrackingDataContext.SyunoSeikyus
                    .Where(item =>
                        item.HpId == hpId && item.PtId == ptId &&
                        item.NyukinKbn == 1 && !listRaiinNo.Contains(item.RaiinNo))
                    .OrderBy(item => item.SinDate).ThenBy(item => item.RaiinNo);
            }
            else
            {
                syunoSeikyuRepo = NoTrackingDataContext.SyunoSeikyus
                    .Where(item =>
                        item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                        listRaiinNo.Contains(item.RaiinNo)).OrderBy(item => item.RaiinNo);
            }

            var raiinInfRepo = NoTrackingDataContext.RaiinInfs.Where(item =>
                item.HpId == hpId && item.PtId == ptId && item.Status > 3 && item.IsDeleted == 0);

            var querySyuno = from syunoSeikyu in syunoSeikyuRepo
                             join raiinInf in raiinInfRepo on
                                 new { syunoSeikyu.HpId, syunoSeikyu.PtId, syunoSeikyu.SinDate, syunoSeikyu.RaiinNo } equals
                                 new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
                             select new
                             {
                                 SyunoSeikyu = syunoSeikyu,
                                 RaiinInf = raiinInf,
                                 hokenPid = raiinInf.HokenPid
                             };

            var listHokenPid = querySyuno.Select(item => item.hokenPid).ToList();

            var listHokenPattern = NoTrackingDataContext.PtHokenPatterns
                .Where(pattern => pattern.HpId == hpId
                                            && pattern.PtId == ptId
                                            && pattern.IsDeleted == 0
                                            && listHokenPid.Contains(pattern.HokenPid));

            var syunoNyukinRepo = NoTrackingDataContext.SyunoNyukin.Where(item =>
                item.HpId == hpId && item.PtId == ptId && item.IsDeleted == 0);

            var kaikeiInfRepo = NoTrackingDataContext.KaikeiInfs.Where(item =>
                item.HpId == hpId && item.PtId == ptId);

            var query = from syuno in querySyuno
                        join syunoNyukin in syunoNyukinRepo on
                            new { syuno.SyunoSeikyu.HpId, syuno.SyunoSeikyu.PtId, syuno.SyunoSeikyu.SinDate, syuno.SyunoSeikyu.RaiinNo } equals
                            new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into
                            listSyunoNyukin
                        join kaikeiInf in kaikeiInfRepo on
                            new { syuno.SyunoSeikyu.HpId, syuno.SyunoSeikyu.PtId, syuno.SyunoSeikyu.SinDate, syuno.SyunoSeikyu.RaiinNo } equals
                            new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo } into
                            listKaikeInf
                        select ConvertToModel(syuno.SyunoSeikyu, syuno.RaiinInf, listSyunoNyukin.ToList(), listKaikeInf.ToList(), listHokenPattern.ToList());

            return query.ToList();

        }

        private SyunoSeikyuModel ConvertToModel(SyunoSeikyu syunoSeikyu, RaiinInf raiinInf, List<SyunoNyukin> syunoNyukin, List<KaikeiInf> kaikeiInf, List<PtHokenPattern> listHokenPattern)
        {
            return new SyunoSeikyuModel
                (
                             syunoSeikyu.HpId,
                             syunoSeikyu.PtId,
                             syunoSeikyu.SinDate,
                             syunoSeikyu.RaiinNo,
                             syunoSeikyu.NyukinKbn,
                             syunoSeikyu.SeikyuTensu,
                             syunoSeikyu.AdjustFutan,
                             syunoSeikyu.SeikyuGaku,
                             syunoSeikyu.SeikyuDetail ?? string.Empty,
                             syunoSeikyu.NewSeikyuTensu,
                             syunoSeikyu.NewAdjustFutan,
                             syunoSeikyu.NewSeikyuGaku,
                             syunoSeikyu.NewSeikyuDetail ?? string.Empty,

                        new SyunoRaiinInfModel(
                             raiinInf.Status,
                             raiinInf.KaikeiTime ?? string.Empty,
                             raiinInf.UketukeSbt
                            ),
                             syunoNyukin == null
                        ? new List<SyunoNyukinModel>()
                        : syunoNyukin.Select(y =>
                            new SyunoNyukinModel(
                            y.HpId,
                            y.PtId,
                            y.SinDate,
                            y.RaiinNo,
                            y.SeqNo,
                            y.SortNo,
                            y.AdjustFutan,
                            y.NyukinGaku,
                            y.PaymentMethodCd,
                            y.NyukinDate,
                            y.UketukeSbt,
                            y.NyukinCmt ?? string.Empty,
                            y.NyukinjiTensu,
                            y.NyukinjiSeikyu,
                            y.NyukinjiDetail ?? string.Empty
                            )
                            ).ToList(),

                       kaikeiInf.Select(x =>
                           new KaikeiInfModel(
                            x.HpId,
                            x.PtId,
                            x.SinDate,
                            x.RaiinNo,
                            x.HokenId,
                            x.Kohi1Id,
                            x.Kohi2Id,
                            x.Kohi3Id,
                            x.Kohi4Id,
                            x.HokenKbn,
                            x.HokenSbtCd,
                            x.ReceSbt ?? string.Empty,
                            x.Houbetu ?? string.Empty,
                            x.Kohi1Houbetu ?? string.Empty,
                            x.Kohi2Houbetu ?? string.Empty,
                            x.Kohi3Houbetu ?? string.Empty,
                            x.Kohi4Houbetu ?? string.Empty,
                            x.HonkeKbn,
                            x.HokenRate,
                            x.PtRate,
                            x.DispRate,
                            x.Tensu,
                            x.TotalIryohi,
                            x.PtFutan,
                            x.JihiFutan,
                            x.JihiTax,
                            x.JihiOuttax,
                            x.JihiFutanTaxfree,
                            x.JihiFutanTaxNr,
                            x.JihiFutanTaxGen,
                            x.JihiFutanOuttaxNr,
                            x.JihiFutanOuttaxGen,
                            x.JihiTaxNr,
                            x.JihiTaxGen,
                            x.JihiOuttaxNr,
                            x.JihiOuttaxGen,
                            x.AdjustFutan,
                            x.AdjustRound,
                            x.TotalPtFutan,
                            x.AdjustFutanVal,
                            x.AdjustFutanRange,
                            x.AdjustRateVal,
                            x.AdjustRateRange
                               )).ToList(),
                        listHokenPattern.FirstOrDefault(itemPattern => itemPattern.HokenPid == raiinInf.HokenPid)?.HokenId ?? 0
            );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<CalcLogModel> GetCalcLog(int hpId, long ptId, int sinDate, List<long> raiinNoList)
        {
            return NoTrackingDataContext.CalcLogs.Where(item =>
                    item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                    raiinNoList.Contains(item.RaiinNo)).OrderBy(item => item.UpdateDate)
                .Select(item => new CalcLogModel(item.RaiinNo, item.LogSbt, item.Text ?? string.Empty)).ToList();
        }

        public List<PtDiseaseModel> GetPtByoMeiList(int hpId, long ptId, int sinDate = 0)
        {
            int year = sinDate / 10000;
            int month = sinDate / 100 % 100;
            int firstDay = 1;
            int lastDay = DateTime.DaysInMonth(year, month);
            int firstDate = year * 10000 + month * 100 + firstDay;
            int lastDate = year * 10000 + month * 100 + lastDay;

            List<int> tenkiKbn = new List<int> { TenkiKbnConst.Cured, TenkiKbnConst.Dead, TenkiKbnConst.Canceled, TenkiKbnConst.InMonth, TenkiKbnConst.Other };

            var listPtByoMei = GetPtDiseaseModels(hpId, ptId, sinDate)
                                             .Where(u => (u.StartDate <= lastDate &&
                                                          u.IsNodspRece == 0 &&
                                                         (u.TenkiKbn == TenkiKbnConst.Continued ||
                                                          tenkiKbn.Contains(u.TenkiKbn) && u.TenkiDate >= firstDate)))
                                             .ToList();

            if (listPtByoMei == null || listPtByoMei.Count == 0)
                return new List<PtDiseaseModel>();

            return listPtByoMei.Select(data => new PtDiseaseModel(data.PtId, data.ByomeiCd, data.SeqNo, data.SortNo, data.SyubyoKbn, data.SikkanKbn, data.Byomei, data.StartDate, data.TenkiDate, data.HosokuCmt, data.TogetuByomei, new List<PrefixSuffixModel>()))
                .OrderBy(data => data.TenkiKbn)
                .ThenBy(data => data.SortNo)
                .ThenByDescending(data => data.StartDate)
                .ThenByDescending(data => data.TenkiDate)
                .ThenBy(data => data.SeqNo)
                .ToList();
        }

        private List<PtDiseaseModel> GetPtDiseaseModels(int hpId, long ptId, int sinDate)
        {
            List<PtByomei> ptByomeis;

            ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                   p.PtId == ptId &&
                                                                   p.IsDeleted != 1 &&
                                                                   (p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                   (p.StartDate <= sinDate && p.TenkiDate >= sinDate))).ToList();

            var PtDiseaseModels = ptByomeis.Select(p => new PtDiseaseModel(
                                                        p.PtId,
                                                        p.ByomeiCd ?? string.Empty,
                                                        p.SeqNo,
                                                        p.SortNo,
                                                        p.SyubyoKbn,
                                                        p.SikkanKbn,
                                                        p.Byomei ?? string.Empty,
                                                        p.StartDate,
                                                        p.TenkiDate,
                                                        p.HosokuCmt ?? string.Empty,
                                                        p.TogetuByomei,
                                                        SyusyokuCdToList(p)
                                                        ))
                                            .ToList();

            var byomeiMstQuery = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == 1)
                                                             .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.SikkanCd, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });
            var byomeiQueryNoTrack = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == 1 &&
                                                                              p.PtId == ptId &&
                                                                              p.IsDeleted != 1 &&
                                                                              (p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                              (p.StartDate <= sinDate && p.TenkiDate >= sinDate)));

            var byomeiMstList = (from ptByomei in byomeiQueryNoTrack
                                 join ptByomeiMst in byomeiMstQuery on new { ptByomei.HpId, ptByomei.ByomeiCd } equals new { ptByomeiMst.HpId, ptByomeiMst.ByomeiCd }
                                 select ptByomeiMst).ToList();

            foreach (var PtDiseaseModel in PtDiseaseModels)
            {

                if (PtDiseaseModel.IsFreeWord)
                {
                    PtDiseaseModel.Byomei = PtDiseaseModel.FullByomei;
                    continue;
                }

                var byomeiMst = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == PtDiseaseModel.ByomeiCd);
                if (byomeiMst != null)
                {
                    PtDiseaseModel.Byomei = byomeiMst.Sbyomei;
                    PtDiseaseModel.Icd10 = byomeiMst.Icd101;
                    PtDiseaseModel.ChangeSikkanCd(byomeiMst.SikkanCd);
                    if (!string.IsNullOrEmpty(byomeiMst.Icd102))
                    {
                        PtDiseaseModel.Icd10 += "/" + byomeiMst.Icd102;
                    }
                    PtDiseaseModel.Icd102013 = byomeiMst.Icd1012013;
                    if (!string.IsNullOrEmpty(byomeiMst.Icd1022013))
                    {
                        PtDiseaseModel.Icd102013 += "/" + byomeiMst.Icd1022013;
                    }

                    PtDiseaseModel.Icd1012013 = byomeiMst.Icd1012013;
                    PtDiseaseModel.Icd1022013 = byomeiMst.Icd1022013;
                }
                else
                {
                    PtDiseaseModel.Icd1012013 = string.Empty;
                    PtDiseaseModel.Icd1022013 = string.Empty;
                }
            }

            return PtDiseaseModels;
        }

        private List<PrefixSuffixModel> SyusyokuCdToList(PtByomei ptByomei)
        {
            List<string> codeList = new()
            {
                ptByomei.SyusyokuCd1 ?? string.Empty,
                ptByomei.SyusyokuCd2 ?? string.Empty,
                ptByomei.SyusyokuCd3 ?? string.Empty,
                ptByomei.SyusyokuCd4 ?? string.Empty,
                ptByomei.SyusyokuCd5 ?? string.Empty,
                ptByomei.SyusyokuCd6 ?? string.Empty,
                ptByomei.SyusyokuCd7 ?? string.Empty,
                ptByomei.SyusyokuCd8 ?? string.Empty,
                ptByomei.SyusyokuCd9 ?? string.Empty,
                ptByomei.SyusyokuCd10 ?? string.Empty,
                ptByomei.SyusyokuCd11 ?? string.Empty,
                ptByomei.SyusyokuCd12 ?? string.Empty,
                ptByomei.SyusyokuCd13 ?? string.Empty,
                ptByomei.SyusyokuCd14 ?? string.Empty,
                ptByomei.SyusyokuCd15 ?? string.Empty,
                ptByomei.SyusyokuCd16 ?? string.Empty,
                ptByomei.SyusyokuCd17 ?? string.Empty,
                ptByomei.SyusyokuCd18 ?? string.Empty,
                ptByomei.SyusyokuCd19 ?? string.Empty,
                ptByomei.SyusyokuCd20 ?? string.Empty,
                ptByomei.SyusyokuCd21 ?? string.Empty
            };
            codeList = codeList.Where(c => c != string.Empty).ToList();

            if (codeList.Count == 0)
            {
                return new List<PrefixSuffixModel>();
            }

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => codeList.Contains(b.ByomeiCd)).ToList();

            List<PrefixSuffixModel> result = new();
            foreach (var code in codeList)
            {
                var byomeiMst = byomeiMstList.FirstOrDefault(b => b.ByomeiCd == code);
                if (byomeiMst == null)
                {
                    continue;
                }
                result.Add(new PrefixSuffixModel(code, byomeiMst.Byomei ?? string.Empty));
            }

            return result;
        }

        public List<PaymentMethodMstModel> GetListPaymentMethodMst(int hpId)
        {
            return NoTrackingDataContext.PaymentMethodMsts
                .Where(item => item.HpId == hpId && item.IsDeleted == 0)
                .OrderBy(item => item.SortNo)
                .Select(item => new PaymentMethodMstModel(
                    item.PaymentMethodCd,
                    item.PayName ?? string.Empty,
                    item.PaySname ?? string.Empty,
                    item.SortNo,
                    item.IsDeleted)).ToList();
        }

        public List<KohiInfModel> GetListKohiByKohiId(int hpId, long ptId, int sinDate, List<int> kohiIds)
        {
            kohiIds = kohiIds.Distinct().ToList();

            var hospitalInfo = NoTrackingDataContext.HpInfs
                .Where(p => p.HpId == hpId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;

            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            var listPtKohi = NoTrackingDataContext.PtKohis
                  .Where(kohi => kohi.HpId == hpId && kohi.PtId == ptId && kohi.IsDeleted == 0 && kohiIds.Contains(kohi.HokenId))
                  .ToList();

            if (!listPtKohi.Any()) return new List<KohiInfModel>();

            var predicateHokenMst = CreateHokenMstExpression(listPtKohi);

            if (predicateHokenMst == null) return new List<KohiInfModel>();

            var hokenMstListRepo = NoTrackingDataContext.HokenMsts
                .Where(
                    entity => entity.HpId == hpId
                              && (entity.PrefNo == prefCd
                              || entity.PrefNo == 0
                              || entity.IsOtherPrefValid == 1))
                .OrderBy(e => e.HpId)
                .ThenBy(e => e.HokenNo)
                .ThenBy(e => e.HokenEdaNo)
                .ThenByDescending(e => e.StartDate)
                .ThenBy(e => e.HokenSbtKbn)
                .ThenBy(e => e.SortNo);

            var hokenMstList = hokenMstListRepo.Where(predicateHokenMst).ToList();

            var predicatePtHokenCheck = CreatePtHokenCheckExpression(listPtKohi);

            if (predicatePtHokenCheck == null) return new List<KohiInfModel>();

            var ptHokenCheckRepos = NoTrackingDataContext.PtHokenChecks.Where(item =>
                item.HpId == hpId && item.PtID == ptId && item.IsDeleted == 0);

            var ptHokenCheckList = ptHokenCheckRepos.Where(predicatePtHokenCheck).ToList();

            return listPtKohi.Select(kohi => CreatePtKohiModel(kohi,
                                    hokenMstList.Where(item =>
                                        item.HokenNo == kohi.HokenNo &&
                                        item.HokenEdaNo == kohi.HokenEdaNo).ToList(),
                                    ptHokenCheckList.Where(item =>
                                        item.HokenGrp == 2 &&
                                        item.HokenId == kohi.HokenId)
                                       .Select(item => new ConfirmDateModel(
                                       item.HokenGrp, item.HokenId, item.CheckDate, item.CheckId, item.CheckMachine ?? string.Empty, item.CheckCmt ?? string.Empty, item.IsDeleted)).ToList(), sinDate))
                .ToList();
        }

        private Expression<Func<PtHokenCheck, bool>> CreatePtHokenCheckExpression(List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(PtHokenCheck));
            Expression expression = null;

            var listKohiId = listPtKohi.Select(item => item.HokenId).ToList();

            CreatePtHokenCheckExpression(listKohiId, 2, ref expression, ref param);

            return expression != null
                    ? Expression.Lambda<Func<PtHokenCheck, bool>>(body: expression, parameters: param)
                    : null;
        }

        private Expression<Func<HokenMst, bool>> CreateHokenMstExpression(List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(HokenMst));
            Expression expression = null;

            CreateHokenMstExpression(listPtKohi, ref expression, ref param);

            return expression != null
                ? Expression.Lambda<Func<HokenMst, bool>>(body: expression, parameters: param)
                : null;
        }

        public bool SaveAccounting(List<SyunoSeikyuModel> listAllSyunoSeikyu, List<SyunoSeikyuModel> syunoSeikyuModels, int hpId, long ptId, int userId, int accDue, int sumAdjust, int thisWari, int thisCredit,
                                   int payType, string comment, bool isDisCharged)
        {

            var raiinNos = syunoSeikyuModels.Select(item => item.RaiinNo).Distinct().ToList();
            var raiinInLists = TrackingDataContext.RaiinInfs
                                    .Where(item => item.HpId == hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == DeleteTypes.None
                                                   && item.Status > RaiinState.TempSave
                                                   && raiinNos.Contains(item.RaiinNo))
                                    .ToList();
            var seikyuLists = TrackingDataContext.SyunoSeikyus
                        .Where(item => item.HpId == hpId
                                       && item.PtId == ptId
                                       && raiinNos.Contains(item.RaiinNo))
                        .ToList();

            int allSeikyuGaku = sumAdjust;
            int adjustFutan = thisWari;
            int nyukinGaku = thisCredit;
            int outAdjustFutan = 0;
            int outNyukinGaku = 0;
            int outNyukinKbn = 0;

            for (int i = 0; i < syunoSeikyuModels.Count; i++)
            {
                var item = syunoSeikyuModels[i];

                int thisSeikyuGaku = 0;
                if (item.SyunoNyukinModels.Any())
                {
                    thisSeikyuGaku = item.SeikyuGaku - item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku) -
                                     item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan);

                }
                bool isLastRecord = i == syunoSeikyuModels.Count - 1;
                if (!isDisCharged)
                {
                    ParseValueUpdate(allSeikyuGaku, thisSeikyuGaku, ref adjustFutan, ref nyukinGaku, out outAdjustFutan, out outNyukinGaku,
                    out outNyukinKbn, isLastRecord);
                    allSeikyuGaku -= thisSeikyuGaku;
                }
                else
                {
                    outNyukinKbn = 2;
                }

                if (item.SyunoNyukinModels.Count != 1 || item.SyunoNyukinModels[0].AdjustFutan != 0 ||
                    item.SyunoNyukinModels[0].NyukinGaku != 0)
                {
                    TrackingDataContext.SyunoNyukin.Add(new SyunoNyukin()
                    {
                        HpId = item.HpId,
                        RaiinNo = item.RaiinNo,
                        PtId = item.PtId,
                        SinDate = item.SinDate,
                        AdjustFutan = outAdjustFutan,
                        NyukinGaku = outNyukinGaku,
                        SortNo = 1,
                        PaymentMethodCd = payType,
                        UketukeSbt = item.RaiinInfModel.UketukeSbt,
                        NyukinCmt = comment,
                        IsDeleted = 0,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        NyukinDate = item.SinDate,
                        NyukinjiTensu = item.SeikyuTensu,
                        NyukinjiDetail = item.SeikyuDetail,
                        NyukinjiSeikyu = item.SeikyuGaku
                    });

                    UpdateStatusRaiinInf(userId, item, raiinInLists);
                    UpdateStatusSyunoSeikyu(userId, item.RaiinNo, outNyukinKbn, seikyuLists);
                }
                else
                {
                    var firstSyunoNyukinModel = item.SyunoNyukinModels?.FirstOrDefault() ?? new();

                    var syuno = TrackingDataContext.SyunoNyukin.FirstOrDefault(x =>
                        x.HpId == (firstSyunoNyukinModel.HpId) &&
                        x.PtId == (firstSyunoNyukinModel.PtId) &&
                        x.RaiinNo == (firstSyunoNyukinModel.RaiinNo) &&
                        x.SortNo == (firstSyunoNyukinModel.SortNo) &&
                        x.SeqNo == (firstSyunoNyukinModel.SeqNo)
                    ) ?? new();

                    syuno.AdjustFutan = outAdjustFutan;
                    syuno.NyukinGaku = outNyukinGaku;
                    syuno.PaymentMethodCd = payType;
                    syuno.UketukeSbt = item.RaiinInfModel.UketukeSbt;
                    syuno.NyukinCmt = comment;
                    syuno.NyukinjiTensu = item.SeikyuTensu;
                    syuno.NyukinjiDetail = item.SeikyuDetail;
                    syuno.NyukinjiSeikyu = item.SeikyuGaku;
                    syuno.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    syuno.UpdateId = userId;
                    syuno.NyukinDate = item.SinDate;

                    UpdateStatusRaiinInf(userId, item, raiinInLists);
                    UpdateStatusSyunoSeikyu(userId, item.RaiinNo, outNyukinKbn, seikyuLists);
                }

            }
            if (accDue != 0 && thisCredit != 0)
            {
                AdjustWariExecute(hpId, ptId, userId, thisCredit, accDue, listAllSyunoSeikyu, syunoSeikyuModels, payType, comment);
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        private void AdjustWariExecute(int hpId, long ptId, int userId, int nyukinGakuEarmarked, int accDue, List<SyunoSeikyuModel> listAllSyunoSeikyu, List<SyunoSeikyuModel> syunoSeikyuModels, int paymentMethod, string comment)
        {
            var listRaiinNo = syunoSeikyuModels.Select(item => item.RaiinNo).ToList();

            var seikyuLists = TrackingDataContext.SyunoSeikyus
                            .Where(item => item.HpId == hpId
                                                && item.PtId == ptId
                                                && listRaiinNo.Contains(item.RaiinNo))
                            .ToList();

            int nyukinGaku = nyukinGakuEarmarked;
            int outAdjustFutan = 0;
            int outNyukinGaku = 0;
            int outNyukinKbn = 0;
            bool isSettled = nyukinGakuEarmarked == accDue;

            listAllSyunoSeikyu = listAllSyunoSeikyu
                .Where(item => !(item.SeikyuGaku != item.NewSeikyuGaku && item.NewSeikyuGaku > 0))
                .ToList();

            foreach (var item in listAllSyunoSeikyu)
            {
                if (nyukinGaku == 0) return;

                int thisSeikyuGaku = 0;
                int sort = 0;
                if (item.SyunoNyukinModels.Any())
                {
                    thisSeikyuGaku = item.SeikyuGaku - item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku) -
                                     item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan);
                    sort = item.SyunoNyukinModels.Max(itemNyukin => itemNyukin.SortNo) + 1;
                }

                ParseEarmarkedValueUpdate(thisSeikyuGaku, ref nyukinGaku, out outNyukinGaku,
                    out outNyukinKbn, isSettled);

                TrackingDataContext.SyunoNyukin.Add(new SyunoNyukin()
                {
                    HpId = item.HpId,
                    RaiinNo = item.RaiinNo,
                    PtId = item.PtId,
                    SinDate = item.SinDate,
                    SortNo = sort,
                    AdjustFutan = outAdjustFutan,
                    NyukinGaku = outNyukinGaku,
                    PaymentMethodCd = paymentMethod,
                    UketukeSbt = item.RaiinInfModel.UketukeSbt,
                    NyukinCmt = comment,
                    IsDeleted = 0,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    NyukinjiTensu = item.SeikyuTensu,
                    NyukinjiDetail = item.SeikyuDetail,
                    NyukinjiSeikyu = item.SeikyuGaku
                });

                UpdateStatusSyunoSeikyu(userId, item.RaiinNo, outNyukinKbn, seikyuLists);
            }
        }

        private void ParseEarmarkedValueUpdate(int thisSeikyuGaku, ref int nyukinGaku, out int outNyukinGaku,
            out int outNyukinKbn, bool isSettled = false)
        {
            if (isSettled)
            {
                outNyukinGaku = thisSeikyuGaku;
                nyukinGaku -= outNyukinGaku;
                outNyukinKbn = 3;
                return;
            }

            if (nyukinGaku >= thisSeikyuGaku)
            {
                outNyukinGaku = thisSeikyuGaku;
                nyukinGaku -= outNyukinGaku;
            }
            else
            {
                outNyukinGaku = nyukinGaku;
                nyukinGaku -= outNyukinGaku;
            }

            thisSeikyuGaku -= outNyukinGaku;
            outNyukinKbn = thisSeikyuGaku == 0 ? 3 : 1;
        }

        private void UpdateStatusRaiinInf(int userId, SyunoSeikyuModel syunoSeikyu, List<RaiinInf> raiinLists)
        {
            var raiin = raiinLists.FirstOrDefault(item => item.RaiinNo == syunoSeikyu.RaiinNo);

            if (raiin != null)
            {
                raiin.Status = RaiinState.Settled;
                raiin.KaikeiTime = CIUtil.DateTimeToTime(CIUtil.GetJapanDateTimeNow());
                raiin.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiin.UpdateId = userId;
            }
        }

        private void UpdateStatusSyunoSeikyu(int userId, long raiinNo, int nyuKinKbn, List<SyunoSeikyu> seikyuLists)
        {
            var seikyu = seikyuLists.FirstOrDefault(item => item.RaiinNo == raiinNo);
            if (seikyu != null)
            {
                seikyu.NyukinKbn = nyuKinKbn;
                seikyu.UpdateDate = CIUtil.GetJapanDateTimeNow();
                seikyu.UpdateId = userId;
            }
        }

        private void ParseValueUpdate(int allSeikyuGaku, int thisSeikyuGaku, ref int adjustFutan, ref int nyukinGaku, out int outAdjustFutan,
            out int outNyukinGaku, out int outNyukinKbn, bool isLastRecord)
        {
            int credit = adjustFutan + nyukinGaku;

            if (credit == allSeikyuGaku || credit < allSeikyuGaku && credit > thisSeikyuGaku)
            {
                if (isLastRecord)
                {
                    outAdjustFutan = adjustFutan;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
                else if (adjustFutan >= thisSeikyuGaku)
                {
                    outAdjustFutan = thisSeikyuGaku;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
                else
                {
                    outAdjustFutan = adjustFutan;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
            }
            else
            {
                outAdjustFutan = adjustFutan;
                outNyukinGaku = nyukinGaku;

                adjustFutan -= outAdjustFutan;
                nyukinGaku -= outNyukinGaku;
            }

            thisSeikyuGaku = thisSeikyuGaku - outAdjustFutan - outNyukinGaku;
            outNyukinKbn = thisSeikyuGaku == 0 ? 3 : 1;
        }

        public bool CheckRaiinInfExist(int hpId, long ptId, long raiinNo)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item =>
                item.HpId == hpId &&
                item.PtId == ptId &&
                item.RaiinNo == raiinNo &&
                item.IsDeleted == DeleteTypes.None);

            return raiinInf != null;
        }

        public List<long> GetRaiinNos(int hpId, long ptId, long raiinNo)
        {
            var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(x =>
                                                                x.HpId == hpId &&
                                                                x.PtId == ptId &&
                                                                x.RaiinNo == raiinNo &&
                                                                x.Status > RaiinState.TempSave &&
                                                                x.IsDeleted == DeleteTypes.None);
            if (oyaRaiinNo == null) return new();

            return NoTrackingDataContext.RaiinInfs.Where(x =>
                                                                x.HpId == hpId &&
                                                                x.PtId == ptId &&
                                                                x.OyaRaiinNo == oyaRaiinNo.OyaRaiinNo &&
                                                                x.IsDeleted == DeleteTypes.None
                                                                ).Select(x => x.RaiinNo).ToList();
        }
        public List<JihiSbtMstModel> GetListJihiSbtMst(int hpId)
        {
            return NoTrackingDataContext.JihiSbtMsts
                .Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None)
                .OrderBy(item => item.SortNo)
                .Select(item => new JihiSbtMstModel(
                                                    item.HpId,
                                                    item.JihiSbt,
                                                    item.SortNo,
                                                    item.Name ?? string.Empty,
                                                    item.IsDeleted))
                .ToList();
        }

        public int GetJihiOuttaxPoint(int hpId, long ptId, List<long> raiinNos)
        {
            var kaikeis = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId && item.PtId == ptId && raiinNos.Contains(item.RaiinNo));

            return kaikeis?.Sum(item => item.JihiOuttax) ?? 0;
        }

        public void CheckOrdInfInOutDrug(int hpId, long ptId, List<long> raiinNos, out bool inDrugExist, out bool outDrugExist)
        {

            inDrugExist = false;
            outDrugExist = false;
            var odrInfList = NoTrackingDataContext.OdrInfs.Where(item => raiinNos.Contains(item.RaiinNo)
                                                                                && item.PtId == ptId
                                                                                && item.IsDeleted == 0
                                                                                && item.OdrKouiKbn >= 20 && item.OdrKouiKbn <= 29
                                                                                && item.HpId == hpId)
                                                        .Select(item => new { item.InoutKbn })
                                                        .ToList();

            if (odrInfList != null && odrInfList.FirstOrDefault(item => item.InoutKbn == 0) != null)
            {
                inDrugExist = true;
            }
            if (odrInfList != null && odrInfList.FirstOrDefault(item => item.InoutKbn == 1) != null)
            {
                outDrugExist = true;
            }
        }

        public byte CheckIsOpenAccounting(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var checkStatusRaiinNo = NoTrackingDataContext.RaiinInfs.Any(x => x.HpId == hpId && x.PtId == ptId && x.RaiinNo == raiinNo && x.Status >= RaiinState.TempSave);

            if (!checkStatusRaiinNo) return CIUtil.NoPaymentInfo;

            int numberCheck = 0;

            var isCompletedCalculation = CheckCompletedCalculation(hpId, ptId, sinDate);

            while (numberCheck < 50 && (isCompletedCalculation == CIUtil.NoPaymentInfo))
            {
                Thread.Sleep(100);
                numberCheck++;
                isCompletedCalculation = CheckCompletedCalculation(hpId, ptId, sinDate);
            }

            return isCompletedCalculation;
        }

        public byte CheckCompletedCalculation(int hpId, long ptId, int sinDate, int calcMode = 0)
        {
            List<CalcStatus> calcStatuses = NoTrackingDataContext.CalcStatus.Where(item =>
                    item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                    item.CalcMode == calcMode).ToList();

            if (!calcStatuses.Any()) return CIUtil.Successed;

            DateTime maxTime = calcStatuses.Select(c => c.CreateDate).DefaultIfEmpty(DateTime.MinValue).Max();

            if (maxTime == DateTime.MinValue)
                return CIUtil.TryAgainLater;

            var listStatus = calcStatuses.Where(item => item.CreateDate == maxTime).ToList();

            if (!listStatus.Any())
                return CIUtil.NoPaymentInfo;

            foreach (var item in listStatus)
            {
                if (item.Status != 8 && item.Status != 9)
                    return CIUtil.NoPaymentInfo;
            }

            return CIUtil.Successed;
        }

        public bool CheckSyunoStatus(int hpId, long raiinNo, long ptId)
        {
            return NoTrackingDataContext.SyunoSeikyus.Any(x =>
                                                            x.HpId == hpId &&
                                                            x.PtId == ptId &&
                                                            x.RaiinNo == raiinNo &&
                                                            x.NyukinKbn <= 0);
        }
    }
}
