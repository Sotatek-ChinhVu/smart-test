using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3080.Models;

namespace Reporting.Statistics.Sta3080.DB;

public class CoSta3080Finder : RepositoryBase, ICoSta3080Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3080Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    public List<CoSeisinDayCareInf> GetSeisinDayCareInfs(int hpId, CoSta3080PrintConf printConf)
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
        #region 条件
        //期間
        odrInfs = odrInfs.Where(x => printConf.FromYm * 100 <= x.SinDate && x.SinDate <= printConf.ToYm * 100 + 31);
        #endregion

        var odrInfDetails = NoTrackingDataContext.OdrInfDetails;

        string[] seisinDayCareItems = new string[] {"180028610", "180028710", "180036030", "180039030", "180039130",
                                                    "180039230", "180007510", "180007610", "180048030", "180048130",
                                                    "180039330", "180036130", "180039530", "180039430", "180049030",
                                                    "180048930", "180007810", "180048430", "180017210", "180048530"
                                                    };

        //精神科デイ・ケアの項目を抽出する
        var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId && seisinDayCareItems.Contains(x.ItemCd));

        var odrJoins = (
            from odrInf in odrInfs
            join ptHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join ptInf in ptInfs on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            join tenMst in tenMsts on
                new { odrInfDetail.HpId, odrInfDetail.ItemCd } equals
                new { tenMst.HpId, tenMst.ItemCd }
            select new { odrInf.HpId, SinYm = odrInf.SinDate / 100, ptInf.PtNum, ptInf.Name, ptInf.KanaName, tenMst.ItemCd, ItemName = tenMst.Name, tenMst.StartDate, tenMst.EndDate }
                );

        //使用期限チェック
        odrJoins = odrJoins.Where(x => x.SinYm * 100 >= x.StartDate && x.SinYm * 100 + 31 <= x.EndDate);

        //精神科デイ・ケアが実施された回数を集計する
        var grpOdrJoins = from odrJoin in odrJoins
                          group odrJoin by new { odrJoin.SinYm, odrJoin.PtNum, odrJoin.Name, odrJoin.KanaName, odrJoin.ItemCd, odrJoin.ItemName }
                        into grpOdrJoin
                          select new
                          {
                              grpOdrJoin.Key.SinYm,
                              grpOdrJoin.Key.PtNum,
                              grpOdrJoin.Key.Name,
                              grpOdrJoin.Key.KanaName,
                              grpOdrJoin.Key.ItemCd,
                              grpOdrJoin.Key.ItemName,
                              OdrCount = grpOdrJoin.Count()
                          };

        var sinKouis = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);

        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(x => x.HpId == hpId && seisinDayCareItems.Contains(x.ItemCd));

        var sinJoins = (
            from sinKoui in sinKouis
            join ptHokenPattern in ptHokenPatterns on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join ptInf in ptInfs on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join sinKouiDetail in sinKouiDetails on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.RpNo, sinKoui.SeqNo, sinKoui.SinYm } equals
                new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SeqNo, sinKouiDetail.SinYm }
            select new { sinKoui.HpId, ptInf.PtNum, sinKoui.SinYm }
                );

        //精神科デイ・ケアの初回算定月を取得する
        var grpSinJoins = from sinJoin in sinJoins
                          group sinJoin by new { sinJoin.PtNum }
                        into grpSinJoin
                          select new
                          {
                              grpSinJoin.Key.PtNum,
                              SyokaiYm = grpSinJoin.Min(x => x.SinYm)
                          };

        var seisinDayCareInfs =
            from grpOdrJoin in grpOdrJoins
            join grpSinJoin in grpSinJoins on
                 new { grpOdrJoin.PtNum } equals
                 new { grpSinJoin.PtNum }
            select new
            {
                grpOdrJoin.SinYm,
                grpOdrJoin.PtNum,
                grpOdrJoin.Name,
                grpOdrJoin.KanaName,
                grpOdrJoin.ItemCd,
                grpOdrJoin.ItemName,
                grpOdrJoin.OdrCount,
                grpSinJoin.SyokaiYm
            };

        var retData = seisinDayCareInfs.AsEnumerable().Select(data => new CoSeisinDayCareInf()
        {
            SinYm = data.SinYm,
            PtNum = data.PtNum,
            Name = data.Name,
            KanaName = data.KanaName,
            ItemCd = data.ItemCd,
            ItemName = data.ItemName,
            OdrCount = data.OdrCount.ToString(),
            SyokaiYm = data.SyokaiYm.ToString()
        }
       ).ToList();

        return retData;
    }
}
