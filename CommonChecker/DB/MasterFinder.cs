using CommonChecker.Models.Futan;
using CommonChecker.Models.MstItem;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public class MasterFinder : RepositoryBase, IMasterFinder
    {

        public MasterFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IpnNameMstModel FindIpnNameMst(int hpId, string ipnNameCd, int sinDate)
        {
            var entity = NoTrackingDataContext.IpnNameMsts.Where(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.IpnNameCd == ipnNameCd)
               .FirstOrDefault();
            return new IpnNameMstModel(entity ?? new IpnNameMst());

        }

        public SanteiCntCheckModel FindSanteiCntCheck(int hpId, int santeiGrpCd, int sinDate)
        {
            var entity = NoTrackingDataContext.SanteiCntChecks.Where(e =>
                 e.HpId == hpId &&
                 e.SanteiGrpCd == santeiGrpCd &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate)
                 .FirstOrDefault();
            return new SanteiCntCheckModel(entity ?? new SanteiCntCheck());

        }

        public SanteiGrpDetailModel FindSanteiGrpDetail(int hpId, string itemCd)
        {
            var entity = NoTrackingDataContext.SanteiGrpDetails.Where(e =>
                 e.HpId == hpId &&
                 e.ItemCd == itemCd)
                 .FirstOrDefault();

            return new SanteiGrpDetailModel(entity ?? new SanteiGrpDetail());

        }

        public TenMstModel FindTenMst(int hpId, string itemCd, int sinDate)
        {
            var entity = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd)
               .FirstOrDefault();

            return new TenMstModel(entity ?? new TenMst());

        }

        public double GetOdrCountInMonth(long ptId, int sinDate, string itemCd)
        {
            int firstDayOfSinDate = sinDate / 100 * 100 + 1;
            DateTime firstDaySinDateDateTime = CIUtil.IntToDate(firstDayOfSinDate);
            DateTime lastDayOfPrevMonthDateTime = firstDaySinDateDateTime.AddDays(-1);
            int lastDayOfPrevMonth = CIUtil.DateTimeToInt(lastDayOfPrevMonthDateTime);

            var odrInfQuery = NoTrackingDataContext.OdrInfs
               .Where(odr => odr.PtId == ptId && odr.SinDate > lastDayOfPrevMonth && odr.SinDate <= sinDate && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0);
            var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails
              .Where(odrDetail => odrDetail.PtId == ptId
              && odrDetail.SinDate > lastDayOfPrevMonth
              && odrDetail.SinDate <= sinDate
              && odrDetail.ItemCd == itemCd);

            var odrJoinDetail = from odrInf in odrInfQuery
                                join odrDetail in odrInfDetailQuery
                                on new { odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                                equals new { odrDetail.PtId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
                                into ListDetail
                                select new
                                {
                                    OdrInf = odrInf,
                                    OdrDetail = ListDetail
                                };
            var allDetailList = odrJoinDetail.AsEnumerable().Select(d => d.OdrDetail).ToList();
            var allDetail = new List<OdrInfDetail>();
            foreach (var detailList in allDetailList)
            {
                allDetail.AddRange(detailList);
            }
            return allDetail.Sum(d => (d.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(d.ItemCd ?? string.Empty)) ? 1 : d.Suryo);
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
