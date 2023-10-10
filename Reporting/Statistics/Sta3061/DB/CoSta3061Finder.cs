﻿using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3061.Models;
using System.Diagnostics;

namespace Reporting.Statistics.Sta3061.DB;

public class CoSta3061Finder : RepositoryBase, ICoSta3061Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    private readonly TenantDataContext _tenantSinKouiRpInf;
    private readonly TenantDataContext _tenantKaikeiInf;
    private readonly TenantDataContext _tenantPtInf;
    private readonly TenantDataContext _tenantRaiinInf;
    private readonly TenantDataContext _tenantKaMst;
    private readonly TenantDataContext _tenantUserMst;
    private readonly TenantDataContext _tenantSinKouiDetail;

    public CoSta3061Finder(ITenantProvider tenantProvider,
                           ICoHpInfFinder hpInfFinder,
                           ITenantProvider tenantSinKouiRpInf,
                           ITenantProvider tenantKaikeiInf,
                           ITenantProvider tenantPtInf,
                           ITenantProvider tenantRaiinInf,
                           ITenantProvider tenantKaMst,
                           ITenantProvider tenantSinKouiDetail,
                           ITenantProvider tenantUserMst) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
        _tenantSinKouiRpInf = tenantSinKouiRpInf.GetNoTrackingDataContext();
        _tenantKaikeiInf = tenantKaikeiInf.GetNoTrackingDataContext();
        _tenantPtInf = tenantPtInf.GetNoTrackingDataContext();
        _tenantRaiinInf = tenantRaiinInf.GetNoTrackingDataContext();
        _tenantKaMst = tenantKaMst.GetNoTrackingDataContext();
        _tenantUserMst = tenantUserMst.GetNoTrackingDataContext();
        _tenantSinKouiDetail = tenantSinKouiDetail.GetNoTrackingDataContext();
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
    public List<CoKouiTensuModel> GetKouiTensu(int hpId, CoSta3061PrintConf printConf)
    {
        try
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            IQueryable<SinKouiCount> sinKouiCounts = NoTrackingDataContext.SinKouiCounts;
            if (printConf.IsSinDate)
            {
                sinKouiCounts = sinKouiCounts.Where(s =>
                    s.HpId == hpId &&
                    s.SinDate >= printConf.StartSinDate &&
                    s.SinDate <= printConf.EndSinDate
                );
            }
            else
            {
                sinKouiCounts = sinKouiCounts.Where(s =>
                    s.HpId == hpId &&
                    s.SinYm >= printConf.StartSinYm &&
                    s.SinYm <= printConf.EndSinYm
                );
            }

            var sinKouis = NoTrackingDataContext.SinKouis;
            var sinKouiRpInfs = _tenantSinKouiRpInf.SinRpInfs;
            var ptInfs = _tenantPtInf.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
            ptInfs = !printConf.IsTester ? ptInfs.Where(p => p.IsTester == 0) : ptInfs;
            var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);

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
                                    p.PtId == ptGrpInf.PtId
                            )
                        select
                            ptGrpInf
                    );
                }
            }
            #endregion
            var raiinInfs = _tenantRaiinInf.RaiinInfs.Where(r => r.Status >= 5 && r.IsDeleted == DeleteStatus.None);
            var kaikeiInfs = _tenantKaikeiInf.KaikeiInfs;

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

            #region フィルム
            IQueryable<SinKouiDetail> sinKouiDetails = _tenantSinKouiDetail.SinKouiDetails;
            if (printConf.IsSinDate)
            {
                int startDate = printConf.StartSinDate / 100;
                int endDate = printConf.EndSinDate / 100;

                sinKouiDetails = sinKouiDetails.Where(s =>
                    s.HpId == hpId &&
                    s.SinYm >= startDate &&
                    s.SinYm <= endDate
                );
            }
            else
            {
                sinKouiDetails = sinKouiDetails.Where(s =>
                    s.HpId == hpId &&
                    s.SinYm >= printConf.StartSinYm &&
                    s.SinYm <= printConf.EndSinYm
                );
            }
            var tenMsts = _tenantSinKouiDetail.TenMsts.Where(t => t.SinKouiKbn == 77);

            var sinKouiFilms = (
                from sinDetail in sinKouiDetails.AsEnumerable()
                join tenMst in tenMsts on
                    new { sinDetail.HpId, sinDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tenMstj
                from tenMsti in tenMstj.Where(t =>
                        t.StartDate <= sinDetail.SinYm * 100 + 31 &&
                        (t.EndDate == 12341234 ? 99999999 : t.EndDate) >= sinDetail.SinYm * 100 + 01
                    )//.DefaultIfEmpty()
                group
                    new { sinDetail } by
                    new
                    {
                        sinDetail.HpId,
                        sinDetail.PtId,
                        sinDetail.SinYm,
                        sinDetail.RpNo,
                        sinDetail.SeqNo
                    } into filmGroupj
                select new SinKouiFilm
                (
                    filmGroupj.Key.HpId,
                    filmGroupj.Key.PtId,
                    filmGroupj.Key.SinYm,
                    filmGroupj.Key.RpNo,
                    filmGroupj.Key.SeqNo
                )
            );
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

            List<long> ptIdList = new();
            List<int> sinYmList = new();
            List<int> rpNoList = new();
            List<int> seqNoList = new();
            List<long> raiinNoList = new();
            List<int> sinDateList = new();

            var ptGrpInfList = ptGrpInfs.ToList();
            var sinKouiCountList = sinKouiCounts.ToList();
            sinKouiCountList = sinKouiCountList.Where(item => (!isPtGrp) || ptGrpInfList.Any(p => p.PtId == item.PtId)).ToList();

            Task taskId1 = Task.Factory.StartNew(() => ptIdList = sinKouiCountList.Select(item => item.PtId).Distinct().ToList());
            Task taskId2 = Task.Factory.StartNew(() => sinYmList = sinKouiCountList.Select(item => item.SinYm).Distinct().ToList());
            Task taskId3 = Task.Factory.StartNew(() => rpNoList = sinKouiCountList.Select(item => item.RpNo).Distinct().ToList());
            Task taskId4 = Task.Factory.StartNew(() => seqNoList = sinKouiCountList.Select(item => item.SeqNo).Distinct().ToList());
            Task taskId5 = Task.Factory.StartNew(() => raiinNoList = sinKouiCountList.Select(item => item.RaiinNo).Distinct().ToList());
            Task taskId6 = Task.Factory.StartNew(() => sinDateList = sinKouiCountList.Select(item => item.SinDate).Distinct().ToList());
            Task.WaitAll(taskId1, taskId2, taskId3, taskId4, taskId5, taskId6);

            var sinKouiList = sinKouis.Where(item => ptIdList.Contains(item.PtId)
                                                     && sinYmList.Contains(item.SinYm)
                                                     && rpNoList.Contains(item.RpNo)
                                                     && seqNoList.Contains(item.SeqNo)
                                      ).ToList();

            var hokenIdList = sinKouiList.Select(item => item.HokenId).Distinct().ToList();
            var hokenPidList = sinKouiList.Select(item => item.HokenPid).Distinct().ToList();

            List<SinRpInf> sinKouiRpInfList = new();
            List<KaikeiInf> kaikeiInfList = new();
            List<PtInf> ptInfList = new();
            List<RaiinInf> raiinInfList = new();

            Task taskList1 = Task.Factory.StartNew(() => sinKouiRpInfList = sinKouiRpInfs.Where(item => ptIdList.Contains(item.PtId)
                                                                                                           && sinYmList.Contains(item.SinYm)
                                                                                                           && rpNoList.Contains(item.RpNo)).ToList());
            Task taskList2 = Task.Factory.StartNew(() => kaikeiInfList = kaikeiInfs.Where(item => ptIdList.Contains(item.PtId)
                                                                                                  && raiinNoList.Contains(item.RaiinNo)
                                                                                                  && sinDateList.Contains(item.SinDate)
                                                                                                  && hokenIdList.Contains(item.HokenId)).ToList());
            Task taskList3 = Task.Factory.StartNew(() => ptInfList = ptInfs.Where(item => ptIdList.Contains(item.PtId)).ToList());
            Task taskList4 = Task.Factory.StartNew(() => raiinInfList = raiinInfs.Where(item => raiinNoList.Contains(item.RaiinNo)).ToList());
            Task.WaitAll(taskList1, taskList2, taskList3, taskList4);

            var tantoIdList = raiinInfList.Select(item => item.TantoId).Distinct().ToList();
            var kaIdList = raiinInfList.Select(item => item.KaId).Distinct().ToList();

            var ptKohiPatternList = ptKohiPatterns.Where(item => ptIdList.Contains(item.PtId)
                                                                 && hokenPidList.Contains(item.HokenPid)
                                                  ).ToList();

            List<PtHokenInf> ptHokenInfList = new();
            List<KaMst> kaMstList = new();
            List<UserMst> userMstList = new();
            List<SinKouiFilm> sinKouiFilmList = new();

            Task taskList6 = Task.Factory.StartNew(() => ptHokenInfList = ptHokenInfs.Where(item => ptIdList.Contains(item.PtId)
                                                                                                    && hokenIdList.Contains(item.HokenId)).ToList());

            Task taskList7 = Task.Factory.StartNew(() => kaMstList = _tenantKaMst.KaMsts.Where(item => item.HpId == hpId && kaIdList.Contains(item.KaId)).ToList());
            Task taskList8 = Task.Factory.StartNew(() => userMstList = _tenantUserMst.UserMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteStatus.None && tantoIdList.Contains(item.UserId)).ToList());
            Task taskList9 = Task.Factory.StartNew(() => sinKouiFilmList = sinKouiFilms.ToList());
            Task.WaitAll(taskList6, taskList7, taskList8, taskList9);

            var ptKohiJoins = (
                     from sinKoui in sinKouiList
                     join ptKohiPattern in ptKohiPatternList on
                         new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                         new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenPid }
                     join ptHokenInf in ptHokenInfList on
                         new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenId } equals
                         new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
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
                         kohiCount = kohiGroupj.Where(x => x.ptKohiPattern.KohiId != 0).Select(x => x.ptKohiPattern.KohiId).Distinct().Count()
                     }
                 );
            var ptKohiJoinList = ptKohiJoins.ToList();
            #endregion

            sinKouiFilmList = sinKouiFilmList.Where(item => ptIdList.Contains(item.PtId)
                                                            && sinYmList.Contains(item.SinYm)
                                                            && rpNoList.Contains(item.RpNo)
                                                            && seqNoList.Contains(item.SeqNo))
                                              .ToList();

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
                join ptKohiJoin in ptKohiJoinList on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.HokenId } equals
                    new { ptKohiJoin.HpId, ptKohiJoin.PtId, ptKohiJoin.SinYm, ptKohiJoin.HokenId }
                join kaMst in kaMstList on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                from kaMstj in kaMstJoin.DefaultIfEmpty()
                join userMst in userMstList on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                from tantoMst in userMstJoin.DefaultIfEmpty()
                join sinFilm in sinKouiFilmList on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinFilm.HpId, sinFilm.PtId, sinFilm.SinYm, sinFilm.RpNo, sinFilm.SeqNo } into sinFilmJoin
                from sinFilmj in sinFilmJoin.DefaultIfEmpty()
                where
                #region 患者グループ条件
                    (
                        (!isPtGrp) ||
                        (
                            from pg in ptGrpInfList
                            select pg
                        ).Any(
                            p =>
                                p.PtId == sinCount.PtId
                        )
                    )
                #endregion
                group
                    new { sinCount, sinKoui, sinRp, raiinInf, kaMstj, tantoMst, ptKohiJoin, kaikeiInf, ptInf, sinFilmj } by
                    new
                    {
                        sinCount.PtId,
                        sinCount.RaiinNo,
                        sinCount.SinDate,
                        raiinInf.KaId,
                        raiinInf.TantoId,
                        ptInf.Birthday,
                        ptKohiJoin.HokenKbn,
                        ptKohiJoin.HokenSbtCd1,
                        ptKohiJoin.HonkeKbn,
                        ptKohiJoin.Houbetu
                    } into sinGroupj
                select new
                {
                    sinGroupj.Key.PtId,
                    PtNum = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.ptInf?.PtNum ?? 0) : 0,
                    PtName = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.ptInf?.Name ?? string.Empty) : string.Empty,
                    Sex = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.ptInf?.Sex ?? 0) : 0,
                    sinGroupj.Key.RaiinNo,
                    sinGroupj.Key.SinDate,
                    sinGroupj.Key.KaId,
                    KaSname = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.kaMstj?.KaSname ?? string.Empty) : string.Empty,
                    sinGroupj.Key.TantoId,
                    TantoSname = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.tantoMst?.Sname ?? string.Empty) : string.Empty,
                    sinGroupj.Key.Birthday,
                    sinGroupj.Key.HokenKbn,
                    sinGroupj.Key.HokenSbtCd1,
                    sinGroupj.Key.HonkeKbn,
                    sinGroupj.Key.Houbetu,
                    KohiCount = sinGroupj != null && sinGroupj.Any() ? sinGroupj.Max(x => x.ptKohiJoin?.kohiCount ?? new()) : new(),
                    TotalTensu = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.CdKbn != "SZ" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    //診察計  ----------
                    Tensu1x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("1") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "A", "B", "C" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count1x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki == "1100" || x.sinKoui.SyukeiSaki == "1200") && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("13") || x.sinKoui.SyukeiSaki.StartsWith("14") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "A", "B", "C" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "1450" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "1450" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //初診
                    Tensu11 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("11") || (x.sinKoui.SyukeiSaki.StartsWith("A11") && x.sinKoui.CdKbn == "A")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count11 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki == "1100" || (x.sinKoui.SyukeiSaki.StartsWith("A11") && x.sinKoui.CdKbn == "A")) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //再診
                    Tensu12 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("12") || (x.sinKoui.SyukeiSaki.StartsWith("A12") && x.sinKoui.CdKbn == "A")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count12 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki == "1200" || (x.sinKoui.SyukeiSaki.StartsWith("A12") && x.sinKoui.CdKbn == "A")) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //医学管理
                    Tensu13 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("13") || (x.sinKoui.SyukeiSaki.StartsWith("A13") && x.sinKoui.CdKbn == "B")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count13 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("13") || (x.sinKoui.SyukeiSaki.StartsWith("A13") && x.sinKoui.CdKbn == "B")) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //在宅
                    Tensu14x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("14") || (x.sinKoui.SyukeiSaki.StartsWith("A") && x.sinKoui.CdKbn == "C")) && x.sinKoui.SyukeiSaki != "1450" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count14x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("14") || (x.sinKoui.SyukeiSaki.StartsWith("A") && x.sinKoui.CdKbn == "C")) && x.sinKoui.SyukeiSaki != "1450" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //在宅薬剤
                    Tensu1450 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "1450" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count1450 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "1450" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //投薬計  ----------
                    Tensu2x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "F" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("2") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "F" }.Contains(x.sinKoui.CdKbn))) && !new string[] { "2100", "2200", "2300" }.Contains(x.sinKoui.SyukeiSaki) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && new string[] { "2100", "2200", "2300" }.Contains(x.sinKoui.SyukeiSaki) ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //薬剤
                    Tensu2100 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && new string[] { "2100", "2200", "2300" }.Contains(x.sinKoui.SyukeiSaki) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2100 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && new string[] { "2100", "2200", "2300" }.Contains(x.sinKoui.SyukeiSaki) ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //調剤
                    Tensu2110 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && new string[] { "2110", "2310" }.Contains(x.sinKoui.SyukeiSaki) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2110 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && new string[] { "2110", "2310" }.Contains(x.sinKoui.SyukeiSaki) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //処方
                    Tensu2500 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2500" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2500 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2500" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //麻毒
                    Tensu2600 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2600" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2600 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2600" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //調基
                    Tensu2700 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2700" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count2700 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "2700" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //注射計  ----------
                    Tensu3x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count3x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId != "IY" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId == "IY" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //皮下筋肉内注射
                    Tensu31 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki.StartsWith("31") && x.sinKoui.RecId != "IY" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count31 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki.StartsWith("31") && x.sinKoui.RecId != "IY" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //静脈内注射
                    Tensu32 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki.StartsWith("32") && x.sinKoui.RecId != "IY" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count32 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki.StartsWith("32") && x.sinKoui.RecId != "IY" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //その他注射
                    Tensu33 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("33") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId != "IY" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count33 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("33") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId != "IY" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //薬剤器材
                    Tensu39 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId == "IY" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count39 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("3") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "G" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.RecId == "IY" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //処置計  ----------
                    Tensu4x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count4x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "4010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "4010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //処置手技
                    Tensu4000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "4010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count4000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("4") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "J" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "4010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //処置薬剤
                    Tensu4010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "4010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count4010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "4010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //手術計  ----------
                    Tensu5x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count5x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "5010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "5010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //手術手技
                    Tensu5000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "5010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count5000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("5") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "K", "L" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "5010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //手術薬剤
                    Tensu5010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "5010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count5010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "5010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //検査計  ----------
                    Tensu6x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count6x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "6010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "6010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //検査手技
                    Tensu6000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "6010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count6000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("6") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "D", "N" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "6010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //検査薬剤
                    Tensu6010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "6010" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count6010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "6010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //画像計  ----------
                    Tensu7x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count7x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "7010" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "7010" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //画像手技
                    Tensu7000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "7010" && x.sinFilmj == null ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count7000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) && x.sinKoui.SyukeiSaki != "7010" && x.sinFilmj == null && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //画像薬剤
                    Tensu7010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "7010" && x.sinFilmj == null ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count7010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "7010" && x.sinFilmj == null ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //画像フィルム
                    Tensu7f = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) && x.sinFilmj != null ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count7f = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("7") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "E" }.Contains(x.sinKoui.CdKbn))) && x.sinFilmj != null ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //その他計  --------
                    Tensu8x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count8x = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki.StartsWith("8") || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) && x.sinKoui.SyukeiSaki != "8020" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0) +
                        sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "8020" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //その他(処方せん)
                    Tensu8000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "8000" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count8000 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "8000" && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //その他(その他)
                    Tensu8010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki == "8010" || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count8010 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && (x.sinKoui.SyukeiSaki == "8010" || (x.sinKoui.SyukeiSaki.StartsWith("A") && new string[] { "H", "I", "M", "R", "JB" }.Contains(x.sinKoui.CdKbn)) || x.sinKoui.SyukeiSaki.StartsWith("Z")) && x.sinCount.SeqNo == 1 && x.sinKoui.TenColCount > 0 ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //その他(薬剤)
                    Tensu8020 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "8020" ? x.sinKoui.Ten / (x.sinKoui.EntenKbn == 1 ? 10 : 1) * x.sinCount.Count : 0),
                    Count8020 = sinGroupj.Sum(x => x.sinRp.SanteiKbn != 2 && x.sinKoui.SyukeiSaki == "8020" ? (x.sinKoui.Ten == 0 ? 0 : x.sinCount.Count) : 0),
                    //自費計  ----------
                    Jihi =
                        sinGroupj.Sum
                        (x =>
                            (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 0 ? x.sinKoui.Ten * 10 * x.sinCount.Count :  //自費(点)
                            (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 1 ? x.sinKoui.Ten * x.sinCount.Count :       //自費(円)
                            x.sinKoui.EntenKbn == 1 && x.sinKoui.CdKbn == "SZ" ? x.sinKoui.Ten * x.sinCount.Count :  //外税
                            0
                        )
                }
            );

            List<CoKouiTensuModel> retDatas = new();
            List<JihiSbtMst> jihiSbtMsts = new();

            Task taskList10 = Task.Factory.StartNew(() =>
            retDatas = joinQuery.AsEnumerable().Select(
                data =>
                    new CoKouiTensuModel()
                    {
                        ReportKbn = printConf.ReportKbn,
                        PtId = data.PtId,
                        PtNum = data.PtNum,
                        PtName = data.PtName,
                        Sex = data.Sex,
                        RaiinNo = data.RaiinNo,
                        SinDate = data.SinDate,
                        KaId = data.KaId,
                        KaSname = data.KaSname,
                        TantoId = data.TantoId,
                        TantoSname = data.TantoSname,
                        Birthday = data.Birthday,
                        HokenKbn = data.HokenKbn,
                        HokenSbtCd1 = data.HokenSbtCd1,
                        HonkeKbn = data.HonkeKbn,
                        Houbetu = data.Houbetu,
                        KohiCount = data.KohiCount,
                        TotalTensu = data.TotalTensu,
                        Tensu1x = data.Tensu1x,
                        Tensu11 = data.Tensu11,
                        Tensu12 = data.Tensu12,
                        Tensu13 = data.Tensu13,
                        Tensu14x = data.Tensu14x,
                        Tensu1450 = data.Tensu1450,
                        Tensu2x = data.Tensu2x,
                        Tensu2100 = data.Tensu2100,
                        Tensu2110 = data.Tensu2110,
                        Tensu2500 = data.Tensu2500,
                        Tensu2600 = data.Tensu2600,
                        Tensu2700 = data.Tensu2700,
                        Tensu3x = data.Tensu3x,
                        Tensu31 = data.Tensu31,
                        Tensu32 = data.Tensu32,
                        Tensu33 = data.Tensu33,
                        Tensu39 = data.Tensu39,
                        Tensu4x = data.Tensu4x,
                        Tensu4000 = data.Tensu4000,
                        Tensu4010 = data.Tensu4010,
                        Tensu5x = data.Tensu5x,
                        Tensu5000 = data.Tensu5000,
                        Tensu5010 = data.Tensu5010,
                        Tensu6x = data.Tensu6x,
                        Tensu6000 = data.Tensu6000,
                        Tensu6010 = data.Tensu6010,
                        Tensu7x = data.Tensu7x,
                        Tensu7000 = data.Tensu7000,
                        Tensu7010 = data.Tensu7010,
                        Tensu7f = data.Tensu7f,
                        Tensu8x = data.Tensu8x,
                        Tensu8000 = data.Tensu8000,
                        Tensu8010 = data.Tensu8010,
                        Tensu8020 = data.Tensu8020,
                        Count1x = data.Count1x,
                        Count11 = data.Count11,
                        Count12 = data.Count12,
                        Count13 = data.Count13,
                        Count14x = data.Count14x,
                        Count1450 = data.Count1450,
                        Count2x = data.Count2x,
                        Count2100 = data.Count2100,
                        Count2110 = data.Count2110,
                        Count2500 = data.Count2500,
                        Count2600 = data.Count2600,
                        Count2700 = data.Count2700,
                        Count3x = data.Count3x,
                        Count31 = data.Count31,
                        Count32 = data.Count32,
                        Count33 = data.Count33,
                        Count39 = data.Count39,
                        Count4x = data.Count4x,
                        Count4000 = data.Count4000,
                        Count4010 = data.Count4010,
                        Count5x = data.Count5x,
                        Count5000 = data.Count5000,
                        Count5010 = data.Count5010,
                        Count6x = data.Count6x,
                        Count6000 = data.Count6000,
                        Count6010 = data.Count6010,
                        Count7x = data.Count7x,
                        Count7000 = data.Count7000,
                        Count7010 = data.Count7010,
                        Count7f = data.Count7f,
                        Count8x = data.Count8x,
                        Count8000 = data.Count8000,
                        Count8010 = data.Count8010,
                        Count8020 = data.Count8020,
                        Jihi = data.Jihi
                    }
            )
            .ToList());

            Task taskList11 = Task.Factory.StartNew(() =>
             jihiSbtMsts = NoTrackingDataContext.JihiSbtMsts
                           .Where(j => j.HpId == hpId && j.IsDeleted == DeleteStatus.None)
                           .OrderBy(j => j.SortNo)
                           .ThenBy(j => j.JihiSbt)
                           .ToList());

            Task.WaitAll(taskList11, taskList10);

            jihiSbtMsts.Add
            (
                new JihiSbtMst()
                {
                    JihiSbt = 0,
                    SortNo = jihiSbtMsts.Max(j => j.SortNo) + 1
                }
            );

            var jihiMeisai = (
                from sinCount in sinKouiCountList
                join sinKoui in sinKouiList on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRp in sinKouiRpInfList on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                    new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo }
                join ptKohiPattern in ptKohiPatternList on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenPid }
                join ptHokenInf in ptHokenInfList on
                    new { ptKohiPattern.HpId, ptKohiPattern.PtId, ptKohiPattern.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                where
                #region 患者グループ条件
                    (
                        (!isPtGrp) ||
                        (
                            from pg in ptGrpInfList
                            select pg
                        ).Any(
                            p =>
                                p.PtId == sinCount.PtId
                        )
                    ) &&
                #endregion
                    (sinRp.SanteiKbn == 2 || sinKoui.CdKbn == "JS" || (sinKoui.EntenKbn == 1 && sinKoui.CdKbn == "SZ"))
                group
                    new { sinCount, sinKoui, sinRp } by
                    new
                    {
                        sinCount.PtId,
                        sinCount.RaiinNo,
                        sinCount.SinDate,
                        sinKoui.JihiSbt
                    } into sinGroupj
                select new
                {
                    sinGroupj.Key.SinDate,
                    sinGroupj.Key.RaiinNo,
                    sinGroupj.Key.JihiSbt,
                    JihiFutan =
                            sinGroupj.Sum
                                (x =>
                                    (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 0 ? x.sinKoui.Ten * 10 * x.sinCount.Count :  //自費(点)
                                    (x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS") && x.sinKoui.EntenKbn == 1 ? x.sinKoui.Ten * x.sinCount.Count :       //自費(円)
                                    x.sinKoui.EntenKbn == 1 && x.sinKoui.CdKbn == "SZ" ? x.sinKoui.Ten * x.sinCount.Count :  //外税
                                    0
                                ),
                    JihiCount = sinGroupj.Sum(x => x.sinRp.SanteiKbn == 2 || x.sinKoui.CdKbn == "JS" ? x.sinCount.Count : 0)
                }
            ).ToList();

            foreach (var retData in retDatas)
            {
                //自費明細
                retData.JihiMeisais = new List<double>();
                retData.CountJihiMeisais = new List<int>();

                foreach (var jihiSbtMst in jihiSbtMsts)
                {
                    retData.JihiMeisais.Add
                    (
                        jihiMeisai.Find(j => j.RaiinNo == retData.RaiinNo && j.JihiSbt == jihiSbtMst.JihiSbt)?.JihiFutan ?? 0
                    );
                    retData.CountJihiMeisais.Add
                    (
                        jihiMeisai.Find(j => j.RaiinNo == retData.RaiinNo && j.JihiSbt == jihiSbtMst.JihiSbt)?.JihiCount ?? 0
                    );
                }
            }

            #region 患者グループの取得
            if (printConf.PtGrpId > 0)
            {
                var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == DeleteStatus.None);

                var ptGrpDatas = (
                    from ptGrpInf in ptGrpInfs
                    join ptGrpItem in ptGrpItems on
                        new { ptGrpInf.HpId, ptGrpInf.GroupId, ptGrpInf.GroupCode } equals
                        new { ptGrpItem.HpId, GroupId = ptGrpItem.GrpId, GroupCode = ptGrpItem.GrpCode }
                    where
                        ptGrpInf.HpId == hpId &&
                        ptGrpInf.GroupId == printConf.PtGrpId &&
                        ptGrpInf.IsDeleted == DeleteStatus.None
                    select new
                    {
                        ptGrpInf.PtId,
                        ptGrpItem.GrpCode,
                        ptGrpItem.GrpCodeName
                    }
                ).ToList();

                foreach (var retData in retDatas)
                {
                    var curGrps = ptGrpDatas.Find(p => p.PtId == retData.PtId);

                    retData.PtGrpCode = curGrps?.GrpCode ?? string.Empty;
                    retData.PtGrpCodeName = curGrps?.GrpCodeName ?? "分類なし";
                }
            }
            #endregion

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return retDatas;
        }
        finally
        {
            _tenantKaikeiInf.Dispose();
            _tenantKaMst.Dispose();
            _tenantPtInf.Dispose();
            _tenantRaiinInf.Dispose();
            _tenantSinKouiRpInf.Dispose();
            _tenantUserMst.Dispose();
        }
    }

    public List<CoJihiSbtMstModel> GetJihiSbtMst(int hpId)
    {
        var jihiSbtMsts = NoTrackingDataContext.JihiSbtMsts.Where(j =>
            j.HpId == hpId &&
            j.IsDeleted == DeleteStatus.None
        )
        .OrderBy(j => j.SortNo)
        .ThenBy(j => j.JihiSbt)
        .ToList();

        return jihiSbtMsts.Select(j => new CoJihiSbtMstModel(j)).ToList();
    }

    public string GetPtGrpName(int hpId, int grpId)
    {
        if (grpId <= 0) return string.Empty;

        var ptGrpMst = NoTrackingDataContext.PtGrpNameMsts
            .FirstOrDefault(p => p.HpId == hpId && p.GrpId == grpId);

        return
            ptGrpMst?.GrpName ?? string.Empty;
    }

    private class SinKouiFilm
    {
        public SinKouiFilm(int hpId, long ptId, int sinYm, int rpNo, int seqNo)
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            RpNo = rpNo;
            SeqNo = seqNo;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int RpNo { get; private set; }

        public int SeqNo { get; private set; }
    }
}
