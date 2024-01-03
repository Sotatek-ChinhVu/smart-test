using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3001.Models;

namespace Reporting.Statistics.Sta3001.DB
{
    public class CoSta3001Finder : RepositoryBase, ICoSta3001Finder
    {
        private ICoHpInfFinder _hpInfFinder;

        public CoSta3001Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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
        /// 採用薬情報取得
        /// </summary>
        /// <param name="printConf"></param>
        /// <returns></returns>
        public List<CoAdpDrugsModel> GetAdpDrugs(int hpId, CoSta3001PrintConf printConf)
        {
            var tenMsts = NoTrackingDataContext.TenMsts.Where(p =>
                p.HpId == hpId &&
                p.DrugKbn > 0);

            //基準日に採用されている薬
            var tgtTenMsts = tenMsts
                .Where(x => x.IsAdopted == 1)
                .Where(x => printConf.StdDate >= x.StartDate)
                .Where(x => printConf.StdDate <= x.EndDate);

            #region 検索条件
            if (printConf.DrugKbns?.Count >= 1)
            {
                tgtTenMsts = tgtTenMsts.Where(x => printConf.DrugKbns.Contains(x.DrugKbn));
            }
            if (printConf.MadokuKbns?.Count >= 1)
            {
                tgtTenMsts = tgtTenMsts.Where(x => printConf.MadokuKbns.Contains(x.MadokuKbn));
            }
            if (printConf.KouseisinKbns?.Count >= 1)
            {
                tgtTenMsts = tgtTenMsts.Where(x => printConf.KouseisinKbns.Contains(x.KouseisinKbn));
            }
            if (printConf.KohatuKbns?.Count >= 1)
            {
                tgtTenMsts = tgtTenMsts.Where(x => printConf.KohatuKbns.Contains(x.KohatuKbn));
            }
            #endregion

            //最新世代の項目を取得する
            var grpTenMsts = tenMsts
                .GroupBy(x => new { x.HpId, x.ItemCd })
                .Select(x => new
                {
                    x.Key.HpId,
                    x.Key.ItemCd,
                    MaxStartDate = x.Max(d => d.StartDate),
                    MinStartDate = x.Min(d => d.StartDate),
                    MaxEndDate = x.Max(d => d.EndDate)
                });

            #region 期間
            if (printConf.StartDateFrom >= 0)
            {
                grpTenMsts = grpTenMsts.Where(x => printConf.StartDateFrom <= x.MinStartDate);
            }
            if (printConf.StartDateTo >= 0)
            {
                grpTenMsts = grpTenMsts.Where(x => printConf.StartDateTo >= x.MinStartDate);
            }
            if (printConf.EndDateFrom >= 0)
            {
                grpTenMsts = grpTenMsts.Where(x => printConf.EndDateFrom <= x.MaxEndDate);
            }
            if (printConf.EndDateTo >= 0)
            {
                grpTenMsts = grpTenMsts.Where(x => printConf.EndDateTo >= x.MaxEndDate);
            }
            #endregion

            var latestTenMsts = (
                from tenMst in tenMsts
                join tgtTenMst in tgtTenMsts on
                    new { tenMst.HpId, tenMst.ItemCd } equals
                    new { tgtTenMst.HpId, tgtTenMst.ItemCd }
                join grpTenMst in grpTenMsts on
                    new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                    new { grpTenMst.HpId, grpTenMst.ItemCd, StartDate = grpTenMst.MaxStartDate }
                select new
                {
                    tenMst.HpId,
                    tenMst.ItemCd,
                    tenMst.Name,
                    tenMst.KanaName1,
                    tenMst.KanaName2,
                    tenMst.KanaName3,
                    tenMst.KanaName4,
                    tenMst.KanaName5,
                    tenMst.KanaName6,
                    tenMst.KanaName7,
                    tenMst.DefaultVal,
                    tenMst.OdrUnitName,
                    tenMst.Ten,
                    tenMst.ReceUnitName,
                    grpTenMst.MinStartDate,
                    grpTenMst.MaxEndDate,
                    tenMst.DrugKbn,
                    tenMst.MadokuKbn,
                    tenMst.KouseisinKbn,
                    tenMst.KohatuKbn,
                    tenMst.ReceName,
                    tenMst.YjCd,
                    tenMst.IpnNameCd
                }
                );

            var ipnNameMsts = NoTrackingDataContext.IpnNameMsts.Where(p => p.HpId == hpId && p.IpnName != null);

            //最新世代の一般名を取得する
            var maxIpnNames = ipnNameMsts.GroupBy(x => new { x.HpId, x.IpnNameCd })
                .Select(x => new
                {
                    x.Key.HpId,
                    x.Key.IpnNameCd,
                    StartDate = x.Max(d => d.StartDate)
                }
                );

            var latestIpnNames = (
                from ipnNameMst in ipnNameMsts
                join maxIpnName in maxIpnNames on
                    new { ipnNameMst.HpId, ipnNameMst.IpnNameCd, ipnNameMst.StartDate } equals
                    new { maxIpnName.HpId, maxIpnName.IpnNameCd, maxIpnName.StartDate }
                select new
                {
                    HpId = ipnNameMst.HpId == null ? 0 : ipnNameMst.HpId,
                    IpnNameCd = ipnNameMst.IpnNameCd == null ? string.Empty : ipnNameMst.IpnNameCd,
                    IpnName = ipnNameMst.IpnName == null ? string.Empty : ipnNameMst.IpnName
                }
                );

            //一般名があれば、採用薬情報に外部結合する
            var joinQuery = (
                            from latestTenMst in latestTenMsts
                            join latestIpnName in latestIpnNames on
                                new { latestTenMst.HpId, latestTenMst.IpnNameCd } equals
                                new { latestIpnName.HpId, latestIpnName.IpnNameCd } into ipnNameMstJoins
                            from ipnNameMstJoin in ipnNameMstJoins.DefaultIfEmpty()
                            select new { latestTenMst, ipnNameMstJoin }
                            ).ToList();

            var retData = joinQuery.AsEnumerable().Select(data => new CoAdpDrugsModel()
            {
                ItemCd = data.latestTenMst.ItemCd,
                Name = data.latestTenMst.Name,
                KanaName1 = data.latestTenMst.KanaName1,
                KanaName2 = data.latestTenMst.KanaName2,
                KanaName3 = data.latestTenMst.KanaName3,
                KanaName4 = data.latestTenMst.KanaName4,
                KanaName5 = data.latestTenMst.KanaName5,
                KanaName6 = data.latestTenMst.KanaName6,
                KanaName7 = data.latestTenMst.KanaName7,
                DefaultVal = data.latestTenMst.DefaultVal,
                OdrUnitName = data.latestTenMst.OdrUnitName,
                Price = data.latestTenMst.Ten,
                ReceUnitName = data.latestTenMst.ReceUnitName,
                StartDate = data.latestTenMst.MinStartDate,
                EndDate = data.latestTenMst.MaxEndDate,
                DrugKbn = data.latestTenMst.DrugKbn,
                MadokuKbn = data.latestTenMst.MadokuKbn,
                KouseisinKbn = data.latestTenMst.KouseisinKbn,
                KohatuKbn = data.latestTenMst.KohatuKbn,
                IpnName = data.ipnNameMstJoin?.IpnName ?? "",
                ReceName = data.latestTenMst.ReceName,
                YjCd = data.latestTenMst.YjCd
            }
            ).ToList();

            return retData;
        }
    }
}
