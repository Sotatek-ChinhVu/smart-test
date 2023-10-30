using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;

namespace CloudUnitTest.CommonChecker.Services;

public class KinkiTainCheckerTest : BaseUT
{
    /// <summary>
    /// Test KinkiTainChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void Test_001_Finder_CheckKinkiTain_WhenExistingOtherDrug_AndExistingM01Kinki()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("T1", "");
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug();
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        tenantTracking.SaveChanges();

        var hpId = 999;
        var ptId = 1231;
        var settingLevel = 4;
        var sinDay = 20230101;
        var addedItemCodes = new List<ItemCodeModel>()
        {
            new("6220816T1", "id1")
        };

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, null, true);

            //Assert
            Assert.True(result.Count == 1);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_002_Finder_CheckKinkiTain_WhenNotExistingOtherDrug()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("T2", "");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.SaveChanges();

        var hpId = 999;
        var ptId = 1231;
        var settingLevel = 4;
        var sinDay = 20230101;
        var addedItemCodes = new List<ItemCodeModel>()
        {
            new("6220816T2", "id1")
        };

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, null, true);

            //Assert
            Assert.True(!result.Any());
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_003_HandleCheckOrderList_KinkiTainCheck_WhenExisitingOtherDrug_ExistingM01Kinki()
    {
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug();
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        tenantTracking.SaveChanges();

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T1",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 3,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
                                    yohoKbn: 0,
                                    ipnCd: "3112004M1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id2",
                                    sinKouiKbn: 21,
                                    itemCd: "Y101",
                                    itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
                                    suryo: 1,
                                    unitName: "・・･・・・",
                                    termVal: 0,
                                    syohoKbn: 0,
                                    syohoLimitKbn: 0,
                                    drugKbn: 0,
                                    yohoKbn: 1,
                                    ipnCd: "",
                                    bunkatu: "",
                                    masterSbt: "",
                                    bunkatuKoui: 0),
        };
        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails)
        };


        var unitCheckerResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiTain, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var kinkiTainChecker = new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiTainChecker.HpID = 999;
        kinkiTainChecker.PtID = 1231;
        kinkiTainChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiTainChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = kinkiTainChecker.HandleCheckOrderList(unitCheckerResult);

            //Assert
            Assert.True(result.ErrorInfo != null && result.CheckingOrderList.Count == 1 && result.CheckingOrderList[0].OrdInfDetails[0].ItemCd == "6220816T3");
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            tenantTracking.SaveChanges();
        }
    }
}