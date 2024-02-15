using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

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
        var ptId = 1231;
        var hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;
        var settingLevel = 4;
        var sinDay = 20230101;
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        var addedItemCodes = new List<ItemCodeModel>()
        {
            new("6220816T1", "id1")
        };

        //Setup M01_KINKI
        var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.HpId == hpId && p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
        var m01Kinki = new M01Kinki();
        if (m01 == null)
        {
            m01Kinki.HpId = hpId;
            m01Kinki.ACd = "1190700";
            m01Kinki.BCd = "1190700";
            m01Kinki.CmtCd = "D006";
            m01Kinki.SayokijyoCd = "S2001";
            m01Kinki.KyodoCd = "";
            m01Kinki.Kyodo = "3";
            m01Kinki.DataKbn = "1";

            tenantTracking.M01Kinki.Add(m01Kinki);
        }
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, new(), true);

            //Assert
            Assert.True(result.Count == 1);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            if (m01 == null)
            {
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            }
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
        cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, new(), true);

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
        int hpId = 1;
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //Setup KinkiLevelSetting 
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 4;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2027,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 4
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        //Setup M01_KINKI
        var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.HpId == 1 && p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
        var m01Kinki = new M01Kinki();
        if (m01 == null)
        {
            m01Kinki.HpId = 1;
            m01Kinki.ACd = "1190700";
            m01Kinki.BCd = "1190700";
            m01Kinki.CmtCd = "D006";
            m01Kinki.SayokijyoCd = "S2001";
            m01Kinki.KyodoCd = "";
            m01Kinki.Kyodo = "3";
            m01Kinki.DataKbn = "1";

            tenantTracking.M01Kinki.Add(m01Kinki);
        }

        tenantTracking.SaveChanges();

        int ptId = 1233;
        var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        tenantTracking.SaveChanges();

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
                                                RealtimeCheckerType.KinkiTain, odrInfoModel, 20230101, ptId, new(new(), new(), new()), new(), new(), true);

        var kinkiTainChecker = new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiTainChecker.HpID = 999;
        kinkiTainChecker.PtID = 1233;
        kinkiTainChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(kinkiTainChecker.HpID, new List<string>() { "6220816T3" }, 20230505, ptId);
        kinkiTainChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = kinkiTainChecker.HandleCheckOrderList(unitCheckerResult);

            //Assert
            Assert.True(result.IsError && result.CheckingOrderList[0].OrdInfDetails[0].ItemCd == "6220816T3" && result.CheckerType == RealtimeCheckerType.KinkiTain);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            if (m01 == null)
            {
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            }
            tenantTracking.SaveChanges();
        }
    }


    [Test]
    public void Test_003_HandleCheckOrder_KinkiTainCheck_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

        // Arrange
        var kinkiTain = new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiTain, odrInfoModel, 20230101, 111);

        // Act and Assert
        Assert.Throws<NotImplementedException>(() => kinkiTain.HandleCheckOrder(unitChecker));
    }

    /// <summary>
    /// setting level = 0
    /// </summary>
    [Test]
    public void Test_005_HandleCheckOrderList_KinkiTainCheck_Test_Setting_0()
    {
        int hpId = 1;
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //Setup KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 0;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2027,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        //Setup M01_KINKI
        var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.HpId == 1 && p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
        var m01Kinki = new M01Kinki();
        if (m01 == null)
        {
            m01Kinki.HpId = 1;
            m01Kinki.ACd = "1190700";
            m01Kinki.BCd = "1190700";
            m01Kinki.CmtCd = "D006";
            m01Kinki.SayokijyoCd = "S2001";
            m01Kinki.KyodoCd = "";
            m01Kinki.Kyodo = "3";
            m01Kinki.DataKbn = "1";

            tenantTracking.M01Kinki.Add(m01Kinki);
        }

        tenantTracking.SaveChanges();

        int ptId = 1233;
        var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        tenantTracking.SaveChanges();

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
                                                RealtimeCheckerType.KinkiTain, odrInfoModel, 20230101, ptId, new(new(), new(), new()), new(), new(), true);

        var kinkiTainChecker = new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiTainChecker.HpID = 999;
        kinkiTainChecker.PtID = 1233;
        kinkiTainChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(kinkiTainChecker.HpID, new List<string>() { "6220816T3" }, 20230505, ptId);
        kinkiTainChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = kinkiTainChecker.HandleCheckOrderList(unitCheckerResult);

            //Assert
            Assert.False(result.IsError && result.CheckingOrderList[0].OrdInfDetails[0].ItemCd == "6220816T3" && result.CheckerType == RealtimeCheckerType.KinkiTain);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            if (m01 == null)
            {
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            }
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// setting level = 6
    /// </summary>
    [Test]
    public void Test_006_HandleCheckOrderList_KinkiTainCheck_Test_Setting_6()
    {
        int hpId = 1;
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //Setup KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 5;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2027,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 5
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        //Setup M01_KINKI
        var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.HpId == 1 && p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
        var m01Kinki = new M01Kinki();
        if (m01 == null)
        {
            m01Kinki.HpId = 1;
            m01Kinki.ACd = "1190700";
            m01Kinki.BCd = "1190700";
            m01Kinki.CmtCd = "D006";
            m01Kinki.SayokijyoCd = "S2001";
            m01Kinki.KyodoCd = "";
            m01Kinki.Kyodo = "3";
            m01Kinki.DataKbn = "1";

            tenantTracking.M01Kinki.Add(m01Kinki);
        }

        tenantTracking.SaveChanges();

        int ptId = 1233;
        var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
        var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
        tenantTracking.SaveChanges();

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
                                                RealtimeCheckerType.KinkiTain, odrInfoModel, 20230101, ptId, new(new(), new(), new()), new(), new(), true);

        var kinkiTainChecker = new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiTainChecker.HpID = 999;
        kinkiTainChecker.PtID = 1233;
        kinkiTainChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(kinkiTainChecker.HpID, new List<string>() { "6220816T3" }, 20230505, ptId);
        kinkiTainChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = kinkiTainChecker.HandleCheckOrderList(unitCheckerResult);

            //Assert
            Assert.False(result.IsError && result.CheckingOrderList[0].OrdInfDetails[0].ItemCd == "6220816T3" && result.CheckerType == RealtimeCheckerType.KinkiTain);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
            if (m01 == null)
            {
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            }
            tenantTracking.SaveChanges();
        }
    }
}