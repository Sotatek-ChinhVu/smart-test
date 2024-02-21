using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3041.Models;

namespace Reporting.Statistics.Sta3041.DB;

public class CoSta3041Finder : RepositoryBase, ICoSta3041Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3041Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    public List<CoKouseisinInf> GetKouseisinInfs(int hpId, CoSta3041PrintConf printConf)
    {

        var ptInfs = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId && x.IsDelete == DeleteStatus.None);
        if (!printConf.IsTester)
        {
            //テスト患者を除く
            ptInfs = ptInfs.Where(x => x.IsTester == 0);
        }

        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.IsDeleted == DeleteStatus.None);
        //健保を抽出する
        int[] kenpos = new int[] { 1, 2 };
        ptHokenPatterns = ptHokenPatterns.Where(x => kenpos.Contains(x.HokenKbn));

        var odrInfs = NoTrackingDataContext.OdrInfs.Where(x => x.HpId == hpId && x.IsDeleted == DeleteStatus.None && x.SanteiKbn == 0);
        //処方を抽出する
        odrInfs = odrInfs.Where(x => 20 <= x.OdrKouiKbn && x.OdrKouiKbn <= 23);
        #region 条件
        //期間
        odrInfs = odrInfs.Where(x => printConf.FromYm * 100 <= x.SinDate && x.SinDate <= printConf.ToYm * 100 + 31);
        #endregion

        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(x => x.HpId == hpId);

        //向精神薬を抽出する
        int[] kouseisinKbns = new int[] { 1, 2, 3, 4 };
        var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId && kouseisinKbns.Contains(x.KouseisinKbn));

        var odrJoins = (
            from odrInf in odrInfs
            join ptHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            select new { odrInf.HpId, odrInf.PtId, odrInf.SinDate, odrInfDetail.ItemCd }
                ).Distinct();

        var joinQueries = (
            from odrJoin in odrJoins
            join ptInf in ptInfs on
                new { odrJoin.HpId, odrJoin.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join tenMst in tenMsts on
                new { odrJoin.HpId, odrJoin.ItemCd } equals
                new { tenMst.HpId, tenMst.ItemCd }
            select new { ptInf, odrJoin, tenMst, YakkaCd7 = tenMst.YakkaCd != null ? tenMst.YakkaCd.Substring(0, 7) : string.Empty }
                ).ToList();

        //診療日が期間外の項目を除外する
        joinQueries = joinQueries.Where(x => x.tenMst.StartDate <= x.odrJoin.SinDate && x.odrJoin.SinDate <= x.tenMst.EndDate).ToList();

        //薬価基準コード前7桁ごとの種類数を集計する
        var grpJoinQueries = from joinQuery in joinQueries
                             group joinQuery by new { joinQuery.ptInf.PtNum, joinQuery.odrJoin.SinDate, joinQuery.tenMst.KouseisinKbn }
                           into grpJoinQuery
                             select new
                             {
                                 grpJoinQuery.Key.PtNum,
                                 grpJoinQuery.Key.SinDate,
                                 grpJoinQuery.Key.KouseisinKbn,
                                 DrugCount = grpJoinQuery.Select(x => x.YakkaCd7).Distinct().Count()
                             }
                           ;

        var kouseisinInfs =
            from joinQuery in joinQueries
            join grpJoinQuery in grpJoinQueries on
                new { joinQuery.ptInf.PtNum, joinQuery.odrJoin.SinDate, joinQuery.tenMst.KouseisinKbn } equals
                new { grpJoinQuery.PtNum, grpJoinQuery.SinDate, grpJoinQuery.KouseisinKbn }
            select new { joinQuery.ptInf, joinQuery.odrJoin.SinDate, joinQuery.tenMst, grpJoinQuery.DrugCount }
                ;

        var retData = kouseisinInfs.AsEnumerable().Select(data => new CoKouseisinInf(data.ptInf, data.SinDate, data.tenMst, data.DrugCount)).ToList();

        return retData;
    }
}
