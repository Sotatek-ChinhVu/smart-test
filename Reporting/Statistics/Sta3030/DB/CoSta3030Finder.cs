using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3030.Models;

namespace Reporting.Statistics.Sta3030.DB;

public class CoSta3030Finder : RepositoryBase, ICoSta3030Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3030Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    public List<CoPtByomeiModel> GetPtByomeiInfs(int hpId, CoSta3030PrintConf printConf)
    {
        #region 患者選択
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None);
        ptInfs = printConf.IsTester ? ptInfs : ptInfs.Where(p => p.IsTester != 1);
        ptInfs = printConf.PtIds?.Count > 0 ? ptInfs.Where(p => printConf.PtIds.Contains(p.PtId)) : ptInfs;
        var ptInfJoins = from ptInf in ptInfs
                         select new { ptInf };
        #endregion

        #region 来院情報
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.IsDeleted == DeleteStatus.None && r.Status >= 5);
        if (printConf.IsRaiinConf)
        {
            //来院日
            raiinInfs = printConf.SinDateFrom > 0 ? raiinInfs.Where(r => r.SinDate >= printConf.SinDateFrom) : raiinInfs;
            raiinInfs = printConf.SinDateTo > 0 ? raiinInfs.Where(r => r.SinDate <= printConf.SinDateTo) : raiinInfs;

            ptInfJoins = (
            from ptInfJoin in ptInfJoins
            join raiinInf in raiinInfs on
            new { ptInfJoin.ptInf.HpId, ptInfJoin.ptInf.PtId } equals
            new { raiinInf.HpId, raiinInf.PtId }
            select new { ptInfJoin.ptInf }
            ).Distinct();
        }
        #endregion

        #region 診療情報
        if (printConf.IsSinConf)
        {
            #region オーダー情報
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(p => p.IsDeleted == DeleteStatus.None);
            var odrDetails = NoTrackingDataContext.OdrInfDetails;

            var sinJoins = (
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
                ItemName = og.Max(z => z.ItemName)
            }
            );
            #endregion

            #region 算定情報
            if (printConf.SanteiOrder == 0)
            {
                var sinKouiCounts = NoTrackingDataContext.SinKouiCounts;
                var sinKouis = NoTrackingDataContext.SinKouis.Where(p => p.IsDeleted == DeleteStatus.None);
                var sinKouiDetails = NoTrackingDataContext.SinKouiDetails;

                sinJoins = (
                    from sinCount in sinKouiCounts
                    join sinKoui in sinKouis on
                       new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } equals
                       new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                    join sinKouiDetail in sinKouiDetails on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo } equals
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                    join raiinInf in raiinInfs on
                        new { sinCount.HpId, sinCount.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.RaiinNo }
                    group
                        new { sinKouiDetail.ItemName } by
                        new { sinCount.HpId, sinCount.PtId, ItemCd = sinKouiDetail.ItemCd == ItemCdConst.CommentFree ? sinKouiDetail.ItemName : sinKouiDetail.ItemCd } into sg
                    select new
                    {
                        sg.Key.HpId,
                        sg.Key.PtId,
                        sg.Key.ItemCd,
                        ItemName = sg.Max(s => s.ItemName),
                    }
                );
            }
            #endregion


            #region 条件

            //検索ワード
            if (printConf.ItemWords != string.Empty)
            {
                string[] values = printConf.ItemWords.Replace("　", " ").Split(' ');
                List<string> searchWords = new List<string>();
                searchWords.AddRange(values);

                if (printConf.ItemWordOpt == 1)
                {
                    //and検索
                    sinJoins = sinJoins.Where(o => searchWords.All(key => o.ItemName.Contains(key)));
                }
                else
                {
                    //or検索
                    sinJoins = sinJoins.Where(o => searchWords.Any(key => o.ItemName.Contains(key)));
                }
            }

            //検索項目
            var itemCds = new List<string>();
            if (printConf.ItemCds?.Count >= 1)
            {
                itemCds.AddRange(printConf.ItemCds);
            }

            var wrkItems = sinJoins;

            for (int i = 0; i < itemCds.Count; i++)
            {
                string wrkCd = itemCds[i];

                var curItems = sinJoins.Where(p => p.ItemCd == wrkCd);

                if (i == 0)
                {
                    wrkItems = curItems;
                }
                else
                {
                    if (printConf.ItemCdOpt == 0)
                    {
                        //or条件
                        if (curItems != null)
                        {
                            wrkItems = wrkItems == null ? curItems : wrkItems.Union(curItems);
                        }
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
            #endregion

            ptInfJoins = (
                from ptInfJoin in ptInfJoins
                join sinJoin in sinJoins on
                new { ptInfJoin.ptInf.HpId, ptInfJoin.ptInf.PtId } equals
                new { sinJoin.HpId, sinJoin.PtId }
                select new { ptInfJoin.ptInf }
                ).Distinct();

        }
        #endregion

        #region 患者病名情報
        var ptByomeis = GetPtByomeis(hpId, printConf);
        #endregion

        #region 結合
        var lastVisits = NoTrackingDataContext.PtLastVisitDates;
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.IsDeleted == DeleteStatus.None);

        var joinQuery =
            from ptInfJoin in ptInfJoins
            join ptByomei in ptByomeis on
                new { ptInfJoin.ptInf.HpId, ptInfJoin.ptInf.PtId } equals
                new { ptByomei.HpId, ptByomei.PtId }
            join lastVisit in lastVisits on
                new { ptByomei.HpId, ptByomei.PtId } equals
                new { lastVisit.HpId, lastVisit.PtId } into lastVisitJoins
            from lastVisitJoin in lastVisitJoins.DefaultIfEmpty()
            join ptHokenInf in ptHokenInfs on
                new { ptByomei.HpId, ptByomei.PtId, HokenId = ptByomei.HokenPid } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfJoins
            from ptHokenInfJoin in ptHokenInfJoins.DefaultIfEmpty()
            orderby ptInfJoin.ptInf.PtNum, ptByomei.Byomei,
                ptByomei.StartDate, ptInfJoin.ptInf.KanaName, ptByomei.TenkiKbn, ptByomei.TenkiDate
            select new
            {
                ptInfJoin.ptInf,
                ptByomei,
                lastVisitJoin,
                ptHokenInfJoin
            }
        ;

        var retData = joinQuery.AsEnumerable().Select(
                data =>
                    new CoPtByomeiModel
                    (
                        data.ptInf,
                        data.ptByomei,
                        data.lastVisitJoin,
                        data.ptHokenInfJoin
                    )
            ).ToList();

        return retData;
        #endregion

    }

    /// <summary>
    /// 患者病名取得
    /// </summary>
    /// <param name="printConf"></param>
    /// <returns></returns>
    private IQueryable<PtByomei> GetPtByomeis(int hpId, CoSta3030PrintConf printConf)
    {
        var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);

        //有効期限
        ptByomeis = printConf.EnableRangeFrom > 0 ? ptByomeis.Where(p => (p.TenkiDate == 0 ? 99999999 : p.TenkiDate) >= printConf.EnableRangeFrom) : ptByomeis;
        ptByomeis = printConf.EnableRangeTo > 0 ? ptByomeis.Where(p => p.StartDate <= printConf.EnableRangeTo) : ptByomeis;
        //開始日
        ptByomeis = printConf.StartDateFrom > 0 ? ptByomeis.Where(p => p.StartDate >= printConf.StartDateFrom) : ptByomeis;
        ptByomeis = printConf.StartDateTo > 0 ? ptByomeis.Where(p => p.StartDate <= printConf.StartDateTo) : ptByomeis;
        //転帰日
        ptByomeis = printConf.TenkiDateFrom > 0 ? ptByomeis.Where(p => (p.TenkiDate == 0 ? -1 : p.TenkiDate) >= printConf.TenkiDateFrom) : ptByomeis;
        ptByomeis = printConf.TenkiDateTo > 0 ? ptByomeis.Where(p => (p.TenkiDate == 0 ? 100000000 : p.TenkiDate) <= printConf.TenkiDateTo) : ptByomeis;
        //転帰区分
        ptByomeis = printConf.TenkiKbns?.Count > 0 ? ptByomeis.Where(p => printConf.TenkiKbns.Contains(p.TenkiKbn)) : ptByomeis;

        #region 主病名
        if (printConf.SyubyoKbns?.Count == 1 && printConf.SyubyoKbns.Contains(1))
        {
            //主病
            ptByomeis = ptByomeis.Where(p => p.SyubyoKbn == 1);
        }
        else if (printConf.SyubyoKbns?.Count == 1 && printConf.SyubyoKbns.Contains(0))
        {
            //主病以外
            ptByomeis = ptByomeis.Where(p => p.SyubyoKbn != 1);
        }
        #endregion

        #region 疑い病名
        const string doubtCd = "8002";
        if (printConf.DoubtKbns?.Count == 1 && printConf.DoubtKbns.Contains(1))
        {
            //疑い
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
        else if (printConf.DoubtKbns?.Count == 1 && printConf.DoubtKbns.Contains(0))
        {
            //疑い以外
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
        #endregion

        #region 検索ワード
        if (printConf.ByomeiWords != string.Empty)
        {
            //スペース区切りでキーワードを分解
            string[] values = printConf?.ByomeiWords?.Replace("　", " ").Split(' ');
            List<string> searchWords = new List<string>();
            if (values != null)
            {
                searchWords.AddRange(values);
            }

            if (printConf.ByomeiWordOpt == 0)
            {
                //or条件
                ptByomeis = ptByomeis.Where(p => searchWords.Any(key => p.Byomei.Contains(key)));
            }
            else
            {
                //and条件
                ptByomeis = ptByomeis.Where(p => searchWords.All(key => p.Byomei.Contains(key)));
            }
        }
        #endregion

        #region 検索病名
        if (printConf.ByomeiCds?.Count > 0 || printConf.FreeByomeis?.Count > 0)
        {
            const string freeByomeiCd = "0000999";
            var notFreeByomeiCds = printConf.ByomeiCds;
            if (printConf.ByomeiCds?.Count >= 1)
            {
                //未コード化病名のコードを除く
                notFreeByomeiCds = printConf.ByomeiCds.Where(b => b != freeByomeiCd).ToList();
            }

            //未コード化病名の病名を加える
            List<string> searchByomeis = printConf.ByomeiCds;
            if (printConf.FreeByomeis?.Count > 0)
            {
                if (searchByomeis?.Count > 0)
                {
                    searchByomeis.AddRange(printConf.FreeByomeis);
                }
                else
                {
                    searchByomeis = printConf.FreeByomeis;
                }
            }

            IQueryable<PtByomei>? wrkByomeis = null;
            var allByomeiPts = ptByomeis;

            foreach (string searchByomei in searchByomeis)
            {
                var curByomeis = ptByomeis;

                //病名と修飾語の組み合わせを分解
                string[]? searchCds = searchByomei?.Replace("　", " ").Split(' ');
                foreach (string searchCd in searchCds)
                {
                    if (!notFreeByomeiCds.Exists(b => b.Contains(searchCd)))
                    {
                        //未コード化傷病名
                        curByomeis = curByomeis.Where(p => p.ByomeiCd == freeByomeiCd && p.Byomei == searchCd);
                    }
                    else if (searchCd.Trim().Length <= 4)
                    {
                        //修飾語
                        curByomeis = curByomeis.Where(p =>
                            p.SyusyokuCd1 == searchCd || p.SyusyokuCd2 == searchCd || p.SyusyokuCd3 == searchCd ||
                            p.SyusyokuCd4 == searchCd || p.SyusyokuCd5 == searchCd || p.SyusyokuCd6 == searchCd ||
                            p.SyusyokuCd7 == searchCd || p.SyusyokuCd8 == searchCd || p.SyusyokuCd9 == searchCd ||
                            p.SyusyokuCd10 == searchCd || p.SyusyokuCd11 == searchCd || p.SyusyokuCd12 == searchCd ||
                            p.SyusyokuCd13 == searchCd || p.SyusyokuCd14 == searchCd || p.SyusyokuCd15 == searchCd ||
                            p.SyusyokuCd16 == searchCd || p.SyusyokuCd17 == searchCd || p.SyusyokuCd18 == searchCd ||
                            p.SyusyokuCd19 == searchCd || p.SyusyokuCd20 == searchCd || p.SyusyokuCd21 == searchCd);
                    }
                    else
                    {
                        //病名
                        curByomeis = curByomeis.Where(p => p.ByomeiCd == searchCd);
                    }
                }

                if (curByomeis != null)
                {
                    //指定された病名
                    wrkByomeis = wrkByomeis == null ? curByomeis : wrkByomeis.Union(curByomeis);
                }

                if (printConf.ByomeiCdOpt == 1)
                {
                    //指定されたすべての病名を持つ患者                        
                    allByomeiPts = (
                       from allByomeiPt in allByomeiPts
                       where
                           (
                               from c in curByomeis
                               select c
                           ).Any(
                               p =>
                                   p.HpId == allByomeiPt.HpId &&
                                   p.PtId == allByomeiPt.PtId
                           )
                       select
                           allByomeiPt
                    );
                }
            }

            if (printConf.ByomeiCdOpt == 1)
            {
                //and条件
                //指定された病名のうち、すべての病名を持つ患者の病名
                wrkByomeis = (
                   from wrkByomei in wrkByomeis
                   where
                           (
                               from allByomeiPt in allByomeiPts
                               select allByomeiPt
                           ).Any(
                               p =>
                                   p.HpId == wrkByomei.HpId &&
                                   p.PtId == wrkByomei.PtId
                           )
                   select
                       wrkByomei
                );
            }

            ptByomeis = wrkByomeis;
        }

        #endregion

        return ptByomeis;
    }
}
