using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.CommonChecker.Finder
{
    public class MasterFinderTest : BaseUT
    {
        [Test]
        public void TC_001_TEST_FindIpnNameMst()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 9999;
            var sinDate = 20230101;
            var ipnNameMsts = new List<IpnNameMst>()
            {
                new IpnNameMst() 
                {
                    StartDate = 20221231,
                    EndDate = 20230102,
                    SeqNo = 999,
                    IpnNameCd = "UT2604"
                },
            };

            tenantTracking.IpnNameMsts.AddRange(ipnNameMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new MasterFinder(TenantProvider.GetNoTrackingDataContext());

            try
            {
                // Act
                var result = realtimcheckerfinder.FindIpnNameMst(hpId, "UT2604", sinDate);

                // Assert
                Assert.True(result.IpnNameMst.IpnNameCd == "UT2604" && result.IpnNameMst.StartDate == 20221231 && result.IpnNameMst.EndDate == 20230102);
            }
            finally
            {
                tenantTracking.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_002_TEST_FindSanteiCntCheck()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 9999;
            var sinDate = 20230101;
            var santeiCntChecks = new List<SanteiCntCheck>()
            {
                new SanteiCntCheck()
                {
                    HpId = hpId,
                    StartDate = 20221231,
                    EndDate = 20230102,
                    SeqNo = 9999,
                    SanteiGrpCd = 1205
                },
            };

            tenantTracking.SanteiCntChecks.AddRange(santeiCntChecks);
            tenantTracking.SaveChanges();

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            var realtimcheckerfinder = new MasterFinder(TenantProvider.GetNoTrackingDataContext());

            try
            {
                // Act
                var result = realtimcheckerfinder.FindSanteiCntCheck(hpId, 1205, sinDate);

                // Assert
                Assert.True(result.SanteiCntCheck.SanteiGrpCd == 1205 && result.SanteiCntCheck.StartDate == 20221231 && result.SanteiCntCheck.EndDate == 20230102);
            }
            finally
            {
                tenantTracking.SanteiCntChecks.RemoveRange(santeiCntChecks);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_003_TEST_FindSanteiGrpDetail()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 9999;
            var santeiGrps = new List<SanteiGrpDetail>()
            {
                new SanteiGrpDetail()
                {
                    HpId = hpId,
                    ItemCd = "UT-0426",
                    SanteiGrpCd = 1205
                },
            };

            tenantTracking.SanteiGrpDetails.AddRange(santeiGrps);
            tenantTracking.SaveChanges();

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            var realtimcheckerfinder = new MasterFinder(TenantProvider.GetNoTrackingDataContext());

            try
            {
                // Act
                var result = realtimcheckerfinder.FindSanteiGrpDetail(hpId, "UT-0426");

                // Assert
                Assert.True(result.SanteiGrpDetail.SanteiGrpCd == 1205 && result.SanteiGrpDetail.ItemCd == "UT-0426");
            }
            finally
            {
                tenantTracking.SanteiGrpDetails.RemoveRange(santeiGrps);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_004_TEST_FindTenMst()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new MasterFinder(TenantProvider.GetNoTrackingDataContext());

            try
            {
                // Act
                var result = realtimcheckerfinder.FindTenMst(1, "UT2720", 20230101);

                // Assert
                Assert.True(result.TenMst.ItemCd == "UT2720" && result.TenMst.YjCd == "UT271026");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }

        }

    }
}
