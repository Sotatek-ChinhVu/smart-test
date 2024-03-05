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
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(s => s.HpId == hpId && s.IsDeleted == DeleteStatus.None);

        //支払方法
        var payMsts = NoTrackingDataContext.PaymentMethodMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);
        //請求情報
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(s => s.HpId == hpId && s.NyukinKbn != 0);  //0:未精算を除く
        var kaikeiFutans = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId)
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
            p => p.HpId == hpId && p.IsDelete == DeleteStatus.None
        );
        if (!printConf.IsTester)
        {
            ptInfs = ptInfs.Where(p => p.IsTester == 0);
        }
        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Where(x => x.HpId == hpId);
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts.Where(k => k.HpId == hpId && k.IsDeleted == DeleteStatus.None);
        //ユーザーマスタ
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted == DeleteStatus.None);

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

        var joinQuery = (
            from syunoNyukin in syunoNyukins
            join syunoSeikyu in syunoSeikyus on
                    new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
                    new { syunoSeikyu.HpId, syunoSeikyu.RaiinNo }
            join raiinInf in raiinInfs on
      new { syunoNyukin.HpId, syunoNyukin.RaiinNo } equals
      new { raiinInf.HpId, raiinInf.RaiinNo }
            join firstRaiin in firstRaiins on
                new { syunoNyukin.HpId, syunoNyukin.PtId } equals
                new { firstRaiin.HpId, firstRaiin.PtId }
            join ptInf in ptInfs on
   new { syunoNyukin.HpId, syunoNyukin.PtId } equals
   new { ptInf.HpId, ptInf.PtId }
            join payMst in payMsts on
            new { syunoNyukin.HpId, syunoNyukin.PaymentMethodCd } equals
            new { payMst.HpId, payMst.PaymentMethodCd } into payMstJoin
            from payMstj in payMstJoin.DefaultIfEmpty()
            join uketukeSbtMst in uketukeSbtMsts on
              new { syunoNyukin.HpId, syunoNyukin.UketukeSbt } equals
              new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeSbtMstJoin
            from uketukeSbtMstj in uketukeSbtMstJoin.DefaultIfEmpty()
            join userMst in userMsts on
                new { syunoNyukin.HpId, syunoNyukin.UpdateId } equals
                new { userMst.HpId, UpdateId = userMst.UserId } into nyukinUserMstJoin
            from nyukinUserMst in nyukinUserMstJoin.DefaultIfEmpty()
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
            where
                    syunoNyukin.HpId == hpId &&
                    (
                        printConf.StartNyukinDate == printConf.EndNyukinDate ?
                            (syunoNyukin.NyukinDate == printConf.StartNyukinDate) :
                            (syunoNyukin.NyukinDate >= printConf.StartNyukinDate && syunoNyukin.NyukinDate <= printConf.EndNyukinDate)
                    ) &&
                    //syunoNyukin.NyukinDate >= printConf.StartNyukinDate &&   開始日と終了日が同じ場合に著しく遅くなる環境があるため
                    //syunoNyukin.NyukinDate <= printConf.EndNyukinDate &&
                    syunoNyukin.IsDeleted == DeleteStatus.None
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

                // anh.vu3 refactor
                //PtFutan = kaikeiFutanj.PtFutan != null ? kaikeiFutanj.PtFutan : 0,
                //JihiFutan = kaikeiFutanj.JihiFutan != null ? kaikeiFutanj.JihiFutan : 0,
                //JihiTax = kaikeiFutanj.JihiTax != null ? kaikeiFutanj.JihiTax : 0,
                //JihiFutanTaxFree = kaikeiFutanj.JihiFutanTaxFree != null ? kaikeiFutanj.JihiFutanTaxFree : 0,
                //JihiFutanTaxNr = kaikeiFutanj.JihiFutanTaxNr != null ? kaikeiFutanj.JihiFutanTaxNr : 0,
                //JihiFutanTaxGen = kaikeiFutanj.JihiFutanTaxGen != null ? kaikeiFutanj.JihiFutanTaxGen : 0,
                //JihiTaxNr = kaikeiFutanj.JihiTaxNr != null ? kaikeiFutanj.JihiTaxNr : 0,
                //JihiTaxGen = kaikeiFutanj.JihiTaxGen != null ? kaikeiFutanj.JihiTaxGen : 0,
                //JihiFutanOuttaxNr = kaikeiFutanj.JihiFutanOuttaxNr != null ? kaikeiFutanj.JihiFutanOuttaxNr : 0,
                //JihiFutanOuttaxGen = kaikeiFutanj.JihiFutanOuttaxGen != null ? kaikeiFutanj.JihiFutanOuttaxGen : 0,
                //JihiOuttaxNr = kaikeiFutanj.JihiOuttaxNr != null ? kaikeiFutanj.JihiOuttaxNr : 0,
                //JihiOuttaxGen = kaikeiFutanj.JihiOuttaxGen != null ? kaikeiFutanj.JihiOuttaxGen : 0,
                //KaikeiAdjustFutan = kaikeiFutanj.AdjustFutan != null ? -kaikeiFutanj.AdjustFutan : 0,
                //HokenKbn = kaikeiHokenj != null ? kaikeiHokenj.HokenKbn : 0,
                //HokenSbtCd = kaikeiHokenj != null ? kaikeiHokenj.HokenSbtCd : 0,
                //ReceSbt = kaikeiHokenj != null ? kaikeiHokenj.ReceSbt : string.Empty,
                PaySname = payMstj.PaySname != null ? payMstj.PaySname : string.Empty,
                raiinInf.OyaRaiinNo,
                raiinInf.KaId,
                KaSname = kaMstj.KaSname != null ? kaMstj.KaSname : string.Empty,
                raiinInf.TantoId,
                TantoSname = tantoMst.Sname != null ? tantoMst.Sname : string.Empty,
                raiinInf.UketukeTime,
                raiinInf.KaikeiTime,
                raiinInf.UketukeId,
                UketukeSname = uketukeUserMst.Sname != null ? uketukeUserMst.Sname : string.Empty,
                raiinInf.SyosaisinKbn,
                ptInf.PtNum,
                PtName = ptInf.Name,
                PtKanaName = ptInf.KanaName,
                UketukeSbtName = uketukeSbtMstj.KbnName != null ? uketukeSbtMstj.KbnName : string.Empty,
                NyukinUserSname = nyukinUserMst.Sname != null ? nyukinUserMst.Sname : string.Empty,
                firstRaiin.FirstRaiinDate
            }
        );
        var joinList = joinQuery.ToList();

        if (printConf.UketukeSbtIds?.Count >= 1)
        {
            //受付種別の条件指定
            joinList = joinList.Where(n => printConf.UketukeSbtIds.Contains(n.NyukinUketukeSbt)).ToList();
        }
        if (printConf.PaymentMethodCds?.Count >= 1)
        {
            //支払区分の条件指定
            joinList = joinList.Where(n => printConf.PaymentMethodCds.Contains(n.NyukinPaymentMethodCd)).ToList();
        }

        var result = joinList.Select(
            data =>
                new CoSyunoInfModel()
                {
                    IsNyukin = true,
                    PreNyukinGaku = 0,
                    PreAdjustFutan = 0,

                    // anh.vu3 refactor
                    //PtFutan = data.PtFutan,
                    //JihiFutan = data.JihiFutan,
                    //JihiTax = data.JihiTax,
                    //JihiFutanTaxFree = data.JihiFutanTaxFree,
                    //JihiFutanTaxNr = data.JihiFutanTaxNr,
                    //JihiFutanTaxGen = data.JihiFutanTaxGen,
                    //JihiTaxNr = data.JihiTaxNr,
                    //JihiTaxGen = data.JihiTaxGen,
                    //JihiFutanOuttaxNr = data.JihiFutanOuttaxNr,
                    //JihiFutanOuttaxGen = data.JihiFutanOuttaxGen,
                    //JihiOuttaxNr = data.JihiOuttaxNr,
                    //JihiOuttaxGen = data.JihiOuttaxGen,
                    //KaikeiAdjustFutan = data.KaikeiAdjustFutan,
                    //HokenKbn = data.HokenKbn,
                    //HokenSbtCd = data.HokenSbtCd,
                    //ReceSbt = data.ReceSbt,
                    FirstRaiinDate = data.FirstRaiinDate,
                    RaiinNo = data.RaiinNo,
                    OyaRaiinNo = data.OyaRaiinNo,
                    SinDate = data.SinDate,
                    PtNum = data.PtNum,
                    PtName = data.PtName,
                    PtKanaName = data.PtKanaName,
                    Tensu = data.SeikyuTensu,
                    NewTensu = data.NewSeikyuTensu,
                    BaseSeikyuGaku = data.SeikyuGaku,
                    BaseNewSeikyuGaku = data.NewSeikyuGaku,
                    NyukinKbn = data.NyukinKbn,
                    NyukinAdjustFutan = data.NyukinAdjustFutan,
                    PayCd = data.NyukinPaymentMethodCd,
                    PaySName = data.PaySname,
                    NyukinSortNo = data.NyukinSortNo,
                    NyukinSeqNo = data.NyukinSeqNo,
                    NyukinGaku = data.NyukinGaku,
                    NyukinDate = data.NyukinDate,
                    NyukinUserId = data.NyukinUpdateId,
                    NyukinUserSname = data.NyukinUserSname,
                    NyukinTime = int.Parse(data.NyukinUpdateDate.ToString("HHmm")),
                    UketukeSbt = data.NyukinUketukeSbt,
                    UketukeSbtName = data.UketukeSbtName,
                    KaId = data.KaId,
                    KaSname = data.KaSname,
                    TantoId = data.TantoId,
                    TantoSname = data.TantoSname,
                    UketukeTime = data.UketukeTime,
                    KaikeiTime = data.KaikeiTime,
                    UketukeId = data.UketukeId,
                    UketukeSname = data.UketukeSname,
                    SyosaisinKbn = data.SyosaisinKbn,
                    NyukinCmt = data.NyukinCmt
                }
        )
        .ToList();

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

                join raiinInf in raiinInfs on
                    new { unSeikyu.HpId, unSeikyu.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join firstRaiin in firstRaiins on
                    new { unSeikyu.HpId, unSeikyu.PtId } equals
                    new { firstRaiin.HpId, firstRaiin.PtId }
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
                join syunoNyukin in syunoNyukins on
          new { unSeikyu.HpId, unSeikyu.RaiinNo } equals
          new { syunoNyukin.HpId, syunoNyukin.RaiinNo } into syunoNyukinJoin
                from syunoNyukinj in syunoNyukinJoin.DefaultIfEmpty()
                select new
                {
                    unSeikyu.RaiinNo,
                    unSeikyu.SinDate,
                    NyukinSeqNo = syunoNyukinj.SeqNo == null ? 0 : syunoNyukinj.SeqNo,
                    NyukinAdjustFutan = syunoNyukinj.AdjustFutan == null ? 0 : syunoNyukinj.AdjustFutan,
                    NyukinPaymentMethodCd = syunoNyukinj.PaymentMethodCd == null ? 0 : syunoNyukinj.PaymentMethodCd,
                    NyukinSortNo = syunoNyukinj.SortNo == null ? 0 : syunoNyukinj.SortNo,
                    NyukinGaku = syunoNyukinj.NyukinGaku == null ? 0 : syunoNyukinj.NyukinGaku,
                    NyukinDate = syunoNyukinj.NyukinDate == null ? 0 : syunoNyukinj.NyukinDate,
                    NyukinUpdateId = syunoNyukinj.UpdateId == null ? 0 : syunoNyukinj.UpdateId,
                    NyukinCmt = syunoNyukinj.NyukinCmt == null ? string.Empty : syunoNyukinj.NyukinCmt,
                    NyukinUpdateDate = syunoNyukinj.UpdateDate == null ? DateTime.MinValue : syunoNyukinj.UpdateDate,
                    unSeikyu.NyukinKbn,
                    unSeikyu.SeikyuTensu,
                    unSeikyu.NewSeikyuTensu,
                    unSeikyu.SeikyuGaku,
                    unSeikyu.NewSeikyuGaku,
                    //PtFutan = kaikeiFutanj.PtFutan != null ? kaikeiFutanj.PtFutan : 0,
                    //JihiFutan = kaikeiFutanj.JihiFutan != null ? kaikeiFutanj.JihiFutan : 0,
                    //JihiTax = kaikeiFutanj.JihiTax != null ? kaikeiFutanj.JihiTax : 0,
                    //JihiFutanTaxFree = kaikeiFutanj.JihiFutanTaxFree != null ? kaikeiFutanj.JihiFutanTaxFree : 0,
                    //JihiFutanTaxNr = kaikeiFutanj.JihiFutanTaxNr != null ? kaikeiFutanj.JihiFutanTaxNr : 0,
                    //JihiFutanTaxGen = kaikeiFutanj.JihiFutanTaxGen != null ? kaikeiFutanj.JihiFutanTaxGen : 0,
                    //JihiTaxNr = kaikeiFutanj.JihiTaxNr != null ? kaikeiFutanj.JihiTaxNr : 0,
                    //JihiTaxGen = kaikeiFutanj.JihiTaxGen != null ? kaikeiFutanj.JihiTaxGen : 0,
                    //JihiFutanOuttaxNr = kaikeiFutanj.JihiFutanOuttaxNr != null ? kaikeiFutanj.JihiFutanOuttaxNr : 0,
                    //JihiFutanOuttaxGen = kaikeiFutanj.JihiFutanOuttaxGen != null ? kaikeiFutanj.JihiFutanOuttaxGen : 0,
                    //JihiOuttaxNr = kaikeiFutanj.JihiOuttaxNr != null ? kaikeiFutanj.JihiOuttaxNr : 0,
                    //JihiOuttaxGen = kaikeiFutanj.JihiOuttaxGen != null ? kaikeiFutanj.JihiOuttaxGen : 0,
                    //KaikeiAdjustFutan = kaikeiFutanj.AdjustFutan != null ? -kaikeiFutanj.AdjustFutan : 0,
                    //HokenKbn = kaikeiHokenj != null ? kaikeiHokenj.HokenKbn : 0,
                    //HokenSbtCd = kaikeiHokenj != null ? kaikeiHokenj.HokenSbtCd : 0,
                    //ReceSbt = kaikeiHokenj != null ? kaikeiHokenj.ReceSbt : string.Empty,
                    raiinInf.OyaRaiinNo,
                    raiinInf.KaId,
                    KaSname = kaMstj.KaSname != null ? kaMstj.KaSname : string.Empty,
                    raiinInf.TantoId,
                    TantoSname = tantoMst.Sname != null ? tantoMst.Sname : string.Empty,
                    raiinInf.UketukeTime,
                    raiinInf.KaikeiTime,
                    raiinInf.UketukeId,
                    UketukeSname = uketukeUserMst.Sname != null ? uketukeUserMst.Sname : string.Empty,
                    raiinInf.SyosaisinKbn,
                    raiinInf.UketukeSbt,
                    ptInf.PtNum,
                    PtName = ptInf.Name,
                    PtKanaName = ptInf.KanaName,
                    UketukeSbtName = uketukeSbtMstj.KbnName != null ? uketukeSbtMstj.KbnName : string.Empty,
                    firstRaiin.FirstRaiinDate
                }
            );
            var seikyus = joinSeikyu.ToList();
            if (printConf.UketukeSbtIds?.Count >= 1)
            {
                //受付種別の条件指定
                seikyus = seikyus.Where(n => printConf.UketukeSbtIds.Contains(n.UketukeSbt)).ToList();
            }

            seikyus?.ForEach(seikyu =>
            {
                result.Add
                (
                    new CoSyunoInfModel()
                    {
                        IsNyukin = !(staMonthType == 2 && seikyu.NyukinKbn >= 1) && seikyu.NyukinSeqNo != 0,  //当日入金レコードがない場合は未入金扱い
                        PreNyukinGaku = 0,
                        PreAdjustFutan = 0,
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
            .Select(n => n.RaiinNo).ToList();

        //未精算or当日入金レコードがない来院の請求情報
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus
            .Where(n =>
                n.HpId == hpId &&
                n.SinDate >= printConf.StartNyukinDate &&
                n.SinDate <= printConf.EndNyukinDate &&
                (n.NyukinKbn == 0 || (n.NyukinKbn >= 1))
            )
            .GroupBy(n => new { n.HpId, n.PtId, n.RaiinNo })
            .Select(n => new { n.Key.HpId, n.Key.PtId, n.Key.RaiinNo });

        var syunoJoins = syunoNyukins.Union(syunoSeikyus);

        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(x => x.HpId == hpId);
        var sinKouis = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);
        var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);

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
            where
                sinKoui.CdKbn == "JS" || sinRpInf.SanteiKbn == 2
            select new
            {
                sinKouiCount.PtId,
                sinKouiCount.RaiinNo,
                sinKoui.JihiSbt,
                sinKoui.Ten,
                sinKoui.EntenKbn,
                EntenRate.Val,
                sinKouiCount.Count,

            });
        var jihiList = jihiQuery.ToList();
        jihiList = jihiList.Where(j => !theDayNyukins.Contains(j.RaiinNo)).ToList();

        var jihiGroup = from jihi in jihiList
                        group new
                        {
                            jihi.PtId,
                            jihi.RaiinNo,
                            jihi.JihiSbt,
                            TotalTen = jihi.EntenKbn == 0 ? jihi.Ten * EntenRate.Val * jihi.Count :
                            jihi.Ten * jihi.Count
                        } by new { jihi.PtId, jihi.RaiinNo, jihi.JihiSbt } into sinKouiGroup
                        select new
                        {
                            sinKouiGroup.Key.PtId,
                            sinKouiGroup.Key.RaiinNo,
                            sinKouiGroup.Key.JihiSbt,
                            JihiFutan = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen)),  //小数点以下切り捨て
                        };

        var result = jihiGroup.Select(
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