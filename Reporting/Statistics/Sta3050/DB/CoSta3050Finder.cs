using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3050.Models;

namespace Reporting.Statistics.Sta3050.DB;

public class CoSta3050Finder : RepositoryBase, ICoSta3050Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3050Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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
    public List<CoSinKouiModel> GetSinKouis(int hpId, CoSta3050PrintConf printConf)
    {
        List<CoSinKouiModel>? sinData;
        try
        {
            if (printConf.DataKind == 0)
            {
                sinData = GetSinKouiDataKinds(hpId, printConf);
            }
            else
            {
                sinData = GetOdrInfs(hpId, printConf);
            }

            return sinData ?? new();
        }
        finally
        {
            sinData = null;
        }
    }

    private List<CoSinKouiModel> GetSinKouiDataKinds(int hpId, CoSta3050PrintConf printConf)
    {
        //var cities = NoTrackingDataContext.TestReports.FromSqlRaw(" SELECT s.pt_id AS \"PtId\", t3.pt_num AS \"PtNum\", t3.kana_name AS \"PtKanaName\", t3.name AS \"PtName\", t3.sex AS \"Sex\", t3.birthday AS \"BirthDay\", s.raiin_no AS \"RaiinNo\", s.sin_ym AS \"SinYm\", s.sin_date AS \"SinDate\", CASE\r\n    WHEN t0.sin_id IN (11, 12) THEN '1x'\r\n    WHEN t0.sin_id IN (24, 25, 26, 27, 28, 29) THEN '2x'\r\n    ELSE t0.sin_id::text\r\nEND AS \"SinId\", t1.suryo AS \"Suryo\", t1.unit_name AS \"UnitName\", s.count AS \"Count\", t1.suryo * s.count::double precision AS \"TotalSuryo\", CASE\r\n    WHEN (t1.odr_item_cd IS NOT NULL) AND (t1.odr_item_cd LIKE 'Z%') AND t1.item_sbt = 0 AND t1.rec_id = 'TO' THEN t1.odr_item_cd\r\n    ELSE t1.item_cd\r\nEND AS \"ItemCd\", CASE\r\n    WHEN (t1.odr_item_cd IS NOT NULL) AND (t1.odr_item_cd LIKE 'Z%') AND t1.item_sbt = 0 AND t1.rec_id = 'TO' THEN t1.odr_item_cd\r\n    WHEN t2.master_sbt = 'C' THEN t1.item_name\r\n    ELSE t1.item_cd\r\nEND AS \"ItemCdCmt\", CASE\r\n    WHEN (t1.odr_item_cd IS NOT NULL) AND (t1.odr_item_cd LIKE 'Z%') AND t1.item_sbt = 0 AND t1.rec_id = 'TO' THEN t1.odr_item_cd\r\n    WHEN t1.item_cd = '810000001' THEN t1.item_name\r\n    ELSE t1.item_cd\r\nEND AS \"SrcItemCd\", t1.item_name AS \"ItemName\", t2.kana_name1 AS \"ItemKanaName1\", t2.kana_name2 AS \"ItemKanaName2\", t2.kana_name3 AS \"ItemKanaName3\", t2.kana_name4 AS \"ItemKanaName4\", t2.kana_name5 AS \"ItemKanaName5\", t2.kana_name6 AS \"ItemKanaName6\", t2.kana_name7 AS \"ItemKanaName7\", r.ka_id AS \"KaId\", k.ka_sname AS \"KaSname\", r.tanto_id AS \"TantoId\", t4.sname AS \"TantoSname\", CASE\r\n    WHEN t2.master_sbt = 'T' AND t2.sin_koui_kbn <> 77 THEN 'T'\r\n    WHEN t2.master_sbt = 'C' THEN '99'\r\n    WHEN t2.sin_koui_kbn IN (20, 21, 22, 23) AND t2.drug_kbn = 1 THEN '21'\r\n    WHEN t2.sin_koui_kbn IN (20, 21, 22, 23) AND t2.drug_kbn = 6 THEN '23'\r\n    WHEN t2.sin_koui_kbn IN (20, 21, 22, 23) AND t2.drug_kbn IN (3, 8) THEN '2x'\r\n    WHEN t2.sin_koui_kbn IN (20, 21, 22, 23) AND t2.drug_kbn = 4 THEN '30'\r\n    WHEN t2.sin_koui_kbn IN (20, 21, 22, 23) THEN '20'\r\n    ELSE t2.sin_koui_kbn::text\r\nEND AS \"SinKouiKbn\", t2.madoku_kbn AS \"MadokuKbn\", t2.drug_kbn AS \"DrugKbn\", t2.kouseisin_kbn AS \"KouseisinKbn\", t2.kazei_kbn AS \"KazeiKbn\", CASE\r\n    WHEN t2.ten_id IN (1, 2, 4, 10, 11, 99) THEN 1\r\n    ELSE 0\r\nEND AS \"EntenKbn\", CASE\r\n    WHEN t2.ten_id IN (5, 6, 7, 9) THEN t1.ten\r\n    WHEN t2.ten_id = 8 THEN -t2.ten\r\n    WHEN t2.ten_id = 10 THEN t2.ten / 10.0\r\n    WHEN t2.ten_id = 11 THEN t2.ten * 10.0\r\n    ELSE t2.ten\r\nEND AS \"Ten\", r.syosaisin_kbn AS \"SyosaisinKbn\", p0.hoken_pid AS \"HokenPid\", p0.hoken_kbn AS \"HokenKbn\", p1.houbetu AS \"Houbetu\", p0.hoken_sbt_cd AS \"HokenSbtCd\", t.inout_kbn AS \"InoutKbn\", t2.kohatu_kbn AS \"KohatuKbn\", t2.is_adopted AS \"IsAdopted\", t2.kizami_id AS \"KizamiId\", t1.ten * s.count::double precision AS \"TenDetail\"\r\nFROM sin_koui_count AS s\r\nINNER JOIN (\r\n    SELECT s0.hp_id, s0.pt_id, s0.sin_ym, s0.rp_no, s0.seq_no, s0.hoken_pid, s0.inout_kbn\r\n    FROM sin_koui AS s0\r\n    WHERE s0.is_deleted = 0\r\n) AS t ON s.hp_id = t.hp_id AND s.pt_id = t.pt_id AND s.sin_ym = t.sin_ym AND s.rp_no = t.rp_no AND s.seq_no = t.seq_no\r\nINNER JOIN (\r\n    SELECT s1.hp_id, s1.pt_id, s1.sin_ym, s1.rp_no, s1.sin_id\r\n    FROM sin_rp_inf AS s1\r\n    WHERE s1.is_deleted = 0\r\n) AS t0 ON s.hp_id = t0.hp_id AND s.pt_id = t0.pt_id AND s.sin_ym = t0.sin_ym AND s.rp_no = t0.rp_no\r\nINNER JOIN (\r\n    SELECT s2.hp_id, s2.pt_id, s2.sin_ym, s2.rp_no, s2.seq_no, s2.item_cd, s2.item_name, s2.item_sbt, s2.odr_item_cd, s2.rec_id, s2.suryo, s2.ten, s2.unit_name\r\n    FROM sin_koui_detail AS s2\r\n    WHERE s2.is_deleted = 0 AND (s2.item_cd IS NOT NULL) AND NOT (s2.item_cd LIKE '@8%') AND NOT (s2.item_cd LIKE '@9%') AND s2.item_cd <> 'XNOODR'\r\n) AS t1 ON s.hp_id = t1.hp_id AND s.pt_id = t1.pt_id AND s.sin_ym = t1.sin_ym AND s.rp_no = t1.rp_no AND s.seq_no = t1.seq_no\r\nINNER JOIN ten_mst AS t2 ON t1.hp_id = t2.hp_id AND CASE\r\n    WHEN (t1.odr_item_cd IS NOT NULL) AND (t1.odr_item_cd LIKE 'Z%') AND t1.item_sbt = 0 AND t1.rec_id = 'TO' THEN t1.odr_item_cd\r\n    ELSE t1.item_cd\r\nEND = t2.item_cd\r\nINNER JOIN (\r\n    SELECT p.hp_id, p.pt_id, p.birthday, p.kana_name, p.name, p.pt_num, p.sex\r\n    FROM pt_inf AS p\r\n    WHERE p.is_delete = 0 AND p.is_tester = 0\r\n) AS t3 ON s.hp_id = t3.hp_id AND s.pt_id = t3.pt_id\r\nINNER JOIN raiin_inf AS r ON s.hp_id = r.hp_id AND s.raiin_no = r.raiin_no\r\nINNER JOIN pt_hoken_pattern AS p0 ON t.hp_id = p0.hp_id AND t.pt_id = p0.pt_id AND t.hoken_pid = p0.hoken_pid\r\nINNER JOIN pt_hoken_inf AS p1 ON p0.hp_id = p1.hp_id AND p0.pt_id = p1.pt_id AND p0.hoken_id = p1.hoken_id\r\nLEFT JOIN ka_mst AS k ON r.hp_id = k.hp_id AND r.ka_id = k.ka_id\r\nLEFT JOIN (\r\n    SELECT u.hp_id, u.sname, u.user_id\r\n    FROM user_mst AS u\r\n    WHERE u.is_deleted = 0\r\n) AS t4 ON r.hp_id = t4.hp_id AND r.tanto_id = t4.user_id\r\nWHERE s.hp_id = 1 AND s.sin_date >= 20200101 AND s.sin_date <= 20201231 AND t2.start_date <= s.sin_date AND CASE\r\n    WHEN t2.end_date = 12341234 THEN 99999999\r\n    ELSE t2.end_date\r\nEND >= s.sin_date").ToList();

        List<CoSinKouiModel>? retData = new();
        try
        {
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(s => s.HpId == hpId);
            sinKouiCounts = printConf.StartSinYm >= 0 ?
                sinKouiCounts.Where(s => s.SinYm >= printConf.StartSinYm && s.SinYm <= printConf.EndSinYm) :
                sinKouiCounts.Where(s => s.SinDate >= printConf.StartSinDate && s.SinDate <= printConf.EndSinDate);

            var sinKouis = NoTrackingDataContext.SinKouis.Where(s => s.IsDeleted == DeleteStatus.None);
            #region 条件指定（院内院外区分）
            if (printConf.InoutKbns?.Count >= 1)
            {
                sinKouis = sinKouis.Where(s => printConf.InoutKbns.Contains(s.InoutKbn));
            }
            #endregion
            var sinKouiRpInfs = NoTrackingDataContext.SinRpInfs.Where(s => s.IsDeleted == DeleteStatus.None);
            #region 条件指定（診療識別）
            if (printConf.SinIds?.Count >= 1)
            {
                List<int> sinIds = new List<int>();
                foreach (var sinId in printConf.SinIds)
                {
                    switch (sinId)
                    {
                        case "1x":
                            sinIds.AddRange(new int[] { 11, 12 });
                            break;
                        case "2x":
                            sinIds.AddRange(new int[] { 24, 25, 26, 27, 28, 29 });
                            break;
                        default:
                            sinIds.Add(sinId.AsInteger());
                            break;
                    }
                }
                sinKouiRpInfs = sinKouiRpInfs.Where(s => sinIds.Contains(s.SinId));
            }
            #endregion

            IQueryable<TenMst> tenMsts = NoTrackingDataContext.TenMsts;

            #region 項目コード変換
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
            #endregion
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

            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(s => s.IsDeleted == DeleteStatus.None && !s.ItemCd.StartsWith("@8") && !s.ItemCd.StartsWith("@9") && s.ItemCd != "XNOODR");
            #region 速度向上のため sinKouiDetails を先に絞り込む
            if (printConf.ItemCds?.Count >= 1 && printConf.ItemSearchOpt == 0)
            {
                var filterCds = new List<string>();
                filterCds.AddRange(printConf.ItemCds);
                filterCds.Add(ItemCdConst.CommentFree);

                if (printConf.SearchWord.AsString() != "")
                {
                    //スペース区切りでキーワードを分解
                    string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                    List<string> searchWords = new List<string>();
                    searchWords.AddRange(values);

                    if (printConf.SearchOpt == 0)
                    {
                        //or条件
                        if (printConf.ItemCds.Any(p => p.StartsWith("Z")))
                        {
                            sinKouiDetails = sinKouiDetails.Where(s =>
                                filterCds.Any(key => s.ItemCd == key ||
                                filterCds.Any(key => s.OdrItemCd == key)) ||
                                searchWords.Any(key => s.ItemName.Contains(key))
                            );
                        }
                        else
                        {
                            sinKouiDetails = sinKouiDetails.Where(s =>
                                filterCds.Any(key => s.ItemCd == key) ||
                                searchWords.Any(key => s.ItemName.Contains(key))
                            );
                        }
                    }
                    else
                    {
                        //and条件
                        if (printConf.ItemCds.Any(p => p.StartsWith("Z")))
                        {
                            sinKouiDetails = sinKouiDetails.Where(s =>
                                filterCds.Any(key => s.ItemCd == key ||
                                filterCds.Any(key => s.OdrItemCd == key)) ||
                                searchWords.All(key => s.ItemName.Contains(key))
                            );
                        }
                        else
                        {
                            sinKouiDetails = sinKouiDetails.Where(s =>
                                filterCds.Any(key => s.ItemCd == key) ||
                                searchWords.All(key => s.ItemName.Contains(key))
                            );
                        }
                    }
                }
                else
                {
                    if (printConf.ItemCds.Any(p => p.StartsWith("Z")))
                    {
                        sinKouiDetails = sinKouiDetails.Where(s => filterCds.Any(key => s.ItemCd == key || filterCds.Any(key => s.OdrItemCd == key)));
                    }
                    else
                    {
                        sinKouiDetails = sinKouiDetails.Where(s => filterCds.Any(key => s.ItemCd == key));
                    }
                }
            }
            #endregion

            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
            ptInfs = !printConf.IsTester ? ptInfs.Where(p => p.IsTester == 0) : ptInfs;
            ptInfs = printConf.StartPtNum > 0 ? ptInfs.Where(p => p.PtNum >= printConf.StartPtNum) : ptInfs;
            ptInfs = printConf.EndPtNum > 0 ? ptInfs.Where(p => p.PtNum <= printConf.EndPtNum) : ptInfs;
            IQueryable<RaiinInf> raiinInfs = NoTrackingDataContext.RaiinInfs;
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

            var kaMsts = NoTrackingDataContext.KaMsts;
            var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.IsDeleted == DeleteStatus.None);

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
                    PtNum = ptInf.PtNum,
                    PtKanaName = ptInf.KanaName,
                    PtName = ptInf.Name,
                    Sex = ptInf.Sex,
                    BirthDay = ptInf.Birthday,
                    RaiinNo = sinCount.RaiinNo,
                    SinYm = sinCount.SinYm,
                    SinDate = sinCount.SinDate,
                    SinId =
                            new int[] { 11, 12 }.Contains(sinRp.SinId) ? "1x" :
                            new int[] { 24, 25, 26, 27, 28, 29 }.Contains(sinRp.SinId) ? "2x" :
                            sinRp.SinId.ToString(),
                    Suryo = sinDetail.Suryo,
                    UnitName = sinDetail.UnitName,
                    Count = sinCount.Count,
                    TotalSuryo = sinDetail.Suryo * sinCount.Count,
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
                    SyosaisinKbn = raiinInf.SyosaisinKbn,
                    HokenPid = ptHokenPattern.HokenPid,
                    HokenKbn = ptHokenPattern.HokenKbn,
                    Houbetu = ptHokenInf.Houbetu,
                    HokenSbtCd = ptHokenPattern.HokenSbtCd,
                    InoutKbn = sinKoui.InoutKbn,
                    KohatuKbn = tenMst.KohatuKbn,
                    IsAdopted = tenMst.IsAdopted,
                    KizamiId = tenMst.KizamiId,
                    TenDetail = sinDetail.Ten * sinCount.Count
                }
            );

            #region 条件指定
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
            if (printConf.ItemCds?.Count >= 1 && printConf.SearchWord.AsString() != "")
            {
                //スペース区切りでキーワードを分解
                string[] values = printConf.SearchWord.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.ItemSearchOpt == 0)
                {
                    //or条件
                    if (printConf.SearchOpt == 0)
                    {
                        //or条件
                        joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key) || searchWords.Any(key => r.ItemName.Contains(key)));
                    }
                    else
                    {
                        //and条件
                        joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key) || searchWords.All(key => r.ItemName.Contains(key)));
                    }
                }
                else
                {
                    //and条件
                    var ptIdList = joinQuery.AsEnumerable().Select(r => new { r.PtId, r.SrcItemCd })
                                            .GroupBy(r => r.PtId)
                                            .Select(r => new
                                            {
                                                ptId = r.Key,
                                                ItemCdList = r.Select(k => k.SrcItemCd).Distinct()
                                            })
                                            .Where(r => printConf.ItemCds.All(key => r.ItemCdList.Contains(key)))
                                            .Select(r => r.ptId).ToList();

                    if (printConf.SearchOpt == 0)
                    {
                        //or条件
                        joinQuery = joinQuery.Where(r => (ptIdList.Any(key => r.PtId == key) && printConf.ItemCds.Any(key => r.SrcItemCd == key)) || searchWords.Any(key => r.ItemName.Contains(key)));
                    }
                    else
                    {
                        //and条件
                        joinQuery = joinQuery.Where(r => (ptIdList.Any(key => r.PtId == key) && printConf.ItemCds.Any(key => r.SrcItemCd == key)) || searchWords.All(key => r.ItemName.Contains(key)));
                    }
                }
            }
            //検索項目
            else if (printConf.ItemCds?.Count >= 1)
            {
                if (printConf.ItemSearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key));
                }
                else
                {
                    //and条件
                    var ptIdList = joinQuery.AsEnumerable().Select(r => new { r.PtId, r.SrcItemCd })
                                                .GroupBy(r => r.PtId)
                                                .Select(r => new
                                                {
                                                    ptId = r.Key,
                                                    ItemCdList = r.Select(k => k.SrcItemCd).Distinct()
                                                })
                                                .Where(r => printConf.ItemCds.All(key => r.ItemCdList.Contains(key)))
                                                .Select(r => r.ptId).ToList();
                    //該当患者が多いとき、SQL作成でオーバーフローが発生していたため、Any→Containsに修正
                    joinQuery = joinQuery.Where(r => ptIdList.Contains(r.PtId) && printConf.ItemCds.Any(key => r.SrcItemCd == key));
                }
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

            var temp = joinQuery.AsEnumerable();

            foreach (var data in temp)
            {
                retData.Add(new CoSinKouiModel()
                {
                    PtId = data.PtId,
                    PtNum = data.PtNum,
                    PtKanaName = data.PtKanaName,
                    PtName = data.PtName,
                    Sex = data.Sex,
                    BirthDay = data.BirthDay,
                    RaiinNo = data.RaiinNo,
                    SinYm = data.SinYm,
                    SinDate = data.SinDate,
                    SinId = data.SinId,
                    Suryo = data.Suryo,
                    UnitName = data.UnitName,
                    Count = data.Count,
                    TotalSuryo = data.TotalSuryo,
                    Money =
                            data.KizamiId == 1 ? (int)Math.Round(data.TenDetail * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero) :
                            (int)Math.Round(data.Ten * data.Suryo * data.Count * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero),
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
                    Ten = data.Ten,
                    SyosaisinKbn = data.SyosaisinKbn,
                    HokenPid = data.HokenPid,
                    HokenKbn = data.HokenKbn,
                    Houbetu = data.Houbetu,
                    HokenSbtCd = data.HokenSbtCd,
                    InoutKbn = data.InoutKbn,
                    KohatuKbn = data.KohatuKbn,
                    IsAdopted = data.IsAdopted
                });
            }

            //retData = joinQuery.AsEnumerable().Select(
            //    data =>
            //        new CoSinKouiModel()
            //        {
            //            PtId = data.PtId,
            //            PtNum = data.PtNum,
            //            PtKanaName = data.PtKanaName,
            //            PtName = data.PtName,
            //            Sex = data.Sex,
            //            BirthDay = data.BirthDay,
            //            RaiinNo = data.RaiinNo,
            //            SinYm = data.SinYm,
            //            SinDate = data.SinDate,
            //            SinId = data.SinId,
            //            Suryo = data.Suryo,
            //            UnitName = data.UnitName,
            //            Count = data.Count,
            //            TotalSuryo = data.TotalSuryo,
            //            Money =
            //                data.KizamiId == 1 ? (int)Math.Round(data.TenDetail * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero) :
            //                (int)Math.Round(data.Ten * data.Suryo * data.Count * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero),
            //            ItemCd = data.ItemCd,
            //            ItemCdCmt = data.ItemCdCmt,
            //            ItemName = data.ItemName,
            //            ItemKanaName1 = data.ItemKanaName1,
            //            ItemKanaName2 = data.ItemKanaName2,
            //            ItemKanaName3 = data.ItemKanaName3,
            //            ItemKanaName4 = data.ItemKanaName4,
            //            ItemKanaName5 = data.ItemKanaName5,
            //            ItemKanaName6 = data.ItemKanaName6,
            //            ItemKanaName7 = data.ItemKanaName7,
            //            KaId = data.KaId,
            //            KaSname = data.KaSname,
            //            TantoId = data.TantoId,
            //            TantoSname = data.TantoSname,
            //            SinKouiKbn = data.SinKouiKbn,
            //            MadokuKbn = data.MadokuKbn,
            //            KouseisinKbn = data.KouseisinKbn,
            //            KazeiKbn = data.KazeiKbn,
            //            EntenKbn = data.EntenKbn,
            //            Ten = data.Ten,
            //            SyosaisinKbn = data.SyosaisinKbn,
            //            HokenPid = data.HokenPid,
            //            HokenKbn = data.HokenKbn,
            //            Houbetu = data.Houbetu,
            //            HokenSbtCd = data.HokenSbtCd,
            //            InoutKbn = data.InoutKbn,
            //            KohatuKbn = data.KohatuKbn,
            //            IsAdopted = data.IsAdopted
            //        }
            //)
            //.ToList();

            #region 公費法別
            var ptKohis = NoTrackingDataContext.PtKohis;

            int startYm = printConf.StartSinYm >= 0 ? printConf.StartSinYm : printConf.StartSinDate / 100;
            int endYm = printConf.StartSinYm >= 0 ? printConf.EndSinYm : printConf.EndSinDate / 100;

            var ptKohiPatterns = (
                from sinKoui in sinKouis
                join ptHokenPattern in ptHokenPatterns on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptKohi1 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                    new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1join
                from ptKohi1j in ptKohi1join.DefaultIfEmpty()
                join ptKohi2 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                    new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2join
                from ptKohi2j in ptKohi2join.DefaultIfEmpty()
                join ptKohi3 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                    new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3join
                from ptKohi3j in ptKohi3join.DefaultIfEmpty()
                join ptKohi4 in ptKohis on
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                    new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4join
                from ptKohi4j in ptKohi4join.DefaultIfEmpty()
                where
                    sinKoui.HpId == hpId &&
                    sinKoui.SinYm >= startYm &&
                    sinKoui.SinYm <= endYm
                group
                    new { ptHokenPattern, ptKohi1j, ptKohi2j, ptKohi3j, ptKohi4j } by
                    new
                    {
                        ptHokenPattern.HpId,
                        ptHokenPattern.PtId,
                        ptHokenPattern.HokenPid,
                        Kohi1Houbetu = ptKohi1j.Houbetu,
                        Kohi2Houbetu = ptKohi2j.Houbetu,
                        Kohi3Houbetu = ptKohi3j.Houbetu,
                        Kohi4Houbetu = ptKohi4j.Houbetu
                    } into ptKohiGroup
                select new
                {
                    ptKohiGroup.Key.PtId,
                    ptKohiGroup.Key.HokenPid,
                    ptKohiGroup.Key.Kohi1Houbetu,
                    ptKohiGroup.Key.Kohi2Houbetu,
                    ptKohiGroup.Key.Kohi3Houbetu,
                    ptKohiGroup.Key.Kohi4Houbetu
                }
            ).ToList();

            //保険情報を足しこみ
            foreach (var wrkData in retData)
            {
                var ptKohiPattern = ptKohiPatterns.Find(p =>
                    p.PtId == wrkData.PtId &&
                    p.HokenPid == wrkData.HokenPid
                );

                if (ptKohiPattern != null)
                {
                    wrkData.Kohi1Houbetu = ptKohiPattern.Kohi1Houbetu;
                    wrkData.Kohi2Houbetu = ptKohiPattern.Kohi2Houbetu;
                    wrkData.Kohi3Houbetu = ptKohiPattern.Kohi3Houbetu;
                    wrkData.Kohi4Houbetu = ptKohiPattern.Kohi4Houbetu;
                }
            }
            ptKohiPatterns = null;

            #endregion

            return retData ?? new();
        }
        finally
        {
            retData = null;
        }
    }

    private List<CoSinKouiModel> GetOdrInfs(int hpId, CoSta3050PrintConf printConf)
    {
        IQueryable<PtInf> ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
        ptInfs = !printConf.IsTester ? ptInfs.Where(p => p.IsTester == 0) : ptInfs;
        ptInfs = printConf.StartPtNum > 0 ? ptInfs.Where(p => p.PtNum >= printConf.StartPtNum) : ptInfs;
        ptInfs = printConf.EndPtNum > 0 ? ptInfs.Where(p => p.PtNum <= printConf.EndPtNum) : ptInfs;

        var ptInfList = ptInfs.ToList();
        var ptIdInfList = ptInfList.Select(item => item.PtId).Distinct().ToList();

        IQueryable<OdrInf> odrInfs = NoTrackingDataContext.OdrInfs.Where(item => ptIdInfList.Contains(item.PtId) && item.IsDeleted == DeleteStatus.None);
        odrInfs = printConf.StartSinYm >= 0 ?
        odrInfs.Where(s => s.SinDate >= printConf.StartSinYm * 100 + 1 && s.SinDate <= printConf.EndSinYm * 100 + 31) :
        odrInfs.Where(s => s.SinDate >= printConf.StartSinDate && s.SinDate <= printConf.EndSinDate);

        var odrInfList = odrInfs.ToList();
        var raiinNoList = odrInfList.Select(item => item.RaiinNo).Distinct().ToList();
        var rpNoList = odrInfList.Select(item => item.RpNo).Distinct().ToList();
        var rpEdaNoList = odrInfList.Select(item => item.RpEdaNo).Distinct().ToList();

        var odrDetails = NoTrackingDataContext.OdrInfDetails.Where(item => raiinNoList.Contains(item.RaiinNo)
                                                                           && rpNoList.Contains(item.RpNo)
                                                                           && rpEdaNoList.Contains(item.RpEdaNo))
                                                            .ToList();
        var itemCdList = odrDetails.Select(item => item.ItemCd).Distinct().ToList();
        var tenMsts = NoTrackingDataContext.TenMsts.Where(item => itemCdList.Contains(item.ItemCd)).ToList();

        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(item => item.Status > 3 && raiinNoList.Contains(item.RaiinNo));
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

        IQueryable<KaMst> kaMsts = NoTrackingDataContext.KaMsts;
        IQueryable<UserMst> userMsts = NoTrackingDataContext.UserMsts.Where(u => u.IsDeleted == DeleteStatus.None);

        var joinOdrs = (
            from odrInf in odrInfList
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
                odrInf.InoutKbn,
                odrInf.HokenPid,
                odrDetail.Suryo,
                odrDetail.TermVal,
                odrDetail.ItemCd,
                odrDetail.ItemName,
                UnitName =
                    tenMsti == null ? string.Empty :
                    (tenMsti.ReceUnitName ?? string.Empty) == string.Empty && (tenMsti.OdrUnitName ?? string.Empty) != string.Empty ? tenMsti.OdrUnitName :
                    tenMsti.ReceUnitName,
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
                KohatuKbn = tenMsti == null ? 0 : tenMsti.KohatuKbn,
                IsAdopted = tenMsti == null ? 0 : tenMsti.IsAdopted
            }
        ).ToList();

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
                PtNum = ptInf.PtNum,
                PtKanaName = ptInf.KanaName,
                PtName = ptInf.Name,
                Sex = ptInf.Sex,
                BirthDay = ptInf.Birthday,
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
                        new string[] { "@SHIN", "@JIKAN" }.Contains(joinOdr.ItemCd) ? joinOdr.Suryo :
                        joinOdr.Suryo == 0 ? 1 :
                        joinOdr.TermVal == 0 ? joinOdr.Suryo :
                        (Math.Round(joinOdr.Suryo * joinOdr.TermVal * 1000) / 1000),
                UnitName = joinOdr.UnitName,
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
                DrugKbn = joinOdr.DrugKbn,
                KouseisinKbn = joinOdr.KouseisinKbn,
                KazeiKbn = joinOdr.KazeiKbn,
                EntenKbn = new int[] { 1, 2, 4, 10, 11, 99 }.Contains(joinOdr.TenId) ? 1 : 0,
                Ten =
                        new int[] { 5, 6, 7, 9 }.Contains(joinOdr.TenId) ? 0 :
                        joinOdr.TenId == 8 ? -joinOdr.Ten :
                        joinOdr.TenId == 10 ? joinOdr.Ten / 10 :
                        joinOdr.TenId == 11 ? joinOdr.Ten * 10 :
                        joinOdr.Ten,
                SyosaisinKbn = raiinInf.SyosaisinKbn,
                HokenKbn = ptHokenPattern.HokenKbn,
                Houbetu = ptHokenInf.Houbetu,
                HokenSbtCd = ptHokenPattern.HokenSbtCd,
                InoutKbn = joinOdr.InoutKbn,
                KohatuKbn = joinOdr.KohatuKbn,
                IsAdopted = joinOdr.IsAdopted,
                HokenPid = joinOdr.HokenPid
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

            if (printConf.ItemSearchOpt == 0)
            {
                //or条件
                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key) || searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key) || searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
            else
            {
                //and条件
                var ptIdList = joinQuery.Select(r => new { r.PtId, r.SrcItemCd })
                                        .GroupBy(r => r.PtId)
                                        .Select(r => new
                                        {
                                            ptId = r.Key,
                                            ItemCdList = r.Select(k => k.SrcItemCd).Distinct()
                                        })
                                        .Where(r => printConf.ItemCds.All(key => r.ItemCdList.Contains(key)))
                                        .Select(r => r.ptId).ToList();

                if (printConf.SearchOpt == 0)
                {
                    //or条件
                    joinQuery = joinQuery.Where(r => (ptIdList.Any(key => r.PtId == key) && printConf.ItemCds.Any(key => r.SrcItemCd == key)) || searchWords.Any(key => r.ItemName.Contains(key)));
                }
                else
                {
                    //and条件
                    joinQuery = joinQuery.Where(r => (ptIdList.Any(key => r.PtId == key) && printConf.ItemCds.Any(key => r.SrcItemCd == key)) || searchWords.All(key => r.ItemName.Contains(key)));
                }
            }
        }
        //検索項目
        else if (printConf.ItemCds?.Count >= 1)
        {
            if (printConf.ItemSearchOpt == 0)
            {
                //or条件
                joinQuery = joinQuery.Where(r => printConf.ItemCds.Any(key => r.SrcItemCd == key));
            }
            else
            {
                //and条件
                var ptIdList = joinQuery.Select(r => new { r.PtId, r.SrcItemCd })
                                        .GroupBy(r => r.PtId)
                                        .Select(r => new
                                        {
                                            ptId = r.Key,
                                            ItemCdList = r.Select(k => k.SrcItemCd).Distinct()
                                        })
                                        .Where(r => printConf.ItemCds.All(key => r.ItemCdList.Contains(key)))
                                        .Select(r => r.ptId).ToList();
                joinQuery = joinQuery.Where(r => ptIdList.Any(key => r.PtId == key) && printConf.ItemCds.Any(key => r.SrcItemCd == key));
            }
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

        List<string> zaiSuryos = new()
            {
                ItemCdConst.ZaiOusin, ItemCdConst.ZaiOusinTokubetu,
                ItemCdConst.ZaiHoumon1_1Dou, ItemCdConst.ZaiHoumon1_1DouIgai,
                ItemCdConst.ZaiHoumon1_2Dou, ItemCdConst.ZaiHoumon1_2DouIgai,
                ItemCdConst.ZaiHoumon2i, ItemCdConst.ZaiHoumon2ro
            };

        var joinList = joinQuery.ToList();

        var retData = joinList.Select(
            data =>
                new CoSinKouiModel()
                {
                    PtId = data.PtId,
                    PtNum = data.PtNum,
                    PtKanaName = data.PtKanaName,
                    PtName = data.PtName,
                    Sex = data.Sex,
                    BirthDay = data.BirthDay,
                    RaiinNo = data.RaiinNo,
                    SinYm = data.SinDate / 100,
                    SinDate = data.SinDate,
                    SinId = data.SinId,
                    Suryo = data.Suryo,
                    UnitName = data.UnitName,
                    Count = data.Count,
                    TotalSuryo = data.Suryo * data.Count,
                    Money =
                        zaiSuryos.Contains(data.ItemCd) ? (int)Math.Round(data.Ten * data.Count * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero) :
                        (int)Math.Round(data.Ten * data.Suryo * data.Count * (data.EntenKbn == 1 ? 1 : 10), MidpointRounding.AwayFromZero),
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
                    Ten = data.Ten,
                    SyosaisinKbn = data.SyosaisinKbn,
                    HokenKbn = data.HokenKbn,
                    Houbetu = data.Houbetu,
                    HokenSbtCd = data.HokenSbtCd,
                    InoutKbn = data.InoutKbn,
                    KohatuKbn = data.KohatuKbn,
                    IsAdopted = data.IsAdopted,
                    HokenPid = data.HokenPid
                }
        )
        .ToList();

        #region 公費法別
        var ptKohis = NoTrackingDataContext.PtKohis;

        int startYmd = printConf.StartSinYm >= 0 ? printConf.StartSinYm * 100 + 1 : printConf.StartSinDate;
        int endYmd = printConf.StartSinYm >= 0 ? printConf.EndSinYm * 100 + 99 : printConf.EndSinDate;

        var ptKohiPatterns = (
            from odrInf in odrInfs
            join ptHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join ptKohi1 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1join
            from ptKohi1j in ptKohi1join.DefaultIfEmpty()
            join ptKohi2 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2join
            from ptKohi2j in ptKohi2join.DefaultIfEmpty()
            join ptKohi3 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3join
            from ptKohi3j in ptKohi3join.DefaultIfEmpty()
            join ptKohi4 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4join
            from ptKohi4j in ptKohi4join.DefaultIfEmpty()
            where
                odrInf.HpId == hpId &&
                odrInf.SinDate >= startYmd &&
                odrInf.SinDate <= endYmd
            group
                new { ptHokenPattern, ptKohi1j, ptKohi2j, ptKohi3j, ptKohi4j } by
                new
                {
                    ptHokenPattern.HpId,
                    ptHokenPattern.PtId,
                    ptHokenPattern.HokenPid,
                    Kohi1Houbetu = ptKohi1j.Houbetu,
                    Kohi2Houbetu = ptKohi2j.Houbetu,
                    Kohi3Houbetu = ptKohi3j.Houbetu,
                    Kohi4Houbetu = ptKohi4j.Houbetu
                } into ptKohiGroup
            select new
            {
                ptKohiGroup.Key.PtId,
                ptKohiGroup.Key.HokenPid,
                ptKohiGroup.Key.Kohi1Houbetu,
                ptKohiGroup.Key.Kohi2Houbetu,
                ptKohiGroup.Key.Kohi3Houbetu,
                ptKohiGroup.Key.Kohi4Houbetu
            }
        ).ToList();

        //保険情報を足しこみ
        foreach (var wrkData in retData)
        {
            var ptKohiPattern = ptKohiPatterns.Find(p =>
                p.PtId == wrkData.PtId &&
                p.HokenPid == wrkData.HokenPid
            );

            if (ptKohiPattern != null)
            {
                wrkData.Kohi1Houbetu = ptKohiPattern.Kohi1Houbetu;
                wrkData.Kohi2Houbetu = ptKohiPattern.Kohi2Houbetu;
                wrkData.Kohi3Houbetu = ptKohiPattern.Kohi3Houbetu;
                wrkData.Kohi4Houbetu = ptKohiPattern.Kohi4Houbetu;
            }
        }
        #endregion

        return retData;
    }
}
