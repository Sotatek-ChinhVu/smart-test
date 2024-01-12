using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;

namespace Reporting.Statistics.Sta1001.DB;

public class CoSta1001Finder : RepositoryBase, ICoSta1001Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta1001Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
        _hpInfFinder.ReleaseResource();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="staMonthType">0:日計 1:月計 2:月計(明細/保険別)</param>
    /// <returns></returns>
    public List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1001PrintConf printConf, int staMonthType)
    {
        //入金情報
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(s => s.IsDeleted == DeleteStatus.None);

        //支払方法
        var payMsts = NoTrackingDataContext.PaymentMethodMsts.Where(p => p.IsDeleted == DeleteStatus.None);
        //請求情報
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(s => s.NyukinKbn != 0);  //0:未精算を除く
        var kaikeiFutans = NoTrackingDataContext.KaikeiInfs
            .GroupBy(k => new { k.HpId, k.PtId, k.RaiinNo })
            .Select(k =>
                new
                {
                    k.Key.HpId,
                    k.Key.PtId,
                    k.Key.RaiinNo,
                    PtFutan = k.Sum(x => x.PtFutan + x.AdjustRound),
                    JihiFutan = k.Sum(x => x.JihiFutan + x.JihiOuttax),
                    JihiTax = k.Sum(x => x.JihiTax + x.JihiOuttax),
                    JihiFutanTaxFree = k.Sum(x => x.JihiFutanTaxfree),
                    JihiFutanTaxNr = k.Sum(x => x.JihiFutanTaxNr),
                    JihiFutanTaxGen = k.Sum(x => x.JihiFutanTaxGen),
                    JihiTaxNr = k.Sum(x => x.JihiTaxNr),
                    JihiTaxGen = k.Sum(x => x.JihiTaxGen),
                    JihiFutanOuttaxNr = k.Sum(x => x.JihiFutanOuttaxNr + x.JihiOuttaxNr),
                    JihiFutanOuttaxGen = k.Sum(x => x.JihiFutanOuttaxGen + x.JihiOuttaxGen),
                    JihiOuttaxNr = k.Sum(x => x.JihiOuttaxNr),
                    JihiOuttaxGen = k.Sum(x => x.JihiOuttaxGen),
                    AdjustFutan = k.Sum(x => x.AdjustFutan),
                });

        //来院情報
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId);
        if (printConf.KaIds?.Count >= 1)
        {
            //診療科の条件指定
            raiinInfs = raiinInfs.Where(r => printConf.KaIds.Contains(r.KaId));
        }
        if (printConf.TantoIds?.Count >= 1)
        {
            //担当医の条件指定
            raiinInfs = raiinInfs.Where(r => printConf.TantoIds.Contains(r.TantoId));
        }

        //初回来院日
        var firstRaiins = (
            from p in raiinInfs
            where
                p.IsDeleted == DeleteStatus.None &&
                p.Status >= 5
            group
                new { p.HpId, p.PtId, p.SinDate } by
                new { p.HpId, p.PtId } into pg
            select new
            {
                pg.Key.HpId,
                pg.Key.PtId,
                FirstRaiinDate = pg.Min(x => x.SinDate)
            }
        );

        //患者情報
        var ptInfs = NoTrackingDataContext.PtInfs.Where(
            p => p.IsDelete == DeleteStatus.None
        );
        if (!printConf.IsTester)
        {
            ptInfs = ptInfs.Where(p => p.IsTester == 0);
        }
        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts;
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts.Where(k => k.IsDeleted == DeleteStatus.None);
        //ユーザーマスタ
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.IsDeleted == DeleteStatus.None);

        var preNyukins = (
            from n in syunoNyukins
            join s in syunoNyukins on
                new { n.HpId, n.RaiinNo } equals
                new { s.HpId, s.RaiinNo } into sJoin
            from sj in sJoin.DefaultIfEmpty()
            where
                n.HpId == hpId &&
                n.NyukinDate >= printConf.StartNyukinDate &&
                n.NyukinDate <= printConf.EndNyukinDate &&
                n.IsDeleted == DeleteStatus.None
            group
                new
                {
                    n.HpId,
                    n.RaiinNo,
                    n.SeqNo,
                    NyukinGaku = n.NyukinDate > sj.NyukinDate || (n.NyukinDate == sj.NyukinDate && n.SortNo > sj.SortNo) ? sj.NyukinGaku : 0,
                    AdjustFutan = n.NyukinDate > sj.NyukinDate || (n.NyukinDate == sj.NyukinDate && n.SortNo > sj.SortNo) ? sj.AdjustFutan : 0
                } by
                new { n.HpId, n.RaiinNo, n.SeqNo } into ng
            select new
            {
                ng.Key.HpId,
                ng.Key.RaiinNo,
                ng.Key.SeqNo,
                PreNyukinGaku = ng.Sum(n => n.NyukinGaku),
                PreAdjustFutan = ng.Sum(n => n.AdjustFutan)
            }
        ).ToList();

        var syunoNyukinList = NoTrackingDataContext.SyunoNyukin
            .Where(s => s.IsDeleted == DeleteStatus.None && s.HpId == hpId && (
                printConf.StartNyukinDate == printConf.EndNyukinDate ?
                    (s.NyukinDate == printConf.StartNyukinDate) :
                    (s.NyukinDate >= printConf.StartNyukinDate && s.NyukinDate <= printConf.EndNyukinDate)
            ) && (printConf.UketukeSbtIds != null && printConf.UketukeSbtIds.Contains(s.UketukeSbt) == true) && (printConf.PaymentMethodCds != null && printConf.PaymentMethodCds.Contains(s.PaymentMethodCd))).ToList();

        var raiinNos = syunoNyukinList.Select(s => s.RaiinNo).Distinct().ToList();
        var ptIds = syunoNyukinList.Select(s => s.PtId).Distinct().ToList();
        var payMethods = syunoNyukinList.Select(s => s.PaymentMethodCd).Distinct().ToList();
        var userIds = syunoNyukinList.Select(s => s.UpdateId).Distinct().ToList();
        var uketukeSbts = syunoNyukinList.Select(s => s.UketukeSbt).Distinct().ToList();
        var syunoSeikyuList = syunoSeikyus.Where(s => raiinNos.Contains(s.RaiinNo)).ToList();
        var raiinInfList = raiinInfs.Where(r => raiinNos.Contains(r.RaiinNo)).ToList();
        var kaIds = raiinInfList.Select(r => r.KaId).Distinct().ToList();
        var tantoIds = raiinInfList.Select(r => r.TantoId).Distinct().ToList();
        var uketukeIds = raiinInfList.Select(r => r.UketukeId).Distinct().ToList();
        var firstRaiinList = firstRaiins.Where(f => ptIds.Contains(f.PtId)).ToList();
        var ptIdList = ptInfs.Where(f => ptIds.Contains(f.PtId)).ToList();
        var payMstList = payMsts.Where(p => payMethods.Contains(p.PaymentMethodCd)).ToList();
        var uketukeSbtMstList = uketukeSbtMsts.Where(u => uketukeSbts.Contains(u.KbnId)).ToList();
        var userList = userMsts.Where(u => userIds.Contains(u.UserId)).ToList();
        var kaMstList = kaMsts.Where(k => kaIds.Contains(k.KaId)).ToList();
        var doctorList = userMsts.Where(u => tantoIds.Contains(u.UserId)).ToList();
        var uketukeList = userMsts.Where(u => uketukeIds.Contains(u.UserId)).ToList();

        var joinList = from syunoNyukin in syunoNyukinList
                       join payMst in payMstList on
               new { syunoNyukin.HpId, syunoNyukin.PaymentMethodCd } equals
               new { payMst.HpId, payMst.PaymentMethodCd } into payMstJoin
                       from payMstj in payMstJoin.DefaultIfEmpty()
                       join syunoSeikyu in syunoSeikyuList on
                           new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
                           new { syunoSeikyu.HpId, syunoSeikyu.RaiinNo }
                       join raiinInf in raiinInfList on
                           new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
                           new { raiinInf.HpId, raiinInf.RaiinNo }
                       join firstRaiin in firstRaiinList on
                           new { syunoNyukin.HpId, syunoNyukin.PtId } equals
                           new { firstRaiin.HpId, firstRaiin.PtId }
                       join ptInf in ptIdList on
                           new { syunoNyukin.HpId, syunoNyukin.PtId } equals
                           new { ptInf.HpId, ptInf.PtId }
                       join uketukeSbtMst in uketukeSbtMstList on
                           new { syunoNyukin.HpId, syunoNyukin.UketukeSbt } equals
                           new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeSbtMstJoin
                       from uketukeSbtMstj in uketukeSbtMstJoin.DefaultIfEmpty()
                       join userMst in userList on
                           new { syunoNyukin.HpId, syunoNyukin.UpdateId } equals
                           new { userMst.HpId, UpdateId = userMst.UserId } into nyukinUserMstJoin
                       from nyukinUserMst in nyukinUserMstJoin.DefaultIfEmpty()
                       join kaMst in kaMstList on
                           new { raiinInf.HpId, raiinInf.KaId } equals
                           new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                       from kaMstj in kaMstJoin.DefaultIfEmpty()
                       join userMst in doctorList on
                           new { raiinInf.HpId, raiinInf.TantoId } equals
                           new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                       from tantoMst in userMstJoin.DefaultIfEmpty()
                       join userMst in uketukeList on
                           new { raiinInf.HpId, raiinInf.UketukeId } equals
                           new { userMst.HpId, UketukeId = userMst.UserId } into uketukeUserMstJoin
                       from uketukeUserMst in uketukeUserMstJoin.DefaultIfEmpty()
                       select new
                       {
                           syunoNyukin.RaiinNo,
                           syunoNyukin.SinDate,
                           NyukinSeqNo = syunoNyukin.SeqNo,
                           NyukinAdjustFutan = syunoNyukin.AdjustFutan,
                           NyukinPaymentMethodCd = syunoNyukin.PaymentMethodCd,
                           NyukinSortNo = syunoNyukin.SortNo,
                           syunoNyukin.NyukinGaku,
                           syunoNyukin.NyukinDate,
                           NyukinUpdateId = syunoNyukin.UpdateId,
                           NyukinUketukeSbt = syunoNyukin.UketukeSbt,
                           syunoNyukin.NyukinCmt,
                           NyukinUpdateDate = syunoNyukin.UpdateDate,
                           syunoSeikyu.NyukinKbn,
                           syunoSeikyu.SeikyuTensu,
                           syunoSeikyu.NewSeikyuTensu,
                           syunoSeikyu.SeikyuGaku,
                           syunoSeikyu.NewSeikyuGaku,
                           PaySname = payMstj != null ? payMstj.PaySname : string.Empty,
                           raiinInf.OyaRaiinNo,
                           raiinInf.KaId,
                           KaSname = kaMstj != null ? kaMstj.KaSname : string.Empty,
                           raiinInf.TantoId,
                           TantoSname = tantoMst != null ? tantoMst.Sname : string.Empty,
                           raiinInf.UketukeTime,
                           raiinInf.KaikeiTime,
                           raiinInf.UketukeId,
                           UketukeSname = uketukeUserMst != null ? uketukeUserMst.Sname : string.Empty,
                           raiinInf.SyosaisinKbn,
                           ptInf.PtNum,
                           PtName = ptInf.Name,
                           PtKanaName = ptInf.KanaName,
                           UketukeSbtName = uketukeSbtMstj != null ? uketukeSbtMstj.KbnName : string.Empty,
                           NyukinUserSname = nyukinUserMst != null ? nyukinUserMst.Sname : string.Empty,
                           firstRaiin.FirstRaiinDate
                       };
        joinList = joinList.ToList();
        
        //var joinQuery = (
        //    from syunoNyukin in syunoNyukins
        //        //join preNyukin in preNyukins on
        //        //    new { syunoNyukin.HpId, syunoNyukin.RaiinNo, syunoNyukin.SeqNo } equals
        //        //    new { preNyukin.HpId, preNyukin.RaiinNo, preNyukin.SeqNo }
        //    join payMst in payMsts on
        //        new { syunoNyukin.HpId, syunoNyukin.PaymentMethodCd } equals
        //        new { payMst.HpId, payMst.PaymentMethodCd } into payMstJoin
        //    from payMstj in payMstJoin.DefaultIfEmpty()
        //    join syunoSeikyu in syunoSeikyus on
        //        new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
        //        new { syunoSeikyu.HpId, syunoSeikyu.RaiinNo }
        //    join raiinInf in raiinInfs on
        //        new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
        //        new { raiinInf.HpId, raiinInf.RaiinNo }
        //    join firstRaiin in firstRaiins on
        //        new { syunoNyukin.HpId, syunoNyukin.PtId } equals
        //        new { firstRaiin.HpId, firstRaiin.PtId }

        //        // anh.vu3 refactor
        //        //join kaikeiFutan in kaikeiFutans on
        //        //    new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
        //        //    new { kaikeiFutan.HpId, kaikeiFutan.RaiinNo } into kaikeiFutanJoin
        //        //from kaikeiFutanj in kaikeiFutanJoin.DefaultIfEmpty()
        //    join ptInf in ptInfs on
        //        new { syunoNyukin.HpId, syunoNyukin.PtId } equals
        //        new { ptInf.HpId, ptInf.PtId }
        //    join uketukeSbtMst in uketukeSbtMsts on
        //        new { syunoNyukin.HpId, syunoNyukin.UketukeSbt } equals
        //        new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeSbtMstJoin
        //    from uketukeSbtMstj in uketukeSbtMstJoin.DefaultIfEmpty()
        //    join userMst in userMsts on
        //        new { syunoNyukin.HpId, syunoNyukin.UpdateId } equals
        //        new { userMst.HpId, UpdateId = userMst.UserId } into nyukinUserMstJoin
        //    from nyukinUserMst in nyukinUserMstJoin.DefaultIfEmpty()
        //    join kaMst in kaMsts on
        //        new { raiinInf.HpId, raiinInf.KaId } equals
        //        new { kaMst.HpId, kaMst.KaId } into kaMstJoin
        //    from kaMstj in kaMstJoin.DefaultIfEmpty()
        //    join userMst in userMsts on
        //        new { raiinInf.HpId, raiinInf.TantoId } equals
        //        new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
        //    from tantoMst in userMstJoin.DefaultIfEmpty()
        //    join userMst in userMsts on
        //        new { raiinInf.HpId, raiinInf.UketukeId } equals
        //        new { userMst.HpId, UketukeId = userMst.UserId } into uketukeUserMstJoin
        //    from uketukeUserMst in uketukeUserMstJoin.DefaultIfEmpty()
        //        //join kaikeiHoken in kaikeiHokens on
        //        //    new { raiinInf.HpId, raiinInf.RaiinNo } equals
        //        //    new { kaikeiHoken.HpId, kaikeiHoken.RaiinNo } into kaikeiHokenJoin
        //        //from kaikeiHokenj in kaikeiHokenJoin.DefaultIfEmpty()
        //    where
        //        syunoNyukin.HpId == hpId &&
        //        (
        //            printConf.StartNyukinDate == printConf.EndNyukinDate ?
        //                (syunoNyukin.NyukinDate == printConf.StartNyukinDate) :
        //                (syunoNyukin.NyukinDate >= printConf.StartNyukinDate && syunoNyukin.NyukinDate <= printConf.EndNyukinDate)
        //        ) &&
        //        //syunoNyukin.NyukinDate >= printConf.StartNyukinDate &&   開始日と終了日が同じ場合に著しく遅くなる環境があるため
        //        //syunoNyukin.NyukinDate <= printConf.EndNyukinDate &&
        //        syunoNyukin.IsDeleted == DeleteStatus.None
        //    select new
        //    {
        //        RaiinNo = syunoNyukin.RaiinNo,
        //        SinDate = syunoNyukin.SinDate,
        //        NyukinSeqNo = syunoNyukin.SeqNo,
        //        NyukinAdjustFutan = syunoNyukin.AdjustFutan,
        //        NyukinPaymentMethodCd = syunoNyukin.PaymentMethodCd,
        //        NyukinSortNo = syunoNyukin.SortNo,
        //        NyukinGaku = syunoNyukin.NyukinGaku,
        //        NyukinDate = syunoNyukin.NyukinDate,
        //        NyukinUpdateId = syunoNyukin.UpdateId,
        //        NyukinUketukeSbt = syunoNyukin.UketukeSbt,
        //        NyukinCmt = syunoNyukin.NyukinCmt,
        //        NyukinUpdateDate = syunoNyukin.UpdateDate,
        //        NyukinKbn = syunoSeikyu.NyukinKbn,
        //        SeikyuTensu = syunoSeikyu.SeikyuTensu,
        //        NewSeikyuTensu = syunoSeikyu.NewSeikyuTensu,
        //        SeikyuGaku = syunoSeikyu.SeikyuGaku,
        //        NewSeikyuGaku = syunoSeikyu.NewSeikyuGaku,

        //        // anh.vu3 refactor
        //        //PtFutan = kaikeiFutanj != null ? kaikeiFutanj.PtFutan : 0,
        //        //JihiFutan = kaikeiFutanj != null ? kaikeiFutanj.JihiFutan : 0,
        //        //JihiTax = kaikeiFutanj != null ? kaikeiFutanj.JihiTax : 0,
        //        //JihiFutanTaxFree = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxFree : 0,
        //        //JihiFutanTaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxNr : 0,
        //        //JihiFutanTaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxGen : 0,
        //        //JihiTaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiTaxNr : 0,
        //        //JihiTaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiTaxGen : 0,
        //        //JihiFutanOuttaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanOuttaxNr : 0,
        //        //JihiFutanOuttaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanOuttaxGen : 0,
        //        //JihiOuttaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiOuttaxNr : 0,
        //        //JihiOuttaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiOuttaxGen : 0,
        //        //KaikeiAdjustFutan = kaikeiFutanj != null ? -kaikeiFutanj.AdjustFutan : 0,
        //        //HokenKbn = kaikeiHokenj!=null? kaikeiHokenj.HokenKbn:0,
        //        //HokenSbtCd = kaikeiHokenj!=null?kaikeiHokenj.HokenSbtCd:0,
        //        //ReceSbt = kaikeiHokenj!=null?kaikeiHokenj.ReceSbt:string.Empty,
        //        PaySname = payMstj != null ? payMstj.PaySname : string.Empty,
        //        raiinInf.OyaRaiinNo,
        //        raiinInf.KaId,
        //        KaSname = kaMstj != null ? kaMstj.KaSname : string.Empty,
        //        raiinInf.TantoId,
        //        TantoSname = tantoMst != null ? tantoMst.Sname : string.Empty,
        //        raiinInf.UketukeTime,
        //        raiinInf.KaikeiTime,
        //        raiinInf.UketukeId,
        //        UketukeSname = uketukeUserMst != null ? uketukeUserMst.Sname : string.Empty,
        //        raiinInf.SyosaisinKbn,
        //        ptInf.PtNum,
        //        PtName = ptInf.Name,
        //        PtKanaName = ptInf.KanaName,
        //        UketukeSbtName = uketukeSbtMstj != null ? uketukeSbtMstj.KbnName : string.Empty,
        //        NyukinUserSname = nyukinUserMst != null ? nyukinUserMst.Sname : string.Empty,
        //        firstRaiin.FirstRaiinDate
        //    }
        //);


        var result = new List<CoSyunoInfModel>();
        foreach (var item in joinList)
        {
            result.Add(
            new CoSyunoInfModel()
            {
                IsNyukin = true,
                PreNyukinGaku = 0,
                PreAdjustFutan = 0,
                FirstRaiinDate = item.FirstRaiinDate,
                RaiinNo = item.RaiinNo,
                OyaRaiinNo = item.OyaRaiinNo,
                SinDate = item.SinDate,
                PtNum = item.PtNum,
                PtName = item.PtName,
                PtKanaName = item.PtKanaName,
                Tensu = item.SeikyuTensu,
                NewTensu = item.NewSeikyuTensu,
                BaseSeikyuGaku = item.SeikyuGaku,
                BaseNewSeikyuGaku = item.NewSeikyuGaku,
                NyukinKbn = item.NyukinKbn,
                NyukinAdjustFutan = item.NyukinAdjustFutan,
                PayCd = item.NyukinPaymentMethodCd,
                PaySName = item.PaySname,
                NyukinSortNo = item.NyukinSortNo,
                NyukinSeqNo = item.NyukinSeqNo,
                NyukinGaku = item.NyukinGaku,
                NyukinDate = item.NyukinDate,
                NyukinUserId = item.NyukinUpdateId,
                NyukinUserSname = item.NyukinUserSname,
                NyukinTime = int.Parse(item.NyukinUpdateDate.ToString("HHmm")),
                UketukeSbt = item.NyukinUketukeSbt,
                UketukeSbtName = item.UketukeSbtName,
                KaId = item.KaId,
                KaSname = item.KaSname,
                TantoId = item.TantoId,
                TantoSname = item.TantoSname,
                UketukeTime = item.UketukeTime,
                KaikeiTime = item.KaikeiTime,
                UketukeId = item.UketukeId,
                UketukeSname = item.UketukeSname,
                SyosaisinKbn = item.SyosaisinKbn,
                NyukinCmt = item.NyukinCmt
            });
        }

        //入金時間
        if (printConf.StartNyukinTime >= 0)
        {
            result = result.Where(r => r.NyukinTime >= printConf.StartNyukinTime).ToList();
        }
        if (printConf.EndNyukinTime >= 0)
        {
            result = result.Where(r => r.NyukinTime <= printConf.EndNyukinTime).ToList();
        }

        //前回入金額の設定
        result.ForEach(r =>
        {
            var preNyukin = preNyukins.Find(p =>
                p.HpId == hpId &&
                p.RaiinNo == r.RaiinNo &&
                p.SeqNo == r.NyukinSeqNo
            );
            if (preNyukin != null)
            {
                r.PreNyukinGaku = preNyukin.PreNyukinGaku;
                r.PreAdjustFutan = preNyukin.PreAdjustFutan;
            }
        }
        );

        #region 未精算or当日入金レコードがない来院の請求情報を追加
        if (!printConf.IsExcludeUnpaid)
        {
            //当日入金がある来院
            var theDayNyukins = syunoNyukins.Where(n =>
                n.HpId == hpId &&
                (staMonthType == 2 ? n.SinDate / 100 == n.NyukinDate / 100 : n.SinDate == n.NyukinDate) &&
                n.IsDeleted == DeleteStatus.None
            ).Select(n => n.RaiinNo);

            //未精算or当日入金レコードがない請求情報を抽出
            var unSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(
                            s =>
                                s.HpId == hpId &&
                                (
                                    printConf.StartNyukinDate == printConf.EndNyukinDate ?
                                        (s.SinDate == printConf.StartNyukinDate) :
                                        (s.SinDate >= printConf.StartNyukinDate && s.SinDate <= printConf.EndNyukinDate)
                                ) &&
                                //s.SinDate >= printConf.StartNyukinDate &&
                                //s.SinDate <= printConf.EndNyukinDate &&
                                (s.NyukinKbn == 0 || (s.NyukinKbn >= 1 && !theDayNyukins.Contains(s.RaiinNo)))
                        );

            var joinSeikyu = (
                from unSeikyu in unSeikyus
                join syunoNyukin in syunoNyukins on
                    new { unSeikyu.HpId, unSeikyu.RaiinNo } equals
                    new { syunoNyukin.HpId, syunoNyukin.RaiinNo } into syunoNyukinJoin
                from syunoNyukinj in syunoNyukinJoin.DefaultIfEmpty()
                join raiinInf in raiinInfs on
                    new { unSeikyu.HpId, unSeikyu.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join firstRaiin in firstRaiins on
                    new { unSeikyu.HpId, unSeikyu.PtId } equals
                    new { firstRaiin.HpId, firstRaiin.PtId }

                    // anh.vu3 refactor
                    //join kaikeiFutan in kaikeiFutans on
                    //    new { unSeikyu.HpId, unSeikyu.RaiinNo } equals
                    //    new { kaikeiFutan.HpId, kaikeiFutan.RaiinNo } into kaikeiFutanJoin
                    //from kaikeiFutanj in kaikeiFutanJoin.DefaultIfEmpty()
                join ptInf in ptInfs on
                    new { unSeikyu.HpId, unSeikyu.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join uketukeSbtMst in uketukeSbtMsts on
                    new { raiinInf.HpId, raiinInf.UketukeSbt } equals
                    new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeSbtMstJoin
                from uketukeSbtMstj in uketukeSbtMstJoin.DefaultIfEmpty()
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                from kaMstj in kaMstJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                from tantoMst in userMstJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.UketukeId } equals
                    new { userMst.HpId, UketukeId = userMst.UserId } into uketukeUserMstJoin
                from uketukeUserMst in uketukeUserMstJoin.DefaultIfEmpty()
                    //join kaikeiHoken in kaikeiHokens on
                    //    new { raiinInf.HpId, raiinInf.RaiinNo } equals
                    //    new { kaikeiHoken.HpId, kaikeiHoken.RaiinNo } into kaikeiHokenJoin
                    //from kaikeiHokenj in kaikeiHokenJoin.DefaultIfEmpty()
                select new
                {
                    RaiinNo = unSeikyu.RaiinNo,
                    SinDate = unSeikyu.SinDate,
                    NyukinSeqNo = syunoNyukinj == null ? 0 : syunoNyukinj.SeqNo,
                    NyukinAdjustFutan = syunoNyukinj == null ? 0 : syunoNyukinj.AdjustFutan,
                    NyukinPaymentMethodCd = syunoNyukinj == null ? 0 : syunoNyukinj.PaymentMethodCd,
                    NyukinSortNo = syunoNyukinj == null ? 0 : syunoNyukinj.SortNo,
                    NyukinGaku = syunoNyukinj == null ? 0 : syunoNyukinj.NyukinGaku,
                    NyukinDate = syunoNyukinj == null ? 0 : syunoNyukinj.NyukinDate,
                    NyukinUpdateId = syunoNyukinj == null ? 0 : syunoNyukinj.UpdateId,
                    NyukinCmt = syunoNyukinj == null ? string.Empty : syunoNyukinj.NyukinCmt,
                    NyukinUpdateDate = syunoNyukinj == null ? DateTime.MinValue : syunoNyukinj.UpdateDate,
                    NyukinKbn = unSeikyu.NyukinKbn,
                    SeikyuTensu = unSeikyu.SeikyuTensu,
                    NewSeikyuTensu = unSeikyu.NewSeikyuTensu,
                    SeikyuGaku = unSeikyu.SeikyuGaku,
                    NewSeikyuGaku = unSeikyu.NewSeikyuGaku,

                    // anh.vu3 refactor
                    //PtFutan = kaikeiFutanj != null ? kaikeiFutanj.PtFutan : 0,
                    //JihiFutan = kaikeiFutanj != null ? kaikeiFutanj.JihiFutan : 0,
                    //JihiTax = kaikeiFutanj != null ? kaikeiFutanj.JihiTax : 0,
                    //JihiFutanTaxFree = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxFree : 0,
                    //JihiFutanTaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxNr : 0,
                    //JihiFutanTaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanTaxGen : 0,
                    //JihiTaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiTaxNr : 0,
                    //JihiTaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiTaxGen : 0,
                    //JihiFutanOuttaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanOuttaxNr : 0,
                    //JihiFutanOuttaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiFutanOuttaxGen : 0,
                    //JihiOuttaxNr = kaikeiFutanj != null ? kaikeiFutanj.JihiOuttaxNr : 0,
                    //JihiOuttaxGen = kaikeiFutanj != null ? kaikeiFutanj.JihiOuttaxGen : 0,
                    //KaikeiAdjustFutan = kaikeiFutanj != null ? -kaikeiFutanj.AdjustFutan : 0,
                    //HokenKbn = kaikeiHokenj != null ? kaikeiHokenj.HokenKbn : 0,
                    //HokenSbtCd = kaikeiHokenj != null ? kaikeiHokenj.HokenSbtCd : 0,
                    //ReceSbt = kaikeiHokenj != null ? kaikeiHokenj.ReceSbt : string.Empty,
                    raiinInf.OyaRaiinNo,
                    raiinInf.KaId,
                    KaSname = kaMstj != null ? kaMstj.KaSname : string.Empty,
                    raiinInf.TantoId,
                    TantoSname = tantoMst != null ? tantoMst.Sname : string.Empty,
                    raiinInf.UketukeTime,
                    raiinInf.KaikeiTime,
                    raiinInf.UketukeId,
                    UketukeSname = uketukeUserMst != null ? uketukeUserMst.Sname : string.Empty,
                    raiinInf.SyosaisinKbn,
                    raiinInf.UketukeSbt,
                    ptInf.PtNum,
                    PtName = ptInf.Name,
                    PtKanaName = ptInf.KanaName,
                    UketukeSbtName = uketukeSbtMstj != null ? uketukeSbtMstj.KbnName : string.Empty,
                    firstRaiin.FirstRaiinDate
                }
            );

            if (printConf.UketukeSbtIds?.Count >= 1)
            {
                //受付種別の条件指定
                joinSeikyu = joinSeikyu.Where(n => printConf.UketukeSbtIds.Contains(n.UketukeSbt));
            }

            var seikyus = joinSeikyu.ToList();

            seikyus?.ForEach(seikyu =>
            {
                result.Add
                (
                    new CoSyunoInfModel()
                    {
                        IsNyukin = !(staMonthType == 2 && seikyu.NyukinKbn >= 1) && seikyu.NyukinSeqNo != 0,  //当日入金レコードがない場合は未入金扱い
                        PreNyukinGaku = 0,
                        PreAdjustFutan = 0,

                        // anh.vu3 refactor
                        //PtFutan = seikyu.PtFutan,
                        //JihiFutan = seikyu.JihiFutan,
                        //JihiTax = seikyu.JihiTax,
                        //JihiFutanTaxFree = seikyu.JihiFutanTaxFree,
                        //JihiFutanTaxNr = seikyu.JihiFutanTaxNr,
                        //JihiFutanTaxGen = seikyu.JihiFutanTaxGen,
                        //JihiTaxNr = seikyu.JihiTaxNr,
                        //JihiTaxGen = seikyu.JihiTaxGen,
                        //JihiFutanOuttaxNr = seikyu.JihiFutanOuttaxNr,
                        //JihiFutanOuttaxGen = seikyu.JihiFutanOuttaxGen,
                        //JihiOuttaxNr = seikyu.JihiOuttaxNr,
                        //JihiOuttaxGen = seikyu.JihiOuttaxGen,
                        //KaikeiAdjustFutan = seikyu.KaikeiAdjustFutan,
                        //HokenKbn = seikyu.HokenKbn,
                        //HokenSbtCd = seikyu.HokenSbtCd,
                        //ReceSbt = seikyu.ReceSbt,
                        FirstRaiinDate = seikyu.FirstRaiinDate,
                        RaiinNo = seikyu.RaiinNo,
                        OyaRaiinNo = seikyu.OyaRaiinNo,
                        SinDate = seikyu.SinDate,
                        PtNum = seikyu.PtNum,
                        PtName = seikyu.PtName,
                        PtKanaName = seikyu.PtKanaName,
                        Tensu = seikyu.SeikyuTensu,
                        NewTensu = seikyu.NewSeikyuTensu,
                        BaseSeikyuGaku = seikyu.SeikyuGaku,
                        BaseNewSeikyuGaku = seikyu.NewSeikyuGaku,
                        NyukinKbn =
                            staMonthType == 0 && seikyu.NyukinKbn >= 1 ||
                            staMonthType == 1 && seikyu.NyukinKbn != 2 ? 0 :
                            seikyu.NyukinKbn,
                        NyukinAdjustFutan = seikyu.NyukinAdjustFutan,
                        PayCd = seikyu.NyukinPaymentMethodCd,
                        PaySName = string.Empty,
                        NyukinSortNo = seikyu.NyukinSortNo,
                        NyukinSeqNo = seikyu.NyukinSeqNo,
                        NyukinGaku = seikyu.NyukinGaku,
                        NyukinDate = seikyu.NyukinDate,
                        NyukinUserId = seikyu.NyukinUpdateId,
                        NyukinUserSname = string.Empty,
                        NyukinTime = int.Parse(seikyu.NyukinUpdateDate.ToString("HHmm")),
                        UketukeSbt = seikyu.UketukeSbt,
                        UketukeSbtName = seikyu.UketukeSbtName,
                        KaId = seikyu.KaId,
                        KaSname = seikyu.KaSname,
                        TantoId = seikyu.TantoId,
                        TantoSname = seikyu.TantoSname,
                        UketukeTime = seikyu.UketukeTime,
                        KaikeiTime = seikyu.KaikeiTime,
                        UketukeId = seikyu.UketukeId,
                        UketukeSname = seikyu.UketukeSname,
                        SyosaisinKbn = seikyu.SyosaisinKbn,
                        NyukinCmt = seikyu.NyukinCmt
                    }
                );
            });
        }
        #endregion

        // anh.vu3 refactor
        // set KaikeiInf to result data 
        var raiinNoList = result.Select(item => item.RaiinNo).Distinct().ToList();
        var kaikeiHokens = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId && raiinNoList.Contains(item.RaiinNo))
                           .AsEnumerable()
                           .GroupBy(k => new { k.HpId, k.PtId, k.RaiinNo })
                           .Select(k => k.OrderByDescending(x => x.HokenSbtCd).Take(1)).SelectMany(k => k)
                           .ToList();
        var kaikeiFutanList = kaikeiFutans.Where(item => item.HpId == hpId && raiinNoList.Contains(item.RaiinNo)).ToList();

        foreach (var syunoInf in result)
        {
            var kaikeiHokenj = kaikeiHokens.FirstOrDefault(item => item.RaiinNo == syunoInf.RaiinNo);
            var kaikeiFutanj = kaikeiFutanList.FirstOrDefault(item => item.RaiinNo == syunoInf.RaiinNo);
            syunoInf.HokenKbn = kaikeiHokenj?.HokenKbn ?? 0;
            syunoInf.HokenSbtCd = kaikeiHokenj?.HokenSbtCd ?? 0;
            syunoInf.ReceSbt = kaikeiHokenj?.ReceSbt ?? string.Empty;
            syunoInf.PtFutan = kaikeiFutanj?.PtFutan ?? 0;
            syunoInf.JihiFutan = kaikeiFutanj?.JihiFutan ?? 0;
            syunoInf.JihiTax = kaikeiFutanj?.JihiTax ?? 0;
            syunoInf.JihiFutanTaxFree = kaikeiFutanj?.JihiFutanTaxFree ?? 0;
            syunoInf.JihiFutanTaxNr = kaikeiFutanj?.JihiFutanTaxNr ?? 0;
            syunoInf.JihiFutanTaxGen = kaikeiFutanj?.JihiFutanTaxGen ?? 0;
            syunoInf.JihiTaxNr = kaikeiFutanj?.JihiTaxNr ?? 0;
            syunoInf.JihiTaxGen = kaikeiFutanj?.JihiTaxGen ?? 0;
            syunoInf.JihiFutanOuttaxNr = kaikeiFutanj?.JihiFutanOuttaxNr ?? 0;
            syunoInf.JihiFutanOuttaxGen = kaikeiFutanj?.JihiFutanOuttaxGen ?? 0;
            syunoInf.JihiOuttaxNr = kaikeiFutanj?.JihiOuttaxNr ?? 0;
            syunoInf.JihiOuttaxGen = kaikeiFutanj?.JihiOuttaxGen ?? 0;
            syunoInf.KaikeiAdjustFutan = kaikeiFutanj != null ? -kaikeiFutanj.AdjustFutan : 0;
        }
        return result;
    }

    /// <summary>
    /// 保険外金額の種別ごとの内訳を取得
    /// </summary>
    /// <param name="printConf"></param>
    /// <returns></returns>
    public List<CoJihiSbtFutan> GetJihiSbtFutan(int hpId, CoSta1001PrintConf printConf)
    {
        //入金情報をベースにする
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin
            .Where(n =>
                n.HpId == hpId &&
                n.NyukinDate >= printConf.StartNyukinDate &&
                n.NyukinDate <= printConf.EndNyukinDate &&
                n.IsDeleted == DeleteStatus.None
            )
            .GroupBy(n => new { n.HpId, n.PtId, n.RaiinNo })
            .Select(n => new { n.Key.HpId, n.Key.PtId, n.Key.RaiinNo });

        //当日入金がある来院
        var theDayNyukins = NoTrackingDataContext.SyunoNyukin
            .Where(n => n.HpId == hpId && n.SinDate == n.NyukinDate && n.IsDeleted == DeleteStatus.None)
            .Select(n => n.RaiinNo);

        //未精算or当日入金レコードがない来院の請求情報
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus
            .Where(n =>
                n.HpId == hpId &&
                n.SinDate >= printConf.StartNyukinDate &&
                n.SinDate <= printConf.EndNyukinDate &&
                (n.NyukinKbn == 0 || (n.NyukinKbn >= 1 && !theDayNyukins.Contains(n.RaiinNo)))
            )
            .GroupBy(n => new { n.HpId, n.PtId, n.RaiinNo })
            .Select(n => new { n.Key.HpId, n.Key.PtId, n.Key.RaiinNo });

        var syunoJoins = syunoNyukins.Union(syunoSeikyus).ToList();
        var sinKouiCounts = new List<SinKouiCount>();
        Parallel.ForEach(syunoJoins, syuno =>
        {
            sinKouiCounts.AddRange(NoTrackingDataContext.SinKouiCounts.Where(s => s.PtId == syuno.PtId && s.RaiinNo == syuno.RaiinNo).ToList());
        });

        var sinKouis = new List<SinKoui>();
        foreach (var item in sinKouiCounts)
        {
            sinKouis.AddRange(NoTrackingDataContext.SinKouis.Where(s => s.CdKbn == "JS" && s.IsDeleted == DeleteStatus.None && s.PtId == item.PtId && s.SinYm == item.SinYm && s.RpNo == item.RpNo && s.SeqNo == item.SeqNo).ToList());
        }

        var sinRpInfs = new List<SinRpInf>();
        foreach (var item in sinKouiCounts)
        {
            sinRpInfs.AddRange(NoTrackingDataContext.SinRpInfs.Where(s => s.IsDeleted == DeleteStatus.None && item.PtId == s.PtId && s.SinYm == item.SinYm && s.RpNo == item.RpNo && s.SanteiKbn == 2).ToList());
        }

        var jihiQuery = (
            from syunoNyukin in syunoJoins
            join sinKouiCount in sinKouiCounts on
                new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.RaiinNo } equals
                new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo }
            join sinKoui in sinKouis on
                new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinRpInf in sinRpInfs on
                new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
            group new
            {
                sinKouiCount.PtId,
                sinKouiCount.RaiinNo,
                sinKoui.JihiSbt,
                TotalTen = sinKoui.EntenKbn == 0 ? sinKoui.Ten * EntenRate.Val * sinKouiCount.Count :
                    sinKoui.Ten * sinKouiCount.Count
            } by new { sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.JihiSbt } into sinKouiGroup
            select new
            {
                sinKouiGroup.Key.PtId,
                sinKouiGroup.Key.RaiinNo,
                sinKouiGroup.Key.JihiSbt,
                JihiFutan = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen)),  //小数点以下切り捨て
            }
        );

        var result = jihiQuery.AsEnumerable().Select(
            data =>
                new CoJihiSbtFutan(
                    data.PtId,
                    data.RaiinNo,
                    data.JihiSbt,
                    data.JihiFutan
                )
        )
        .ToList();

        return result;
    }


    /// <summary>
    /// 自費種別マスタの取得
    /// </summary>
    /// <returns></returns>
    public List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId)
    {
        var jihiSbtMsts = NoTrackingDataContext.JihiSbtMsts.Where(j =>
            j.HpId == hpId &&
            j.IsDeleted == DeleteStatus.None
        )
        .OrderBy(j => j.JihiSbt)
        .ToList();

        var result = jihiSbtMsts.Select(j => new CoJihiSbtMstModel(j)).ToList();
        result.Add(
            new CoJihiSbtMstModel(
                new JihiSbtMst()
                {
                    JihiSbt = 0,
                    SortNo = 0,
                    Name = "自費算定"
                }
            ));

        return result.OrderBy(j => j.SortNo).ToList();
    }

    public string GetRaiinCmtInf(int hpId, long raiinNo)
    {
        return
            NoTrackingDataContext.RaiinCmtInfs.FirstOrDefault(r =>
                r.HpId == hpId &&
                r.RaiinNo == raiinNo &&
                r.CmtKbn == 1 &&
                r.IsDelete == DeleteStatus.None
            )?.Text ?? string.Empty;
    }

}