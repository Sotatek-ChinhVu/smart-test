using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        public AccountingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo, bool getAll = false)
        {
            try
            {
                IQueryable<SyunoSeikyu> syunoSeikyuRepo;

                var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.Where(item => item.RaiinNo == raiinNo && item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted == 0).FirstOrDefault();

                if (oyaRaiinNo == null || oyaRaiinNo.Status <= 3)
                {
                    return new List<AccountingModel>();
                }

                var listRaiinNo = NoTrackingDataContext.RaiinInfs.Where(
                    item => item.OyaRaiinNo == oyaRaiinNo.OyaRaiinNo && item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate && item.IsDeleted == 0 && item.Status > 3).ToList();

                if (getAll == true)
                {
                    syunoSeikyuRepo = NoTrackingDataContext.SyunoSeikyus
                        .Where(item =>
                            item.HpId == hpId && item.PtId == ptId &&
                            item.NyukinKbn == 1 && !listRaiinNo.Select(item => item.RaiinNo).Contains(item.RaiinNo))
                        .OrderBy(item => item.SinDate).ThenBy(item => item.RaiinNo);
                }
                else
                {
                    syunoSeikyuRepo = NoTrackingDataContext.SyunoSeikyus
                        .Where(item =>
                            item.HpId == hpId && item.PtId == ptId && item.SinDate == sinDate &&
                            listRaiinNo.Select(item => item.RaiinNo).Contains(item.RaiinNo)).OrderBy(item => item.RaiinNo);
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

                var query = from syuno in querySyuno.AsEnumerable()
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
            catch (Exception)
            {

                throw;
            }

        }

        private AccountingModel ConvertToModel(SyunoSeikyu syunoSeikyu, RaiinInf raiinInf, List<SyunoNyukin> syunoNyukin, List<KaikeiInf> kaikeiInf, List<PtHokenPattern> listHokenPattern)
        {
            return new AccountingModel
                (
                    new SyunoSeikyuModel(
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
                             syunoSeikyu.NewSeikyuDetail ?? string.Empty
                        ),
                        new SyunoRaiinInfModel(
                             raiinInf.Status,
                             raiinInf.KaikeiTime,
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

        public AccountingInfModel GetAccountingInfAllRaiinNo(List<AccountingModel> accountingModels)
        {
            try
            {
                var isSettled = accountingModels.Select(item => item.SyunoSeikyu.NyukinKbn != 0).FirstOrDefault();

                var TotalPoint = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuTensu);

                var KanFutan = accountingModels.Sum(item => item.PtFutan + item.AdjustRound);

                var TotalSelfExpense =
                    accountingModels.Sum(item => item.JihiFutan + item.JihiOuttax);
                var Tax =
                    accountingModels.Sum(item => item.JihiTax + item.JihiOuttax);
                var AdjustFutan = accountingModels.Sum(item => item.AdjFutan);

                var DebitBalance = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku -
                                                      item.SyunoNyukinModels.Sum(itemNyukin =>
                                                          itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));

                var SumAdjust = 0;
                var SumAdjustView = 0;
                var ThisCredit = 0;
                var ThisWari = 0;
                var PayType = 0;
                if (isSettled == true)
                {
                    SumAdjust = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku);
                    SumAdjustView = SumAdjust;
                    ThisCredit =
                       accountingModels.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku));
                    ThisWari =
                       accountingModels.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan));
                    PayType = accountingModels.Where(item => item.SyunoNyukinModels.Count > 0)
                       .Select(item => item.SyunoNyukinModels.Where(itemNyukin => itemNyukin.PaymentMethodCd > 0)
                           .Select(itemNyukin => itemNyukin.PaymentMethodCd).FirstOrDefault())
                       .FirstOrDefault();
                }
                else
                {
                    SumAdjust = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku);
                    SumAdjustView = SumAdjust + DebitBalance;

                    ThisCredit = SumAdjust;
                }
                return new AccountingInfModel(TotalPoint, KanFutan, TotalSelfExpense, Tax, DebitBalance, SumAdjust, SumAdjustView, ThisCredit, ThisWari, PayType, AdjustFutan);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void ReleaseResource()
        {
            DisposeDataContext();
        }



    }
}
