using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3040.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Reporting.Statistics.Sta3040.DB;

public class CoSta3040Finder : RepositoryBase, ICoSta3040Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    private readonly TenantDataContext _tenantPtInf;
    private readonly TenantDataContext _tenantPtHokenPattern;
    private readonly TenantDataContext _tenantOdrInfDetail;
    private readonly TenantDataContext _tenantYakkaSyusaiMst;
    private readonly TenantDataContext _tenantDrugUnitConv;

    public CoSta3040Finder(ITenantProvider tenantProvider,
        ITenantProvider tenantPtInf,
        ITenantProvider tenantPtHokenPattern,
        ITenantProvider tenantOdrInfDetail,
        ITenantProvider tenantYakkaSyusaiMst,
        ITenantProvider tenantDrugUnitConv,
        ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
        _tenantPtInf = tenantPtInf.GetNoTrackingDataContext();
        _tenantPtHokenPattern = tenantPtHokenPattern.GetNoTrackingDataContext();
        _tenantOdrInfDetail = tenantOdrInfDetail.GetNoTrackingDataContext();
        _tenantYakkaSyusaiMst = tenantYakkaSyusaiMst.GetNoTrackingDataContext();
        _tenantDrugUnitConv = tenantDrugUnitConv.GetNoTrackingDataContext();
    }
    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    public List<CoUsedDrugInf> GetUsedDrugInfs(int hpId, CoSta3040PrintConf printConf)
    {

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        try
        {
            var ptInfs = _tenantPtInf.PtInfs.Where(x => x.IsDelete == DeleteStatus.None);
            if (!printConf.IsTester)
            {
                //テスト患者を除く
                ptInfs = ptInfs.Where(x => x.IsTester == 0);
            }

            var ptHokenPatterns = _tenantPtHokenPattern.PtHokenPatterns.Where(x => x.IsDeleted == DeleteStatus.None);
            //自費・労災・自賠を除く
            int[] expHokKbns = new int[] { 0, 11, 12, 13, 14 };
            ptHokenPatterns = ptHokenPatterns.Where(x => !expHokKbns.Contains(x.HokenKbn));

            var odrInfs = NoTrackingDataContext.OdrInfs.Where(x => x.HpId == hpId && x.IsDeleted == DeleteStatus.None && x.SanteiKbn == 0);
            #region 条件
            //期間
            odrInfs = odrInfs.Where(x => printConf.FromYm * 100 <= x.SinDate && x.SinDate <= printConf.ToYm * 100 + 31);

            //診療識別
            var odrKouiKbnExpression = CreateOdrKouiKbnExpression(printConf.SinryoSbt);

            var odrInfList = odrInfs.ToList();
            if (odrKouiKbnExpression != null)
            {
                odrInfList = odrInfList.AsQueryable().Where(odrKouiKbnExpression).ToList();
            }
            #endregion

            var odrInfDetails = _tenantOdrInfDetail.OdrInfDetails;
            var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.DrugKbn > 0);
            var yakkaSyusaiMsts = _tenantYakkaSyusaiMst.YakkaSyusaiMsts;
            var drugUnitConvs = _tenantDrugUnitConv.DrugUnitConvs;

            List<long> ptIdList = new();
            List<int> hokenPidList = new();
            List<long> raiinNoList = new();
            List<long> rpNoList = new();
            List<long> rpEdaNoList = new();

            Task taskId1 = Task.Factory.StartNew(() => ptIdList = odrInfList.Select(item => item.PtId).Distinct().ToList());
            Task taskId2 = Task.Factory.StartNew(() => hokenPidList = odrInfList.Select(item => item.HokenPid).Distinct().ToList());
            Task taskId3 = Task.Factory.StartNew(() => raiinNoList = odrInfList.Select(item => item.RaiinNo).Distinct().ToList());
            Task taskId4 = Task.Factory.StartNew(() => rpNoList = odrInfList.Select(item => item.RpNo).Distinct().ToList());
            Task taskId5 = Task.Factory.StartNew(() => rpEdaNoList = odrInfList.Select(item => item.RpEdaNo).Distinct().ToList());
            Task.WaitAll(taskId1, taskId2, taskId3, taskId4, taskId5);

            List<PtHokenPattern> ptHokenPatternList = new();
            List<PtInf> ptInfList = new();
            List<OdrInfDetail> odrInfDetailList = new();
            List<YakkaSyusaiMst> yakkaSyusaiMstList = new();
            List<DrugUnitConv> drugUnitConvList = new();

            Task taskList1 = Task.Factory.StartNew(() => ptHokenPatternList = ptHokenPatterns.Where(item => item.HpId == hpId && ptIdList.Contains(item.PtId) && hokenPidList.Contains(item.HokenPid)).ToList());
            Task taskList2 = Task.Factory.StartNew(() => ptInfList = ptInfs.Where(item => item.HpId == hpId && ptIdList.Contains(item.PtId)).ToList());
            Task taskList3 = Task.Factory.StartNew(() => odrInfDetailList = odrInfDetails.Where(item => item.HpId == hpId && ptIdList.Contains(item.PtId) && raiinNoList.Contains(item.RaiinNo) && rpNoList.Contains(item.RpNo) && rpEdaNoList.Contains(item.RpEdaNo)).ToList());
            Task.WaitAll(taskList1, taskList2, taskList3);

            var itemCdList = odrInfDetailList.Select(item => item.ItemCd).Distinct().ToList();
            var tenMstList = tenMsts.Where(item => item.HpId == hpId && itemCdList.Contains(item.ItemCd)).ToList();
            var yakkaCdList = tenMstList.Select(item => item.YakkaCd).Distinct().ToList();

            Task taskList4 = Task.Factory.StartNew(() => yakkaSyusaiMstList = yakkaSyusaiMsts.Where(item => item.HpId == hpId && itemCdList.Contains(item.ItemCd) && yakkaCdList.Contains(item.YakkaCd)).ToList());
            Task taskList5 = Task.Factory.StartNew(() => drugUnitConvList = drugUnitConvs.Where(item => itemCdList.Contains(item.ItemCd)).ToList());
            Task.WaitAll(taskList4, taskList5);

            var odrDrugJoinQuery = (
                from odrInf in odrInfList
                join ptHokenPattern in ptHokenPatternList on
                    new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join ptInf in ptInfList on
                    new { odrInf.HpId, odrInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join odrInfDetail in odrInfDetailList on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join tenMst in tenMstList on
                    new { odrInfDetail.HpId, odrInfDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                join yakkaSyusaiMst in yakkaSyusaiMstList on
                    new { tenMst.HpId, tenMst.ItemCd, tenMst.YakkaCd } equals
                    new { yakkaSyusaiMst.HpId, yakkaSyusaiMst.ItemCd, yakkaSyusaiMst.YakkaCd } into yakkaSyusaiJoins
                from yakkaSyusaiJoin in yakkaSyusaiJoins.DefaultIfEmpty()
                join drugUnitConv in drugUnitConvList on
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

            var odrDrugInfs = odrDrugJoinQuery.AsEnumerable().Select(
                data =>
                    new CoOdrDrugInf(data.odrInf, data.odrInfDetail, data.tenMst, data.yakkaSyusaiJoin, data.drugUnitConvJoin)
            ).ToList();

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
            }
            ).ToList();

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return retData;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            _tenantPtInf.Dispose();
            _tenantPtHokenPattern.Dispose();
            _tenantDrugUnitConv.Dispose();
            _tenantOdrInfDetail.Dispose();
            _tenantYakkaSyusaiMst.Dispose();
        }
    }


    /// <summary>
    /// 診療識別のOR条件を作成する
    /// </summary>
    /// <param name="listSinryoSbts">診療識別リスト</param>
    /// <returns></returns>
    private Expression<Func<OdrInf, bool>> CreateOdrKouiKbnExpression(List<int> listSinryoSbts)
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

            expression = expression == null ? expressionOdrInf : Expression.Or(expression, expressionOdrInf);
        }

        return expression != null
            ? Expression.Lambda<Func<OdrInf, bool>>(body: expression, parameters: param)
            : null;
    }
}
