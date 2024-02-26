using Domain.Constant;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.Reception;
using Domain.Models.ReceptionSameVisit;
using Helper.Extension;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;
        public AccountingRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            key = GetCacheKey() + CacheKeyConstant.PaymentMethodMsts;
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
        public List<ReceptionDto> GetListRaiinInf(int hpId, long ptId, int sinDate, long raiinNo, bool isGetHeader = false, bool getAll = true)
        {
            List<RaiinInf> listRaiinInf;
            if (getAll)
            {
                var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item => item.RaiinNo == raiinNo && item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted == 0);

                if (oyaRaiinNo == null)
                {
                    return new();
                }

                listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(
                  item => item.OyaRaiinNo == oyaRaiinNo.OyaRaiinNo && item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate && item.IsDeleted == 0 && item.Status > RaiinState.TempSave).ToList();
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
                        countAcc = countAcc - listRaiinInf.Count;
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

        private List<int> CreatePtKohi(List<PtHokenPattern> listPtHokenPattern)
        {
            var kohis = new List<int>();
            if (listPtHokenPattern != null && listPtHokenPattern.Count > 0)
            {
                foreach (var pattern in listPtHokenPattern)
                {
                    if (pattern.PtId > 0)
                    {
                        kohis.AddRange(new[]
                        {
                            pattern.Kohi1Id,
                            pattern.Kohi2Id,
                            pattern.Kohi3Id,
                            pattern.Kohi4Id
                        });
                    }
                }
            }

            return kohis.Where(x => x > 0).Distinct().ToList();
        }

        private void CreatePtHokenCheckExpression(List<int> listHokenId, int hokenGrp, ref Expression? expression, ref ParameterExpression param)
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

        private Expression<Func<PtHokenCheck, bool>>? CreatePtHokenCheckExpression(List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(PtHokenCheck));
            Expression? expression = null;

            var listKohiId = listPtKohi.Select(item => item.HokenId).ToList();

            CreatePtHokenCheckExpression(listKohiId, 2, ref expression, ref param);

            return expression != null
                    ? Expression.Lambda<Func<PtHokenCheck, bool>>(body: expression, parameters: param)
                    : null;
        }

        private Expression<Func<HokenMst, bool>>? CreateHokenMstExpression(List<PtHokenInf> listPtHokenInf, List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(HokenMst));
            Expression? expression = null;

            CreateHokenMstExpression(listPtHokenInf, ref expression, ref param);
            CreateHokenMstExpression(listPtKohi, ref expression, ref param);

            return expression != null
                ? Expression.Lambda<Func<HokenMst, bool>>(body: expression, parameters: param)
                : null;
        }

        private void CreateHokenMstExpression(List<PtHokenInf> listPtHokenInf, ref Expression? expression, ref ParameterExpression param)
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

        private void CreateHokenMstExpression(List<PtKohi> listPtKohi, ref Expression? expression, ref ParameterExpression param)
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

        private Expression<Func<HokenMst, bool>>? CreateHokenMstExpression(List<PtKohi> listPtKohi)
        {
            var param = Expression.Parameter(typeof(HokenMst));
            Expression? expression = null;

            CreateHokenMstExpression(listPtKohi, ref expression, ref param);

            return expression != null
                ? Expression.Lambda<Func<HokenMst, bool>>(body: expression, parameters: param)
                : null;
        }

        private HokenInfModel CreateHokenInfModel(PtHokenInf ePtHokenInf, List<ConfirmDateModel> ConfirmDateModelList, int sinDay)
        {
            HokenInfModel hokenInfModel = new();
            if (ePtHokenInf != null)
            {
                hokenInfModel = new HokenInfModel(ePtHokenInf.HpId, ePtHokenInf.PtId, ePtHokenInf.HokenId, ePtHokenInf.HokenKbn, ePtHokenInf.Houbetu ?? string.Empty, ePtHokenInf.StartDate, ePtHokenInf.EndDate, sinDay, new(), ConfirmDateModelList.Select(p => new ConfirmDateModel(p.HokenGrp, p.HokenId, p.SeqNo, p.CheckId, p.CheckMachine, p.CheckComment, p.ConfirmDate)).ToList());
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
                kohiInfModel = new KohiInfModel(eKohiInf.HokenId, eKohiInf.PrefNo, eKohiInf.HokenNo, eKohiInf.HokenEdaNo, eKohiInf.FutansyaNo ?? string.Empty, eKohiInf.StartDate, eKohiInf.EndDate, sinDay, hokenMstModel, ConfirmDateModelList.Select(p => new ConfirmDateModel(p.HokenGrp, p.HokenId, p.SeqNo, p.CheckId, p.CheckMachine, p.CheckComment, p.ConfirmDate)).ToList());
            }

            return kohiInfModel;
        }

        public List<SyunoSeikyuModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> listRaiinNo, bool getAll = false)
        {
            IEnumerable<SyunoSeikyu> syunoSeikyuRepo;

            if (getAll)
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

            var listPtByoMei = GetPtDiseaseModels(hpId, ptId)
                                             .Where(u => (u.StartDate <= lastDate &&
                                                          u.IsNodspRece == 0 &&
                                                         (u.TenkiKbn == TenkiKbnConst.Continued ||
                                                          tenkiKbn.Contains(u.TenkiKbn) && u.TenkiDate >= firstDate)))
                                             .ToList();

            if (listPtByoMei == null || listPtByoMei.Count == 0)
                return new List<PtDiseaseModel>();

            return listPtByoMei.Select(data => new PtDiseaseModel(data.PtId, data.ByomeiCd, data.SeqNo, data.SortNo, data.SyubyoKbn, data.SikkanKbn, data.Byomei, data.StartDate, data.TenkiDate, data.HosokuCmt, data.TogetuByomei, data.HokenId, new List<PrefixSuffixModel>(), data.TenkiKbn))
                .OrderBy(data => data.TenkiKbn)
                .ThenBy(data => data.SortNo)
                .ThenByDescending(data => data.StartDate)
                .ThenByDescending(data => data.TenkiDate)
                .ThenBy(data => data.SeqNo)
                .ToList();
        }

        private List<PtDiseaseModel> GetPtDiseaseModels(int hpId, long ptId)
        {
            List<PtByomei> ptByomeis;

            ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                   p.PtId == ptId &&
                                                                   p.IsDeleted != 1).ToList();

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
                                                        p.IsNodspRece,
                                                        p.TenkiKbn,
                                                        p.HokenPid,
                                                        SyusyokuCdToList(hpId, p)
                                                        ))
                                            .ToList();

            var byomeiMstQuery = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId)
                                                             .Select(item => new { item.HpId, item.ByomeiCd, item.Sbyomei, item.SikkanCd, item.Icd101, item.Icd102, item.Icd1012013, item.Icd1022013 });
            var byomeiQueryNoTrack = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                              p.PtId == ptId &&
                                                                              p.IsDeleted != 1
                                                                              );

            var byomeiMstList = (from ptByomei in byomeiQueryNoTrack
                                 join ptByomeiMst in byomeiMstQuery on new { ptByomei.HpId, ptByomei.ByomeiCd } equals new { ptByomeiMst.HpId, ptByomeiMst.ByomeiCd }
                                 select ptByomeiMst).ToList();

            foreach (var PtDiseaseModel in PtDiseaseModels)
            {
                StringBuilder icd10 = new();
                StringBuilder icd102013 = new();

                if (PtDiseaseModel.IsFreeWord)
                {
                    PtDiseaseModel.Byomei = PtDiseaseModel.FullByomei;
                    continue;
                }

                var byomeiMst = byomeiMstList.FirstOrDefault(item => item.ByomeiCd == PtDiseaseModel.ByomeiCd);
                if (byomeiMst != null)
                {
                    PtDiseaseModel.Byomei = byomeiMst.Sbyomei;
                    icd10.Append(byomeiMst.Icd101);
                    PtDiseaseModel.ChangeSikkanCd(byomeiMst.SikkanCd);
                    if (!string.IsNullOrEmpty(byomeiMst.Icd102))
                    {
                        icd10.Append("/");
                        icd10.Append(byomeiMst.Icd102);
                    }
                    icd102013.Append(byomeiMst.Icd1012013);
                    if (!string.IsNullOrEmpty(byomeiMst.Icd1022013))
                    {
                        icd102013.Append("/");
                        icd102013.Append(byomeiMst.Icd1022013);
                    }

                    PtDiseaseModel.Icd1012013 = byomeiMst.Icd1012013;
                    PtDiseaseModel.Icd1022013 = byomeiMst.Icd1022013;
                }
                else
                {
                    PtDiseaseModel.Icd1012013 = string.Empty;
                    PtDiseaseModel.Icd1022013 = string.Empty;
                }
                PtDiseaseModel.Icd10 = icd10.ToString();
                PtDiseaseModel.Icd102013 = icd102013.ToString();
            }

            return PtDiseaseModels;
        }

        private List<PrefixSuffixModel> SyusyokuCdToList(int hpId, PtByomei ptByomei)
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
            codeList = codeList.Where(c => c != string.Empty).Distinct().ToList();

            if (codeList.Count == 0)
            {
                return new List<PrefixSuffixModel>();
            }

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId && codeList.Contains(b.ByomeiCd)).ToList();

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
            var finalKey = key;
            IEnumerable<PaymentMethodMst> paymentMethodList;
            if (!_cache.KeyExists(finalKey))
            {
                paymentMethodList = ReloadCache();
            }
            else
            {
                paymentMethodList = ReadCache();
            }
            return paymentMethodList
                .Where(item => item.HpId == hpId)
                .OrderBy(item => item.SortNo)
                .Select(item => new PaymentMethodMstModel(
                    item.PaymentMethodCd,
                    item.PayName ?? string.Empty,
                    item.PaySname ?? string.Empty,
                    item.SortNo,
                item.IsDeleted)).ToList();
        }

        #region [cache GetListPaymentMethodMst]
        private IEnumerable<PaymentMethodMst> ReloadCache()
        {
            var finalKey = key;
            var paymentMethodList =
                    NoTrackingDataContext.PaymentMethodMsts
                    .Where(s => s.IsDeleted == 0)
                    .ToList();
            var json = JsonSerializer.Serialize(paymentMethodList);
            _cache.StringSet(finalKey, json);

            return paymentMethodList;
        }

        private IEnumerable<PaymentMethodMst> ReadCache()
        {
            var finalKey = key;
            var results = _cache.StringGet(finalKey);
            var json = results.AsString();
            var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<PaymentMethodMst>>(json) : new();
            return datas ?? new();
        }

        #endregion [cache GetListPaymentMethodMst]

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

        public bool SaveAccounting(List<SyunoSeikyuModel> listAllSyunoSeikyu, List<SyunoSeikyuModel> syunoSeikyuModels, int hpId, long ptId, int userId, int accDue, int sumAdjust, int thisWari, int thisCredit,
                                   int payType, string comment, bool isDisCharged, string kaikeiTime)
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
            thisCredit = (isDisCharged && accDue == 1) ? 0 : thisCredit;
            int allSeikyuGaku = sumAdjust;
            int adjustFutan = thisWari;
            int nyukinGaku = thisCredit;
            int outAdjustFutan = 0;
            int outNyukinGaku = 0;
            int outNyukinKbn = 0;

            for (int i = 0; i < syunoSeikyuModels.Count; i++)
            {
                var item = syunoSeikyuModels[i];

                int thisSeikyuGaku = item.SeikyuGaku - (item.SyunoNyukinModels.Count == 0 ? 0 : item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku)) -
                                 (item.SyunoNyukinModels.Count == 0 ? 0 : item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan));

                if (!isDisCharged)
                {
                    ParseValueUpdate(allSeikyuGaku, thisSeikyuGaku, ref adjustFutan, ref nyukinGaku, out outAdjustFutan, out outNyukinGaku,
                                     out outNyukinKbn);
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

                    UpdateStatusRaiinInf(userId, item, raiinInLists, kaikeiTime);
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

                    UpdateStatusRaiinInf(userId, item, raiinInLists, kaikeiTime);
                    UpdateStatusSyunoSeikyu(userId, item.RaiinNo, outNyukinKbn, seikyuLists);
                }

            }
            if (accDue != 0 && (thisCredit != 0 && isDisCharged) || (nyukinGaku != 0 && !isDisCharged))
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
                    NyukinjiSeikyu = item.SeikyuGaku,
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

        private void UpdateStatusRaiinInf(int userId, SyunoSeikyuModel syunoSeikyu, List<RaiinInf> raiinLists, string kaikeiTime)
        {
            var raiin = raiinLists.FirstOrDefault(item => item.RaiinNo == syunoSeikyu.RaiinNo);

            if (raiin != null)
            {
                raiin.Status = RaiinState.Settled;
                raiin.KaikeiTime = kaikeiTime;
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
            out int outNyukinGaku, out int outNyukinKbn)
        {
            int credit = adjustFutan + nyukinGaku;

            if (credit == allSeikyuGaku || credit < allSeikyuGaku && credit > thisSeikyuGaku)
            {
                if (adjustFutan >= thisSeikyuGaku)
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

        public List<long> GetRaiinNos(int hpId, long ptId, long raiinNo, bool getAll = true)
        {
            if (getAll)
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
            return NoTrackingDataContext.RaiinInfs.Where(x =>
                                                                x.HpId == hpId &&
                                                                x.PtId == ptId &&
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
            {
                return CIUtil.NoPaymentInfo;
            }

            if (listStatus.Any(item => item.Status != 8 && item.Status != 9))
            {
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

        public ReceptionDto GetRaiinInfModel(int hpId, long ptId, int sinDate, long raiinNo, List<KaikeiInfModel> kaikeis)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item =>
                                                                          item.SinDate == sinDate &&
                                                                          item.RaiinNo == raiinNo &&
                                                                          item.PtId == ptId &&
                                                                          item.HpId == hpId &&
                                                                          item.IsDeleted == DeleteTypes.None);
            if (raiinInf == null)
                return new();
            var kaMst = NoTrackingDataContext.KaMsts.FirstOrDefault(item =>
                item.HpId == hpId && item.KaId == raiinInf.KaId && item.IsDeleted == 0);

            var ptHokenPattern =
                FindPtHokenPatternById(hpId, ptId, sinDate, raiinInf.HokenPid, raiinNo);
            return new ReceptionDto(raiinNo,
                                    raiinInf.UketukeNo,
                                    kaMst?.KaSname ?? string.Empty,
                                    0,
                                    ptHokenPattern,
                                    kaikeis
                                    );

        }

        public HokenPatternModel FindPtHokenPatternById(int hpId, long ptId, int sinDay, int patternId = 0, long raiinNo = 0, bool isGetDeleted = false)
        {
            var hokenPattern = new HokenPatternModel();

            var hospitalInfo = NoTrackingDataContext.HpInfs
                .Where(p => p.HpId == hpId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;

            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            // PtInf
            var ptInf = NoTrackingDataContext.PtInfs
                .FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == 0);

            if (ptInf == null) return hokenPattern;

            if (patternId == 0 && raiinNo != 0)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(u => u.HpId == hpId && u.RaiinNo == raiinNo && u.SinDate == sinDay && u.IsDeleted == DeleteTypes.None);

                if (raiinInf == null) return hokenPattern;

                patternId = raiinInf.HokenPid;
            }

            if (ptInf == null) return hokenPattern;

            var ptHokenPattern = NoTrackingDataContext.PtHokenPatterns
                .FirstOrDefault(pattern => pattern.HpId == hpId
                                                     && pattern.PtId == ptId
                                                     && pattern.HokenPid == patternId
                                                     && (pattern.IsDeleted == 0 || isGetDeleted));

            if (ptHokenPattern == null) return hokenPattern;

            var ptHokenInf = NoTrackingDataContext.PtHokenInfs
                .FirstOrDefault(hoken => hoken.HpId == hpId
                                                   && hoken.PtId == ptId
                                                   && hoken.HokenId == ptHokenPattern.HokenId
                                                   && (hoken.IsDeleted == 0 || isGetDeleted));

            var ptKohiQueryRepos = NoTrackingDataContext.PtKohis
                .Where(kohi => kohi.HpId == hpId
                                                  && kohi.PtId == ptId
                                                  && (kohi.IsDeleted == 0 || isGetDeleted));

            var predicateKohi = CreatePtKohiExpression(new List<int>()
                {
                    ptHokenPattern.Kohi1Id,
                    ptHokenPattern.Kohi2Id,
                    ptHokenPattern.Kohi3Id,
                    ptHokenPattern.Kohi4Id
                });

            List<PtKohi> listPtKohi = new List<PtKohi>();
            if (predicateKohi != null)
            {
                listPtKohi = ptKohiQueryRepos.Where(predicateKohi).ToList();
            }

            if (ptHokenInf == null && listPtKohi.Count <= 0) return hokenPattern;

            var predicateHokenMst = CreateHokenMstExpression(new List<PtHokenInf>() { ptHokenInf ?? new() }, listPtKohi);

            if (predicateHokenMst == null) return hokenPattern;

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

            var ptKohi1 = listPtKohi.FirstOrDefault(item => item.HokenId == ptHokenPattern.Kohi1Id);
            var ptKohi2 = listPtKohi.FirstOrDefault(item => item.HokenId == ptHokenPattern.Kohi2Id);
            var ptKohi3 = listPtKohi.FirstOrDefault(item => item.HokenId == ptHokenPattern.Kohi3Id);
            var ptKohi4 = listPtKohi.FirstOrDefault(item => item.HokenId == ptHokenPattern.Kohi4Id);

            //get hoken check confirm date
            var hokenChecks = NoTrackingDataContext.PtHokenChecks
                                .Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == 0)
                                .OrderBy(x => x.HokenGrp)
                                .ThenBy(x => x.HokenId)
                                .ThenBy(x => x.SeqNo)
                                .ToList();

            var hokenConfirmList = hokenChecks.Where(hokenConfirm => hokenConfirm.HokenGrp == 1 &&
                                                                     hokenConfirm.HokenId == ptHokenInf?.HokenId &&
                                                                     hokenConfirm.IsDeleted == 0)
                                                                          .OrderByDescending(hokenConfirm => hokenConfirm.CheckDate)
                                                                          .Select(item => new ConfirmDateModel(
                                                                                          item.HokenGrp,
                                                                                          item.HokenId,
                                                                                          item.CheckDate,
                                                                                          item.CheckId,
                                                                                          item.CheckMachine ?? string.Empty,
                                                                                          item.CheckCmt ?? string.Empty,
                                                                                          item.IsDeleted))
                                                                          .ToList();

            hokenPattern = new HokenPatternModel(
                 ptHokenPattern.PtId, ptHokenPattern.HokenPid, ptHokenPattern.HokenId, ptHokenPattern.StartDate, ptHokenPattern.EndDate, ptHokenPattern.HokenSbtCd, ptHokenPattern.HokenKbn, ptHokenPattern.Kohi1Id, ptHokenPattern.Kohi2Id, ptHokenPattern.Kohi3Id, ptHokenPattern.Kohi4Id,
                ptHokenInf == null
                    ? new()
                    : CreatePtHokenInfModel(ptHokenInf,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptHokenInf.HokenNo &&
                            item.HokenEdaNo == ptHokenInf.HokenEdaNo).ToList(),
                        hokenConfirmList, sinDay),
                ptKohi1 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi1,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi1.HokenNo &&
                            item.HokenEdaNo == ptKohi1.HokenEdaNo).ToList(),
                        new(), sinDay),
                ptKohi2 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi2,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi2.HokenNo &&
                            item.HokenEdaNo == ptKohi2.HokenEdaNo).ToList(),
                        new(), sinDay),
                ptKohi3 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi3,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi3.HokenNo &&
                            item.HokenEdaNo == ptKohi3.HokenEdaNo).ToList(),
                        new(), sinDay),
                ptKohi4 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi4,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi4.HokenNo &&
                            item.HokenEdaNo == ptKohi4.HokenEdaNo).ToList(),
                        new(), sinDay)
            );

            return hokenPattern;
        }

        private Expression<Func<PtKohi, bool>> CreatePtKohiExpression(List<int> listKohiId)
        {
            var param = Expression.Parameter(typeof(PtKohi));
            Expression? expression = null;

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

            return expression != null
                ? Expression.Lambda<Func<PtKohi, bool>>(body: expression, parameters: param)
                : Expression.Lambda<Func<PtKohi, bool>>(Expression.Constant(false), param);
        }

        private HokenInfModel CreatePtHokenInfModel(PtHokenInf ePtHokenInf, List<HokenMst> hokenMstLists, List<ConfirmDateModel> ptHokenCheckModelList, int sinDay)
        {
            var hokenInfModel = new HokenInfModel();
            if (ePtHokenInf != null)
            {
                HokenMst hokenMst;
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
                        hokenMst = hokMstMapped?.LastOrDefault() ?? new();
                    }
                    else
                    {
                        hokenMst = firstMapped;
                    }
                }
                else
                {
                    // have just one hoken mst with HokenNo and HokenEdaNo
                    hokenMst = hokMstMapped?.FirstOrDefault() ?? new();
                }

                var hokenMstModel = new HokenMstModel();

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
                        new());
                }

                hokenInfModel = new HokenInfModel(ePtHokenInf.HpId,
                                        ePtHokenInf.PtId,
                                        ePtHokenInf.HokenId,
                                        ePtHokenInf.HokenKbn,
                                        ePtHokenInf.Houbetu ?? string.Empty,
                                        ePtHokenInf.StartDate,
                                        ePtHokenInf.EndDate,
                                        sinDay,
                                        hokenMstModel,
                                        ptHokenCheckModelList);
            }

            return hokenInfModel ?? new();
        }

        public List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, bool isGetDeleted = false)
        {
            var ptHokenPatternList = new List<HokenPatternModel>();

            var hospitalInfo = NoTrackingDataContext.HpInfs
            .Where(p => p.HpId == hpId)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefault();

            var prefCd = 0;

            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            var ptInf = NoTrackingDataContext.PtInfs
                .FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete == 0);
            if (ptInf == null) return ptHokenPatternList;

            var listPtHokenPattern = NoTrackingDataContext.PtHokenPatterns
                .Where(pattern => pattern.HpId == hpId
                                                     && pattern.PtId == ptId
                                                     && (pattern.IsDeleted == 0 || isGetDeleted));

            if (!listPtHokenPattern.Any()) return ptHokenPatternList;

            var ptHokenInfRepos = NoTrackingDataContext.PtHokenInfs
                .Where(hoken => hoken.HpId == hpId
                                                   && hoken.PtId == ptId
                                                   && (hoken.IsDeleted == 0 || isGetDeleted));

            var hokenIds = listPtHokenPattern.Select(item => item.HokenId).Distinct().ToList();

            var listPtHokenInf = new List<PtHokenInf>();
            if (hokenIds.Any())
            {
                listPtHokenInf = ptHokenInfRepos.Where(p => hokenIds.Contains(p.HokenId)).ToList();
            }

            var ptKohiRepos = NoTrackingDataContext.PtKohis
                                       .Where(kohi => kohi.HpId == hpId
                                                                      && kohi.PtId == ptId
                                                                      && (kohi.IsDeleted == 0 || isGetDeleted));


            var listPtKohi = new List<PtKohi>();
            if (hokenIds.Any())
            {
                listPtKohi = ptKohiRepos.Where(p => hokenIds.Contains(p.HokenId)).ToList();
            }

            var predicateHokenMst = CreateHokenMstExpression(listPtHokenInf, listPtKohi);

            if (predicateHokenMst == null) return ptHokenPatternList;

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
                    : CreateHokenInfModel(ptHokenInf, new(), sinDay),
                ptKohi1 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi1,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi1.HokenNo &&
                            item.HokenEdaNo == ptKohi1.HokenEdaNo).ToList(),
                        new(), sinDay),
                ptKohi2 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi2,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi2.HokenNo &&
                            item.HokenEdaNo == ptKohi2.HokenEdaNo).ToList(),
                         new(), sinDay),
                ptKohi3 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi3,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi3.HokenNo &&
                            item.HokenEdaNo == ptKohi3.HokenEdaNo).ToList(),
                        new(), sinDay),
                ptKohi4 == null
                    ? new()
                    : CreatePtKohiModel(ptKohi4,
                        hokenMstList.Where(item =>
                            item.HokenNo == ptKohi4.HokenNo &&
                            item.HokenEdaNo == ptKohi4.HokenEdaNo).ToList(),
                       new(), sinDay)
            ));
            }

            return ptHokenPatternList;

        }

        public List<HokenPatternModel> FindPtHokenPatternList(int hpId, long ptId, int sinDay, List<int> listPatternId)
        {
            List<HokenPatternModel> ptHokenPatternList = new();

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

            var hokenIds = listPtHokenPattern.Select(item => item.HokenId).Distinct().ToList();

            List<PtHokenInf> listPtHokenInf = new();
            if (hokenIds != null)
            {
                listPtHokenInf = ptHokenInfRepos.Where(p => hokenIds.Contains(p.HokenId)).ToList();
            }

            var ptKohiRepos = NoTrackingDataContext.PtKohis
                .Where(kohi => kohi.HpId == hpId
                                                  && kohi.PtId == ptId
                                                  && kohi.IsDeleted == 0);

            var kohis = CreatePtKohi(listPtHokenPattern);

            List<PtKohi> listPtKohi = new();
            if (kohis != null)
            {
                listPtKohi = ptKohiRepos.Where(p => kohis.Contains(p.HokenId)).ToList();
            }

            var predicateHokenMst = CreateHokenMstExpression(listPtHokenInf, listPtKohi);

            if (predicateHokenMst == null) return ptHokenPatternList;

            var hokenMstListRepo = NoTrackingDataContext.HokenMsts
                .Where(
                    entity => entity.HpId == hpId
                              && (entity.PrefNo == 0
                              || entity.IsOtherPrefValid == 1))
                .OrderBy(e => e.HpId)
                .ThenBy(e => e.HokenNo)
                .ThenBy(e => e.HokenEdaNo)
                .ThenByDescending(e => e.StartDate)
                .ThenBy(e => e.HokenSbtKbn)
                .ThenBy(e => e.SortNo);

            var hokenMstList = hokenMstListRepo.Where(predicateHokenMst).ToList();

            var ptHokenCheckRepos = NoTrackingDataContext.PtHokenChecks.Where(item =>
                item.HpId == hpId && item.PtID == ptId && item.IsDeleted == 0);

            if (kohis == null || hokenIds == null) return ptHokenPatternList;

            var ptHokenCheckList = ptHokenCheckRepos.Where(p =>
                                                    kohis.Contains(p.HokenId) && p.HokenGrp == 2
                                                    || hokenIds.Contains(p.HokenId) && p.HokenGrp == 1).ToList();

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

        public List<AccountingFormMstModel> GetAccountingFormMstModels(int hpId)
        {
            var result = NoTrackingDataContext.AccountingFormMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0)
                        .AsEnumerable()
                        .Select(x => new AccountingFormMstModel(x.HpId, x.FormNo, x.FormName ?? string.Empty, x.FormType, x.PrintSort, x.MiseisanKbn, x.SaiKbn, x.MisyuKbn, x.SeikyuKbn, x.HokenKbn, x.Form ?? string.Empty, x.Base, x.SortNo, x.IsDeleted, x.CreateDate, x.UpdateDate, x.CreateId, x.UpdateId, false))
                        .ToList();
            return result;
        }

        public void UpdateAccountingFormMst(int userId, List<AccountingFormMstModel> models)
        {
            List<AccountingFormMst> addEntities = new();
            List<AccountingFormMst> updateEntities = new();
            foreach (var model in models)
            {
                if (!model.CheckDefaultValue() && model.ModelModified)
                {
                    var accountingFormMst = ConvertAccountingFormMstModelToAccountingFormMst(model);
                    accountingFormMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    accountingFormMst.UpdateId = userId;
                    if (model.FormNo == 0 && model.IsDeleted == 0)
                    {
                        accountingFormMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                        accountingFormMst.CreateId = userId;

                        addEntities.Add(accountingFormMst);
                    }
                    else
                    {
                        updateEntities.Add(accountingFormMst);
                    }
                }
            }
            TrackingDataContext.AccountingFormMsts.AddRange(addEntities);
            TrackingDataContext.AccountingFormMsts.UpdateRange(updateEntities);
            TrackingDataContext.SaveChanges();
        }

        public AccountingFormMst ConvertAccountingFormMstModelToAccountingFormMst(AccountingFormMstModel accountingFormMstModel)
        {
            var accountingFormMst = new AccountingFormMst();
            accountingFormMst.HpId = accountingFormMstModel.HpId;
            accountingFormMst.FormNo = accountingFormMstModel.FormNo;
            accountingFormMst.FormName = accountingFormMstModel.FormName;
            accountingFormMst.FormType = accountingFormMstModel.FormType;
            accountingFormMst.PrintSort = accountingFormMstModel.PrintSort;
            accountingFormMst.MiseisanKbn = accountingFormMstModel.MiseisanKbn;
            accountingFormMst.SaiKbn = accountingFormMstModel.SaiKbn;
            accountingFormMst.HokenKbn = accountingFormMstModel.HokenKbn;
            accountingFormMst.Form = accountingFormMstModel.Form;
            accountingFormMst.Base = accountingFormMstModel.Base;
            accountingFormMst.SortNo = accountingFormMstModel.SortNo;
            accountingFormMst.IsDeleted = accountingFormMstModel.IsDeleted;
            accountingFormMst.CreateId = accountingFormMstModel.CreateId;
            accountingFormMst.CreateDate = (accountingFormMstModel.FormNo > 0) ? TimeZoneInfo.ConvertTimeToUtc(accountingFormMstModel.CreateDate) : accountingFormMstModel.CreateDate;
            accountingFormMst.UpdateId = accountingFormMstModel.UpdateId;
            accountingFormMst.UpdateDate = accountingFormMstModel.UpdateDate;
            accountingFormMst.MisyuKbn = accountingFormMstModel.MisyuKbn;
            accountingFormMst.SeikyuKbn = accountingFormMstModel.SeikyuKbn;

            return accountingFormMst;
        }

        public List<HokenInfModel> GetListHokenSelect(int hpId, List<KaikeiInfModel> listKaikeiInf, long ptId)
        {
            if (listKaikeiInf == null || listKaikeiInf.Count <= 0)
                return new();

            var listHokenId = listKaikeiInf.Select(item => item.HokenId).Distinct().ToList();

            var listHokenInf = NoTrackingDataContext.PtHokenInfs.Where(item =>
                item.HpId == hpId && item.PtId == ptId && item.IsDeleted == 0 && listHokenId.Contains(item.HokenId) && item.HokenId > 0);

            var listHokenSeleted = from kaikeiInf in listKaikeiInf
                                   join ptHokenInf in listHokenInf on
                                       kaikeiInf.HokenId equals ptHokenInf.HokenId
                                   select new
                                   {
                                       KaikeiInf = kaikeiInf,
                                       PtHokenInf = ptHokenInf
                                   };

            return listHokenSeleted.Select(item => new HokenInfModel(item.PtHokenInf.PtId,
                                                                     item.KaikeiInf.HokenId,
                                                                     item.KaikeiInf.HokenKbn,
                                                                     item.PtHokenInf.HokensyaNo ?? string.Empty,
                                                                     item.PtHokenInf.HonkeKbn,
                                                                     item.PtHokenInf.StartDate,
                                                                     item.PtHokenInf.EndDate,
                                                                     item.PtHokenInf.Houbetu ?? string.Empty
                                                                     ))
                                                                     .OrderBy(item => item.HokenId)
                                                                     .ToList();
        }
    }
}
