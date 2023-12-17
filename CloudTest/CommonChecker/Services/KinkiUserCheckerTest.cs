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

public class KinkiUserCheckerTest : BaseUT
{
    /// <summary>
    /// Test KinkiUserChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void Test_001_KinkiUserCheckerTest_WhenCurrentOrderCodeConstantAcdAndCheckingOrderCodeConstantBcd()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 3;
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
                Val = 3
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_002_KinkiUserCheckerTest_WhenCurrentOrderCodeConstantBcdAndCheckingOrderCodeConstantAcd()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);

        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_003_KinkiUserCheckerTest_WhenSettingLevelIsFalse()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        int settingLevel = 6;
        if (systemConf != null)
        {
            systemConf.Val = settingLevel;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2027,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = settingLevel
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);

        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);
            systemConf.Val = temp;

            //// Assert
            Assert.True(!result.IsError);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_004_CheckKinkiUser_Finder_CurrentCodeConstantACd_AddedCodeConstantBCd()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

        var hpId = 999;
        var ptId = 1231;
        var settingLevel = 4;
        var sinDay = 20230101;
        var listCurrentOrderCode = new List<ItemCodeModel>()
        {
            new("6111K01", "id1")
        };

        var listAddedOrderCode = new List<ItemCodeModel>()
        {
            new("6404K01", "id1")
        };

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);
        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckKinkiUser(hpId, settingLevel, sinDay, listCurrentOrderCode, listAddedOrderCode);

            //Assert
            Assert.True(result.Count == 1);
        }
        finally
        {
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckKinkiUser_005_HandleCheckOrderList_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
            {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

        var odrInfoModel = new List<OrdInfoModel>()
            {
            new OrdInfoModel(21, 0, ordInfDetails)
            };

        // Arrange
        var kinkiUser = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                 RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);


        // Act and Assert
        Assert.Throws<NotImplementedException>(() => kinkiUser.HandleCheckOrderList(unitChecker));
    }

    /// <summary>
    /// settingLevel = 0
    /// </summary>
    public void Test_006_KinkiUserCheckerTest_SettingLevel_0()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

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
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(!result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// settingLevel = 5
    /// </summary>
    public void Test_007_KinkiUserCheckerTest_SettingLevel_5()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

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
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(!result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }


    /// <summary>
    /// Test KinkiUserChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void Test_008_KinkiUserCheckerTest_AYjCd_Differrent_BYjCd()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K08", "K08");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K08");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 3;
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
                Val = 3
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "6404K08", "6111K08" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K08",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K08",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// settingLevel = -1
    /// </summary>
    [Test]
    public void Test_009_KinkiUserCheckerTest_SettingLevel_Less_Than_0()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = -1;
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
                Val = -1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(!result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// settingLevel = 6
    /// </summary>
    public void Test_009_KinkiUserCheckerTest_SettingLevel_Is_6()
    {
        ///Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        tenantTracking.SaveChanges();

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 6;
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
                Val = 6
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiUserChecker.HpID = 999;
        kinkiUserChecker.PtID = 111;
        kinkiUserChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiUserChecker.InitFinder(tenantNoTracking, cache);

        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K01",
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

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        kinkiUserChecker.CurrentListOrder = currentList;

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K01",
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
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);
        try
        {
            // Act
            var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(!result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
            tenantTracking.SaveChanges();
        }
    }
}
