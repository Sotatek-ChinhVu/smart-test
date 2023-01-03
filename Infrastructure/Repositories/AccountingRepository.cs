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

            if (raiinNoList.Count > 1)
            {
                foreach (var item in raiinNoList)
                {
                    var seikyu = NoTrackingDataContext.SyunoSeikyus.Where(x => x.HpId == hpId && x.PtId == ptId && x.RaiinNo == item.RaiinNo).ToList();
                    if (seikyu.Any())
                    {
                        seikyuTensu += seikyu.SeikyuTensu;
                    }

                    var nyukinList = NoTrackingDataContext.SyunoNyukin.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).AsEnumerable();
                    if (nyukinList.Any())
                    {
                        syunoNyukinModels.AddRange(nyukinList);
                    }

                    var kaikeiList = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId && x.RaiinNo == item.RaiinNo && x.RaiinNo == item.RaiinNo).ToList();
                }

                var accountingList =
            }

        }

        private AccountingModel ConvertToAccountingModel(int hpId, long ptId, SyunoSeikyu seikyu, SyunoNyukin nyukin, RaiinInf raiinItem, KaikeiInf kaikeiInf)
        {
            return new AccountingModel
                (
                hpId,
                ptId,
                seikyu.SinDate,
                raiinItem.OyaRaiinNo,
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
    }
}
