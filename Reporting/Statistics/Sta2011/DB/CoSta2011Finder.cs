using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.DB;
using Reporting.Statistics.Sta2010.Models;
using Reporting.Statistics.Sta2011.Models;

namespace Reporting.Statistics.Sta2011.DB
{
    public class CoSta2011Finder : RepositoryBase, ICoSta2011Finder
    {
        private readonly ICoHpInfFinder _hpInfFinder;
        private readonly ICoSta2010Finder _sta2010Finder;

        public CoSta2011Finder(ICoHpInfFinder hpInfFinder, ICoSta2010Finder sta2010Finder, ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
            _sta2010Finder = sta2010Finder;
        }

        public void ReleaseResource()
        {
            _hpInfFinder.ReleaseResource();
            _sta2010Finder.ReleaseResource();
            DisposeDataContext();
        }

        public CoHpInfModel GetHpInf(int hpId, int sinDate)
        {
            return _hpInfFinder.GetHpInf(hpId, sinDate);
        }

        private CoSta2010PrintConf convertPrintConf(CoSta2011PrintConf printConf)
        {
            return
                new CoSta2010PrintConf(printConf.MenuId)
                {
                    SeikyuYm = printConf.SeikyuYm,
                    KaIds = printConf.KaIds,
                    TantoIds = printConf.TantoIds,
                    SeikyuTypes = printConf.SeikyuTypes,
                    IsTester = printConf.IsTester
                };
        }

        /// <summary>
        /// レセプト情報の取得
        /// </summary>
        /// <param name="printConf"></param>
        /// <param name="prefNo"></param>
        /// <returns></returns>
        public List<CoReceInfModel> GetReceInfs(int hpId, CoSta2011PrintConf printConf, int prefNo)
        {
            CoSta2010PrintConf wrkPrintConf = convertPrintConf(printConf);
            return _sta2010Finder.GetReceInfs(hpId, wrkPrintConf, prefNo);
        }

        /// <summary>
        /// 在宅患者の一覧を取得
        /// </summary>
        /// <param name="printConf"></param>
        /// <returns></returns>
        public List<CoZaitakuModel> GetZaitakuReces(int hpId, CoSta2011PrintConf printConf)
        {
            if (printConf.ZaitakuItems == null) return new();

            var receInfs = NoTrackingDataContext.ReceInfs.Where(x => x.HpId == hpId);
            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId);
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o => o.HpId == hpId && o.IsDeleted == DeleteStatus.None);
            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(d => d.HpId == hpId && d.ItemCd != null && printConf.ZaitakuItems.Contains(d.ItemCd));

            var receHokens = (
                from r in receInfs
                join p in ptHokenPatterns on
                    new { r.HpId, r.PtId, r.HokenId } equals
                    new { p.HpId, p.PtId, p.HokenId }
                where
                    r.HpId == hpId &&
                    r.SeikyuYm == printConf.SeikyuYm
                select new
                {
                    r.HpId,
                    r.SeikyuYm,
                    r.PtId,
                    r.SinYm,
                    r.HokenId,
                    p.HokenPid,
                }
            );

            var retQuery = (
                from r in receHokens
                join o in odrInfs on
                    new { r.HpId, r.PtId, r.SinYm, r.HokenPid } equals
                    new { o.HpId, o.PtId, SinYm = o.SinDate / 100, o.HokenPid }
                join d in odrInfDetails on
                    new { o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo } equals
                    new { d.HpId, d.RaiinNo, d.RpNo, d.RpEdaNo }
                group new { r.SeikyuYm, r.PtId, r.SinYm, r.HokenId } by
                    new { r.SeikyuYm, r.PtId, r.SinYm, r.HokenId } into rg
                select new
                {
                    rg.Key
                }
            );

            var result = retQuery.AsEnumerable().Select(
                r => new CoZaitakuModel
                (
                    seikyuYm: r.Key.SeikyuYm,
                    ptId: r.Key.PtId,
                    sinYm: r.Key.SinYm,
                    hokenId: r.Key.HokenId
                )
            ).ToList();

            return result;
        }
    }
}
