using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services;

public class KinkiOTCCheckerTest : BaseUT
{
    /// <summary>
    /// Test KinkiOTCChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void KinkiOTCChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
    {
        int hpId = 1;
        //setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiOTC, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiOTCChecker = new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiOTCChecker.HpID = 1;
        kinkiOTCChecker.PtID = 111;
        kinkiOTCChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(kinkiOTCChecker.HpID, new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiOTCChecker.InitFinder(TenantProvider, cache);

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

        if (systemConf != null) systemConf.Val = temp;
        tenantTracking.SaveChanges();

        // Act
        var result = kinkiOTCChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
        // Assert
        Assert.True(result.ErrorOrderList.Count == 0);
    }

    [Test]
    public void KinkiOTCChecker_002_KinkiOTC()
    {
        int hpId = 1;
        //Setup
        //KinkiLevelSetting
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        int settingLevel = 4;
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
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = settingLevel
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        var prOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
        tenantTracking.PtOtcDrug.AddRange(prOtcDrugs);
        var m38Ingredients = CommonCheckerData.ReadM38Ingredients(hpId, "");
        tenantTracking.M38Ingredients.AddRange(m38Ingredients);
        tenantTracking.SaveChanges();

        //setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiOTC, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var kinkiOTCChecker = new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>();

        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "620160501" }, 20230101, 1231);
        kinkiOTCChecker.HpID = hpId;
        kinkiOTCChecker.PtID = 1231;
        kinkiOTCChecker.Sinday = 20230404;
        kinkiOTCChecker.InitFinder(TenantProvider, cache);

        try
        {
            ///Act
            var result = kinkiOTCChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            ///Assert
            Assert.True(result.IsError && result.CheckerType == RealtimeCheckerType.KinkiOTC);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.PtOtcDrug.RemoveRange(prOtcDrugs);
            tenantTracking.M38Ingredients.RemoveRange(m38Ingredients);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void KinkiOTCChecker_003_HandleCheckOrder_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

        // Arrange
        var kinkiOTC = new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiOTC, odrInfoModel, 20230101, 111);

        // Act and Assert
        Assert.Throws<NotImplementedException>(() => kinkiOTC.HandleCheckOrder(unitChecker));
    }

    [Test]
    public void KinkiOTCChecker_004_KinkiOTC_Test_SettingLevel_Is_0()
    {
        int hpId = 1;
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        int settingLevel = 0;
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
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = settingLevel
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        var prOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
        tenantTracking.PtOtcDrug.AddRange(prOtcDrugs);
        var m38Ingredients = CommonCheckerData.ReadM38Ingredients(hpId, "");
        tenantTracking.M38Ingredients.AddRange(m38Ingredients);
        tenantTracking.SaveChanges();

        //setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiOTC, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var kinkiOTCChecker = new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>();

        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(999, new List<string>() { "620160501" }, 20230101, 1231);
        kinkiOTCChecker.HpID = 999;
        kinkiOTCChecker.PtID = 1231;
        kinkiOTCChecker.Sinday = 20230404;
        kinkiOTCChecker.InitFinder(TenantProvider, cache);

        try
        {
            ///Act
            var result = kinkiOTCChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            ///Assert
            Assert.False(result.IsError);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.PtOtcDrug.RemoveRange(prOtcDrugs);
            tenantTracking.M38Ingredients.RemoveRange(m38Ingredients);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void KinkiOTCChecker_005_KinkiOTC_Test_SettingLevel_Is_5()
    {
        int hpId = 1;
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        int settingLevel = 5;
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
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = settingLevel
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        var prOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
        tenantTracking.PtOtcDrug.AddRange(prOtcDrugs);
        var m38Ingredients = CommonCheckerData.ReadM38Ingredients(hpId, "");
        tenantTracking.M38Ingredients.AddRange(m38Ingredients);
        tenantTracking.SaveChanges();

        //setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiOTC, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var kinkiOTCChecker = new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>();

        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(999, new List<string>() { "620160501" }, 20230101, 1231);
        kinkiOTCChecker.HpID = 999;
        kinkiOTCChecker.PtID = 1231;
        kinkiOTCChecker.Sinday = 20230404;
        kinkiOTCChecker.InitFinder(TenantProvider, cache);

        try
        {
            ///Act
            var result = kinkiOTCChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            ///Assert
            Assert.False(result.IsError);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.PtOtcDrug.RemoveRange(prOtcDrugs);
            tenantTracking.M38Ingredients.RemoveRange(m38Ingredients);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }
}
