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
using CalculateService.Extensions;

namespace CloudUnitTest.CommonChecker.Services;

public class KinkiSuppleCheckerTest : BaseUT
{
    /// <summary>
    /// Test KinkiSuppleChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void KinkiSuppleChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
    {
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
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiSuppleChecker.HpID = 1;
        kinkiSuppleChecker.PtID = 111;
        kinkiSuppleChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //KinkiLevelSetting
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
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);
        kinkiSuppleChecker.InitFinder(TenantProvider, cache);

        try
        {
            // Act
            var result = kinkiSuppleChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void KinkiSuppleChecker_002_HandleCheckOrder_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

        // Arrange
        var kinkiSupple = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111);

        // Act and Assert
        Assert.Throws<NotImplementedException>(() => kinkiSupple.HandleCheckOrder(unitChecker));
    }

    [Test]
    public void KinkiSuppleChecker_003_Test_ErrorSupple_SettingLevel_Is_4()
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
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiSuppleChecker.HpID = hpId;
        kinkiSuppleChecker.PtID = 13934;
        kinkiSuppleChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //setup SystemCof
        //KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 4;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = hpId,
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

        //Setup Data test
        var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
        var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
        var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.PtSupples.AddRange(ptSupples);
        tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
        tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "611170008" }, 20230101, 13934);
        kinkiSuppleChecker.InitFinder(TenantProvider, cache);

        try
        {
            // Act
            var result = kinkiSuppleChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count > 0 && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtSupples.RemoveRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Test KinkiSuppleChecker With Setting Value is 0
    /// </summary>
    [Test]
    public void KinkiSuppleChecker_004_Test_WhenSettingLevel_Is_0()
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
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiSuppleChecker.HpID = hpId;
        kinkiSuppleChecker.PtID = 13934;
        kinkiSuppleChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //setup SystemCof
        //KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 0;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = hpId,
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

        //Setup Data test
        var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
        var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
        var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.PtSupples.AddRange(ptSupples);
        tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
        tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);
        kinkiSuppleChecker.InitFinder(TenantProvider, cache);

        try
        {
            // Act
            var result = kinkiSuppleChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.False(result.ErrorOrderList.Count > 0 && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtSupples.RemoveRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Test KinkiSuppleChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void KinkiSuppleChecker_005_Test_WhenSettingLevel_Is_5()
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
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiSuppleChecker.HpID = hpId;
        kinkiSuppleChecker.PtID = 13934;
        kinkiSuppleChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //setup SystemCof
        //KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 5;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = hpId,
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


        //Setup Data test
        var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
        var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
        var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.PtSupples.AddRange(ptSupples);
        tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
        tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
        tenantTracking.M01Kinki.AddRange(m01Kinki);

        tenantTracking.SaveChanges();

        var diseaseChecker = new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>();
        diseaseChecker.HpID = 999;
        diseaseChecker.PtID = 1231;
        diseaseChecker.Sinday = 20230505;


        var cache = new MasterDataCacheService(TenantProvider);
        kinkiSuppleChecker.InitFinder(TenantProvider, cache);

        try
        {
            // Act
            var result = kinkiSuppleChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.False(result.ErrorOrderList.Count > 0 && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtSupples.RemoveRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void KinkiSuppleChecker_006_Test_ErrorSupple_SettingLevel_Is_1()
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
                                                                RealtimeCheckerType.KinkiSupplement, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
        kinkiSuppleChecker.HpID = hpId;
        kinkiSuppleChecker.PtID = 13934;
        kinkiSuppleChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //setup SystemCof KinkiLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 3;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = hpId,
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

        //Setup Data test
        var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
        var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
        var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
        var m01Kinki = CommonCheckerData.ReadM01Kinki(hpId);
        tenantTracking.PtSupples.AddRange(ptSupples);
        tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
        tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
        tenantTracking.M01Kinki.AddRange(m01Kinki);
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(kinkiSuppleChecker.HpID, new List<string>() { "936DIS003" }, 20230505, 1231);
        kinkiSuppleChecker.InitFinder(TenantProvider, cache);

        try
        {
            // Act
            var result = kinkiSuppleChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count > 0 && result.IsError);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtSupples.RemoveRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
            tenantTracking.M01Kinki.RemoveRange(m01Kinki);
            tenantTracking.SaveChanges();
        }
    }
}
