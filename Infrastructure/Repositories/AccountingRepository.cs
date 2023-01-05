using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        public AccountingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            
        }

        public List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo, bool getAll = false)
        {
            IQueryable<SyunoSeikyu> syunoSeikyuRepo = null;

            var oyaRaiinNo = NoTrackingDataContext.RaiinInfs.Where(item => item.RaiinNo == raiinNo && item.HpId == hpId && item.SinDate == sinDate && item.IsDeleted == 0).FirstOrDefault();

            if(oyaRaiinNo == null || oyaRaiinNo.Status <= 3)
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
                        select new
                        {
                            syunoSeikyu = syuno.SyunoSeikyu,
                            raiinInf = syuno.RaiinInf,
                            ListSyunoNyukin = listSyunoNyukin,
                            ListKaikeiInf = listKaikeInf
                        };

            var result = query.AsEnumerable().Select(item => new AccountingModel(
                    new SyunoSeikyuModel(
                        item.syunoSeikyu.HpId,
                        item.syunoSeikyu.PtId,
                        item.syunoSeikyu.SinDate,
                        item.syunoSeikyu.RaiinNo,
                        item.syunoSeikyu.NyukinKbn,
                        item.syunoSeikyu.SeikyuTensu,
                        item.syunoSeikyu.AdjustFutan,
                        item.syunoSeikyu.SeikyuGaku,
                        item.syunoSeikyu.SeikyuDetail,
                        item.syunoSeikyu.NewSeikyuTensu,
                        item.syunoSeikyu.NewAdjustFutan,
                        item.syunoSeikyu.NewSeikyuGaku,
                        item.syunoSeikyu.NewSeikyuDetail
                    ),
                    new SyunoRaiinInfModel(
                        item.raiinInf.Status,
                        item.raiinInf.KaikeiTime,
                        item.raiinInf.UketukeSbt
                        ),
                        item.ListSyunoNyukin == null
                    ? new List<SyunoNyukinModel>()
                    : item.ListSyunoNyukin.Select(y =>
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
                        y.NyukinCmt,
                        y.NyukinjiTensu,
                        y.NyukinjiSeikyu,
                        y.NyukinjiDetail
                        )
                        ).ToList(),

                   item.ListKaikeiInf.Select(x =>
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
                        x.ReceSbt,
                        x.Houbetu,
                        x.Kohi1Houbetu,
                        x.Kohi2Houbetu,
                        x.Kohi3Houbetu,
                        x.Kohi4Houbetu,
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
                           )
                       ).ToList(),
                   listHokenPattern.FirstOrDefault(itemPattern => itemPattern.HokenPid == item.RaiinInf.HokenPid)?.HokenId ?? 0))
                .ToList();
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }



    }
}
