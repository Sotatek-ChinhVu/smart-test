using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services;

public class AgeCheckerTest : BaseUT
{
    [Test]
    public void CheckAge_001_ReturnsEmptyList_WhenPatientInfoIsNull()
    {
        //Setup
        int hpId = 1;
        long ptId = 0;
        int sinDay = 20230605;
        int level = 0;
        int ageTypeCheckSetting = 1;
        var listItemCode = new List<ItemCodeModel>();
        var kensaInfDetailModels = new List<KensaInfDetailModel>();
        bool isDataOfDb = true;

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        // Act
        var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

        // Assert
        Assert.True(!result.Any());
    }

    [Test]
    public void CheckAge_002_ReturnErrorList_WhenPatientInfoWasBorn1940()
    {
        //Setup
        int hpId = 1;
        long ptId = 123;
        int sinDay = 20230605;
        int level = 10;
        int ageTypeCheckSetting = 1;
        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("620001936", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("620003776", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("622634701", "")
        };

        var kensaInfDetailModels = new List<KensaInfDetailModel>();
        bool isDataOfDb = true;

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620001936" }, sinDay, ptId);
        var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        // Act
        var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

        // Assert
        Assert.True(result.Any());
    }

    [Test]
    public void CheckAge_003_ReturnErrorList_WhenPatientInfoWasBorn2000()
    {
        //Setup
        int hpId = 1;
        long ptId = 6215;
        int sinDay = 20230605;
        int level = 10;
        int ageTypeCheckSetting = 1;
        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("620001936", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("620003776", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("622634701", "")
        };

        var kensaInfDetailModels = new List<KensaInfDetailModel>();
        bool isDataOfDb = true;

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        // Act
        var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

        // Assert
        Assert.True(result.Any());
    }

    [Test]
    public void CheckAge_004_ReturnErrorList_WhenPatientInfoWasBorn2020()
    {
        //Setup
        int hpId = 1;
        long ptId = 99999637;
        int sinDay = 20230605;
        int level = 10;
        int ageTypeCheckSetting = 1;
        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("620001936", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("620003776", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("622634701", "")
        };

        var kensaInfDetailModels = new List<KensaInfDetailModel>();
        bool isDataOfDb = true;

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        // Act
        var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

        // Assert
        Assert.True(result.Any());
    }

    [Test]
    public void CheckAge_005_TestAgeChecker_WhenAgeTypeCheckSettingValueIs0()
    {
        //Setup
        int hpId = 1;
        long ptId = 99999637;
        int sinDay = 20230605;
        int level = 10;
        int ageTypeCheckSetting = 0;
        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("620001936", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("620003776", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("620525101", ""),
            new ItemCodeModel("641210022", ""),
            new ItemCodeModel("620004717", ""),
            new ItemCodeModel("622634701", "")
        };

        var kensaInfDetailModels = new List<KensaInfDetailModel>();
        bool isDataOfDb = true;

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        // Act
        var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

        // Assert
        Assert.True(result.Any());
    }

    [Test]
    public void CheckAge_006_TestAgeChecker_HandleCheckOrderList_TestSettingLevelIs0()
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

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Age, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ageChecker = new AgeChecker<OrdInfoModel, OrdInfoDetailModel>();
        ageChecker.HpID = 1;
        ageChecker.PtID = 111;
        ageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //AgeLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 3);
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
                GrpEdaNo = 3,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);

        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        ageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            var result = ageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

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
    public void CheckAge_007_TestAgeChecker_HandleCheckOrderList_TestSettingLevelIsMoreThan10()
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

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Age, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ageChecker = new AgeChecker<OrdInfoModel, OrdInfoDetailModel>();
        ageChecker.HpID = 1;
        ageChecker.PtID = 111;
        ageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        //AgeLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 11;
        if (systemConf != null)
        {
            systemConf.Val = 11;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2027,
                GrpEdaNo = 3,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 11
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        var cache = new MasterDataCacheService(TenantProvider);

        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        ageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            var result = ageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

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
    public void CheckAge_008_TestAgeChecker_HandleCheckOrderList_Test_CheckedResult_IsNotNull_And_CountMoreThan_0()
    {
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //AgeLevelSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 8;
        int settingLevel = 8;
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

        var tenMsts = CommonCheckerData.ReadTenMst("AGE008", "AGE008");
        var m14 = CommonCheckerData.ReadM14AgeCheck();
        var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS003");
        var ptByomei = CommonCheckerData.ReadPtByomei();
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M14AgeCheck.AddRange(m14);
        tenantTracking.SaveChanges();

        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "6220816AGE", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Age, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var ageChecker = new AgeChecker<OrdInfoModel, OrdInfoDetailModel>();
        ageChecker.HpID = 1;
        ageChecker.PtID = 111;
        ageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

        var cache = new MasterDataCacheService(TenantProvider);

        cache.InitCache(new List<string>() { "6220816AGE" }, 20230101, 1231);
        ageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            var result = ageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.IsError == true &&
                        result.ErrorOrderList.Count == 1 &&
                        result.IsDataOfDb == true &&
                        result.CheckerType == RealtimeCheckerType.Age &&
                        result.ErrorInfo != null
                );
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M14AgeCheck.RemoveRange(m14);
            if (systemConf != null) systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }


    [Test]
    public void CheckAge_009_HandleCheckOrder_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

        // Arrange
        var ageChecker = new AgeChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new  UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Age, odrInfoModel, 20230101, 111);

        // Act and Assert
        Assert.Throws<NotImplementedException>(() => ageChecker.HandleCheckOrder(unitChecker));
    }
}
