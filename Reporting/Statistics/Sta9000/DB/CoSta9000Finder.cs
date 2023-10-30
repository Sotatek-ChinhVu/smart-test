using Domain.Constant;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Services;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta9000.Models;
using System.Data;
using System.Globalization;

namespace Reporting.Statistics.Sta9000.DB;

public class CoSta9000Finder : RepositoryBase, ICoSta9000Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    private readonly ISystemConfig _systemConfig;

    public CoSta9000Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder, ISystemConfig systemConfig) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
        _systemConfig = systemConfig;
    }


    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    /// <summary>
    /// 患者情報の取得
    /// </summary>
    /// <param name="ptConf"></param>
    /// <returns></returns>
    public List<CoPtInfModel> GetPtInfs(int hpId,
        CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
        CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
        CoSta9000KensaConf? kensaConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);

        var ptFirstVisits = GetPtFirstVisits();
        var ptLastVisits = NoTrackingDataContext.PtLastVisitDates;
        var ptCmts = NoTrackingDataContext.PtCmtInfs.Where(p => p.IsDeleted == DeleteStatus.None);

        //患者情報 + 初回来院日 + 最終来院日 + 患者コメント
        var joinPtInfs = (
            from ptInf in ptInfs
            join ptFirstVisit in ptFirstVisits on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptFirstVisit.HpId, ptFirstVisit.PtId } into pfJoin
            from ptFirstVisitJoin in pfJoin.DefaultIfEmpty()
            join ptLastVisit in ptLastVisits on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptLastVisit.HpId, ptLastVisit.PtId } into plJoin
            from ptVisitJoin in plJoin.DefaultIfEmpty()
            join ptCmt in ptCmts on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptCmt.HpId, ptCmt.PtId } into pcJoin
            from ptCmtJoin in pcJoin.DefaultIfEmpty()
            where
                ptInf.HpId == hpId
            orderby
                ptInf.PtNum
            select new
            {
                ptInf,
                ptFirstVisitJoin,
                ptVisitJoin,
                ptCmt = ptCmtJoin.Text
            }
        );

        var timeout = TimeSpan.FromMinutes(60);

        var cancellationTokenSource = new CancellationTokenSource(timeout);
        var cancellationToken = cancellationTokenSource.Token;

        var queryTask = Task.Run(() =>
        {
            return joinPtInfs.AsEnumerable().Select(
            d =>
                new CoPtInfModel
                (
                    d.ptInf,
                    d.ptFirstVisitJoin?.SinDate ?? 0,
                    d.ptVisitJoin?.LastVisitDate ?? 0,
                    d.ptCmt
                )
        ).ToList();
        }, cancellationToken);

        var ptDatas = queryTask.Result;

        //検査条件で絞りこみ
        ptDatas = GetPtInfKensaFilter(hpId, kensaConf, ptDatas);

        #region 算定条件（調整額・調整率・自動算定）の取得
        var ptSanteis = NoTrackingDataContext.PtSanteiConfs;
        int nowDate = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();

        var santeiDatas = (
            from ptSantei in ptSanteis
            join ptInf in ptInfs on
                new { ptSantei.HpId, ptSantei.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            where
                ptSantei.IsDeleted == DeleteStatus.None &&
                ptSantei.StartDate <= nowDate &&
                ptSantei.EndDate >= nowDate
            select
                ptSantei
        ).ToList();
        #endregion

        #region 患者グループの取得
        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs;
        var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == DeleteStatus.None);

        var ptGrpDatas = (
            from ptGrpInf in ptGrpInfs
            join ptGrpItem in ptGrpItems on
                new { ptGrpInf.HpId, ptGrpInf.GroupId, ptGrpInf.GroupCode } equals
                new { ptGrpItem.HpId, GroupId = ptGrpItem.GrpId, GroupCode = ptGrpItem.GrpCode }
            join ptInf in ptInfs on
                new { ptGrpInf.HpId, ptGrpInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            where
                ptGrpInf.IsDeleted == DeleteStatus.None
            select new
            {
                ptGrpInf.PtId,
                ptGrpItem.GrpId,
                ptGrpItem.GrpCode,
                ptGrpItem.GrpCodeName
            }
        ).ToList();

        var ptGrpMsts = NoTrackingDataContext.PtGrpNameMsts
            .Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None)
            .OrderBy(p => p.GrpId)
            .ToList();
        #endregion

        #region 患者情報へ格納
        foreach (var ptData in ptDatas)
        {
            //調整額
            ptData.AdjFutan =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 1)
                        .Select(s => string.Format("{0}({1}円)", s.EdaNo == 1 ? "自費除く" : s.EdaNo == 2 ? "自費のみ" : "すべて", s.KbnVal))
                );
            //調整率
            ptData.AdjRate =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 2)
                        .Select(s => string.Format("{0}({1}%)", s.EdaNo == 1 ? "自費除く" : s.EdaNo == 2 ? "自費のみ" : "すべて", s.KbnVal))
                );
            //自動算定
            ptData.AutoSantei =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 3)
                        .Select(s => s.KbnVal == 1 ? "地域包括診療料" : s.KbnVal == 2 ? "認知症地域包括診療料" : "")
                );
            //患者グループ
            ptData.PtGrps = new List<CoPtInfModel.PtGrp>();

            foreach (var ptGrpMst in ptGrpMsts)
            {
                var curGrps = ptGrpDatas.FirstOrDefault(p => p.PtId == ptData.PtId && p.GrpId == ptGrpMst.GrpId);

                ptData.PtGrps.Add
                (
                    new CoPtInfModel.PtGrp()
                    {
                        GrpId = ptGrpMst.GrpId,
                        GrpName = ptGrpMst.GrpName ?? string.Empty,
                        GrpCode = curGrps?.GrpCode ?? string.Empty,
                        GrpCodeName = curGrps?.GrpCodeName ?? string.Empty
                    }
                );
            }
        }
        #endregion

        return ptDatas;
    }

    public List<CoPtInfModel> GetPtInfs(int hpId,
    CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
    CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
    CoSta9000KensaConf? kensaConf, List<long> ptIds)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);

        if (ptIds.Count > 0)
        {
            ptIds = ptIds.Distinct().ToList();
            ptInfs = ptInfs.Where(p => ptIds.Contains(p.PtId));
        }

        var ptFirstVisits = GetPtFirstVisits();
        var ptLastVisits = NoTrackingDataContext.PtLastVisitDates;
        var ptCmts = NoTrackingDataContext.PtCmtInfs.Where(p => p.IsDeleted == DeleteStatus.None);

        //患者情報 + 初回来院日 + 最終来院日 + 患者コメント
        var joinPtInfs = (
            from ptInf in ptInfs
            join ptFirstVisit in ptFirstVisits on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptFirstVisit.HpId, ptFirstVisit.PtId } into pfJoin
            from ptFirstVisitJoin in pfJoin.DefaultIfEmpty()
            join ptLastVisit in ptLastVisits on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptLastVisit.HpId, ptLastVisit.PtId } into plJoin
            from ptVisitJoin in plJoin.DefaultIfEmpty()
            join ptCmt in ptCmts on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptCmt.HpId, ptCmt.PtId } into pcJoin
            from ptCmtJoin in pcJoin.DefaultIfEmpty()
            where
                ptInf.HpId == hpId
            orderby
                ptInf.PtNum
            select new
            {
                ptInf,
                ptFirstVisitJoin,
                ptVisitJoin,
                ptCmt = ptCmtJoin.Text
            }
        );

        var ptDatas = joinPtInfs.AsEnumerable().Select(
            d =>
                new CoPtInfModel
                (
                    d.ptInf,
                    d.ptFirstVisitJoin?.SinDate ?? 0,
                    d.ptVisitJoin?.LastVisitDate ?? 0,
                    d.ptCmt
                )
        ).ToList();

        //検査条件で絞りこみ
        ptDatas = GetPtInfKensaFilter(hpId, kensaConf, ptDatas);

        #region 算定条件（調整額・調整率・自動算定）の取得
        var ptSanteis = NoTrackingDataContext.PtSanteiConfs;
        int nowDate = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();

        var santeiDatas = (
            from ptSantei in ptSanteis
            join ptInf in ptInfs on
                new { ptSantei.HpId, ptSantei.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            where
                ptSantei.IsDeleted == DeleteStatus.None &&
                ptSantei.StartDate <= nowDate &&
                ptSantei.EndDate >= nowDate
            select
                ptSantei
        ).ToList();
        #endregion

        #region 患者グループの取得
        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs;
        var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == DeleteStatus.None);

        var ptGrpDatas = (
            from ptGrpInf in ptGrpInfs
            join ptGrpItem in ptGrpItems on
                new { ptGrpInf.HpId, ptGrpInf.GroupId, ptGrpInf.GroupCode } equals
                new { ptGrpItem.HpId, GroupId = ptGrpItem.GrpId, GroupCode = ptGrpItem.GrpCode }
            join ptInf in ptInfs on
                new { ptGrpInf.HpId, ptGrpInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            where
                ptGrpInf.IsDeleted == DeleteStatus.None
            select new
            {
                ptGrpInf.PtId,
                ptGrpItem.GrpId,
                ptGrpItem.GrpCode,
                ptGrpItem.GrpCodeName
            }
        ).ToList();

        var ptGrpMsts = NoTrackingDataContext.PtGrpNameMsts
            .Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None)
            .OrderBy(p => p.GrpId)
            .ToList();
        #endregion

        #region 患者情報へ格納
        foreach (var ptData in ptDatas)
        {
            //調整額
            ptData.AdjFutan =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 1)
                        .Select(s => string.Format("{0}({1}円)", s.EdaNo == 1 ? "自費除く" : s.EdaNo == 2 ? "自費のみ" : "すべて", s.KbnVal))
                );
            //調整率
            ptData.AdjRate =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 2)
                        .Select(s => string.Format("{0}({1}%)", s.EdaNo == 1 ? "自費除く" : s.EdaNo == 2 ? "自費のみ" : "すべて", s.KbnVal))
                );
            //自動算定
            ptData.AutoSantei =
                string.Join
                (
                    " ",
                    santeiDatas
                        .Where(s => s.PtId == ptData.PtId && s.KbnNo == 3)
                        .Select(s => s.KbnVal == 1 ? "地域包括診療料" : s.KbnVal == 2 ? "認知症地域包括診療料" : "")
                );
            //患者グループ
            ptData.PtGrps = new List<CoPtInfModel.PtGrp>();

            foreach (var ptGrpMst in ptGrpMsts)
            {
                var curGrps = ptGrpDatas.FirstOrDefault(p => p.PtId == ptData.PtId && p.GrpId == ptGrpMst.GrpId);

                ptData.PtGrps.Add
                (
                    new CoPtInfModel.PtGrp()
                    {
                        GrpId = ptGrpMst.GrpId,
                        GrpName = ptGrpMst.GrpName ?? string.Empty,
                        GrpCode = curGrps?.GrpCode ?? string.Empty,
                        GrpCodeName = curGrps?.GrpCodeName ?? string.Empty
                    }
                );
            }
        }
        #endregion

        return ptDatas;
    }

    /// <summary>
    /// 処方一覧の取得
    /// </summary>
    /// <param name="ptConf"></param>
    /// <param name="hokenConf"></param>
    /// <param name="byomeiConf"></param>
    /// <param name="raiinConf"></param>
    /// <param name="sinConf"></param>
    /// <param name="karteConf"></param>
    /// <param name="kensaConf"></param>
    /// <returns></returns>
    public List<CoDrugOdrModel> GetDrugOrders(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);

        //オーダー
        var odrInfs = NoTrackingDataContext.OdrInfs;
        var odrDetails = NoTrackingDataContext.OdrInfDetails.Where(d => d.DrugKbn > 0);
        //来院情報
        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);

        if (sinConf != null)
        {
            //検索ワード
            if (sinConf.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[] values = sinConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (sinConf.WordOpt == 0)
                {
                    //or条件
                    odrDetails = odrDetails.Where(p => searchWords.Any(key => p.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    odrDetails = odrDetails.Where(p => searchWords.All(key => p.ItemName.Contains(key)));
                }
            }
            //検索項目
            if (sinConf.ItemCds?.Count >= 3)
            {
                List<string> wrkItems = new List<string>();
                for (int i = 0; i + 3 <= sinConf.ItemCds.Count; i += 3)
                {
                    wrkItems.Add(sinConf.ItemCds[i]);
                }

                if (sinConf.ItemCmts?.Count >= 1)
                {
                    odrDetails = odrDetails.Where(p => wrkItems.Contains(p.ItemCd) || (p.ItemCd == string.Empty && sinConf.ItemCmts.Contains(p.ItemName)));
                }
                else
                {
                    odrDetails = odrDetails.Where(p => wrkItems.Contains(p.ItemCd));
                }
            }
            else if (sinConf.ItemCmts?.Count >= 1)
            {
                odrDetails = odrDetails.Where(p => p.ItemCd == string.Empty && sinConf.ItemCmts.Contains(p.ItemName));
            }
        }

        //処方オーダーの取得
        var odrJoins = (
            from odrInf in odrInfs
            join ptInf in ptInfs on
                new { odrInf.HpId, odrInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join odrDetail in odrDetails on
                new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
            join raiinInf in raiinInfs on
                new { odrInf.HpId, odrInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.RaiinNo }
            where
                odrInf.IsDeleted == DeleteStatus.None &&
                new int[] { 21, 22, 23, 28 }.Contains(odrInf.OdrKouiKbn)
            group
                new { odrInf.DaysCnt, odrDetail.Suryo, odrDetail.ItemName } by
                new { odrInf.HpId, odrInf.PtId, odrInf.SinDate, odrDetail.ItemCd, odrDetail.UnitName } into og
            select new
            {
                og.Key.PtId,
                og.Key.SinDate,
                og.Key.ItemCd,
                og.Key.UnitName,
                ItemName = og.Max(z => z.ItemName),
                Suryo = og.Sum(z => z.DaysCnt * z.Suryo)
            }
        );

        var retDatas = odrJoins.AsEnumerable().Select(
            d =>
                new CoDrugOdrModel()
                {
                    PtId = d.PtId,
                    SinDate = d.SinDate,
                    ItemCd = d.ItemCd,
                    UnitName = d.UnitName,
                    ItemName = d.ItemName,
                    Suryo = d.Suryo
                }
        ).ToList();

        return retDatas;
    }

    /// <summary>
    /// 患者病名の取得
    /// </summary>
    public List<CoPtByomeiModel> GetPtByomeis(
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        (var ptByomeis, bool isByomeiConf) = GetPtByomeis(byomeiConf);

        //検索病名
        if (isByomeiConf)
        {
            const string freeByomeiCd = "0000999";
            if (byomeiConf.ByomeiCds?.Count >= 1 && byomeiConf.Byomeis?.Count >= 1)
            {
                ptByomeis = ptByomeis.Where(p => byomeiConf.ByomeiCds.Contains(p.ByomeiCd) || (p.ByomeiCd == freeByomeiCd && byomeiConf.Byomeis.Contains(p.Byomei)));
            }
            else if (byomeiConf.ByomeiCds?.Count >= 1)
            {
                ptByomeis = ptByomeis.Where(p => byomeiConf.ByomeiCds.Contains(p.ByomeiCd));
            }
            else if (byomeiConf.Byomeis?.Count >= 1)
            {
                ptByomeis = ptByomeis.Where(p => p.ByomeiCd == freeByomeiCd && byomeiConf.Byomeis.Contains(p.Byomei));
            }
        }

        var byomeiJoins = (
            from ptInf in ptInfs
            join ptByomei in ptByomeis on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptByomei.HpId, ptByomei.PtId }
            select
                ptByomei
        );

        var retDatas = byomeiJoins.AsEnumerable().Select(d => new CoPtByomeiModel(d)).ToList();

        return retDatas;
    }

    /// <summary>
    /// 保険情報の取得
    /// </summary>
    public List<CoPtHokenModel> GetPtHokens(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        (var ptHokenPatterns, var ptHokenInfs, var ptKohis, var isHokenConf, var isKohiConf) = GetPtHokenPatterns(hokenConf);

        var ptHokens = (
            from ptInf in ptInfs
            join hokenPattern in ptHokenPatterns on
                new { ptInf.HpId, ptInf.PtId } equals
                new { hokenPattern.HpId, hokenPattern.PtId }
            join ptHokenInf in ptHokenInfs on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            join ptKohi1 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi1Id } equals
                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into k1Join
            from kohi1Join in k1Join.DefaultIfEmpty()
            join ptKohi2 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi2Id } equals
                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into k2Join
            from kohi2Join in k2Join.DefaultIfEmpty()
            join ptKohi3 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi3Id } equals
                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into k3Join
            from kohi3Join in k3Join.DefaultIfEmpty()
            join ptKohi4 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi4Id } equals
                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into k4Join
            from kohi4Join in k4Join.DefaultIfEmpty()
            select new
            {
                hokenPattern,
                ptHokenInf,
                kohi1Join,
                kohi2Join,
                kohi3Join,
                kohi4Join
            }
        );

        var retDatas = ptHokens.AsEnumerable()
            .Select(
                d =>
                    new CoPtHokenModel
                    (
                        d.hokenPattern,
                        d.ptHokenInf,
                        d.kohi1Join,
                        d.kohi2Join,
                        d.kohi3Join,
                        d.kohi4Join
                    )
            ).ToList();

        return retDatas;
    }

    /// <summary>
    /// 来院情報の取得
    /// </summary>
    public List<CoRaiinInfModel> GetRaiinInfs(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);
        var raiinCmts = NoTrackingDataContext.RaiinCmtInfs.Where(r => r.IsDelete == DeleteStatus.None && r.CmtKbn == 1);
        var raiinBikos = NoTrackingDataContext.RaiinCmtInfs.Where(r => r.IsDelete == DeleteStatus.None && r.CmtKbn == 9);

        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts;
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts;
        //職員マスタ
        var userMsts = NoTrackingDataContext.UserMsts;

        //初回来院日
        var ptFirstVisits = GetPtFirstVisits();

        //保険情報
        (var ptHokenPatterns, var ptHokenInfs, var ptKohis, var isHokenConf, var isKohiConf) = GetPtHokenPatterns(hokenConf);

        //保険の絞り込み
        if (isHokenConf)
        {
            raiinInfs = (
                from raiinInf in raiinInfs
                join ptHokenPattern in ptHokenPatterns on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                select
                    raiinInf
            );
        }

        var raiinInfJoins = (
            from ptInf in ptInfs
            join raiinInf in raiinInfs on
                new { ptInf.HpId, ptInf.PtId } equals
                new { raiinInf.HpId, raiinInf.PtId }
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
                new { userMst.HpId, TantoId = userMst.UserId } into tantoMstJoin
            from tantoMstj in tantoMstJoin.DefaultIfEmpty()
            join raiinCmt in raiinCmts on
                new { raiinInf.HpId, raiinInf.RaiinNo } equals
                new { raiinCmt.HpId, raiinCmt.RaiinNo } into raiinCmtJoin
            from raiinCmtj in raiinCmtJoin.DefaultIfEmpty()
            join raiinBiko in raiinBikos on
                new { raiinInf.HpId, raiinInf.RaiinNo } equals
                new { raiinBiko.HpId, raiinBiko.RaiinNo } into raiinBikoJoin
            from raiinBikoj in raiinBikoJoin.DefaultIfEmpty()
            join ptFirstVisit in ptFirstVisits on
                new { raiinInf.HpId, raiinInf.PtId } equals
                new { ptFirstVisit.HpId, ptFirstVisit.PtId } into ptFirstVisitJoin
            from ptFirstVisitj in ptFirstVisitJoin.DefaultIfEmpty()
            select new
            {
                raiinInf,
                uketukeSbtMstj,
                kaMstj,
                tantoMstj,
                raiinCmt = raiinCmtj.Text,
                raiinBiko = raiinBikoj.Text,
                ptFirstVisitj
            }
        );

        //診療日に無効な担当医を排除
        raiinInfJoins = raiinInfJoins.Where(x => x.tantoMstj == null || (x.tantoMstj.StartDate <= x.raiinInf.SinDate && x.raiinInf.SinDate <= x.tantoMstj.EndDate));

        var raiinDatas = raiinInfJoins.AsEnumerable()
            .Select(
                r =>
                    new CoRaiinInfModel
                    (
                        r.raiinInf,
                        r.uketukeSbtMstj,
                        r.kaMstj,
                        r.tantoMstj,
                        r.raiinCmt,
                        r.raiinBiko,
                        r.ptFirstVisitj?.SinDate ?? 0
                    )
            ).ToList();

        #region 来院区分の取得
        var raiinKbnMsts = NoTrackingDataContext.RaiinKbnMsts
            .Where(r => r.HpId == hpId && r.IsDeleted == DeleteStatus.None)
            .OrderBy(r => r.GrpCd)
            .ToList();
        var raiinKbnInfs = NoTrackingDataContext.RaiinKbnInfs.Where(r => r.IsDelete == DeleteStatus.None);
        var raiinKbnDetails = NoTrackingDataContext.RaiinKbnDetails.Where(r => r.IsDeleted == DeleteStatus.None);

        var raiinKbns = (
            from raiinKbnInf in raiinKbnInfs
            join raiinInf in raiinInfs on
                new { raiinKbnInf.HpId, raiinKbnInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.RaiinNo }
            join raiinKbnDetail in raiinKbnDetails on
                new { raiinKbnInf.HpId, raiinKbnInf.GrpId, raiinKbnInf.KbnCd } equals
                new { raiinKbnDetail.HpId, GrpId = raiinKbnDetail.GrpCd, raiinKbnDetail.KbnCd }
            select new
            {
                raiinKbnInf.RaiinNo,
                raiinKbnInf.GrpId,
                raiinKbnDetail.KbnName
            }
        ).ToList();

        foreach (var raiinData in raiinDatas)
        {
            raiinData.RaiinKbns = new List<CoRaiinInfModel.RaiinKbn>();

            foreach (var raiinKbnMst in raiinKbnMsts)
            {
                var curKbns = raiinKbns.Where(r => r.RaiinNo == raiinData.RaiinNo && r.GrpId == raiinKbnMst.GrpCd).FirstOrDefault();

                raiinData.RaiinKbns.Add
                (
                    new CoRaiinInfModel.RaiinKbn()
                    {
                        GrpId = raiinKbnMst.GrpCd,
                        GrpName = raiinKbnMst.GrpName ?? string.Empty,
                        KbnName = curKbns?.KbnName ?? string.Empty
                    }
                );
            }
        }
        #endregion

        return raiinDatas;
    }

    /// <summary>
    /// 診療情報(オーダー)の取得
    /// </summary>
    public List<CoOdrInfModel> GetOdrInfs(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);
        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts;
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts;
        //職員マスタ
        var userMsts = NoTrackingDataContext.UserMsts;
        //オーダー
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(p => p.IsDeleted == DeleteStatus.None);
        IQueryable<OdrInfDetail> odrDetails = NoTrackingDataContext.OdrInfDetails;
        //保険情報
        (var wrkHokenPatterns, var wrkHokenInfs, var wrkKohis, var isHokenConf, var isKohiConf) = GetPtHokenPatterns(hokenConf);

        //保険の絞り込み
        if (isHokenConf)
        {
            odrInfs = (
                from odrInf in odrInfs
                join wrkHokenPattern in wrkHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                    new { wrkHokenPattern.HpId, wrkHokenPattern.PtId, wrkHokenPattern.HokenPid }
                select
                    odrInf
            );
        }

        //保険情報
        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns;
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs;
        var ptKohis = NoTrackingDataContext.PtKohis;

        if (sinConf != null)
        {
            //検索ワード
            if (sinConf.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[] values = sinConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (sinConf.WordOpt == 0)
                {
                    //or条件
                    odrDetails = odrDetails.Where(p => searchWords.Any(key => p.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    odrDetails = odrDetails.Where(p => searchWords.All(key => p.ItemName.Contains(key)));
                }
            }
            //検索項目
            if (sinConf.ItemCds?.Count >= 3)
            {
                List<string> wrkItems = new List<string>();
                for (int i = 0; i + 3 <= sinConf.ItemCds.Count; i += 3)
                {
                    wrkItems.Add(sinConf.ItemCds[i]);
                }

                if (sinConf.ItemCmts?.Count >= 1)
                {
                    odrDetails = odrDetails.Where(p => wrkItems.Contains(p.ItemCd) || (p.ItemCd == string.Empty && sinConf.ItemCmts.Contains(p.ItemName)));
                }
                else
                {
                    odrDetails = odrDetails.Where(p => wrkItems.Contains(p.ItemCd));
                }
            }
            else if (sinConf.ItemCmts?.Count >= 1)
            {
                odrDetails = odrDetails.Where(p => p.ItemCd == string.Empty && sinConf.ItemCmts.Contains(p.ItemName));
            }
        }

        var odrDatas = (
            from ptInf in ptInfs
            join raiinInf in raiinInfs on
                new { ptInf.HpId, ptInf.PtId } equals
                new { raiinInf.HpId, raiinInf.PtId }
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
                new { userMst.HpId, TantoId = userMst.UserId } into tantoMstJoin
            from tantoMstj in tantoMstJoin.DefaultIfEmpty()
            join odrInf in odrInfs on
                new { raiinInf.HpId, raiinInf.RaiinNo } equals
                new { odrInf.HpId, odrInf.RaiinNo }
            join odrDetail in odrDetails on
                new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
                //join ptHokenPattern in ptHokenPatterns on
                //    new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                //join ptHokenInf in ptHokenInfs on
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                //    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                //join ptKohi1 in ptKohis on
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                //    new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1Join
                //from ptKohi1j in ptKohi1Join.DefaultIfEmpty()
                //join ptKohi2 in ptKohis on
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                //    new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2Join
                //from ptKohi2j in ptKohi2Join.DefaultIfEmpty()
                //join ptKohi3 in ptKohis on
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                //    new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3Join
                //from ptKohi3j in ptKohi3Join.DefaultIfEmpty()
                //join ptKohi4 in ptKohis on
                //    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                //    new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4Join
                //from ptKohi4j in ptKohi4Join.DefaultIfEmpty()
            select new
            {
                raiinInf,
                uketukeSbtMstj,
                kaMstj,
                tantoMstj,
                odrInf,
                odrDetail
                //ptHokenPattern,
                //hokenHoubetu = ptHokenInf.Houbetu,
                //kohi1Houbetu = ptKohi1j.Houbetu,
                //kohi2Houbetu = ptKohi2j.Houbetu,
                //kohi3Houbetu = ptKohi3j.Houbetu,
                //kohi4Houbetu = ptKohi4j.Houbetu
            }
        );

        var retDatas = odrDatas.AsEnumerable()
            .Select(
                d =>
                    new CoOdrInfModel
                    (
                        d.raiinInf,
                        d.uketukeSbtMstj,
                        d.kaMstj,
                        d.tantoMstj,
                        d.odrInf,
                        d.odrDetail
                    //d.ptHokenPattern,
                    ///d.hokenHoubetu
                    //d.kohi1Houbetu,
                    //d.kohi2Houbetu,
                    //d.kohi3Houbetu,
                    //d.kohi4Houbetu
                    )
            ).ToList();

        #region '保険情報の設定（joinするとindexを設定しても著しくパフォーマンスが低下するため別で取得する）'
        var hokenDatas = (
            from ptInf in ptInfs
            join ptHokenPattern in ptHokenPatterns on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId }
            join ptHokenInf in ptHokenInfs on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            select new
            {
                ptHokenPattern,
                ptHokenInf
            }
        ).ToList();

        var kohiDatas = (
            from ptInf in ptInfs
            join ptKohi in ptKohis on
                new { ptInf.HpId, ptInf.PtId } equals
                new { ptKohi.HpId, ptKohi.PtId }
            select new
            {
                ptKohi
            }
        ).AsEnumerable().Select(k => k.ptKohi).ToList();

        foreach (var retData in retDatas)
        {
            retData.PtHokenPattern = hokenDatas.Find(h => h.ptHokenPattern.PtId == retData.PtId && h.ptHokenPattern.HokenPid == retData.HokenPid)?.ptHokenPattern;
            retData.HokenHoubetu = hokenDatas.Find(h => h.ptHokenPattern.PtId == retData.PtId && h.ptHokenPattern.HokenPid == retData.HokenPid)?.ptHokenInf.Houbetu;

            if (retData.PtHokenPattern.Kohi1Id == 0) continue;
            retData.Kohi1Houbetu = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.PtHokenPattern.Kohi1Id)?.Houbetu;

            if (retData.PtHokenPattern.Kohi2Id == 0) continue;
            retData.Kohi2Houbetu = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.PtHokenPattern.Kohi2Id)?.Houbetu;

            if (retData.PtHokenPattern.Kohi3Id == 0) continue;
            retData.Kohi3Houbetu = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.PtHokenPattern.Kohi3Id)?.Houbetu;

            if (retData.PtHokenPattern.Kohi4Id == 0) continue;
            retData.Kohi4Houbetu = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.PtHokenPattern.Kohi4Id)?.Houbetu;
        }
        #endregion

        return retDatas;
    }

    public List<CoSinKouiModel> GetSinKouis(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        //対象患者
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        //来院
        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);
        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts;
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts;
        //職員マスタ
        var userMsts = NoTrackingDataContext.UserMsts;
        //診療行為
        var sinCounts = NoTrackingDataContext.SinKouiCounts;
        var sinRpInfs = NoTrackingDataContext.SinRpInfs;
        var sinKouis = NoTrackingDataContext.SinKouis.Where(item => item.InoutKbn != 1);
        IQueryable<SinKouiDetail> sinKouiDetails = NoTrackingDataContext.SinKouiDetails;
        var jihiSbtMsts = NoTrackingDataContext.JihiSbtMsts;
        //保険情報
        (var ptHokenPatterns, var ptHokenInfs, var ptKohis, var isHokenConf, var isKohiConf) = GetPtHokenPatterns(hokenConf);

        //保険の絞り込み
        if (isHokenConf)
        {
            sinKouis = (
                from sinKoui in sinKouis
                join ptHokenPattern in ptHokenPatterns on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                select
                    sinKoui
            );
        }

        if (sinConf != null)
        {
            //検索ワード
            if (sinConf.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[] values = sinConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (sinConf.WordOpt == 0)
                {
                    //or条件
                    sinKouiDetails = sinKouiDetails.Where(p => searchWords.Any(key => p.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    sinKouiDetails = sinKouiDetails.Where(p => searchWords.All(key => p.ItemName.Contains(key)));
                }
            }
            //検索項目
            if (sinConf.ItemCds?.Count >= 3)
            {
                List<string> wrkItems = new List<string>();
                for (int i = 0; i + 3 <= sinConf.ItemCds.Count; i += 3)
                {
                    wrkItems.Add(sinConf.ItemCds[i]);
                }

                if (sinConf.ItemCmts?.Count >= 1)
                {
                    sinKouiDetails = sinKouiDetails.Where(p => wrkItems.Contains(p.ItemCd) || (p.ItemCd == ItemCdConst.CommentFree && sinConf.ItemCmts.Contains(p.ItemName)));
                }
                else
                {
                    sinKouiDetails = sinKouiDetails.Where(p => wrkItems.Contains(p.ItemCd));
                }
            }
            else if (sinConf.ItemCmts?.Count >= 1)
            {
                sinKouiDetails = sinKouiDetails.Where(p => p.ItemCd == ItemCdConst.CommentFree && sinConf.ItemCmts.Contains(p.ItemName));
            }
        }

        var sinDatas = (
            from ptInf in ptInfs
            join raiinInf in raiinInfs on
                new { ptInf.HpId, ptInf.PtId } equals
                new { raiinInf.HpId, raiinInf.PtId }
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
                new { userMst.HpId, TantoId = userMst.UserId } into tantoMstJoin
            from tantoMstj in tantoMstJoin.DefaultIfEmpty()
            join sinCount in sinCounts on
                new { raiinInf.HpId, raiinInf.RaiinNo } equals
                new { sinCount.HpId, sinCount.RaiinNo }
            join sinRpInf in sinRpInfs on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
            join sinKoui in sinKouis on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinDetail in sinKouiDetails on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinDetail.HpId, sinDetail.PtId, sinDetail.SinYm, sinDetail.RpNo, sinDetail.SeqNo }
            join jihiSbtMst in jihiSbtMsts on
                new { sinKoui.HpId, sinKoui.JihiSbt } equals
                new { jihiSbtMst.HpId, jihiSbtMst.JihiSbt } into jihiSbtMstJoin
            from jihiSbtMstj in jihiSbtMstJoin.DefaultIfEmpty()
            select new
            {
                raiinInf,
                uketukeSbtMstj,
                kaMstj,
                tantoMstj,
                sinCount,
                sinRpInf,
                sinKoui,
                sinDetail,
                jihiSbtMstj
            }
        );

        var retDatas = sinDatas.AsEnumerable()
            .Select(
                d =>
                    new CoSinKouiModel
                    (
                        d.raiinInf,
                        d.uketukeSbtMstj,
                        d.kaMstj,
                        d.tantoMstj,
                        d.sinCount,
                        d.sinRpInf,
                        d.sinKoui,
                        d.sinDetail,
                        d.jihiSbtMstj
                    )
            ).ToList();

        return retDatas;
    }

    public List<CoKarteInfModel> GetKarteInfs(int hpId,
                CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
                CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf)
    {
        //対象患者
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        //来院
        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);
        //受付種別マスタ
        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts;
        //診療科マスタ
        var kaMsts = NoTrackingDataContext.KaMsts;
        //職員マスタ
        var userMsts = NoTrackingDataContext.UserMsts;
        //カルテ
        var karteInfs = NoTrackingDataContext.KarteInfs.Where(k => k.IsDeleted == DeleteStatus.None);

        if (karteConf != null)
        {
            //カルテ区分
            karteInfs = karteConf.KarteKbns?.Count >= 1 ? karteInfs.Where(k => karteConf.KarteKbns.Contains(k.KarteKbn)) : karteInfs;
            //文字列検索
            karteInfs = karteConf.SearchWords?.Count >= 1 ? karteInfs.Where(r => karteConf.SearchWords.Any(key => r.Text.Contains(key))) : karteInfs;
        }

        var sinDatas = (
            from ptInf in ptInfs
            join raiinInf in raiinInfs on
                new { ptInf.HpId, ptInf.PtId } equals
                new { raiinInf.HpId, raiinInf.PtId }
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
                new { userMst.HpId, TantoId = userMst.UserId } into tantoMstJoin
            from tantoMstj in tantoMstJoin.DefaultIfEmpty()
            join karteInf in karteInfs on
                new { raiinInf.HpId, raiinInf.RaiinNo } equals
                new { karteInf.HpId, karteInf.RaiinNo }
            select new
            {
                raiinInf,
                uketukeSbtMstj,
                kaMstj,
                tantoMstj,
                karteInf
            }
        );

        var retDatas = sinDatas.AsEnumerable()
            .Select(
                d =>
                    new CoKarteInfModel
                    (
                        d.raiinInf,
                        d.uketukeSbtMstj,
                        d.kaMstj,
                        d.tantoMstj,
                        d.karteInf
                    )
            ).ToList();

        return retDatas;
    }

    public List<CoKensaModel> GetKensaInfs(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf, CoSta9000KensaConf kensaConf)
    {
        //対象患者
        var ptInfs = GetPtInfs(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf).ToList();
        //検査
        var kensaInfs = GetKensaInfs(hpId, kensaConf);

        var retDatas = (
            from ptInf in ptInfs
            join kensaInf in kensaInfs on
                ptInf.PtId equals kensaInf.PtId
            select
                kensaInf
        ).ToList();

        return retDatas;
    }


    /// <summary>
    /// 対象患者の取得
    ///     （検査情報の抽出条件を除く）
    /// </summary>
    /// <param name="ptConf">患者情報の抽出条件</param>
    /// <param name="hokenConf">保険情報の抽出条件</param>
    /// <param name="byomeiConf">病名情報の抽出条件</param>
    /// <param name="raiinConf">来院情報の抽出条件</param>
    /// <param name="sinConf">診療情報の抽出条件</param>
    /// <param name="karteConf">カルテ情報の抽出条件</param>
    /// <returns></returns>
    private IQueryable<PtInf> GetPtInfs(
        CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
        CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf)
    {
        #region 患者情報
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(p => p.IsDeleted == DeleteStatus.None);

        //テスト患者
        ptInfs = !(ptConf?.IsTester ?? false) ? ptInfs.Where(p => p.IsTester == 0) : ptInfs;

        bool isPtGrp = false;
        if (ptConf != null)
        {
            //患者番号
            ptInfs = ptConf.StartPtNum > 0 ? ptInfs.Where(p => p.PtNum >= ptConf.StartPtNum) : ptInfs;
            ptInfs = ptConf.EndPtNum > 0 ? ptInfs.Where(p => p.PtNum <= ptConf.EndPtNum) : ptInfs;
            ptInfs = ptConf.PtNums?.Count > 0 ? ptInfs.Where(p => (ptConf.PtNums.Count == 0 || ptConf.PtNums.Contains(p.PtNum))) : ptInfs;
            //カナ氏名
            ptInfs = ptConf.KanaName != string.Empty ? ptInfs.Where(p => p.KanaName != null && p.KanaName.Contains(ptConf.KanaName) == true) : ptInfs;
            ptInfs = ptConf.Name != string.Empty ? ptInfs.Where(p => p.Name != null && p.Name.Contains(ptConf.Name)) : ptInfs;
            //生年月日
            ptInfs = ptConf.StartBirthday > 0 ? ptInfs.Where(p => p.Birthday >= ptConf.StartBirthday) : ptInfs;
            ptInfs = ptConf.EndBirthday > 0 ? ptInfs.Where(p => p.Birthday <= ptConf.EndBirthday) : ptInfs;
            //年齢 .. (基準の日付 – 誕生日) / 10000
            int baseDate = ptConf.AgeBaseDate > 0 ? ptConf.AgeBaseDate : CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();
            ptInfs = (ptConf.StartAge != null && ptConf.StartAge > 0) ? ptInfs.Where(p => (baseDate - p.Birthday) / 10000 >= ptConf.StartAge) : ptInfs;
            ptInfs = (ptConf.EndAge != null && ptConf.EndAge > 0) ? ptInfs.Where(p => (baseDate - p.Birthday) / 10000 <= ptConf.EndAge) : ptInfs;
            //性別
            ptInfs = ptConf.Sex > 0 ? ptInfs.Where(p => p.Sex == ptConf.Sex) : ptInfs;
            //郵便番号
            ptInfs = ptConf.HomePost != string.Empty ? ptInfs.Where(p => p.HomePost != null && p.HomePost.StartsWith(ptConf.HomePost)) : ptInfs;
            //住所
            ptInfs = ptConf.HomeAddress != string.Empty ? ptInfs.Where(p => (p.HomeAddress1 + p.HomeAddress2).Contains(ptConf.HomeAddress)) : ptInfs;
            //電話
            ptInfs = ptConf.Tel != string.Empty ? ptInfs.Where(p =>
                (p.Tel1 != null && p.Tel1.Replace("-", "").Contains(ptConf.Tel)) || (p.Tel2 != null && p.Tel2.Replace("-", "").Contains(ptConf.Tel)) || (p.RenrakuTel != null && p.RenrakuTel.Replace("-", "").Contains(ptConf.Tel))
            ) : ptInfs;
            //登録日
            DateTime startRegDate;
            if (DateTime.TryParseExact(ptConf.StartRegDate.ToString(), "yyyyMMdd", null, DateTimeStyles.None, out startRegDate))
            {
                ptInfs = ptInfs.Where(p => p.CreateDate >= CIUtil.SetKindUtc(startRegDate));
            }
            DateTime endRegDate;
            if (DateTime.TryParseExact(ptConf.EndRegDate.ToString(), "yyyyMMdd", null, DateTimeStyles.None, out endRegDate))
            {
                endRegDate = endRegDate.AddDays(1);
                ptInfs = ptInfs.Where(p => p.CreateDate < CIUtil.SetKindUtc(endRegDate));
            }

            //患者グループ
            if (ptConf.PtGrps?.Count >= 1)
            {
                isPtGrp = true;
                foreach (var ptGrp in ptConf.PtGrps)
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
        }

        #endregion
        #region 保険情報
        (var ptHokenPatterns, var ptHokenInfs, var ptKohis, var isHokenConf, var isKohiConf) = GetPtHokenPatterns(hokenConf);

        var ptHokens = (
            from hokenPattern in ptHokenPatterns
            join ptHokenInf in ptHokenInfs on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            select new
            {
                hokenPattern.HpId,
                hokenPattern.PtId
            }
        );

        var ptKohi1s = (
            from hokenPattern in ptHokenPatterns
            join ptKohi1 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi1Id } equals
                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId }
            select new
            {
                hokenPattern.HpId,
                hokenPattern.PtId
            }
        );

        var ptKohi2s = (
            from hokenPattern in ptHokenPatterns
            join ptKohi2 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi2Id } equals
                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId }
            select new
            {
                hokenPattern.HpId,
                hokenPattern.PtId
            }
        );

        var ptKohi3s = (
            from hokenPattern in ptHokenPatterns
            join ptKohi3 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi3Id } equals
                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId }
            select new
            {
                hokenPattern.HpId,
                hokenPattern.PtId
            }
        );

        var ptKohi4s = (
            from hokenPattern in ptHokenPatterns
            join ptKohi4 in ptKohis on
                new { hokenPattern.HpId, hokenPattern.PtId, hokenPattern.Kohi4Id } equals
                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId }
            select new
            {
                hokenPattern.HpId,
                hokenPattern.PtId
            }
        );
        #endregion

        #region 病名情報
        (var ptByomeis, bool isByomeiConf) = GetPtByomeis(byomeiConf);
        #endregion

        #region 来院情報
        bool isVisitConf = false;

        (var raiinInfs, bool isRaiinConf) = GetRaiinInfs(raiinConf);
        IQueryable<PtLastVisitDate> ptLastVisits = NoTrackingDataContext.PtLastVisitDates;

        if (raiinConf != null && (raiinConf.StartLastVisitDate > 0 || raiinConf.EndLastVisitDate > 0))
        {
            var initVisits = ptLastVisits;

            //最終来院日
            ptLastVisits = raiinConf.StartLastVisitDate > 0 ? ptLastVisits.Where(p => p.LastVisitDate >= raiinConf.StartLastVisitDate) : ptLastVisits;
            ptLastVisits = raiinConf.EndLastVisitDate > 0 ? ptLastVisits.Where(p => p.LastVisitDate <= raiinConf.EndLastVisitDate) : ptLastVisits;

            //条件有無
            isVisitConf = initVisits != ptLastVisits;
        }
        #endregion

        #region 診療情報
        //算定
        var sinKouis = NoTrackingDataContext.SinKouis.Where(item => item.InoutKbn != 1);
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts;
        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails;

        //保険の絞り込み
        if (isHokenConf)
        {
            sinKouis = (
                from sinKoui in sinKouis
                join ptHokenPattern in ptHokenPatterns on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptHokenInf in ptHokenInfs on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                where
                    (!isKohiConf) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi1Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi2Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi3Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi4Id
                    )
                select
                    sinKoui
            );
        }

        //来院の絞り込み
        var sinJoins = (
            from sinCount in sinKouiCounts
            join sinKoui in sinKouis on
               new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
               new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
            join sinDetail in sinKouiDetails on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                new { sinDetail.HpId, sinDetail.PtId, sinDetail.SinYm, sinDetail.RpNo, sinDetail.SeqNo }
            join raiinInf in raiinInfs on
                new { sinCount.HpId, sinCount.PtId, sinCount.SinDate, sinCount.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
            group
                new { sinCount.Count, sinDetail.Suryo, sinDetail.ItemName } by
                new { sinCount.HpId, sinCount.PtId, ItemCd = sinDetail.ItemCd == ItemCdConst.CommentFree ? sinDetail.ItemName : sinDetail.ItemCd } into sg
            select new
            {
                sg.Key.HpId,
                sg.Key.PtId,
                sg.Key.ItemCd,
                ItemName = sg.Max(s => s.ItemName),
                ItemCount = sg.Sum(s => s.Count * s.Suryo)
            }
        );

        bool isSinConf = sinConf != null && sinConf.DataKind == 0;
        if (isSinConf)
        {
            //検索ワード
            if (sinConf?.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[]? values = sinConf?.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                if (values != null)
                {
                    searchWords.AddRange(values);
                }

                var keywordConditions = searchWords.Select(keyword => $"%{keyword}%").Distinct().ToList();

                if (sinConf?.WordOpt == 0)
                {
                    //or条件
                    sinJoins = sinJoins.Where(item => keywordConditions.Any(condition => EF.Functions.Like(item.ItemName ?? string.Empty, condition)));
                }
                else
                {
                    //and条件
                    sinJoins = sinJoins.Where(item => keywordConditions.All(condition => EF.Functions.Like(item.ItemName ?? string.Empty, condition)));
                }
            }
            //検索項目
            var itemCds = new List<string>();
            if (sinConf?.ItemCds?.Count >= 1)
            {
                itemCds.AddRange(sinConf.ItemCds);
            }
            int count = sinConf?.ItemCmts?.Count ?? 0;
            for (int i = 0; i < count; i++)
            {
                itemCds.Add(sinConf?.ItemCmts[i] ?? string.Empty);
                itemCds.Add("");
                itemCds.Add("");
            }

            if (itemCds.Count >= 3)
            {
                var wrkItems = sinJoins;

                for (int i = 0; i + 3 <= itemCds.Count; i += 3)
                {
                    string wrkCd = itemCds[i];
                    int lowVal = itemCds[i + 1].AsInteger();
                    int highVal = itemCds[i + 2].AsInteger();

                    var curItems =
                        lowVal == 0 && highVal == 0 ?
                            sinJoins.Where(p => p.ItemCd == wrkCd) :
                        lowVal != 0 ?
                            sinJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount >= lowVal) :
                        highVal != 0 ?
                            sinJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount <= highVal) :
                        sinJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount >= lowVal && p.ItemCount <= highVal);

                    if (i == 0)
                    {
                        wrkItems = curItems;
                    }
                    else
                    {
                        if (sinConf?.ItemCdOpt == 0)
                        {
                            //or条件
                            wrkItems = wrkItems.Union(curItems);
                        }
                        else
                        {
                            //and条件
                            wrkItems = (
                                from wrkItem in wrkItems
                                where
                                    (
                                        from c in curItems
                                        select c
                                    ).Any(
                                        p =>
                                            p.HpId == wrkItem.HpId &&
                                            p.PtId == wrkItem.PtId
                                    )
                                select
                                    wrkItem
                            );
                        }
                    }
                }

                sinJoins = wrkItems;
            }
        }
        //保険条件の追加
        if (isHokenConf && isRaiinConf && (sinConf?.DataKind ?? 0) == 0)
        {
            isSinConf = true;
        }

        //オーダー
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(p => p.IsDeleted == DeleteStatus.None);
        var odrDetails = NoTrackingDataContext.OdrInfDetails;

        //保険の絞り込み
        if (isHokenConf)
        {
            odrInfs = (
                from odrInf in odrInfs
                join ptHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptHokenInf in ptHokenInfs on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                where
                    (!isKohiConf) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi1Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi2Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi3Id
                    ) ||
                    (
                        from ptKohi in ptKohis
                        select ptKohi
                    ).Any(
                        k =>
                            k.HpId == ptHokenPattern.HpId &&
                            k.PtId == ptHokenPattern.PtId &&
                            k.HokenId == ptHokenPattern.Kohi4Id
                    )
                select
                    odrInf
            );
        }

        //来院の絞り込み
        var odrJoins = (
            from odrInf in odrInfs
            join odrDetail in odrDetails on
                new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
            join raiinInf in raiinInfs on
                new { odrInf.HpId, odrInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.RaiinNo }
            group
                new { odrInf.DaysCnt, odrDetail.Suryo, odrDetail.ItemName, odrDetail.TermVal, odrDetail.YohoKbn } by
                new { odrInf.HpId, odrInf.PtId, ItemCd = odrDetail.ItemCd == string.Empty ? odrDetail.ItemName : odrDetail.ItemCd } into og
            select new
            {
                og.Key.HpId,
                og.Key.PtId,
                og.Key.ItemCd,
                ItemName = og.Max(z => z.ItemName),
                ItemCount = og.Sum(z => (z.YohoKbn == 1 ? 1 : z.DaysCnt) * (z.TermVal > 0 ? (z.Suryo == 0 ? 1 : z.Suryo) * z.TermVal : (z.Suryo == 0 ? 1 : z.Suryo)))
            }
        );

        bool isOdrConf = sinConf != null && sinConf.DataKind == 1;
        if (isOdrConf)
        {
            //検索ワード
            if (sinConf?.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[]? values = sinConf?.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                if (values != null)
                {
                    searchWords.AddRange(values);
                }

                var keywordConditions = searchWords.Select(keyword => $"%{keyword}%").Distinct().ToList();
                if (sinConf?.WordOpt == 0)
                {
                    //or条件
                    odrJoins = odrJoins.Where(item => keywordConditions.Any(condition => EF.Functions.Like(item.ItemName ?? string.Empty, condition)));
                }
                else
                {
                    //and条件
                    odrJoins = odrJoins.Where(item => keywordConditions.All(condition => EF.Functions.Like(item.ItemName ?? string.Empty, condition)));
                }
            }
            //検索項目
            var ItemCds = new List<string>();
            if (sinConf?.ItemCds?.Count >= 1)
            {
                ItemCds.AddRange(sinConf.ItemCds);
            }
            int count = sinConf?.ItemCmts?.Count ?? 0;
            for (int i = 0; i < count; i++)
            {
                ItemCds.Add(sinConf?.ItemCmts[i] ?? string.Empty);
                ItemCds.Add("");
                ItemCds.Add("");
            }

            if (ItemCds.Count >= 3)
            {
                var wrkItems = odrJoins;

                for (int i = 0; i + 3 <= ItemCds.Count; i += 3)
                {
                    string wrkCd = ItemCds[i];
                    int lowVal = ItemCds[i + 1].AsInteger();
                    int highVal = ItemCds[i + 2].AsInteger();

                    var curItems =
                        lowVal == 0 && highVal == 0 ?
                            odrJoins.Where(p => p.ItemCd == wrkCd) :
                        lowVal != 0 ?
                            odrJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount >= lowVal) :
                        highVal != 0 ?
                            odrJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount <= highVal) :
                        odrJoins.Where(p => p.ItemCd == wrkCd && p.ItemCount >= lowVal && p.ItemCount <= highVal);

                    if (i == 0)
                    {
                        wrkItems = curItems;
                    }
                    else
                    {
                        if (sinConf?.ItemCdOpt == 0)
                        {
                            //or条件
                            wrkItems = wrkItems.Union(curItems);
                        }
                        else
                        {
                            //and条件
                            wrkItems = (
                                from wrkItem in wrkItems
                                where
                                    (
                                        from c in curItems
                                        select c
                                    ).Any(
                                        p =>
                                            p.HpId == wrkItem.HpId &&
                                            p.PtId == wrkItem.PtId
                                    )
                                select
                                    wrkItem
                            );
                        }
                    }
                }

                odrJoins = wrkItems;
            }
        }
        //保険条件の追加
        if (isHokenConf && isRaiinConf && (sinConf?.DataKind ?? 0) == 1)
        {
            isOdrConf = true;
        }
        #endregion

        #region カルテ情報
        var karteInfs = NoTrackingDataContext.KarteInfs.Where(k => k.IsDeleted == DeleteStatus.None);

        bool isKarteConf = karteConf != null;
        if (isKarteConf)
        {
            //来院の絞り込み
            if (isRaiinConf)
            {
                karteInfs = (
                    from karteInf in karteInfs
                    join raiinInf in raiinInfs on
                        new { karteInf.HpId, karteInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.RaiinNo }
                    select
                        karteInf
                );
            }

            //カルテ区分
            karteInfs = karteConf?.KarteKbns?.Count > 0 ? karteInfs.Where(k => karteConf.KarteKbns.Contains(k.KarteKbn)) : karteInfs;

            //文字列検索
            if (karteConf?.WordOpt == 0)
            {
                //or条件

                var keywordConditions = karteConf.SearchWords.Select(keyword => $"%{keyword}%").Distinct().ToList();
                karteInfs = karteConf.SearchWords?.Count >= 1 ?
                                     karteInfs.Where(item => keywordConditions.Any(condition => EF.Functions.Like(item.Text ?? string.Empty, condition)))
                            : karteInfs;
            }
            else
            {
                //and条件
                var wrkKartes = karteInfs;

                int count = karteConf?.SearchWords.Count ?? 0;
                for (int i = 0; i < count; i++)
                {
                    var wrkWord = karteConf?.SearchWords[i] ?? string.Empty;
                    var curKartes = karteInfs.Where(p => (p.Text ?? string.Empty).Contains(wrkWord));

                    wrkKartes = (
                        from wrkKarte in wrkKartes
                        where
                            (
                                from c in curKartes
                                select c
                            ).Any(
                                p =>
                                    p.HpId == wrkKarte.HpId &&
                                    p.PtId == wrkKarte.PtId
                            )
                        select
                            wrkKarte
                    );
                }

                karteInfs = wrkKartes;
            }
        }
        #endregion

        //各条件を結合
        var retDatas = (
            from ptInf in ptInfs
            where
            #region 患者グループ条件
                (
                    (!isPtGrp) ||
                    (
                        from pg in ptGrpInfs
                        select pg
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
            #endregion
            #region 保険条件
                (
                    (!isHokenConf) ||
                    (
                        from ph in ptHokens
                        select ph
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    ) &&
                    (
                        (!isKohiConf) ||
                        (
                            from pk in ptKohi1s
                            select pk
                        ).Any(
                            p =>
                                p.HpId == ptInf.HpId &&
                                p.PtId == ptInf.PtId
                        ) ||
                        (
                            from pk in ptKohi2s
                            select pk
                        ).Any(
                            p =>
                                p.HpId == ptInf.HpId &&
                                p.PtId == ptInf.PtId
                        ) ||
                        (
                            from pk in ptKohi3s
                            select pk
                        ).Any(
                            p =>
                                p.HpId == ptInf.HpId &&
                                p.PtId == ptInf.PtId
                        ) ||
                        (
                            from pk in ptKohi4s
                            select pk
                        ).Any(
                            p =>
                                p.HpId == ptInf.HpId &&
                                p.PtId == ptInf.PtId
                        )
                    )
                ) &&
            #endregion
            #region 病名条件
                (
                    (!isByomeiConf) ||
                    (
                        from pb in ptByomeis
                        select pb
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
            #endregion
            #region 来院条件
                (
                    (!isRaiinConf) ||
                    (
                        from r in raiinInfs
                        select r
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
                (
                    (!isVisitConf) ||
                    (
                        from r in ptLastVisits
                        select r
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
            #endregion
            #region 診療条件
                (
                    (!isSinConf) ||
                    (
                        from sj in sinJoins
                        select sj
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
                (
                    (!isOdrConf) ||
                    (
                        from oj in odrJoins
                        select oj
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                ) &&
            #endregion
            #region カルテ条件
                (
                    (!isKarteConf) ||
                    (
                        from kr in karteInfs
                        select kr
                    ).Any(
                        p =>
                            p.HpId == ptInf.HpId &&
                            p.PtId == ptInf.PtId
                    )
                )
            #endregion
            select
                ptInf
        );

        return retDatas;
    }

    private (IQueryable<PtHokenPattern> ptHokenPatterns, IQueryable<PtHokenInf> ptHokenInfs, IQueryable<PtKohi> ptKohis, bool isHokenConf, bool isKohiConf)
        GetPtHokenPatterns(CoSta9000HokenConf? hokenConf)
    {
        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(p => p.IsDeleted == DeleteStatus.None);
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.IsDeleted == DeleteStatus.None);
        var ptKohis = NoTrackingDataContext.PtKohis.Where(p => p.IsDeleted == DeleteStatus.None);

        if (hokenConf != null && (hokenConf.Houbetu1 != string.Empty || hokenConf.Houbetu2 != string.Empty || hokenConf.Houbetu3 != string.Empty || hokenConf.Houbetu4 != string.Empty))
        {
            ptHokenPatterns = (
                from ptHokenPattern in ptHokenPatterns
                join ptKohi1 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                    new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1Join
                from ptKohi1j in ptKohi1Join.DefaultIfEmpty()
                join ptKohi2 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                    new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2Join
                from ptKohi2j in ptKohi2Join.DefaultIfEmpty()
                join ptKohi3 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                    new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3Join
                from ptKohi3j in ptKohi3Join.DefaultIfEmpty()
                join ptKohi4 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                    new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4Join
                from ptKohi4j in ptKohi4Join.DefaultIfEmpty()
                where
                    //法別番号
                    (hokenConf.Houbetu1 != string.Empty ?
                        (ptKohi1j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu1 || (ptKohi2j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu1 ||
                         ptKohi3j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu1 || (ptKohi4j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu1) : true) &&
                    (hokenConf.Houbetu2 != string.Empty ?
                        (ptKohi1j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu2 || (ptKohi2j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu2 ||
                         ptKohi3j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu2 || (ptKohi4j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu2) : true) &&
                    (hokenConf.Houbetu3 != string.Empty ?
                        (ptKohi1j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu3 || (ptKohi2j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu3 ||
                         ptKohi3j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu3 || (ptKohi4j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu3) : true) &&
                    (hokenConf.Houbetu4 != string.Empty ?
                        (ptKohi1j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu4 || (ptKohi2j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu4 ||
                         ptKohi3j.FutansyaNo.Substring(0, 2) == hokenConf.Houbetu4 || (ptKohi4j.FutansyaNo ?? string.Empty).Substring(0, 2) == hokenConf.Houbetu4) : true)
                select
                    ptHokenPattern
            );
        }

        bool isHokenConf = hokenConf != null;
        bool isKohiConf = false;
        if (isHokenConf)
        {
            var initKohis = ptKohis;
            string sEdaNo = hokenConf.EdaNo.TrimStart('0');

            //保険者番号
            ptHokenInfs = hokenConf.StartHokensyaNo != string.Empty ? ptHokenInfs.Where(p => p.HokensyaNo.CompareTo(hokenConf.StartHokensyaNo) >= 0) : ptHokenInfs;
            ptHokenInfs = hokenConf.EndHokensyaNo != string.Empty ? ptHokenInfs.Where(p => p.HokensyaNo.CompareTo(hokenConf.EndHokensyaNo) <= 0) : ptHokenInfs;
            //記号
            ptHokenInfs = hokenConf.Kigo != string.Empty ? ptHokenInfs.Where(p => p.Kigo.Contains(hokenConf.Kigo)) : ptHokenInfs;
            //番号
            ptHokenInfs = hokenConf.Bango != string.Empty ? ptHokenInfs.Where(p => p.Bango.Contains(hokenConf.Bango)) : ptHokenInfs;
            ptHokenInfs = hokenConf.EdaNo != string.Empty ? ptHokenInfs.Where(p => p.EdaNo == hokenConf.EdaNo || p.EdaNo == sEdaNo) : ptHokenInfs;
            //本人・家族
            ptHokenInfs = hokenConf.HonkeKbn > 0 ? ptHokenInfs.Where(p => p.HonkeKbn == hokenConf.HonkeKbn) : ptHokenInfs;
            //公費負担者番号
            ptKohis = hokenConf.StartFutansyaNo != string.Empty ? ptKohis.Where(p => p.FutansyaNo.CompareTo(hokenConf.StartFutansyaNo) >= 0) : ptKohis;
            ptKohis = hokenConf.EndFutansyaNo != string.Empty ? ptKohis.Where(p => p.FutansyaNo.CompareTo(hokenConf.EndFutansyaNo) <= 0) : ptKohis;
            //公費特殊番号
            ptKohis = hokenConf.StartTokusyuNo != string.Empty ? ptKohis.Where(p => p.TokusyuNo.CompareTo(hokenConf.StartTokusyuNo) >= 0) : ptKohis;
            ptKohis = hokenConf.EndTokusyuNo != string.Empty ? ptKohis.Where(p => p.TokusyuNo.CompareTo(hokenConf.EndTokusyuNo) <= 0) : ptKohis;
            //有効期限
            ptHokenPatterns = hokenConf.StartDate > 0 ? ptHokenPatterns.Where(p => p.EndDate >= hokenConf.StartDate) : ptHokenPatterns;
            ptHokenPatterns = hokenConf.EndDate > 0 ? ptHokenPatterns.Where(p => p.StartDate <= hokenConf.EndDate) : ptHokenPatterns;
            //保険種別
            if (hokenConf.HokenSbts?.Count >= 1)
            {
                //保険種別
                List<int> hokenKbns = new List<int>();
                if (hokenConf.HokenSbts.Contains(1) || hokenConf.HokenSbts.Contains(2)) hokenKbns.Add(1);                                      //社保・公費
                if (hokenConf.HokenSbts.Contains(3) || hokenConf.HokenSbts.Contains(4) || hokenConf.HokenSbts.Contains(5)) hokenKbns.Add(2);   //国保・退職・後期
                if (hokenConf.HokenSbts.Contains(6)) { hokenKbns.Add(11); hokenKbns.Add(12); hokenKbns.Add(13); }                              //労災
                if (hokenConf.HokenSbts.Contains(7)) hokenKbns.Add(14);                                                                        //自賠
                if (hokenConf.HokenSbts.Contains(8) || hokenConf.HokenSbts.Contains(9)) hokenKbns.Add(0);                                      //自費・自レ

                ptHokenPatterns = ptHokenPatterns.Where(r => hokenKbns.Contains(r.HokenKbn));

                if (hokenKbns.Contains(1))
                {
                    //社保除く
                    ptHokenPatterns = !hokenConf.HokenSbts.Contains(1) ? ptHokenPatterns.Where(p => p.HokenSbtCd / 100 != 1) : ptHokenPatterns;
                    //公費除く
                    ptHokenPatterns = !hokenConf.HokenSbts.Contains(2) ? ptHokenPatterns.Where(p => p.HokenSbtCd / 100 != 5) : ptHokenPatterns;
                }
                if (hokenKbns.Contains(2))
                {
                    //国保除く
                    ptHokenPatterns = !hokenConf.HokenSbts.Contains(3) ? ptHokenPatterns.Where(p => p.HokenSbtCd / 100 != 2) : ptHokenPatterns;
                    //退職除く
                    ptHokenPatterns = !hokenConf.HokenSbts.Contains(4) ? ptHokenPatterns.Where(p => p.HokenSbtCd / 100 != 4) : ptHokenPatterns;
                    //後期除く
                    ptHokenPatterns = !hokenConf.HokenSbts.Contains(5) ? ptHokenPatterns.Where(p => p.HokenSbtCd / 100 != 3) : ptHokenPatterns;
                }
                if (hokenKbns.Contains(0))
                {
                    //自費除く
                    ptHokenInfs = !hokenConf.HokenSbts.Contains(8) ? ptHokenInfs.Where(p => p.Houbetu != "108") : ptHokenInfs;
                    //自レ除く
                    ptHokenInfs = !hokenConf.HokenSbts.Contains(9) ? ptHokenInfs.Where(p => p.Houbetu != "109") : ptHokenInfs;
                }
            }
            //法別番号
            ptHokenInfs = hokenConf.Houbetu0 != string.Empty ? ptHokenInfs.Where(p => p.Houbetu == hokenConf.Houbetu0) : ptHokenInfs;
            //高額区分
            ptHokenInfs = hokenConf.KogakuKbns?.Count > 0 ? ptHokenInfs.Where(p => hokenConf.KogakuKbns.Contains(p.KogakuKbn)) : ptHokenInfs;
            //公費保険番号
            int startNo = hokenConf.StartKohiHokenNo.HokenNo * 100000 + hokenConf.StartKohiHokenNo.HokenEdaNo;
            ptKohis = hokenConf.StartKohiHokenNo.HokenNo > 0 ? ptKohis.Where(p => p.HokenNo * 100000 + p.HokenEdaNo >= startNo) : ptKohis;
            int endNo = hokenConf.EndKohiHokenNo.HokenNo * 100000 + hokenConf.EndKohiHokenNo.HokenEdaNo;
            ptKohis = hokenConf.EndKohiHokenNo.HokenNo > 0 ? ptKohis.Where(p => p.HokenNo * 100000 + p.HokenEdaNo <= endNo) : ptKohis;

            //条件有無
            isKohiConf = initKohis != ptKohis;
        }

        return (ptHokenPatterns, ptHokenInfs, ptKohis, isHokenConf, isKohiConf);
    }

    /// <summary>
    /// 病名情報の取得
    /// </summary>
    /// <param name="byomeiConf"></param>
    /// <returns></returns>
    private (IQueryable<PtByomei> ptByomeis, bool isByomeiConf) GetPtByomeis(CoSta9000ByomeiConf? byomeiConf)
    {
        var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.IsDeleted == DeleteStatus.None);

        bool isByomeiConf = byomeiConf != null;
        if (isByomeiConf)
        {
            //開始日
            ptByomeis = byomeiConf?.StartStartDate > 0 ? ptByomeis.Where(p => p.StartDate >= byomeiConf.StartStartDate) : ptByomeis;
            ptByomeis = byomeiConf?.EndStartDate > 0 ? ptByomeis.Where(p => p.StartDate <= byomeiConf.EndStartDate) : ptByomeis;
            //転帰日
            ptByomeis = byomeiConf?.StartTenkiDate > 0 ? ptByomeis.Where(p => p.TenkiDate >= byomeiConf.StartTenkiDate) : ptByomeis;
            ptByomeis = byomeiConf?.EndTenkiDate > 0 ? ptByomeis.Where(p => p.TenkiDate <= byomeiConf.EndTenkiDate) : ptByomeis;
            //転帰区分
            ptByomeis = byomeiConf?.TenkiKbns?.Count > 0 ? ptByomeis.Where(p => byomeiConf.TenkiKbns.Contains(p.TenkiKbn)) : ptByomeis;
            //疾患区分
            ptByomeis = byomeiConf?.SikkanKbns?.Count > 0 ? ptByomeis.Where(p => byomeiConf.SikkanKbns.Contains(p.SikkanKbn)) : ptByomeis;
            //難病外来コード
            ptByomeis = byomeiConf?.NanbyoCds?.Count > 0 ? ptByomeis.Where(p => byomeiConf.NanbyoCds.Contains(p.NanByoCd)) : ptByomeis;
            //疑い病名
            const string doubtCd = "8002";
            if (byomeiConf.IsDoubt == 1)
            {
                ptByomeis = ptByomeis.Where(p =>
                    p.SyusyokuCd1 == doubtCd || p.SyusyokuCd2 == doubtCd || p.SyusyokuCd3 == doubtCd ||
                    p.SyusyokuCd4 == doubtCd || p.SyusyokuCd5 == doubtCd || p.SyusyokuCd6 == doubtCd ||
                    p.SyusyokuCd7 == doubtCd || p.SyusyokuCd8 == doubtCd || p.SyusyokuCd9 == doubtCd ||
                    p.SyusyokuCd10 == doubtCd || p.SyusyokuCd11 == doubtCd || p.SyusyokuCd12 == doubtCd ||
                    p.SyusyokuCd13 == doubtCd || p.SyusyokuCd14 == doubtCd || p.SyusyokuCd15 == doubtCd ||
                    p.SyusyokuCd16 == doubtCd || p.SyusyokuCd17 == doubtCd || p.SyusyokuCd18 == doubtCd ||
                    p.SyusyokuCd19 == doubtCd || p.SyusyokuCd20 == doubtCd || p.SyusyokuCd21 == doubtCd
                );
            }
            else if (byomeiConf.IsDoubt == 2)
            {
                ptByomeis = ptByomeis.Where(p =>
                    p.SyusyokuCd1 != doubtCd && p.SyusyokuCd2 != doubtCd && p.SyusyokuCd3 != doubtCd &&
                    p.SyusyokuCd4 != doubtCd && p.SyusyokuCd5 != doubtCd && p.SyusyokuCd6 != doubtCd &&
                    p.SyusyokuCd7 != doubtCd && p.SyusyokuCd8 != doubtCd && p.SyusyokuCd9 != doubtCd &&
                    p.SyusyokuCd10 != doubtCd && p.SyusyokuCd11 != doubtCd && p.SyusyokuCd12 != doubtCd &&
                    p.SyusyokuCd13 != doubtCd && p.SyusyokuCd14 != doubtCd && p.SyusyokuCd15 != doubtCd &&
                    p.SyusyokuCd16 != doubtCd && p.SyusyokuCd17 != doubtCd && p.SyusyokuCd18 != doubtCd &&
                    p.SyusyokuCd19 != doubtCd && p.SyusyokuCd20 != doubtCd && p.SyusyokuCd21 != doubtCd
                );
            }
            //検索ワード
            if (byomeiConf.SearchWord != string.Empty)
            {
                //スペース区切りでキーワードを分解
                string[] values = byomeiConf.SearchWord?.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                if (values != null)
                {
                    searchWords.AddRange(values);
                }

                var keywordConditions = searchWords.Select(keyword => $"%{keyword}%").Distinct().ToList();
                if (byomeiConf.WordOpt == 0)
                {
                    //or条件
                    ptByomeis = ptByomeis.Where(item => keywordConditions.Any(condition => EF.Functions.Like(item.Byomei ?? string.Empty, condition)));
                }
                else
                {
                    //and条件
                    ptByomeis = ptByomeis.Where(item => keywordConditions.All(condition => EF.Functions.Like(item.Byomei ?? string.Empty, condition)));
                }
            }
            //検索病名
            const string freeByomeiCd = "0000999";
            if (byomeiConf.ByomeiCds?.Count >= 1)
            {
                //未コード化病名を除く
                byomeiConf.ByomeiCds = byomeiConf.ByomeiCds.Where(s => s != freeByomeiCd).ToList();
            }

            if (byomeiConf.ByomeiCdOpt == 0)
            {
                //or条件
                if (byomeiConf.ByomeiCds?.Count >= 1 && byomeiConf.Byomeis?.Count >= 1)
                {
                    ptByomeis = ptByomeis.Where(p => byomeiConf.ByomeiCds.Contains(p.ByomeiCd) || (p.ByomeiCd == freeByomeiCd && byomeiConf.Byomeis.Contains(p.Byomei)));
                }
                else if (byomeiConf.ByomeiCds?.Count >= 1)
                {
                    ptByomeis = ptByomeis.Where(p => byomeiConf.ByomeiCds.Contains(p.ByomeiCd));
                }
                else if (byomeiConf.Byomeis?.Count >= 1)
                {
                    ptByomeis = ptByomeis.Where(p => p.ByomeiCd == freeByomeiCd && byomeiConf.Byomeis.Contains(p.Byomei));
                }
            }
            else
            {
                //and条件
                List<string> wrkCds = new List<string>();
                if (byomeiConf.ByomeiCds?.Count >= 1) wrkCds.AddRange(byomeiConf.ByomeiCds);
                if (byomeiConf.Byomeis?.Count >= 1) wrkCds.AddRange(byomeiConf.Byomeis);

                var wrkItems = ptByomeis;

                for (int i = 0; i < wrkCds.Count; i++)
                {
                    var wrkCd = wrkCds[i];
                    IQueryable<PtByomei> curItems;
                    if ((byomeiConf.ByomeiCds ?? new()).Contains(wrkCd))
                    {
                        curItems = ptByomeis.Where(p => p.ByomeiCd == wrkCd);
                    }
                    else
                    {
                        curItems = ptByomeis.Where(p => p.ByomeiCd == freeByomeiCd && p.Byomei == wrkCd);
                    }

                    wrkItems = (
                        from wrkItem in wrkItems
                        where
                            (
                                from c in curItems
                                select c
                            ).Any(
                                p =>
                                    p.HpId == wrkItem.HpId &&
                                    p.PtId == wrkItem.PtId
                            )
                        select
                            wrkItem
                    );
                }

                ptByomeis = wrkItems;
            }
        }
        return (ptByomeis, isByomeiConf);
    }

    /// <summary>
    /// 来院情報の取得
    /// </summary>
    /// <param name="raiinConf"></param>
    /// <returns></returns>
    private (IQueryable<RaiinInf> raiinInfs, bool isRaiinConf) GetRaiinInfs(CoSta9000RaiinConf? raiinConf)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p => p.IsDeleted == DeleteStatus.None);
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);

        bool isRaiinConf = false;
        if (raiinConf != null)
        {
            var initRaiins = raiinInfs;

            //来院日
            raiinInfs = raiinConf.StartSinDate > 0 ? raiinInfs.Where(p => p.SinDate >= raiinConf.StartSinDate) : raiinInfs;
            raiinInfs = raiinConf.EndSinDate > 0 ? raiinInfs.Where(p => p.SinDate <= raiinConf.EndSinDate) : raiinInfs;
            //状態
            raiinInfs = raiinConf.Statuses?.Count > 0 ? raiinInfs.Where(p => raiinConf.Statuses.Contains(p.Status)) : raiinInfs;
            //受付種別
            raiinInfs = raiinConf.UketukeSbts?.Count > 0 ? raiinInfs.Where(p => raiinConf.UketukeSbts.Contains(p.UketukeSbt)) : raiinInfs;
            //診療科
            raiinInfs = raiinConf.KaIds?.Count > 0 ? raiinInfs.Where(p => raiinConf.KaIds.Contains(p.KaId)) : raiinInfs;
            //担当医
            raiinInfs = raiinConf.TantoIds?.Count > 0 ? raiinInfs.Where(p => raiinConf.TantoIds.Contains(p.TantoId)) : raiinInfs;

            #region 時間枠区分
            if (raiinConf.JikanKbns?.Count > 0)
            {
                List<int> jikanKbns = new List<int>();
                foreach (int jikanKbnConfs in raiinConf.JikanKbns)
                {
                    switch (jikanKbnConfs)
                    {
                        case 1:
                            jikanKbns.Add(0);
                            break;
                        case 2:
                            jikanKbns.Add(1);
                            break;
                        case 3:
                            jikanKbns.Add(2);
                            jikanKbns.Add(6);
                            break;
                        case 4:
                            jikanKbns.Add(3);
                            jikanKbns.Add(7);
                            break;
                        case 5:
                            jikanKbns.Add(4);
                            jikanKbns.Add(5);
                            break;
                        default:
                            break;
                    }
                }
                raiinInfs = jikanKbns.Count > 0 ? raiinInfs.Where(p => jikanKbns.Contains(p.JikanKbn)) : raiinInfs;
            }
            #endregion

            #region 来院日時点の年齢
            if (raiinConf.AgeFrom >= 0 || raiinConf.AgeTo >= 0)
            {
                var raiinJoins = (
                      from raiinInf in raiinInfs
                      join ptInf in ptInfs on
                          new { raiinInf.HpId, raiinInf.PtId } equals
                          new { ptInf.HpId, ptInf.PtId }
                      select new
                      {
                          raiinInf,
                          ptInf
                      }
                  );

                //年齢 .. (診療日 – 誕生日) / 10000
                raiinInfs =
                    raiinConf.AgeFrom >= 0 && raiinConf.AgeTo >= 0 ?
                        raiinJoins.Where
                            (p =>
                                (p.raiinInf.SinDate - p.ptInf.Birthday) / 10000 >= raiinConf.AgeFrom &&
                                (p.raiinInf.SinDate - p.ptInf.Birthday) / 10000 <= raiinConf.AgeTo
                            ).Select(p => p.raiinInf) :
                    raiinConf.AgeFrom >= 0 ?
                        raiinJoins.Where(p => (p.raiinInf.SinDate - p.ptInf.Birthday) / 10000 >= raiinConf.AgeFrom).Select(p => p.raiinInf) :
                    raiinConf.AgeTo >= 0 ?
                        raiinJoins.Where(p => (p.raiinInf.SinDate - p.ptInf.Birthday) / 10000 <= raiinConf.AgeTo).Select(p => p.raiinInf) :
                    raiinInfs;
            }
            #endregion

            #region 新患
            if (raiinConf.IsSinkan == 1)
            {
                var ptFirstVisits = GetPtFirstVisits();

                var firstVisitJ = (
                        from raiinInf in raiinInfs
                        join ptFirstVisit in ptFirstVisits on
                            new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate } equals
                            new { ptFirstVisit.HpId, ptFirstVisit.PtId, ptFirstVisit.SinDate }
                        select
                            raiinInf
                    );

                raiinInfs = firstVisitJ;
            }
            #endregion

            //条件有無
            isRaiinConf = initRaiins != raiinInfs;
        }
        return (raiinInfs, isRaiinConf);
    }

    public List<CoKensaModel> GetKensaInfs(int hpId, CoSta9000KensaConf kensaConf)
    {
        List<CoKensaModel> kensaDatas = new List<CoKensaModel>();

        //Planet接続設定
        string hostname = _systemConfig.PlanetHostName();
        string database = _systemConfig.PlanetDatabase();
        string username = _systemConfig.PlanetUserName();
        string password = _systemConfig.PlanetPassword();

        switch (_systemConfig.PlanetType())
        {
            #region 標準
            case 0:
                IQueryable<KensaInf> kensaInfs = NoTrackingDataContext.KensaInfs;
                IQueryable<KensaInfDetail> kensaDetails = NoTrackingDataContext.KensaInfDetails;
                var kensaMsts = NoTrackingDataContext.KensaMsts.Where(k => k.IsDelete == DeleteStatus.None);
                var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);

                if (kensaConf != null)
                {
                    //依頼日
                    kensaInfs = kensaConf.StartIraiDate > 0 ? kensaInfs.Where(p => p.IraiDate >= kensaConf.StartIraiDate) : kensaInfs;
                    kensaInfs = kensaConf.EndIraiDate > 0 ? kensaInfs.Where(p => p.IraiDate <= kensaConf.EndIraiDate) : kensaInfs;

                    //検査結果（項目の絞り込み）
                    if (kensaConf.ItemCds?.Count >= 4)
                    {
                        //or条件
                        kensaDetails = kensaConf.ItemCds?.Count >= 1 ? kensaDetails.Where(p => kensaConf.ItemCds.Contains(p.KensaItemCd)) : kensaDetails;
                    }
                }

                var kensaJoins = (
                    from kensaInf in kensaInfs
                    join ptInf in ptInfs on
                        new { kensaInf.HpId, kensaInf.PtId } equals
                        new { ptInf.HpId, ptInf.PtId }
                    join kensaDetail in kensaDetails on
                        new { kensaInf.HpId, kensaInf.PtId, kensaInf.IraiCd } equals
                        new { kensaDetail.HpId, kensaDetail.PtId, kensaDetail.IraiCd }
                    join kensaMst in kensaMsts on
                        new { kensaDetail.HpId, kensaDetail.KensaItemCd } equals
                        new { kensaMst.HpId, kensaMst.KensaItemCd } into kensaMstJoin
                    from kensaMstj in kensaMstJoin.DefaultIfEmpty()
                    where
                        kensaInf.HpId == hpId &&
                        kensaInf.IsDeleted == DeleteStatus.None
                    select new
                    {
                        kensaInf,
                        ptInf,
                        kensaDetail,
                        kensaMstj
                    }
                );

                kensaDatas = kensaJoins.AsEnumerable().Select(
                    d =>
                        new CoKensaModel()
                        {
                            PtId = d.kensaInf.PtId,
                            IraiDate = d.kensaInf.IraiDate,
                            CenterCd = d.kensaMstj?.CenterCd,
                            KensaItemCd = d.kensaDetail.KensaItemCd,
                            KensaName = d.kensaMstj?.KensaName,
                            ResultVal = d.kensaDetail.ResultVal,
                            UnitName = d.kensaMstj?.Unit,
                            ResultType = d.kensaDetail.ResultType,
                            AbnormalKbn = d.kensaDetail.AbnormalKbn,
                            StandardVal =
                                d.ptInf.Sex == 1 ? d.kensaMstj?.MaleStd :
                                d.ptInf.Sex == 2 ? d.kensaMstj?.FemaleStd :
                                string.Empty,
                            SortKey = string.Format
                                (
                                    "{0:D10}{1:D10}{2:D10}{3:D10}{4}{5}",
                                    d.kensaInf.IraiDate, d.kensaInf.RaiinNo, d.kensaInf.IraiCd, d.kensaMstj?.SortNo,
                                    d.kensaMstj?.OyaItemCd.AsString().PadRight(10, '0'), d.kensaDetail.KensaItemCd.AsString().PadRight(10, '0')
                                )
                        }
                ).ToList();

                break;
            #endregion
            #region Planet改
            case 1:
                using (var msSqlAccess = new RenkeiMsSqlDataAccess(hostname, database, username, password))
                {
                    string sql =
                        " SELECT" +
                        "     KZ.患者番号," +
                        "     KZ.依頼日," +
                        "     KZ.センターコード," +
                        "     KM.項目コード," +
                        "     KO.項目名称," +
                        "     KM.検査結果値," +
                        "     KM.検査値形態," +
                        "     KO.単位," +
                        "     KM.基準値区分," +
                        "     KM.下限値," +
                        "     KM.上限値," +
                        "     KM.判定," +
                        "     KO.表示コード," +
                        "     KZ.検査センターＫＥＹ" +
                        " FROM" +
                        "     TAIKEKKA KZ" +
                        "     INNER JOIN TAIKEKKAM KM ON" +
                        "             KZ.センターコード = KM.センターコード" +
                        "         AND KZ.検査センターＫＥＹ = KM.検査センターＫＥＹ" +
                        "     INNER JOIN TAIKOM KO ON" +
                        "             KM.項目センターコード = KO.センターコード" +
                        "         AND KM.項目コード = KO.項目コード" +
                        " WHERE" +
                        "     1 = 1";

                    //依頼日
                    sql = kensaConf?.StartIraiDate > 0 ? sql + $" AND KZ.依頼日 >= '{kensaConf.StartIraiDate}'" : sql;
                    sql = kensaConf?.EndIraiDate > 0 ? sql + $" AND KZ.依頼日 <= '{kensaConf.EndIraiDate}'" : sql;

                    //項目の絞り込み
                    if (kensaConf.ItemCds?.Count >= 4)
                    {
                        sql += " AND (";
                        for (int i = 0; i < kensaConf.ItemCds.Count; i += 4)
                        {
                            if (i > 0) sql += " OR ";
                            sql += $"KM.項目コード = '{kensaConf.ItemCds[i]}'";
                        }
                        sql += ")";
                    }

                    var dt = msSqlAccess.ExecuteReader(sql);
                    if (dt != null)
                    {
                        kensaDatas = (
                            from rw in dt.AsEnumerable()
                            select new CoKensaModel()
                            {
                                PtId = rw["患者番号"].AsInteger(),
                                IraiDate = rw["依頼日"].AsInteger(),
                                CenterCd = rw["センターコード"].AsString(),
                                KensaItemCd = rw["項目コード"].AsString(),
                                KensaName = rw["項目名称"].AsString(),
                                ResultVal = rw["検査結果値"].AsString(),
                                ResultType = rw["検査値形態"].AsString(),
                                UnitName = rw["単位"].AsString(),
                                AbnormalKbn = rw["判定"].AsString(),
                                StandardVal =
                                    rw["基準値区分"].AsString() == "L" ? rw["下限値"].AsString() + "未満" :
                                    rw["基準値区分"].AsString() == "E" ? rw["下限値"].AsString() + "以下" :
                                    rw["基準値区分"].AsString() == "U" ? rw["下限値"].AsString() + "以上" :
                                    rw["下限値"].AsString() + "～" + rw["上限値"].AsString(),
                                SortKey =
                                    rw["依頼日"].AsString().PadRight(8, '0') +
                                    rw["表示コード"].AsString().PadRight(20, '0') +
                                    rw["項目コード"].AsString().PadRight(20, '0') +
                                    rw["検査センターＫＥＹ"].AsString().PadRight(20, '0')
                            }
                        ).ToList();
                    }
                }

                break;
            #endregion
            #region Planet Next
            case 2:
                using (var msSqlAccess = new RenkeiMsSqlDataAccess(hostname, database, username, password))
                {
                    string sql =
                        " SELECT" +
                        "     Z.KANJID," +
                        "     CONVERT(VARCHAR, Z.UKEDT, 112) UKEDT," +
                        "     Z.USERCD," +
                        "     I.ITEMCD," +
                        "     I.SEIN," +
                        "     K.EDTDT," +
                        "     K.EDTDTN," +
                        "     Z.JENDER," +
                        "     I.KJKNJ_M," +
                        "     I.KJKNJ_F," +
                        "     K.IJOKBN," +
                        "     IG.SERNO" +
                        " FROM" +
                        "     ZOKU Z" +
                        "     INNER JOIN KENS K ON" +
                        "             Z.FILDT = K.FILDT" +
                        "         AND Z.JIGCD = K.JIGCD" +
                        "         AND Z.IDNO = K.IDNO" +
                        "     INNER JOIN ITEM I ON" +
                        "             K.JISSIK = I.JISSIK" +
                        "         AND K.ITEMCD = I.ITEMCD" +
                        "     LEFT JOIN (" +
                        "                SELECT" +
                        "                    Min(B.SERNO) SERNO," +
                        "                    B.ITEMCD_S" +
                        "                FROM" +
                        "                    ITMG A INNER JOIN ITMGL B ON" +
                        "                            A.GRPNO = B.GRPNO" +
                        "                        AND A.KUBN = B.KUBN" +
                        "                WHERE" +
                        "                        A.KUBN = 'S'" +
                        "                    AND A.GRPNO = 1" +
                        "                GROUP BY B.ITEMCD_S" +
                        "               ) IG ON" +
                        "         I.ITEMCD = IG.ITEMCD_S" +
                        " WHERE" +
                        "     I.JISSI_E = '2079/06/06'";

                    //依頼日
                    sql = kensaConf?.StartIraiDate > 0 ? sql + $" AND Z.UKEDT >= '{CIUtil.SDateToShowSDate(kensaConf.StartIraiDate)}'" : sql;
                    sql = kensaConf?.EndIraiDate > 0 ? sql + $" AND Z.UKEDT <= '{CIUtil.SDateToShowSDate(kensaConf.EndIraiDate)}'" : sql;

                    //項目の絞り込み
                    if (kensaConf.ItemCds?.Count >= 4)
                    {
                        sql += " AND (";
                        for (int i = 0; i < kensaConf.ItemCds.Count; i += 4)
                        {
                            if (i > 0) sql += " OR ";
                            sql += $"K.ITEMCD = '{kensaConf.ItemCds[i]}'";
                        }
                        sql += ")";
                    }

                    var dt = msSqlAccess.ExecuteReader(sql);
                    if (dt != null)
                    {
                        kensaDatas = (
                            from rw in dt.AsEnumerable()
                            select new CoKensaModel()
                            {
                                PtId = rw["KANJID"].AsInteger(),
                                IraiDate = rw["UKEDT"].AsInteger(),
                                CenterCd = rw["USERCD"].AsString(),
                                KensaItemCd = rw["ITEMCD"].AsString(),
                                KensaName = rw["SEIN"].AsString(),
                                ResultVal = rw["EDTDT"].AsString() == string.Empty ? rw["EDTDTN"].AsString() : rw["EDTDT"].AsString(),
                                AbnormalKbn =
                                    rw["IJOKBN"].AsString().CompareTo("4") > 0 ? "H" :
                                    rw["IJOKBN"].AsString().CompareTo("4") < 0 ? "L" :
                                    string.Empty,
                                StandardVal =
                                    rw["JENDER"].AsString() == "M" ? rw["KJKNJ_M"].AsString() :
                                    rw["JENDER"].AsString() == "F" ? rw["KJKNJ_F"].AsString() :
                                    string.Empty,
                                SortKey =
                                    rw["UKEDT"].AsString().PadRight(8, '0') +
                                    rw["SERNO"].AsString().PadRight(6, '0') +
                                    rw["ITEMCD"].AsString().PadRight(6, '0')
                            }
                        ).ToList();
                    }
                }

                break;
                #endregion
        }

        #region 検査結果の絞り込み
        if (kensaConf?.ItemCds?.Count >= 4)
        {
            //項目の絞り込み
            if (kensaConf.ItemCdOpt == 1)
            {
                //and条件
                var wrkDatas = kensaDatas;

                for (int i = 0; i + 4 < kensaConf.ItemCds.Count; i += 4)
                {
                    var wrkCd = kensaConf.ItemCds[i];
                    var curDatas = kensaDatas.Where(p => p.KensaItemCd == wrkCd);

                    wrkDatas = (
                        from wrkData in wrkDatas
                        where
                            (
                                from c in curDatas
                                select c
                            ).Any(
                                p => p.PtId == wrkData.PtId
                            )
                        select
                            wrkData
                    ).ToList();
                }

                kensaDatas = wrkDatas;
            }

            //結果値の絞り込み
            List<CoKensaModel> wrkItems = new List<CoKensaModel>();
            if (kensaConf.ItemCdOpt == 1)
            {
                wrkItems.AddRange(kensaDatas);
            }

            for (int i = 0; i + 4 <= kensaConf.ItemCds.Count; i += 4)
            {
                string wrkCd = kensaConf.ItemCds[i];
                string lowVal = kensaConf.ItemCds[i + 1];
                string highVal = kensaConf.ItemCds[i + 2];
                string abnormalKbn = kensaConf.ItemCds[i + 3];

                var curItems = kensaDatas.Where(k => k.KensaItemCd == wrkCd).ToList();
                if (lowVal.AsString() != string.Empty)
                {
                    double numLowVal;
                    if (double.TryParse(lowVal, out numLowVal))
                    {
                        curItems = curItems.Where(k => k.NumResultVal >= numLowVal).ToList();
                    }
                    else
                    {
                        curItems = curItems.Where(k => k.ResultVal.CompareTo(lowVal) >= 0).ToList();
                    }
                }
                if (highVal.AsString() != string.Empty)
                {
                    double numHighVal;
                    if (double.TryParse(highVal, out numHighVal))
                    {
                        curItems = curItems.Where(k => k.NumResultVal <= numHighVal).ToList();
                    }
                    else
                    {
                        curItems = curItems.Where(k => k.ResultVal.CompareTo(highVal) <= 0).ToList();
                    }
                }
                if (abnormalKbn.AsString() != string.Empty)
                {
                    curItems =
                        abnormalKbn == "1" ? curItems.Where(k => k.AbnormalKbn == "H").ToList() :
                        abnormalKbn == "2" ? curItems.Where(k => k.AbnormalKbn == "L").ToList() :
                        abnormalKbn == "3" ? curItems.Where(k => k.AbnormalKbn == "H" || k.AbnormalKbn == "L").ToList() :
                        curItems;
                }

                if (kensaConf.ItemCdOpt == 0)
                {
                    //or条件
                    wrkItems.AddRange(curItems);
                }
                else
                {
                    //and条件
                    wrkItems = (
                        from wrkItem in wrkItems
                        where
                            (
                                from c in curItems
                                select c
                            ).Any(
                                p =>
                                    p.PtId == wrkItem.PtId
                            )
                        select
                            wrkItem
                    ).ToList();
                }
            }

            kensaDatas = wrkItems;
        }
        #endregion

        #region PtNum -> PtId変換
        if (kensaDatas.Count >= 1 && _systemConfig.PlanetType() != 0)
        {
            List<long> ptNums = kensaDatas.GroupBy(p => p.PtId).Select(p => p.Key).ToList();

            var ptIds = NoTrackingDataContext.PtInfs.Where
                (
                    p =>
                        p.HpId == hpId &&
                        ptNums.Contains(p.PtNum) &&
                        p.IsDelete == DeleteStatus.None
                ).ToList();

            foreach (var kensaData in kensaDatas)
            {
                kensaData.PtId = ptIds.Find(p => p.PtNum == kensaData.PtId)?.PtId ?? 0;
            }
        }
        #endregion

        return kensaDatas;
    }

    /// <summary>
    /// 患者一覧を検査条件で絞り込み
    /// </summary>
    /// <param name="kensaConf"></param>
    /// <param name="ptInfs"></param>
    /// <returns></returns>
    private List<CoPtInfModel> GetPtInfKensaFilter(int hpId, CoSta9000KensaConf kensaConf, List<CoPtInfModel> ptInfs)
    {
        if (kensaConf == null) return ptInfs;

        var kensaDatas = GetKensaInfs(hpId, kensaConf);

        if (kensaDatas?.Count == 0) return new();

        return
            (
                from ptInf in ptInfs
                where
                    (
                        from k in kensaDatas
                        select k
                    ).Any(
                        p =>
                            p.PtId == ptInf.PtId
                    )
                select
                    ptInf
            ).ToList();
    }

    /// <summary>
    /// 初回来院情報取得
    /// </summary>
    /// <returns></returns>
    private IQueryable<RaiinInf> GetPtFirstVisits()
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p => p.IsDeleted == DeleteStatus.None && p.Status >= RaiinState.Calculate);

        //最小の来院日＝初回来院日
        var firstRaiinDates = (
            from raiinInf in raiinInfs
            join ptInf in ptInfs on
                new { raiinInf.HpId, raiinInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            group new { ptInf.HpId, ptInf.PtId, raiinInf.SinDate }
            by new { ptInf.HpId, ptInf.PtId }
        into grpRaiinDate
            select new
            {
                grpRaiinDate.Key.HpId,
                grpRaiinDate.Key.PtId,
                FirstVisitDate = grpRaiinDate.Min(g => g.SinDate)
            }
            );

        //同日来院のなかで来院番号の若い方
        var firstRaiins = (
            from raiinInf in raiinInfs
            join firstRaiinDate in firstRaiinDates on
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate } equals
                new { firstRaiinDate.HpId, firstRaiinDate.PtId, SinDate = firstRaiinDate.FirstVisitDate }
            group new { firstRaiinDate.HpId, firstRaiinDate.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
            by new { firstRaiinDate.HpId, firstRaiinDate.PtId, raiinInf.SinDate }
        into grpRaiin
            select new
            {
                grpRaiin.Key.HpId,
                grpRaiin.Key.PtId,
                grpRaiin.Key.SinDate,
                RaiinNo = grpRaiin.Min(g => g.RaiinNo)
            }
            );

        var firstVistJoins = (
            from raiinInf in raiinInfs
            join firstRaiin in firstRaiins on
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                new { firstRaiin.HpId, firstRaiin.PtId, firstRaiin.SinDate, firstRaiin.RaiinNo }
            select
              raiinInf
            );

        return firstVistJoins;
    }

    public Dictionary<int, string> GetUserSNameByUserIdDictionary(int hpId, List<int> userIdList)
    {
        userIdList = userIdList.Distinct().ToList();
        Dictionary<int, string> result = new();
        var userMstList = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId && userIdList.Contains(item.UserId) && item.IsDeleted == 0).ToList();
        foreach (var item in userMstList)
        {
            result.Add(item.UserId, item.Sname ?? string.Empty);
        }
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
