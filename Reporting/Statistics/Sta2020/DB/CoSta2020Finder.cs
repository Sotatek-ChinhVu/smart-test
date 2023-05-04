﻿using Domain.Constant;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2020.Models;

namespace Reporting.Statistics.Sta2020.DB
{
    public class CoSta2020Finder : RepositoryBase, ICoSta2020Finder
    {
        private readonly ICoHpInfFinder _hpInfFinder;

        public CoSta2020Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
        }

        public CoHpInfModel GetHpInf(int hpId, int sinYm)
        {
            return _hpInfFinder.GetHpInf(hpId, sinYm * 100 + 1);
        }

        /// <summary>
        /// 診療情報取得
        /// </summary>
        /// <param name="printConf"></param>
        /// <returns></returns>
        public List<CoSinKouiModel> GetSinKouis(CoSta2020PrintConf printConf)
        {
            List<CoSinKouiModel> sinData;
            if (printConf.DataKind == 0)
            {
                sinData = getSinKouis(printConf);
            }
            else
            {
                sinData = getOdrInfs(printConf);
            }
            return sinData;
        }

        private List<CoSinKouiModel> getSinKouis(int hpId, CoSta2020PrintConf printConf)
        {
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(s => s.HpId == hpId);
            sinKouiCounts = printConf.StartSinYm >= 0 ?
                sinKouiCounts.Where(s => s.SinYm >= printConf.StartSinYm && s.SinYm <= printConf.EndSinYm) :
                sinKouiCounts.Where(s => s.SinDate >= printConf.StartSinDate && s.SinDate <= printConf.EndSinDate);

            var sinKouis = NoTrackingDataContext.SinKouis.Where(x => x.HpId == hpId);
            var sinKouiRpInfs = NoTrackingDataContext.SinRpInfs.Where(x => x.HpId == hpId);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(s => s.HpId == hpId && !s.ItemCd.StartsWith("@8") && !s.ItemCd.StartsWith("@9") && s.ItemCd != "XNOODR");
            var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None);
            if (!printConf.IsTester)
            {
                ptInfs = ptInfs.Where(p => p.IsTester == 0);
            }
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId);
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

            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId);
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId);
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

            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId);
            var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from sinCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRp in sinKouiRpInfs on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo } equals
                    new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo }
                join sinDetail in sinKouiDetails on
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDetail.HpId, sinDetail.PtId, sinDetail.SinYm, sinDetail.RpNo, sinDetail.SeqNo }
                join tenMst in tenMsts on
                    new { sinDetail.HpId, ItemCd = (sinDetail.OdrItemCd.StartsWith("Z") && sinDetail.ItemSbt == 0 && sinDetail.RecId == "TO" ? sinDetail.OdrItemCd : sinDetail.ItemCd) } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                join ptInf in ptInfs on
                    new { sinCount.HpId, sinCount.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join raiinInf in raiinInfs on
                    new { sinCount.HpId, sinCount.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join ptHokenPattern in ptHokenPatterns on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptHokenInf in ptHokenInfs on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
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
                    tenMst.StartDate <= sinCount.SinDate &&
                    (tenMst.EndDate == 12341234 ? 99999999 : tenMst.EndDate) >= sinCount.SinDate
                select new
                {
                    PtId = sinCount.PtId,
                    RaiinNo = sinCount.RaiinNo,
                    SinYm = sinCount.SinYm,
                    SinDate = sinCount.SinDate,
                    SinId =
                            new int[] { 11, 12 }.Contains(sinRp.SinId) ? "1x" :
                            new int[] { 24, 25, 26, 27, 28, 29 }.Contains(sinRp.SinId) ? "2x" :
                            sinRp.SinId.ToString(),
                    Suryo = sinDetail.Suryo * sinCount.Count,
                    //UnitName = sinDetail.UnitName,
                    ItemCd = (sinDetail.OdrItemCd.StartsWith("Z") && sinDetail.ItemSbt == 0 && sinDetail.RecId == "TO" ? sinDetail.OdrItemCd : sinDetail.ItemCd),
                    ItemCdCmt =
                        (
                            sinDetail.OdrItemCd.StartsWith("Z") && sinDetail.ItemSbt == 0 && sinDetail.RecId == "TO" ? sinDetail.OdrItemCd :
                            tenMst.MasterSbt == "C" ? sinDetail.ItemName :
                            sinDetail.ItemCd
                        ),
                    SrcItemCd =
                        (
                            sinDetail.OdrItemCd.StartsWith("Z") && sinDetail.ItemSbt == 0 && sinDetail.RecId == "TO" ? sinDetail.OdrItemCd :
                            sinDetail.ItemCd == ItemCdConst.CommentFree ? sinDetail.ItemName :
                            sinDetail.ItemCd
                        ),
                    ItemName = sinDetail.ItemName,
                    ItemKanaName1 = tenMst.KanaName1,
                    ItemKanaName2 = tenMst.KanaName2,
                    ItemKanaName3 = tenMst.KanaName3,
                    ItemKanaName4 = tenMst.KanaName4,
                    ItemKanaName5 = tenMst.KanaName5,
                    ItemKanaName6 = tenMst.KanaName6,
                    ItemKanaName7 = tenMst.KanaName7,
                    KaId = raiinInf.KaId,
                    KaSname = kaMstj.KaSname,
                    TantoId = raiinInf.TantoId,
                    TantoSname = tantoMst.Sname,
                    SinKouiKbn =
                            tenMst.MasterSbt == "T" && tenMst.SinKouiKbn != 77 ? "T" :
                            tenMst.MasterSbt == "C" ? "99" :
                            new int[] { 20, 21, 22, 23 }.Contains(tenMst.SinKouiKbn) && tenMst.DrugKbn == 1 ? "21" :
                            new int[] { 20, 21, 22, 23 }.Contains(tenMst.SinKouiKbn) && tenMst.DrugKbn == 6 ? "23" :
                            new int[] { 20, 21, 22, 23 }.Contains(tenMst.SinKouiKbn) && (tenMst.DrugKbn == 3 || tenMst.DrugKbn == 8) ? "2x" :
                            new int[] { 20, 21, 22, 23 }.Contains(tenMst.SinKouiKbn) && tenMst.DrugKbn == 4 ? "30" :
                            new int[] { 20, 21, 22, 23 }.Contains(tenMst.SinKouiKbn) ? "20" :
                            tenMst.SinKouiKbn.ToString(),
                    MadokuKbn = tenMst.MadokuKbn,
                    DrugKbn = tenMst.DrugKbn,
                    KouseisinKbn = tenMst.KouseisinKbn,
                    KazeiKbn = tenMst.KazeiKbn,
                    EntenKbn = new int[] { 1, 2, 4, 10, 11, 99 }.Contains(tenMst.TenId) ? 1 : 0,
                    Ten =
                            new int[] { 5, 6, 7, 9 }.Contains(tenMst.TenId) ? sinDetail.Ten :
                            tenMst.TenId == 8 ? -tenMst.Ten :
                            tenMst.TenId == 10 ? tenMst.Ten / 10 :
                            tenMst.TenId == 11 ? tenMst.Ten * 10 :
                            tenMst.Ten,
                    InoutKbn = sinKoui.InoutKbn,
                    KohatuKbn = tenMst.KohatuKbn,
                    IsAdopted = tenMst.IsAdopted,
                    KizamiId = tenMst.KizamiId,
                    TenDetail = sinDetail.Ten * sinCount.Count
                }
            );

            #region 条件指定
            //診療識別
            if (printConf.SinIds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.SinIds.Contains(s.SinId));
            }
            //診療行為区分
            if (printConf.SinKouiKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.SinKouiKbns.Contains(s.SinKouiKbn));
            }
            //麻毒区分
            if (printConf.MadokuKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.MadokuKbns.Contains(s.MadokuKbn));
            }
            //向精神薬区分
            if (printConf.KouseisinKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.KouseisinKbns.Contains(s.KouseisinKbn));
            }
            //院内院外区分
            if (printConf.InoutKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.InoutKbns.Contains(s.InoutKbn));
            }
            //後発医薬品区分
            if (printConf.KohatuKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.KohatuKbns.Contains(s.KohatuKbn));
            }
            //採用区分
            if (printConf.IsAdopteds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.IsAdopteds.Contains(s.IsAdopted));
            }
            //検索項目＆検索ワード
            if (printConf.ItemCds?.Count >= 1)
            {
                for (int i = 0; i < printConf.ItemCds.Count; i++)
                {
                    var sinDate = printConf.StartSinYm >= 0 ? printConf.StartSinYm * 100 + 1 : printConf.StartSinDate;
                    var itemCd = printConf.ItemCds[i];

                    var wrkItemCd = tenMsts.Where(t =>
                        t.HpId == hpId &&
                        t.ItemCd == itemCd &&
                        !t.ItemCd.StartsWith("Z") &&
                        t.StartDate <= sinDate
                    ).Select(t => new { t.ItemCd, t.SanteiItemCd, t.StartDate })
                    .OrderByDescending(t => t.StartDate)
                    .FirstOrDefault();

                    if (wrkItemCd != null &&
                        wrkItemCd.ItemCd != wrkItemCd.SanteiItemCd &&
                        wrkItemCd.SanteiItemCd != ItemCdConst.NoSantei)
                    {
                        printConf.ItemCds[i] = wrkItemCd.SanteiItemCd;
                    }
                }
            }
            #region コメントマスター(CO)の名称取得
            var itemCmts = new List<string>();
            foreach (var itemCd in printConf.ItemCds.Where(i => i.StartsWith("CO")))
            {
                var sinDate = printConf.StartSinYm >= 0 ? printConf.StartSinYm * 100 + 1 : printConf.StartSinDate;

                var itemName = tenMsts.Where(t =>
                    t.HpId == hpId &&
                    t.ItemCd == itemCd &&
                    t.StartDate <= sinDate
                ).OrderByDescending(t => t.StartDate)
                .Select(t => t.Name)
                .FirstOrDefault();

                if (!string.IsNullOrEmpty(itemName))
                {
                    itemCmts.Add(itemName);
                }
            }
            printConf.ItemCds.AddRange(itemCmts);
            printConf.ItemCds.RemoveAll(i => i.StartsWith("CO"));
            #endregion
            if (printConf.ItemCds?.Count >= 1 && printConf.SearchWord.AsString() != "")
            {
                //スペース区切りでキーワードを分解
                string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Contains(r.SrcItemCd) || searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Contains(r.SrcItemCd) || searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
            //検索項目
            else if (printConf.ItemCds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.ItemCds.Contains(s.SrcItemCd));
            }
            //検索ワード
            else if (printConf.SearchWord.AsString() != "")
            {
                //スペース区切りでキーワードを分解
                string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
            #endregion

            var retData = joinQuery.AsEnumerable().Select(
                data =>
                    new CoSinKouiModel()
                    {
                        PtId = data.PtId,
                        RaiinNo = data.RaiinNo,
                        SinYm = data.SinYm,
                        SinDate = data.SinDate,
                        SinId = data.SinId,
                        Suryo = data.Suryo,
                        //UnitName = data.sinDetail.UnitName,
                        Money =
                            data.KizamiId == 1 ? (int)Math.Round(data.TenDetail * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero) :
                            (int)Math.Round(data.Ten * data.Suryo * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero),
                        ItemCd = data.ItemCd,
                        ItemCdCmt = data.ItemCdCmt,
                        ItemName = data.ItemName,
                        ItemKanaName1 = data.ItemKanaName1,
                        ItemKanaName2 = data.ItemKanaName2,
                        ItemKanaName3 = data.ItemKanaName3,
                        ItemKanaName4 = data.ItemKanaName4,
                        ItemKanaName5 = data.ItemKanaName5,
                        ItemKanaName6 = data.ItemKanaName6,
                        ItemKanaName7 = data.ItemKanaName7,
                        KaId = data.KaId,
                        KaSname = data.KaSname,
                        TantoId = data.TantoId,
                        TantoSname = data.TantoSname,
                        SinKouiKbn = data.SinKouiKbn,
                        MadokuKbn = data.MadokuKbn,
                        KouseisinKbn = data.KouseisinKbn,
                        KazeiKbn = data.KazeiKbn,
                        EntenKbn = data.EntenKbn,
                        Ten = data.Ten
                    }
            )
            .ToList();
            return retData;
        }

        private List<CoSinKouiModel> getOdrInfs(int hpId, CoSta2020PrintConf printConf)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(s => s.HpId == hpId && s.IsDeleted == DeleteStatus.None);
            odrInfs = printConf.StartSinYm >= 0 ?
                odrInfs.Where(s => s.SinDate >= printConf.StartSinYm * 100 + 1 && s.SinDate <= printConf.EndSinYm * 100 + 31) :
                odrInfs.Where(s => s.SinDate >= printConf.StartSinDate && s.SinDate <= printConf.EndSinDate);

            var odrDetails = NoTrackingDataContext.OdrInfDetails.Where(x => x.HpId == hpId);
            var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None);
            if (!printConf.IsTester)
            {
                ptInfs = ptInfs.Where(p => p.IsTester == 0);
            }
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.Status > 3);
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

            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId);
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId);
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

            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId);
            var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted == DeleteStatus.None);

            var joinOdrs = (
                from odrInf in odrInfs
                join odrDetail in odrDetails on
                    new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrDetail.HpId, odrDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tenMstj
                from tenMsti in tenMstj.Where(t =>
                        t.StartDate <= odrDetail.SinDate &&
                        (t.EndDate == 12341234 ? 99999999 : t.EndDate) >= odrDetail.SinDate
                    ).DefaultIfEmpty()
                select new
                {
                    odrInf.HpId,
                    odrInf.RaiinNo,
                    odrInf.RpNo,
                    odrInf.RpEdaNo,
                    odrInf.PtId,
                    odrInf.OdrKouiKbn,
                    odrInf.SinDate,
                    odrInf.DaysCnt,
                    odrInf.HokenPid,
                    odrDetail.Suryo,
                    odrDetail.TermVal,
                    odrDetail.ItemCd,
                    ItemName = odrDetail.ItemName,
                    TenId = tenMsti == null ? 0 : tenMsti.TenId,
                    Ten = tenMsti == null ? 0.0 : tenMsti.Ten,
                    KanaName1 = tenMsti == null ? string.Empty : tenMsti.KanaName1,
                    KanaName2 = tenMsti == null ? string.Empty : tenMsti.KanaName2,
                    KanaName3 = tenMsti == null ? string.Empty : tenMsti.KanaName3,
                    KanaName4 = tenMsti == null ? string.Empty : tenMsti.KanaName4,
                    KanaName5 = tenMsti == null ? string.Empty : tenMsti.KanaName5,
                    KanaName6 = tenMsti == null ? string.Empty : tenMsti.KanaName6,
                    KanaName7 = tenMsti == null ? string.Empty : tenMsti.KanaName7,
                    MasterSbt = tenMsti == null ? string.Empty : tenMsti.MasterSbt,
                    SinKouiKbn = tenMsti == null ? 99 : tenMsti.SinKouiKbn,
                    DrugKbn = tenMsti == null ? 0 : tenMsti.DrugKbn,
                    MadokuKbn = tenMsti == null ? 0 : tenMsti.MadokuKbn,
                    KouseisinKbn = tenMsti == null ? 0 : tenMsti.KouseisinKbn,
                    KazeiKbn = tenMsti == null ? 0 : tenMsti.KazeiKbn,
                    odrInf.InoutKbn,
                    KohatuKbn = tenMsti == null ? 0 : tenMsti.KohatuKbn,
                    IsAdopted = tenMsti == null ? 0 : tenMsti.IsAdopted
                }
            );

            var joinQuery = (
                from joinOdr in joinOdrs
                join ptInf in ptInfs on
                    new { joinOdr.HpId, joinOdr.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join raiinInf in raiinInfs on
                    new { joinOdr.HpId, joinOdr.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join ptHokenPattern in ptHokenPatterns on
                    new { joinOdr.HpId, joinOdr.PtId, joinOdr.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptHokenInf in ptHokenInfs on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                from kaMstj in kaMstJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                from tantoMst in userMstJoin.DefaultIfEmpty()
                select new
                {
                    PtId = joinOdr.PtId,
                    RaiinNo = joinOdr.RaiinNo,
                    SinYm = joinOdr.SinDate / 100,
                    SinDate = joinOdr.SinDate,
                    SinId =
                            new int[] { 10, 11, 12 }.Contains(joinOdr.OdrKouiKbn) ? "1x" :
                            new int[] { 20, 24, 25, 26, 27, 28, 29 }.Contains(joinOdr.OdrKouiKbn) ? "2x" :
                            new int[] { 30, 33, 34 }.Contains(joinOdr.OdrKouiKbn) ? "33" :
                            new int[] { 60, 61, 62, 63, 64 }.Contains(joinOdr.OdrKouiKbn) ? "60" :
                            new int[] { 80, 81, 82, 83, 84 }.Contains(joinOdr.OdrKouiKbn) ? "80" :
                            joinOdr.OdrKouiKbn.ToString(),
                    Suryo =
                            (
                                new string[] { "@SHIN", "@JIKAN" }.Contains(joinOdr.ItemCd) ? joinOdr.Suryo :
                                joinOdr.Suryo == 0 ? 1 :
                                joinOdr.TermVal == 0 ? joinOdr.Suryo :
                                //Math.Round(joinOdr.Suryo * joinOdr.TermVal, 3, MidpointRounding.AwayFromZero)
                                (Math.Round(joinOdr.Suryo * joinOdr.TermVal * 1000) / 1000)
                            ) * (
                                new int[] { 21, 22, 23 }.Contains(joinOdr.OdrKouiKbn) ? joinOdr.DaysCnt : 1
                            ),
                    Count = new int[] { 21, 22, 23 }.Contains(joinOdr.OdrKouiKbn) ? joinOdr.DaysCnt : 1,
                    ItemCd = joinOdr.ItemCd == string.Empty || joinOdr.ItemCd == null ? ItemCdConst.CommentFree : joinOdr.ItemCd,
                    ItemCdCmt =
                        joinOdr.ItemCd == string.Empty || joinOdr.ItemCd == null || joinOdr.MasterSbt == "C" ? joinOdr.ItemName :
                        joinOdr.ItemCd,
                    SrcItemCd = joinOdr.ItemCd == string.Empty || joinOdr.ItemCd == null ? joinOdr.ItemName : joinOdr.ItemCd,
                    ItemName = joinOdr.ItemName,
                    ItemKanaName1 = joinOdr.KanaName1,
                    ItemKanaName2 = joinOdr.KanaName2,
                    ItemKanaName3 = joinOdr.KanaName3,
                    ItemKanaName4 = joinOdr.KanaName4,
                    ItemKanaName5 = joinOdr.KanaName5,
                    ItemKanaName6 = joinOdr.KanaName6,
                    ItemKanaName7 = joinOdr.KanaName7,
                    KaId = raiinInf.KaId,
                    KaSname = kaMstj.KaSname,
                    TantoId = raiinInf.TantoId,
                    TantoSname = tantoMst.Sname,
                    SinKouiKbn =
                            joinOdr.MasterSbt == "T" && joinOdr.SinKouiKbn != 77 ? "T" :
                            joinOdr.MasterSbt == "C" ? "99" :
                            new int[] { 20, 21, 22, 23 }.Contains(joinOdr.SinKouiKbn) && joinOdr.DrugKbn == 1 ? "21" :
                            new int[] { 20, 21, 22, 23 }.Contains(joinOdr.SinKouiKbn) && joinOdr.DrugKbn == 6 ? "23" :
                            new int[] { 20, 21, 22, 23 }.Contains(joinOdr.SinKouiKbn) && (joinOdr.DrugKbn == 3 || joinOdr.DrugKbn == 8) ? "2x" :
                            new int[] { 20, 21, 22, 23 }.Contains(joinOdr.SinKouiKbn) && joinOdr.DrugKbn == 4 ? "30" :
                            new int[] { 20, 21, 22, 23 }.Contains(joinOdr.SinKouiKbn) ? "20" :
                            joinOdr.SinKouiKbn.ToString(),
                    MadokuKbn = joinOdr.MadokuKbn,
                    KouseisinKbn = joinOdr.KouseisinKbn,
                    KazeiKbn = joinOdr.KazeiKbn,
                    EntenKbn = new int[] { 1, 2, 4, 10, 11, 99 }.Contains(joinOdr.TenId) ? 1 : 0,
                    Ten =
                            new int[] { 5, 6, 7, 9 }.Contains(joinOdr.TenId) ? 0 :
                            joinOdr.TenId == 8 ? -joinOdr.Ten :
                            joinOdr.TenId == 10 ? joinOdr.Ten / 10 :
                            joinOdr.TenId == 11 ? joinOdr.Ten * 10 :
                            joinOdr.Ten,
                    InoutKbn = joinOdr.InoutKbn,
                    KohatuKbn = joinOdr.KohatuKbn,
                    IsAdopted = joinOdr.IsAdopted,
                    DrugKbn = joinOdr.DrugKbn
                }
            );

            #region 条件指定
            //診療識別
            if (printConf.SinIds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.SinIds.Contains(s.SinId));
            }
            //診療行為区分
            if (printConf.SinKouiKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.SinKouiKbns.Contains(s.SinKouiKbn));
            }
            //麻毒区分
            if (printConf.MadokuKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.MadokuKbns.Contains(s.MadokuKbn));
            }
            //向精神薬区分
            if (printConf.KouseisinKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.KouseisinKbns.Contains(s.KouseisinKbn));
            }
            //院内院外区分
            if (printConf.InoutKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.InoutKbns.Contains(s.InoutKbn));
            }
            //後発医薬品区分
            if (printConf.KohatuKbns?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => s.DrugKbn == 0 || printConf.KohatuKbns.Contains(s.KohatuKbn));
            }
            //採用区分
            if (printConf.IsAdopteds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.IsAdopteds.Contains(s.IsAdopted));
            }
            #region コメントマスター(CO)の名称取得
            var itemCmts = new List<string>();
            foreach (var itemCd in printConf.ItemCds.Where(i => i.StartsWith("CO")))
            {
                var sinDate = printConf.StartSinYm >= 0 ? printConf.StartSinYm * 100 + 1 : printConf.StartSinDate;

                var itemName = tenMsts.Where(t =>
                    t.HpId == hpId &&
                    t.ItemCd == itemCd &&
                    t.StartDate <= sinDate
                ).OrderByDescending(t => t.StartDate)
                .Select(t => t.Name)
                .FirstOrDefault();

                if (!string.IsNullOrEmpty(itemName))
                {
                    itemCmts.Add(itemName);
                }
            }
            printConf.ItemCds.AddRange(itemCmts);
            printConf.ItemCds.RemoveAll(i => i.StartsWith("CO"));
            #endregion
            //検索項目＆検索ワード
            if (printConf.ItemCds?.Count >= 1 && printConf.SearchWord.AsString() != "")
            {
                //スペース区切りでキーワードを分解
                string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Contains(r.SrcItemCd) || searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Contains(r.SrcItemCd) || searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
            //検索項目
            else if (printConf.ItemCds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(s => printConf.ItemCds.Contains(s.SrcItemCd));
            }
            //検索ワード
            else if (printConf.SearchWord.AsString() != "")
            {
                //スペース区切りでキーワードを分解
                string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
            #endregion

            List<string> zaiSuryos = new List<string>
            {
                ItemCdConst.ZaiOusin, ItemCdConst.ZaiOusinTokubetu,
                ItemCdConst.ZaiHoumon1_1Dou, ItemCdConst.ZaiHoumon1_1DouIgai,
                ItemCdConst.ZaiHoumon1_2Dou, ItemCdConst.ZaiHoumon1_2DouIgai,
                ItemCdConst.ZaiHoumon2i, ItemCdConst.ZaiHoumon2ro
            };

            var retData = joinQuery.AsEnumerable().Select(
                data =>
                    new CoSinKouiModel()
                    {
                        PtId = data.PtId,
                        RaiinNo = data.RaiinNo,
                        SinYm = data.SinYm,
                        SinDate = data.SinDate,
                        SinId = data.SinId,
                        Suryo = data.Suryo,
                        Money =
                            zaiSuryos.Contains(data.ItemCd) ? (int)Math.Round(data.Ten * data.Count * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero) :
                            (int)Math.Round(data.Ten * data.Suryo * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero),
                        ItemCd = data.ItemCd,
                        ItemCdCmt = data.ItemCdCmt,
                        ItemName = data.ItemName,
                        ItemKanaName1 = data.ItemKanaName1,
                        ItemKanaName2 = data.ItemKanaName2,
                        ItemKanaName3 = data.ItemKanaName3,
                        ItemKanaName4 = data.ItemKanaName4,
                        ItemKanaName5 = data.ItemKanaName5,
                        ItemKanaName6 = data.ItemKanaName6,
                        ItemKanaName7 = data.ItemKanaName7,
                        KaId = data.KaId,
                        KaSname = data.KaSname,
                        TantoId = data.TantoId,
                        TantoSname = data.TantoSname,
                        SinKouiKbn = data.SinKouiKbn,
                        MadokuKbn = data.MadokuKbn,
                        KouseisinKbn = data.KouseisinKbn,
                        KazeiKbn = data.KazeiKbn,
                        EntenKbn = data.EntenKbn,
                        Ten = data.Ten
                    }
            )
            .ToList();

            return retData;
        }
    }
}
