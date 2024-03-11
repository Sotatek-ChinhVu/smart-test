using Reporting.Statistics.DB;
using Reporting.Statistics.Sta3062.Models;
using Reporting.Statistics.Model;
using Infrastructure.Interfaces;
using Infrastructure.Base;
using Domain.Constant;
using Entity.Tenant;
using System.Linq;

namespace Reporting.Statistics.Sta3062.DB;

public class CoSta3062Finder : RepositoryBase, ICoSta3062Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;


    public CoSta3062Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    /// <summary>
    /// 診療情報取得
    /// </summary>
    /// <param name="printConf"></param>
    /// <returns></returns>
    public List<CoKouiTensuModel> GetKouiTensu(int hpId, CoSta3062PrintConf printConf)
    {
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(item => item.HpId == hpId
                                                                              && item.SinYm >= printConf.StartSinYm
                                                                              && item.SinYm <= printConf.EndSinYm)
                                                               .ToList();
        var ptIdList = sinKouiCounts.Select(item => item.PtId).Distinct().ToList();
        var sinYmList = sinKouiCounts.Select(item => item.SinYm).Distinct().ToList();
        var rpNoList = sinKouiCounts.Select(item => item.RpNo).Distinct().ToList();
        var seqNoList = sinKouiCounts.Select(item => item.SeqNo).Distinct().ToList();
        var sinDateList = sinKouiCounts.Select(item => item.SinDate).Distinct().ToList();
        var raiinNoList = sinKouiCounts.Select(item => item.RaiinNo).Distinct().ToList();

        var sinKouisRawQuery = NoTrackingDataContext.SinKouis.Where(item => item.HpId == hpId && item.IsDeleted == DeleteStatus.None);
        var sinKouis = sinKouisRawQuery.Where(item => ptIdList.Contains(item.PtId)
                                                      && sinYmList.Contains(item.SinYm)
                                                      && rpNoList.Contains(item.RpNo)
                                                      && seqNoList.Contains(item.SeqNo)
                                       ).ToList();
        var hokenIdList = sinKouis.Select(item => item.HokenId).Distinct().ToList();
        var hokenPidList = sinKouis.Select(item => item.HokenPid).Distinct().ToList();

        var sinKouiRpInfs = NoTrackingDataContext.SinRpInfs.Where(item => item.HpId == hpId
                                                                          && item.IsDeleted == DeleteStatus.None
                                                                          && ptIdList.Contains(item.PtId)
                                                                          && sinYmList.Contains(item.SinYm)
                                                                          && rpNoList.Contains(item.RpNo))
                                                           .ToList();

        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId && item.IsDelete == DeleteStatus.None);
        ptInfs = !printConf.IsTester ? ptInfs.Where(item => item.IsTester == 0) : ptInfs;
        var ptInfList = ptInfs.Where(item => ptIdList.Contains(item.PtId)).ToList();

        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(item => item.HpId == hpId && item.IsDeleted == DeleteStatus.None && ptIdList.Contains(item.PtId)).ToList();
        #region 条件指定(患者グループ)
        bool isPtGrp = false;
        if (printConf.PtGrps?.Count >= 1)
        {
            isPtGrp = true;
            foreach (var ptGrp in printConf.PtGrps)
            {
                var curPtGrps = ptGrpInfs.Where(p => p.GroupId == ptGrp.GrpId && p.GroupCode == ptGrp.GrpCode);
                ptGrpInfs = (
                    from ptGrpInf in ptGrpInfs
                    where
                        (
                            from p in curPtGrps
                            select p
                        ).Any(
                            p =>
                                p.HpId == ptGrpInf.HpId &&
                                p.PtId == ptGrpInf.PtId
                )
                    select
                            ptGrpInf
                ).ToList();
            }
        }
        #endregion
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId && item.Status >= 5 && item.IsDeleted == DeleteStatus.None);
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId
                                                                        && ptIdList.Contains(item.PtId)
                                                                        && sinDateList.Contains(item.SinDate)
                                                                        && raiinNoList.Contains(item.RaiinNo)
                                                                        && hokenIdList.Contains(item.HokenId)
                                                         ).ToList();
        #region 条件指定
        //診療科
        if (printConf.KaIds?.Count >= 1)
        {
            raiinInfs = raiinInfs.Where(r => printConf.KaIds.Contains(r.KaId));
        }
        //担当医
        if (printConf.TantoIds?.Count >= 1)
        {
            raiinInfs = raiinInfs.Where(r => printConf.TantoIds.Contains(r.TantoId));
        }
        var raiinInfList = raiinInfs.Where(item => raiinNoList.Contains(item.RaiinNo)).ToList();
        var kaIdList = raiinInfList.Select(item => item.KaId).Distinct().ToList();
        var tantoIdList = raiinInfList.Select(item => item.TantoId).Distinct().ToList();
        #endregion

        var ptHokenPatternRawQuery = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId);
        var ptHokenInfRawQuery = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId);
        var ptKohis = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId);

        #region 条件指定(保険種別)
        if (printConf.HokenSbts?.Count >= 1)
        {
            //保険種別
            List<int> hokenKbns = new List<int>();
            if (printConf.HokenSbts.Contains(1)) hokenKbns.Add(1);                                              //社保
            if (printConf.HokenSbts.Contains(2) || printConf.HokenSbts.Contains(3)) hokenKbns.Add(2);           //国保・後期
            if (printConf.HokenSbts.Contains(10)) { hokenKbns.Add(11); hokenKbns.Add(12); hokenKbns.Add(13); }  //労災
            if (printConf.HokenSbts.Contains(11)) hokenKbns.Add(14);                                            //自賠
            if (printConf.HokenSbts.Contains(0) || printConf.HokenSbts.Contains(12)) hokenKbns.Add(0);          //自費・自レ

            ptHokenPatternRawQuery = ptHokenPatternRawQuery.Where(r => hokenKbns.Contains(r.HokenKbn));

            if (printConf.HokenSbts.Contains(2) && !printConf.HokenSbts.Contains(3))
            {
                //後期を除く
                ptHokenPatternRawQuery = ptHokenPatternRawQuery.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 == 3));
            }
            else if (!printConf.HokenSbts.Contains(2) && printConf.HokenSbts.Contains(3))
            {
                //国保一般・退職を除く
                ptHokenPatternRawQuery = ptHokenPatternRawQuery.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 != 3));
            }

            if (printConf.HokenSbts.Contains(0) && !printConf.HokenSbts.Contains(12))
            {
                //自費レセを除く
                ptHokenInfRawQuery = ptHokenInfRawQuery.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "109"));
            }
            else if (!printConf.HokenSbts.Contains(0) && printConf.HokenSbts.Contains(12))
            {
                //自費を除く
                ptHokenInfRawQuery = ptHokenInfRawQuery.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "108"));
            }
        }
        #endregion

        #region 公費 単独併用判断
        var ptKohiPatternRawQuery = (
            from ptHokenPattern in ptHokenPatternRawQuery
            join ptKohi in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                new { ptKohi.HpId, ptKohi.PtId, Kohi1Id = ptKohi.HokenId } into ptKohi1j
            from ptKohi1 in ptKohi1j.DefaultIfEmpty()
            select new
            {
                ptHokenPattern.HpId,
                ptHokenPattern.PtId,
                ptHokenPattern.HokenPid,
                ptHokenPattern.HokenId,
                ptHokenPattern.HokenKbn,
                HokenSbtCd1 = ptHokenPattern.HokenSbtCd / 100,
                KohiId = ptKohi1.HokenSbtKbn == 2 ? 0 : ptHokenPattern.Kohi1Id
            }
        );

        var ptKohiPattern2 = (
            from ptHokenPattern in ptHokenPatternRawQuery
            join ptKohi in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                new { ptKohi.HpId, ptKohi.PtId, Kohi2Id = ptKohi.HokenId } into ptKohi2j
            from ptKohi2 in ptKohi2j.DefaultIfEmpty()
            select new
            {
                ptHokenPattern.HpId,
                ptHokenPattern.PtId,
                ptHokenPattern.HokenPid,
                ptHokenPattern.HokenId,
                ptHokenPattern.HokenKbn,
                HokenSbtCd1 = ptHokenPattern.HokenSbtCd / 100,
                KohiId = ptKohi2.HokenSbtKbn == 2 ? 0 : ptHokenPattern.Kohi2Id
            }
        );
        ptKohiPatternRawQuery.Union(ptKohiPattern2);

        var ptKohiPattern3 = (
            from ptHokenPattern in ptHokenPatternRawQuery
            join ptKohi in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                new { ptKohi.HpId, ptKohi.PtId, Kohi3Id = ptKohi.HokenId } into ptKohi3j
            from ptKohi3 in ptKohi3j.DefaultIfEmpty()
            select new
            {
                ptHokenPattern.HpId,
                ptHokenPattern.PtId,
                ptHokenPattern.HokenPid,
                ptHokenPattern.HokenId,
                ptHokenPattern.HokenKbn,
                HokenSbtCd1 = ptHokenPattern.HokenSbtCd / 100,
                KohiId = ptKohi3.HokenSbtKbn == 2 ? 0 : ptHokenPattern.Kohi3Id
            }
        );
        ptKohiPatternRawQuery.Union(ptKohiPattern3);

        var ptKohiPattern4 = (
            from ptHokenPattern in ptHokenPatternRawQuery
            join ptKohi in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                new { ptKohi.HpId, ptKohi.PtId, Kohi4Id = ptKohi.HokenId } into ptKohi4j
            from ptKohi4 in ptKohi4j.DefaultIfEmpty()
            select new
            {
                ptHokenPattern.HpId,
                ptHokenPattern.PtId,
                ptHokenPattern.HokenPid,
                ptHokenPattern.HokenId,
                ptHokenPattern.HokenKbn,
                HokenSbtCd1 = ptHokenPattern.HokenSbtCd / 100,
                KohiId = ptKohi4.HokenSbtKbn == 2 ? 0 : ptHokenPattern.Kohi4Id
            }
        );
        ptKohiPatternRawQuery.Union(ptKohiPattern4);

        var ptKohiJoins = (
            from sinKoui in sinKouisRawQuery
            join ptKohiPattern in ptKohiPatternRawQuery on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenPid }
            join ptHokenInf in ptHokenInfRawQuery on
                new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            where
                sinKoui.HpId == hpId &&
                sinKoui.SinYm >= printConf.StartSinYm &&
                sinKoui.SinYm <= printConf.EndSinYm
            group
                new { sinKoui, ptKohiPattern } by
                new
                {
                    sinKoui.HpId,
                    sinKoui.PtId,
                    sinKoui.SinYm,
                    sinKoui.HokenId,
                    ptHokenInf.HonkeKbn,
                    ptHokenInf.Houbetu,
                    ptKohiPattern.HokenKbn,
                    ptKohiPattern.HokenSbtCd1
                } into kohiGroupj
            select new
            {
                kohiGroupj.Key.HpId,
                kohiGroupj.Key.PtId,
                kohiGroupj.Key.SinYm,
                kohiGroupj.Key.HokenId,
                kohiGroupj.Key.HonkeKbn,
                kohiGroupj.Key.Houbetu,
                kohiGroupj.Key.HokenKbn,
                kohiGroupj.Key.HokenSbtCd1,
                KohiCount = kohiGroupj.Where(x => x.ptKohiPattern.KohiId != 0).Select(x => x.ptKohiPattern.KohiId).Distinct().Count()
            }
        ).ToList();
        #endregion

        var ptHokenPatternList = ptHokenPatternRawQuery.Where(item => ptIdList.Contains(item.PtId)
                                                                      && hokenPidList.Contains(item.HokenPid))
                                                       .ToList();
        var ptHokenInfList = ptHokenInfRawQuery.Where(item => ptIdList.Contains(item.PtId)
                                                              && hokenIdList.Contains(item.HokenId))
                                               .ToList();

        var kaMsts = NoTrackingDataContext.KaMsts.Where(item => item.HpId == hpId && kaIdList.Contains(item.KaId)).ToList();
        var userMsts = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteStatus.None && tantoIdList.Contains(item.UserId)).ToList();

        var joinQuery = (
            from sinCount in sinKouiCounts
            join sinKoui in sinKouis on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinRp in sinKouiRpInfs on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo }
            join kaikeiInf in kaikeiInfs on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinDate, sinCount.RaiinNo, sinKoui.HokenId } equals
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo, kaikeiInf.HokenId }
            join ptInf in ptInfList on
                new { sinCount.HpId, sinCount.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join raiinInf in raiinInfList on
                new { sinCount.HpId, sinCount.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.RaiinNo }
            join ptHokenPattern in ptHokenPatternList on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join ptHokenInf in ptHokenInfList on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            join kaMst in kaMsts on
                new { raiinInf.HpId, raiinInf.KaId } equals
                new { kaMst.HpId, kaMst.KaId } into kaMstJoin
            from kaMstj in kaMstJoin.DefaultIfEmpty()
            join userMst in userMsts on
                new { raiinInf.HpId, raiinInf.TantoId } equals
                new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
            from tantoMst in userMstJoin.DefaultIfEmpty()
            where
            #region 患者グループ条件
                (
                    (!isPtGrp) ||
                    (
                        from pg in ptGrpInfs
                        select pg
                    ).Any(
                        p =>
                            p.HpId == sinCount.HpId &&
                            p.PtId == sinCount.PtId
                    )
                )
            #endregion
            group
                new { sinCount, sinKoui, sinRp, raiinInf, kaMstj, tantoMst, ptHokenInf, kaikeiInf, ptInf } by
                new
                {
                    sinCount.PtId,
                    sinCount.RaiinNo,
                    sinCount.SinDate,
                    raiinInf.SyosaisinKbn,
                    raiinInf.KaId,
                    raiinInf.TantoId,
                    ptHokenInf.HokenId
                } into sinGroupj
            select new
            {
                sinGroupj.Key.PtId,
                sinGroupj.Key.RaiinNo,
                sinGroupj.Key.SinDate,
                sinGroupj.Key.SyosaisinKbn,
                sinGroupj.Key.KaId,
                KaSname = sinGroupj.Max(x => x?.kaMstj?.KaSname ?? string.Empty),
                sinGroupj.Key.TantoId,
                TantoSname = sinGroupj.Max(x => x?.tantoMst?.Sname ?? string.Empty),
                HokenId = sinGroupj.Key.HokenId,
                TotalTensu = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn != "SZ" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                Tensu0 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("11") || (x.sinKoui.SyukeiSaki.StartsWith("A11") && new string[] { "A" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //初診
                Tensu1 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("12") || (x.sinKoui.SyukeiSaki.StartsWith("A12") && new string[] { "A" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //再診
                Tensu2 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("13") || (x.sinKoui.SyukeiSaki.StartsWith("A13") && new string[] { "B" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //医学管理
                Tensu3 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("14") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "C" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "1450" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //在宅
                Tensu4 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "1450" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //薬剤器材
                Tensu5 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "F" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //投薬
                Tensu6 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //注射
                Tensu7 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //処置
                Tensu8 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //手術
                Tensu9 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //検査
                Tensu10 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //画像
                Tensu11 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //その他
                Jihi =
                    sinGroupj.Sum
                    (x =>
                        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 0 ? x.sinKoui.Ten * 10 * x.sinCount.Count :  //自費(点)
                        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 1 ? x.sinKoui.Ten * x.sinCount.Count :       //自費(円)
                        x.sinKoui.EntenKbn == 1 && x.sinKoui.CdKbn == "SZ" ? x.sinKoui.Ten * x.sinCount.Count :  //外税
                        0
                    ),
                TotalIryohi = sinGroupj.GroupBy(x => new { x.kaikeiInf.RaiinNo, x.kaikeiInf.TotalIryohi }).Sum(x => x.Key.TotalIryohi)
            }
        );

        var retData = joinQuery.Select(
            data =>
                new CoKouiTensuModel()
                {
                    ReportKbn = 0,
                    PtId = data.PtId,
                    RaiinNo = data.RaiinNo,
                    SinDate = data.SinDate,
                    SyosaisinKbn = data.SyosaisinKbn,
                    KaId = data.KaId,
                    KaSname = data.KaSname,
                    TantoId = data.TantoId,
                    TantoSname = data.TantoSname,
                    HokenId = data.HokenId,
                    HokenKbn = 0,
                    HokenSbtCd1 = 0,
                    HonkeKbn = 0,
                    Houbetu = "",
                    KohiCount = 0,
                    TotalTensu = data.TotalTensu,
                    Tensu0 = data.Tensu0,
                    Tensu1 = data.Tensu1,
                    Tensu2 = data.Tensu2,
                    Tensu3 = data.Tensu3,
                    Tensu4 = data.Tensu4,
                    Tensu5 = data.Tensu5,
                    Tensu6 = data.Tensu6,
                    Tensu7 = data.Tensu7,
                    Tensu8 = data.Tensu8,
                    Tensu9 = data.Tensu9,
                    Tensu10 = data.Tensu10,
                    Tensu11 = data.Tensu11,
                    Jihi = data.Jihi,
                    TotalIryohi = data.TotalIryohi
                }
        )
        .ToList();

        //保険情報を足しこみ
        foreach (var wrkData in retData)
        {
            var ptKohiJoin = ptKohiJoins.Find(p =>
                p.PtId == wrkData.PtId &&
                p.SinYm == wrkData.SinYm &&
                p.HokenId == wrkData.HokenId
            );

            if (ptKohiJoin != null)
            {
                wrkData.HokenKbn = ptKohiJoin.HokenKbn;
                wrkData.HokenSbtCd1 = ptKohiJoin.HokenSbtCd1;
                wrkData.HonkeKbn = ptKohiJoin.HonkeKbn;
                wrkData.Houbetu = ptKohiJoin.Houbetu;
                wrkData.KohiCount = ptKohiJoin.KohiCount;
            }
        }

        return retData;
    }

}
