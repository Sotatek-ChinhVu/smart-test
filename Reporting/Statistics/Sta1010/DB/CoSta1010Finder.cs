using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1010.Models;

namespace Reporting.Statistics.Sta1010.DB;

public class CoSta1010Finder : RepositoryBase, ICoSta1010Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    public CoSta1010Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
    }

    public void ReleaseResource()
    {
        _hpInfFinder.ReleaseResource();
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    public List<CoSyunoInfModel> GetSyunoInfs(int hpId, CoSta1010PrintConf printConf)
    {
        int startDate = printConf.StartSinDate;
        int endDate = printConf.EndSinDate == 0 ? 99999999 : printConf.EndSinDate;

        //未収区分
        if ((printConf.MisyuKbns.Count) >= 1)
        {
            printConf.IncludeMisyu = printConf.MisyuKbns.Contains(1);
            printConf.IncludeMenjyo = printConf.MisyuKbns.Contains(2);
            printConf.IncludeAdjustFutan = printConf.MisyuKbns.Contains(3);
        }
        else
        {
            //すべて
            printConf.IncludeMisyu = true;
            printConf.IncludeMenjyo = true;
            printConf.IncludeAdjustFutan = true;
        }

        //請求情報
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(s => s.HpId == hpId && s.NyukinKbn != 0);  //0:未精算 を除く
        if (!printConf.IncludeMenjyo)
        {
            syunoSeikyus = syunoSeikyus.Where(s => s.NyukinKbn != 2);  //免除を除く
        }

        //入金情報
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin
            .Where(n =>
                n.HpId == hpId &&
                n.IsDeleted == DeleteStatus.None &&
                n.NyukinDate <= (printConf.IncludeOutRangeNyukin ? 99999999 : endDate) &&
                    (
                        (
                            from seikyu in syunoSeikyus
                            where
                                seikyu.SinDate >= startDate &&
                                seikyu.SinDate <= endDate
                            select seikyu
                        ).Any(
                            s =>
                                s.HpId == n.HpId &&
                                s.PtId == n.PtId &&
                                s.RaiinNo == n.RaiinNo
                        )
                    )
                )
            .GroupBy(n => new { n.HpId, n.RaiinNo })
            .Select(n =>
                new
                {
                    n.Key.HpId,
                    n.Key.RaiinNo,
                    AdjustFutan = n.Sum(x => x.AdjustFutan),
                    NyukinGaku = n.Sum(x => x.NyukinGaku)
                    //NyukinCmt = string.Join("／", n.Select(x => x.NyukinCmt))
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
        ptInfs = printConf.StartPtNum > 0 ? ptInfs.Where(p => p.PtNum >= printConf.StartPtNum) : ptInfs;
        ptInfs = printConf.EndPtNum > 0 ? ptInfs.Where(p => p.PtNum <= printConf.EndPtNum) : ptInfs;

        //来院情報
        IQueryable<RaiinInf> raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId);
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

        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId);
        //ユーザーマスタ
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted == DeleteStatus.None);
        //保険パターン
        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);
        //会計情報
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId);

        //最終来院日
        var ptLastVisits = NoTrackingDataContext.PtLastVisitDates.Where(x => x.HpId == hpId);

        var joinQuery = (
            from syunoSeikyu in syunoSeikyus
            join syunoNyukin in syunoNyukins on
                new { syunoSeikyu.HpId, syunoSeikyu.RaiinNo } equals
                new { syunoNyukin.HpId, syunoNyukin.RaiinNo }
            join ptInf in ptInfs on
                new { syunoSeikyu.HpId, syunoSeikyu.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join raiinInf in raiinInfs on
                new { syunoSeikyu.HpId, syunoSeikyu.PtId, syunoSeikyu.SinDate, syunoSeikyu.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
            join kaMst in kaMsts on
                new { raiinInf.HpId, raiinInf.KaId } equals
                new { kaMst.HpId, kaMst.KaId } into kaMstJoin
            from kaMstj in kaMstJoin.DefaultIfEmpty()
            join userMst in userMsts on
                new { raiinInf.HpId, raiinInf.TantoId } equals
                new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
            from tantoMst in userMstJoin.DefaultIfEmpty()
            join ptHokenPattern in ptHokenPatterns on
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join kaikeiInf in kaikeiInfs on
                new { raiinInf.HpId, raiinInf.RaiinNo, ptHokenPattern.HokenId } equals
                new { kaikeiInf.HpId, kaikeiInf.RaiinNo, kaikeiInf.HokenId }
            join ptLastVisit in ptLastVisits on
                new { syunoSeikyu.HpId, syunoSeikyu.PtId } equals
                new { ptLastVisit.HpId, ptLastVisit.PtId } into ptLastVisitJoin
            from ptLastVisitj in ptLastVisitJoin.DefaultIfEmpty()
            select new
            {
                syunoSeikyu,
                syunoNyukin,
                ptInf,
                raiinInf,
                kaMstj,
                tantoMst,
                kaikeiInf,
                ptLastVisitj
            }
        );

        //請求金額に変更がある患者
        if (printConf.IsDiffSeikyu)
        {
            joinQuery = joinQuery.Where(q => q.syunoSeikyu.NewSeikyuGaku != q.syunoSeikyu.SeikyuGaku);
        }
        //未収金がある患者
        joinQuery = printConf.IsNewSeikyu ?
            joinQuery.Where(q =>
                (printConf.IncludeMisyu && q.syunoSeikyu.NewSeikyuGaku - q.syunoNyukin.AdjustFutan != q.syunoNyukin.NyukinGaku) ||
                q.syunoSeikyu.NyukinKbn == 2 ||
                (printConf.IncludeAdjustFutan && (q.syunoSeikyu.NewAdjustFutan != 0 || q.syunoNyukin.AdjustFutan != 0))
            ) :
            joinQuery.Where(q =>
                (printConf.IncludeMisyu && q.syunoSeikyu.SeikyuGaku - q.syunoNyukin.AdjustFutan != q.syunoNyukin.NyukinGaku) ||
                q.syunoSeikyu.NyukinKbn == 2 ||
                (printConf.IncludeAdjustFutan && (q.syunoSeikyu.AdjustFutan != 0 || q.syunoNyukin.AdjustFutan != 0))
            );

        var result = joinQuery.AsEnumerable().Select(
            data =>
                new CoSyunoInfModel(
                    printConf.IsNewSeikyu,
                    printConf.IncludeAdjustFutan,
                    data.syunoSeikyu,
                    data.syunoNyukin.AdjustFutan,
                    data.syunoNyukin.NyukinGaku,
                    data.ptInf,
                    data.raiinInf,
                    data.kaMstj,
                    data.tantoMst,
                    data.kaikeiInf.HokenKbn,
                    data.kaikeiInf.HokenSbtCd,
                    data.kaikeiInf.ReceSbt ?? string.Empty,
                    data.ptLastVisitj.LastVisitDate
                )
        )
        .ToList();

        #region '未精算の来院を未収とする'
        if (printConf.IncludeUnpaid)
        {
            var syunoUnpaids = NoTrackingDataContext.SyunoSeikyus.Where(s =>
                s.HpId == hpId &&
                s.SinDate >= startDate &&
                s.SinDate <= endDate &&
                s.NyukinKbn == 0
            );

            var joinUnpaid = (
                from syunoUnpaid in syunoUnpaids
                join ptInf in ptInfs on
                    new { syunoUnpaid.HpId, syunoUnpaid.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join raiinInf in raiinInfs on
                    new { syunoUnpaid.HpId, syunoUnpaid.PtId, syunoUnpaid.SinDate, syunoUnpaid.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                from kaMstj in kaMstJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                from tantoMst in userMstJoin.DefaultIfEmpty()
                join ptHokenPattern in ptHokenPatterns on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join kaikeiInf in kaikeiInfs on
                    new { raiinInf.HpId, raiinInf.RaiinNo, ptHokenPattern.HokenId } equals
                    new { kaikeiInf.HpId, kaikeiInf.RaiinNo, kaikeiInf.HokenId }
                join ptLastVisit in ptLastVisits on
                    new { syunoUnpaid.HpId, syunoUnpaid.PtId } equals
                    new { ptLastVisit.HpId, ptLastVisit.PtId } into ptLastVisitJoin
                from ptLastVisitj in ptLastVisitJoin.DefaultIfEmpty()
                select new
                {
                    syunoUnpaid,
                    ptInf,
                    raiinInf,
                    kaMstj,
                    tantoMst,
                    kaikeiInf,
                    ptLastVisitj
                }
            );

            var retUnpaid = joinUnpaid.AsEnumerable().Select(
                data =>
                    new CoSyunoInfModel(
                        printConf.IsNewSeikyu,
                        printConf.IncludeAdjustFutan,
                        data.syunoUnpaid,
                        0,
                        0,
                        data.ptInf,
                        data.raiinInf,
                        data.kaMstj,
                        data.tantoMst,
                        data.kaikeiInf.HokenKbn,
                        data.kaikeiInf.HokenSbtCd,
                        data.kaikeiInf.ReceSbt ?? string.Empty,
                        data.ptLastVisitj.LastVisitDate
                    )
            )
            .ToList();

            result.AddRange(retUnpaid);
        }
        #endregion

        //コメント付与
        var nyukinCmts = NoTrackingDataContext.SyunoNyukin.Where(n =>
            n.HpId == hpId &&
            n.IsDeleted == DeleteStatus.None &&
            n.NyukinDate >= startDate &&
            n.NyukinDate <= endDate &&
            (n.NyukinCmt ?? "") != "")
        .ToList();

        foreach (var r in result)
        {
            r.NyukinCmt =
                string.Join("／", nyukinCmts.Where(n => n.HpId == hpId && n.RaiinNo == r.RaiinNo).Select(n => n.NyukinCmt));
        }

        return result;
    }
}
