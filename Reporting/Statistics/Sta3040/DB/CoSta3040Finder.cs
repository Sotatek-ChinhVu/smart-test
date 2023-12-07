using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3040.Models;
using System.Linq.Expressions;

namespace Reporting.Statistics.Sta3040.DB;

public class CoSta3040Finder : RepositoryBase, ICoSta3040Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3040Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    public List<CoUsedDrugInf> GetUsedDrugInfs(int hpId, CoSta3040PrintConf printConf)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(x => x.IsDelete == DeleteStatus.None);
        if (!printConf.IsTester)
        {
            //テスト患者を除く
            ptInfs = ptInfs.Where(x => x.IsTester == 0);
        }

        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None);
        //自費・労災・自賠を除く
        int[] ExpHokKbns = new int[] { 0, 11, 12, 13, 14 };
        ptHokenPatterns = ptHokenPatterns.Where(x => !ExpHokKbns.Contains(x.HokenKbn));

        //期間
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(x => x.HpId == hpId
                                                               && x.IsDeleted == DeleteStatus.None
                                                               && x.SanteiKbn == 0
                                                               && printConf.FromYm * 100 <= x.SinDate
                                                               && x.SinDate <= printConf.ToYm * 100 + 31);

        var odrInfDetails = NoTrackingDataContext.OdrInfDetails;
        var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.DrugKbn > 0);
        var yakkaSyusaiMsts = NoTrackingDataContext.YakkaSyusaiMsts;
        var drugUnitConvs = NoTrackingDataContext.DrugUnitConvs;

        var odrDrugJoinQuery = (
            from odrInf in odrInfs
            join ptHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
            join ptInf in ptInfs on
                new { odrInf.HpId, odrInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            join tenMst in tenMsts on
                new { odrInfDetail.HpId, odrInfDetail.ItemCd } equals
                new { tenMst.HpId, tenMst.ItemCd }
            join yakkaSyusaiMst in yakkaSyusaiMsts on
                new { tenMst.HpId, tenMst.ItemCd, tenMst.YakkaCd } equals
                new { yakkaSyusaiMst.HpId, yakkaSyusaiMst.ItemCd, yakkaSyusaiMst.YakkaCd } into yakkaSyusaiJoins
            from yakkaSyusaiJoin in yakkaSyusaiJoins.DefaultIfEmpty()
            join drugUnitConv in drugUnitConvs on
                new { tenMst.ItemCd } equals
                new { drugUnitConv.ItemCd } into drugUnitConvJoins
            from drugUnitConvJoin in drugUnitConvJoins.DefaultIfEmpty()
            select new { odrInf, odrInfDetail, tenMst, yakkaSyusaiJoin, drugUnitConvJoin }
                );

        //診療日が期間外の薬情報を除外する
        odrDrugJoinQuery = odrDrugJoinQuery.Where(x => (x.tenMst.StartDate <= x.odrInf.SinDate && x.odrInf.SinDate <= x.tenMst.EndDate)
            && (x.yakkaSyusaiJoin == null || x.yakkaSyusaiJoin.StartDate <= x.odrInf.SinDate && x.odrInf.SinDate <= x.yakkaSyusaiJoin.EndDate)
            && (x.drugUnitConvJoin == null || x.drugUnitConvJoin.StartDate <= x.odrInf.SinDate && x.odrInf.SinDate <= x.drugUnitConvJoin.EndDate));

        //後発医薬品の規格単位数量の割合を算出する際に除外する医薬品
        odrDrugJoinQuery = odrDrugJoinQuery.Where(x => x.yakkaSyusaiJoin == null || x.yakkaSyusaiJoin.IsNotarget == 0);

        List<CoOdrDrugInf> odrDrugInfs;

        //診療識別
        var odrKouiKbnExpression = CreateOdrKouiKbnExpression(printConf.SinryoSbt);
        if (odrKouiKbnExpression != null)
        {
            var odrDrugJoinList = odrDrugJoinQuery.ToList();
            var odrDrugJoin = odrDrugJoinList.Select(item => item.odrInf).AsQueryable().Where(odrKouiKbnExpression).ToList();
            odrDrugInfs = (from data in odrDrugJoinList
                           join joinItem in odrDrugJoin on data.odrInf equals joinItem
                           select new CoOdrDrugInf(data.odrInf, data.odrInfDetail, data.tenMst, data.yakkaSyusaiJoin, data.drugUnitConvJoin))
                           .ToList();
        }
        else
        {
            odrDrugInfs = odrDrugJoinQuery.AsEnumerable().Select(
            data =>
                new CoOdrDrugInf(data.odrInf, data.odrInfDetail, data.tenMst, data.yakkaSyusaiJoin, data.drugUnitConvJoin)
            ).ToList();
        }

        var grpOdrDrugInfs = from odrDrugInf in odrDrugInfs
                             group odrDrugInf by new { odrDrugInf.SinYm, odrDrugInf.ItemCd, odrDrugInf.ReceName, odrDrugInf.ReceUnitName }
                             into grpOdrDrugInf
                             orderby grpOdrDrugInf.FirstOrDefault()?.SinYm
                             select new
                             {
                                 grpOdrDrugInf.Key.SinYm,
                                 grpOdrDrugInf.Key.ItemCd,
                                 grpOdrDrugInf.Key.ReceName,
                                 Suryo = grpOdrDrugInf.Sum(x => x.Suryo),
                                 grpOdrDrugInf.Key.ReceUnitName,
                                 Price = grpOdrDrugInf.Max(x => x.Price),
                                 Kbn = grpOdrDrugInf.Max(x => x.Kbn),
                                 SuryoKaisu = grpOdrDrugInf.Sum(x => x.SuryoKaisu),
                                 UnitName = grpOdrDrugInf.Max(x => x.UnitName),
                                 TermVal = grpOdrDrugInf.Max(x => x.TermVal),
                                 CnvVal = grpOdrDrugInf.Max(x => x.CnvVal),
                                 ExixtCnvVal = grpOdrDrugInf.Max(x => x.ExistCnvVal)
                             };

        var retData = grpOdrDrugInfs.Select(data => new CoUsedDrugInf()
        {
            SinYm = data.SinYm,
            ItemCd = data.ItemCd,
            ReceName = data.ReceName,
            Suryo = data.Suryo,
            ReceUnitName = data.ReceUnitName,
            Price = data.Price,
            Kbn = data.Kbn,
            SuryoKaisu = data.SuryoKaisu,
            UnitName = data.UnitName,
            TermVal = data.TermVal,
            CnvVal = data.CnvVal,
            ExistCnvVal = data.ExixtCnvVal
        }).ToList();

        return retData;
    }

    /// <summary>
    /// 診療識別のOR条件を作成する
    /// </summary>
    /// <param name="listSinryoSbts">診療識別リスト</param>
    /// <returns></returns>
    private Expression<Func<OdrInf, bool>>? CreateOdrKouiKbnExpression(List<int> listSinryoSbts)
    {
        var param = Expression.Parameter(typeof(OdrInf));
        Expression? expression = null;

        if (listSinryoSbts == null || listSinryoSbts?.Count == 0)
        {
            //未指定の場合は全て
            listSinryoSbts = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        for (int i = 0; i < listSinryoSbts?.Count; i++)
        {
            Expression? expressionOdrInf = null;
            switch (listSinryoSbts[i])
            {
                case 1:
                    {   //在宅
                        var valOdrKouiKbn = Expression.Constant(14);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        expressionOdrInf = Expression.Equal(valOdrKouiKbn, memberOdrKouiKbn);
                        break;
                    }
                case 2:
                    {  //処方
                        var valOdrKouiKbnMin = Expression.Constant(20);
                        var valOdrKouiKbnMax = Expression.Constant(29);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        var valIn = Expression.Constant(0); //院内
                        var memberInoutKbn = Expression.Property(param, nameof(OdrInf.InoutKbn));
                        var expressionInOutKbn = Expression.Equal(valIn, memberInoutKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        expressionOdrInf = Expression.And(expressionOdrInf, expressionInOutKbn);
                        break;
                    }
                case 3:
                    {  //注射
                        var valOdrKouiKbnMin = Expression.Constant(30);
                        var valOdrKouiKbnMax = Expression.Constant(39);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                case 4:
                    {   //指導
                        var valOdrKouiKbn = Expression.Constant(13);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        expressionOdrInf = Expression.Equal(valOdrKouiKbn, memberOdrKouiKbn);
                        break;
                    }
                case 5:
                    {  //処置
                        var valOdrKouiKbnMin = Expression.Constant(40);
                        var valOdrKouiKbnMax = Expression.Constant(49);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                case 6:
                    {  //手術
                        var valOdrKouiKbnMin = Expression.Constant(50);
                        var valOdrKouiKbnMax = Expression.Constant(59);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                case 7:
                    {  //検査
                        var valOdrKouiKbnMin = Expression.Constant(60);
                        var valOdrKouiKbnMax = Expression.Constant(69);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                case 8:
                    {  //画像
                        var valOdrKouiKbnMin = Expression.Constant(70);
                        var valOdrKouiKbnMax = Expression.Constant(79);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                case 9:
                    {  //その他
                        var valOdrKouiKbnMin = Expression.Constant(80);
                        var valOdrKouiKbnMax = Expression.Constant(89);
                        var memberOdrKouiKbn = Expression.Property(param, nameof(OdrInf.OdrKouiKbn));
                        var expressionOdrKouiKbnMin = Expression.LessThanOrEqual(valOdrKouiKbnMin, memberOdrKouiKbn);
                        var expressionOdrKouiKbnMax = Expression.GreaterThanOrEqual(valOdrKouiKbnMax, memberOdrKouiKbn);
                        expressionOdrInf = Expression.And(expressionOdrKouiKbnMin, expressionOdrKouiKbnMax);
                        break;
                    }
                default:
                    break;
            }

            if (expressionOdrInf != null)
            {
                expression = expression == null ? expressionOdrInf : Expression.Or(expression, expressionOdrInf);
            }
        }

        return expression != null
            ? Expression.Lambda<Func<OdrInf, bool>>(body: expression, parameters: param)
            : null;
    }
}
