using CloudUnitTest.SampleData;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;

namespace CloudUnitTest.CommonChecker.Services
{
    public class KinkiTainCheckerTest : BaseUT
    {
        [Test]
        public void Test_001_KinkiTainChecker()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("T1", "");
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.SaveChanges();

            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());
            var hpId = 999;
            var ptId = 1231;
            var settingLevel = 4;
            var sinDay = 20230101;
            var addedItemCodes = new List<ItemCodeModel>()
            {
                new("6220816T1", "id1")
            };

            ////Act
            var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, null, true);

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            tenantTracking.SaveChanges();

            ///Assert
            Assert.True(result.Count == 1);
        }
    }
}
