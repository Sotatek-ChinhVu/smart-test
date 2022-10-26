﻿using EmrCalculateApi.Futan.DB.Finder;
using EmrCalculateApi.Futan.Models;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using PostgreDataContext;

namespace EmrCalculateApi.Implementation.Finder
{
    public class MasterFinder : IMasterFinder
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;

        public MasterFinder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }

        public IpnNameMstModel FindIpnNameMst(string ipnNameCd, int sinDate)
        {
            var entity = _tenantNoTrackingDataContext.IpnNameMsts.Where(p =>
                   p.HpId == TempIdentity.HpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.IpnNameCd == ipnNameCd)
               .FirstOrDefault();

            if (entity != null)
            {
                return new IpnNameMstModel(entity);
            }
            return null;
        }

        public SanteiCntCheckModel FindSanteiCntCheck(int santeiGrpCd, int sinDate)
        {
            var entity = _tenantNoTrackingDataContext.SanteiCntChecks.Where(e =>
                 e.HpId == TempIdentity.HpId &&
                 e.SanteiGrpCd == santeiGrpCd &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate)
                 .FirstOrDefault();

            if (entity != null)
            {
                return new SanteiCntCheckModel(entity);
            }
            return null;
        }

        public SanteiGrpDetailModel FindSanteiGrpDetail(string itemCd)
        {
            var entity = _tenantNoTrackingDataContext.SanteiGrpDetails.Where(e =>
                 e.HpId == TempIdentity.HpId &&
                 e.ItemCd == itemCd)
                 .FirstOrDefault();

            if (entity != null)
            {
                return new SanteiGrpDetailModel(entity);
            }
            return null;
        }

        public TenMstModel FindTenMst(string itemCd, int sinDate)
        {
            var entity = _tenantNoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == TempIdentity.HpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd)
               .FirstOrDefault();

            if (entity != null)
            {
                return new TenMstModel(entity);
            }
            return null;
        }

        public double GetOdrCountInMonth(long ptId, int sinDate, string itemCd)
        {
            int firstDayOfSinDate = sinDate / 100 * 100 + 1;
            DateTime firstDaySinDateDateTime = CIUtil.IntToDate(firstDayOfSinDate);
            DateTime lastDayOfPrevMonthDateTime = firstDaySinDateDateTime.AddDays(-1);
            int lastDayOfPrevMonth = CIUtil.DateTimeToInt(lastDayOfPrevMonthDateTime);

            var odrInfQuery = _tenantNoTrackingDataContext.OdrInfs
               .Where(odr => odr.PtId == ptId && odr.SinDate > lastDayOfPrevMonth && odr.SinDate <= sinDate && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0);
            var odrInfDetailQuery = _tenantNoTrackingDataContext.OdrInfDetails
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
            return allDetail.Sum(d => (d.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(d.ItemCd)) ? 1 : d.Suryo);
        }
    }
}
