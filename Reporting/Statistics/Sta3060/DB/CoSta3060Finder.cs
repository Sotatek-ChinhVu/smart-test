using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3060.Models;

namespace Reporting.Statistics.Sta3060.DB;

public class CoSta3060Finder : RepositoryBase, ICoSta3060Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3060Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
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
    public List<CoKouiTensuModel> GetKouiTensu(int hpId, CoSta3060PrintConf printConf)
    {
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
            s.HpId == hpId &&
            s.SinYm >= printConf.StartSinYm &&
        s.SinYm <= printConf.EndSinYm
        );

        var sinKouis = NoTrackingDataContext.SinKouis;
        var sinKouiRpInfs = NoTrackingDataContext.SinRpInfs;
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
        ptInfs = !printConf.IsTester ? ptInfs.Where(p => p.IsTester == 0) : ptInfs;
        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(p => p.IsDeleted == DeleteStatus.None);
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
                );
            }
        }
        #endregion
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.Status >= 5 && r.IsDeleted == DeleteStatus.None);
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs;
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
        #endregion

        IQueryable<PtHokenPattern> ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns;
        IQueryable<PtHokenInf> ptHokenInfs = NoTrackingDataContext.PtHokenInfs;
        IQueryable<PtKohi> ptKohis = NoTrackingDataContext.PtKohis;
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

            ptHokenPatterns = ptHokenPatterns.Where(r => hokenKbns.Contains(r.HokenKbn));

            if (printConf.HokenSbts.Contains(2) && !printConf.HokenSbts.Contains(3))
            {
                //後期を除く
                ptHokenPatterns = ptHokenPatterns.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 == 3));
            }
            else if (!printConf.HokenSbts.Contains(2) && printConf.HokenSbts.Contains(3))
            {
                //国保一般・退職を除く
                ptHokenPatterns = ptHokenPatterns.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 != 3));
            }

            if (printConf.HokenSbts.Contains(0) && !printConf.HokenSbts.Contains(12))
            {
                //自費レセを除く
                ptHokenInfs = ptHokenInfs.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "109"));
            }
            else if (!printConf.HokenSbts.Contains(0) && printConf.HokenSbts.Contains(12))
            {
                //自費を除く
                ptHokenInfs = ptHokenInfs.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "108"));
            }
        }
        #endregion

        #region 公費 単独併用判断
        var ptKohiPatterns = (
            from ptHokenPattern in ptHokenPatterns
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
            from ptHokenPattern in ptHokenPatterns
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
        ptKohiPatterns.Union(ptKohiPattern2);

        var ptKohiPattern3 = (
            from ptHokenPattern in ptHokenPatterns
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
        ptKohiPatterns.Union(ptKohiPattern3);

        var ptKohiPattern4 = (
            from ptHokenPattern in ptHokenPatterns
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
        ptKohiPatterns.Union(ptKohiPattern4);

        var ptKohiJoins = (
            from sinKoui in sinKouis
            join ptKohiPattern in ptKohiPatterns on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenPid }
            join ptHokenInf in ptHokenInfs on
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

        var kaMsts = NoTrackingDataContext.KaMsts;
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.IsDeleted == DeleteStatus.None);

        var sinKouiCountList = sinKouiCounts.ToList();
        var ptIdList = sinKouiCountList.Select(item => item.PtId).Distinct().ToList();
        var sinYmList = sinKouiCountList.Select(item => item.SinYm).Distinct().ToList();
        var rpNoList = sinKouiCountList.Select(item => item.RpNo).Distinct().ToList();
        var seqNoList = sinKouiCountList.Select(item => item.SeqNo).Distinct().ToList();
        var raiinNoList = sinKouiCountList.Select(item => item.RaiinNo).Distinct().ToList();
        var sinDateList = sinKouiCountList.Select(item => item.SinDate).Distinct().ToList();

        var sinKouiList = sinKouis.Where(item => ptIdList.Contains(item.PtId)
                                                 && sinYmList.Contains(item.SinYm)
                                                 && rpNoList.Contains(item.RpNo)
                                                 && seqNoList.Contains(item.SeqNo)
                                  ).ToList();

        var hokenIdList = sinKouiList.Select(item => item.HokenId).Distinct().ToList();
        var hokenPidList = sinKouiList.Select(item => item.HokenPid).Distinct().ToList();

        var sinKouiRpInfList = sinKouiRpInfs.Where(item => ptIdList.Contains(item.PtId)
                                                           && sinYmList.Contains(item.SinYm)
                                                           && rpNoList.Contains(item.RpNo)
                                            ).ToList();

        var kaikeiInfList = kaikeiInfs.Where(item => ptIdList.Contains(item.PtId)
                                                     && raiinNoList.Contains(item.RaiinNo)
                                                     && sinDateList.Contains(item.SinDate)
                                                     && hokenIdList.Contains(item.HokenId)
                                      ).ToList();

        var ptInfList = ptInfs.Where(item => ptIdList.Contains(item.PtId)).ToList();
        var raiinInfList = raiinInfs.Where(item => raiinNoList.Contains(item.RaiinNo)).ToList();
        var tantoIdList = raiinInfList.Select(item => item.TantoId).Distinct().ToList();

        var ptHokenPatternList = ptHokenPatterns.Where(item => ptIdList.Contains(item.PtId)
                                                               && hokenPidList.Contains(item.HokenPid)
                                                ).ToList();
        var ptHokenInfList = ptHokenInfs.Where(item => ptIdList.Contains(item.PtId)
                                                       && hokenIdList.Contains(item.HokenId)
                                        ).ToList();

        var kaIdList = raiinInfList.Select(item => item.KaId).Distinct().ToList();
        var kaMstList = kaMsts.Where(item => kaIdList.Contains(item.KaId)).ToList();
        var userMstList = userMsts.Where(item => tantoIdList.Contains(item.UserId)).ToList();

        var joinQueryTest = (
            from sinCount in sinKouiCountList
            join sinKoui in sinKouiList on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinRp in sinKouiRpInfList on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo }
            join kaikeiInf in kaikeiInfList on
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
            join kaMst in kaMstList on
                new { raiinInf.HpId, raiinInf.KaId } equals
                new { kaMst.HpId, kaMst.KaId } into kaMstJoin
            from kaMstj in kaMstJoin.DefaultIfEmpty()
            join userMst in userMstList on
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
                    ptInf.Birthday,
                    ptHokenInf.HokenId
                } into sinGroupj
            select new
            {
                sinGroupj.Key.PtId,
                sinGroupj.Key.RaiinNo,
                sinGroupj.Key.SinDate,
                sinGroupj.Key.SyosaisinKbn,
                sinGroupj.Key.KaId,
                //KaSname = sinGroupj.Max(x => x.kaMstj.KaSname),
                sinGroupj.Key.TantoId,
                //TantoSname = sinGroupj.Max(x => x.tantoMst.Sname),
                sinGroupj.Key.Birthday,
                HokenId = sinGroupj.Key.HokenId,
                //TotalTensu = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn != "SZ" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                //TensuA = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "A" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //初再診
                //TensuB = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "B" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //医学管理
                //TensuC = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "C" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //在宅
                //TensuD = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "D" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //検査
                //TensuE = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "E" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //画像診断
                //TensuF = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "F" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //投薬
                //TensuG = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "G" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //注射
                //TensuH = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "H" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //リハビリ
                //TensuI = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "I" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //精神
                //TensuJ = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "J" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //処置
                //TensuK = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "K" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //手術
                //TensuL = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "L" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //麻酔
                //TensuM = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "M" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //放射線治療
                //TensuN = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "N" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //病理
                //Jihi =
                //    sinGroupj.Sum
                //    (x =>
                //        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 0 ? x.sinKoui.Ten * 10 * x.sinCount.Count :  //自費(点)
                //        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 1 ? x.sinKoui.Ten * x.sinCount.Count :       //自費(円)
                //        x.sinKoui.EntenKbn == 1 && x.sinKoui.CdKbn == "SZ" ? x.sinKoui.Ten * x.sinCount.Count :  //外税
                //        0
                //    ),
                //Tensu1 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "A", "B", "C" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //診察
                //Tensu2 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "F" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //投薬
                //Tensu3 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //注射
                //Tensu4 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //処置
                //Tensu5 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //手術
                //Tensu6 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //検査
                //Tensu7 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //画像
                //Tensu8 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //その他

                //TotalPtFutan = sinGroupj.GroupBy(x => new { x.kaikeiInf.RaiinNo, x.kaikeiInf.TotalPtFutan }).Sum(x => x.Key.TotalPtFutan),
                //PtNum = sinGroupj.Max(x => x.ptInf.PtNum),
                //PtName = sinGroupj.Max(x => x.ptInf.Name),
                //TensuSyosin = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn == "A" && (x.sinKoui.SyukeiSaki.StartsWith("11") || x.sinKoui.SyukeiSaki.StartsWith("A11")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //初診
                //TensuSaisin = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn == "A" && (x.sinKoui.SyukeiSaki.StartsWith("12") || x.sinKoui.SyukeiSaki.StartsWith("A12")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0)  //再診

            }
        );

        var joinQueryListTest = joinQueryTest.ToList();

        var joinQuery = (
            from sinCount in sinKouiCountList
            join sinKoui in sinKouiList on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinRp in sinKouiRpInfList on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo }
            join kaikeiInf in kaikeiInfList on
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
            join kaMst in kaMstList on
                new { raiinInf.HpId, raiinInf.KaId } equals
                new { kaMst.HpId, kaMst.KaId } into kaMstJoin
            from kaMstj in kaMstJoin.DefaultIfEmpty()
            join userMst in userMstList on
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
                    ptInf.Birthday,
                    ptHokenInf.HokenId
                } into sinGroupj
            select new
            {
                sinGroupj.Key.PtId,
                sinGroupj.Key.RaiinNo,
                sinGroupj.Key.SinDate,
                sinGroupj.Key.SyosaisinKbn,
                sinGroupj.Key.KaId,
                KaSname = sinGroupj.Max(x => x.kaMstj.KaSname),
                sinGroupj.Key.TantoId,
                TantoSname = sinGroupj.Max(x => x.tantoMst.Sname),
                sinGroupj.Key.Birthday,
                HokenId = sinGroupj.Key.HokenId,
                TotalTensu = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn != "SZ" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                TensuA = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "A" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //初再診
                TensuB = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "B" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //医学管理
                TensuC = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "C" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //在宅
                TensuD = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "D" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //検査
                TensuE = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "E" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //画像診断
                TensuF = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "F" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //投薬
                TensuG = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "G" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //注射
                TensuH = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "H" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //リハビリ
                TensuI = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "I" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //精神
                TensuJ = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "J" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //処置
                TensuK = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "K" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //手術
                TensuL = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "L" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //麻酔
                TensuM = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "M" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //放射線治療
                TensuN = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || x.sinKoui.SyukeiSaki.StartsWith("A")) && x.sinKoui.CdKbn == "N" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //病理
                Jihi =
                    sinGroupj.Sum
                    (x =>
                        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 0 ? x.sinKoui.Ten * 10 * x.sinCount.Count :  //自費(点)
                        (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 1 ? x.sinKoui.Ten * x.sinCount.Count :       //自費(円)
                        x.sinKoui.EntenKbn == 1 && x.sinKoui.CdKbn == "SZ" ? x.sinKoui.Ten * x.sinCount.Count :  //外税
                        0
                    ),
                Tensu1 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "A", "B", "C" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //診察
                Tensu2 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "F" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //投薬
                Tensu3 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //注射
                Tensu4 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //処置
                Tensu5 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //手術
                Tensu6 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //検査
                Tensu7 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //画像
                Tensu8 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),  //その他

                TotalPtFutan = sinGroupj.GroupBy(x => new { x.kaikeiInf.RaiinNo, x.kaikeiInf.TotalPtFutan }).Sum(x => x.Key.TotalPtFutan),
                PtNum = sinGroupj.Max(x => x.ptInf.PtNum),
                PtName = sinGroupj.Max(x => x.ptInf.Name),
                TensuSyosin = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn == "A" && (x.sinKoui.SyukeiSaki.StartsWith("11") || x.sinKoui.SyukeiSaki.StartsWith("A11")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0), //初診
                TensuSaisin = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn == "A" && (x.sinKoui.SyukeiSaki.StartsWith("12") || x.sinKoui.SyukeiSaki.StartsWith("A12")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0)  //再診
            }
        );

        var joinQueryList = joinQuery.ToList();

        var retData = joinQueryList.Select(
            data =>
                new CoKouiTensuModel()
                {
                    ReportKbn = printConf.ReportKbn,
                    PtId = data.PtId,
                    RaiinNo = data.RaiinNo,
                    SinDate = data.SinDate,
                    SyosaisinKbn = data.SyosaisinKbn,
                    KaId = data.KaId,
                    KaSname = data.KaSname,
                    TantoId = data.TantoId,
                    TantoSname = data.TantoSname,
                    Birthday = data.Birthday,
                    HokenId = data.HokenId,
                    HokenKbn = 0,
                    HokenSbtCd1 = 0,
                    HonkeKbn = 0,
                    Houbetu = "",
                    KohiCount = 0,
                    TotalTensu = data.TotalTensu,
                    TensuA = data.TensuA,
                    TensuB = data.TensuB,
                    TensuC = data.TensuC,
                    TensuD = data.TensuD,
                    TensuE = data.TensuE,
                    TensuF = data.TensuF,
                    TensuG = data.TensuG,
                    TensuH = data.TensuH,
                    TensuI = data.TensuI,
                    TensuJ = data.TensuJ,
                    TensuK = data.TensuK,
                    TensuL = data.TensuL,
                    TensuM = data.TensuM,
                    TensuN = data.TensuN,
                    Jihi = data.Jihi,
                    Tensu1 = data.Tensu1,
                    Tensu2 = data.Tensu2,
                    Tensu3 = data.Tensu3,
                    Tensu4 = data.Tensu4,
                    Tensu5 = data.Tensu5,
                    Tensu6 = data.Tensu6,
                    Tensu7 = data.Tensu7,
                    Tensu8 = data.Tensu8,
                    TotalPtFutan = data.TotalPtFutan,
                    PtNum = data.PtNum,
                    PtName = data.PtName,
                    TensuSyosin = data.TensuSyosin,
                    TensuSaisin = data.TensuSaisin
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
