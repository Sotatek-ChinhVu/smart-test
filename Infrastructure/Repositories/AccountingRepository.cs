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

        public AccountingModel GetAccountingInfo(int hpId, long ptId, long oyaRaiinNo)
        {
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>();
            List<SyunoNyukinModel> syunoNyukinModels = new List<SyunoNyukinModel>();
            int seikyuTensu = 0;

            var raiinNoList = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.OyaRaiinNo == oyaRaiinNo).ToList();



            var seikyuList = NoTrackingDataContext.SyunoSeikyus.Where(x => x.HpId == hpId && x.PtId == ptId && x.RaiinNo == oyaRaiinNo).ToList();

            var nyukinList = NoTrackingDataContext.SyunoNyukin.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).ToList();

            var kaikeiList = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId && x.RaiinNo == oyaRaiinNo && x.RaiinNo == oyaRaiinNo).ToList();


            var accountingList = (from seikyu in seikyuList
                                  join nyukin in nyukinList on new { seikyu.HpId, seikyu.PtId, seikyu.SinDate, seikyu.RaiinNo }
                                                        equals new { nyukin.HpId, nyukin.PtId, nyukin.SinDate, nyukin.RaiinNo }
                                  join kaikei in kaikeiList on new { seikyu.HpId, seikyu.PtId, seikyu.SinDate, seikyu.RaiinNo }
                                                        equals new { kaikei.HpId, kaikei.PtId, kaikei.SinDate, kaikei.RaiinNo }
                                  where 1 == 1
                                  select ConvertToAccountingModel(hpId, ptId, seikyu, nyukin, kaikei)
                                  ).ToList();

          
        }

        private AccountingModel ConvertToAccountingModel(int hpId, long ptId, SyunoSeikyu seikyu, SyunoNyukin nyukin, KaikeiInf kaikeiInf)
        {
            return new AccountingModel
                (
                hpId,
                ptId,
                seikyu.SinDate,
                seikyu.RaiinNo,
                seikyu.AdjustFutan,
                nyukin != null ? nyukin.NyukinGaku : 0,
                nyukin != null ? nyukin.PaymentMethodCd : 0,
                nyukin.NyukinDate,
                nyukin.UketukeSbt,
                nyukin.NyukinCmt,
                nyukin.NyukinjiTensu,
                nyukin.NyukinjiSeikyu,
                nyukin.NyukinjiDetail,
                nyukin.IsDeleted,
                seikyu.NyukinKbn,
                seikyu.SeikyuTensu,
                seikyu.SeikyuGaku,
                seikyu.SeikyuDetail,
                seikyu.NewSeikyuTensu,
                seikyu.NewAdjustFutan,
                seikyu.NewSeikyuGaku,
                seikyu.NewSeikyuDetail,
                kaikeiInf.JihiFutan,
                kaikeiInf.JihiOuttax
                );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public SyunoSeikyuModel GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> listRaiinNo, bool getAll = false)
        {
            IQueryable<SyunoSeikyu> syunoSeikyuRepo = null;

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
                                 HokenPid = raiinInf.HokenPid
                             };

            var listHokenPid = querySyuno.Select(item => item.HokenPid).ToList();

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
                            SyunoSeikyu = syuno.SyunoSeikyu,
                            RaiinInf = syuno.RaiinInf,
                            ListSyunoNyukin = listSyunoNyukin,
                            ListKaikeiInf = listKaikeInf
                        };

            return query.AsEnumerable().Select(item => new SyunoSeikyuModel(item.SyunoSeikyu,
                item.RaiinInf,
                item.ListSyunoNyukin == null
                    ? new List<SyunoNyukin>()
                    : item.ListSyunoNyukin.ToList(),
                item.ListKaikeiInf.ToList(),
                listHokenPattern.FirstOrDefault(itemPattern => itemPattern.HokenPid == item.RaiinInf.HokenPid)?.HokenId ?? 0))
                .ToList();
        }
    }
}
