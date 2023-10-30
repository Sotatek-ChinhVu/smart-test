using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.DB;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Interactor.CommonChecker.CommonMedicalCheck;
using Moq;

namespace CloudUnitTest.CommonChecker.Services;

public class DayLimitCheckerTest : BaseUT
{
    /// <summary>
    /// Test Is Error Order with ItemCd = 2
    /// </ summary >
    [Test]
    public void CheckDayLimit_001_DayLimitError_WhenDrugExpired()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day001", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day001", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        //Setup Data test
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("day001", "day001");
        var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day001");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day001");

        var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
        dayLimitChecker.HpID = 999;
        dayLimitChecker.PtID = 111;
        dayLimitChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        dayLimitChecker.InitFinder(tenantNoTracking, cache);

        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        try
        {
            // Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 1);
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Test OdrKouiKbn = 21
    /// </summary>
    [Test]
    public void CheckDayLitmit_002_WhenCheckingOderWithOdrKouiKbnIs21()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id2", 21, "Y101" + "day002", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
        dayLimitChecker.HpID = 999;
        dayLimitChecker.PtID = 111;
        dayLimitChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        dayLimitChecker.InitFinder(tenantNoTracking, cache);
        // Act
        var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
        // Assert
        Assert.True(!result.ErrorOrderList.Any());
    }

    /// <summary>
    ///Test Order with IsDrugUsage is false
    ///YohoKbn <= 0 and ItemCd != ItemCdConst.TouyakuChozaiNaiTon || ItemCd != ItemCdConst.TouyakuChozaiGai  -> !IsDrugUsage
    /// </summary>
    [Test]
    public void CheckDayLitmit_003_WhenCheckingOderWithOdrHasIsDrugUsageIsFalse()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day003", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day003", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 0, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
        dayLimitChecker.HpID = 999;
        dayLimitChecker.PtID = 111;
        dayLimitChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        dayLimitChecker.InitFinder(tenantNoTracking, cache);

        // Act
        var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
        // Assert
        Assert.True(!result.ErrorOrderList.Any());
    }

    /// <summary>
    /// Test Interactor CheckListOrder
    /// </summary>
    [Test]
    public void CheckDayLitmit_004_CommonMedicalCheck_CheckListOrder_GetErrorOrder_DaylimitError()
    {
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var mock = new Mock<IRealtimeOrderErrorFinder>();
        var tenMsts = CommonCheckerData.ReadTenMst("day004", "day004");
        var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day004");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day004");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        int hpId = 999;
        long ptId = 1212;
        int sinDay = 20230101;

        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day004", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day004", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var currentListOdr = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };

        var checkingListOdr = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };

        var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

        try
        {
            var unitCheckInfoModel = commonMedicalCheck.CheckListOrder(hpId, ptId, sinDay, currentListOdr, checkingListOdr, new(), new(), new(), true, new());

            // Assert
            Assert.True(unitCheckInfoModel.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Test Non Error Day limit when DRUG_DAY_LIMIT table has no data
    /// </summary>
    [Test]
    public void CheckDayLitmit_005_CommonMedicalCheck_CheckListOrder_GetErrorDetails_DaylimitError()
    {
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var mock = new Mock<IRealtimeOrderErrorFinder>();
        var tenMsts = CommonCheckerData.ReadTenMst("day005", "day005");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day005");

        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        int hpId = 999;
        long ptId = 1212;
        int sinDay = 20230101;

        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day005", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day005", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var currentListOdr = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };

        var checkingListOdr = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };
        var realtimeCheckerCondition = new RealTimeCheckerCondition(
                                            isCheckingDuplication: false,
                                            isCheckingKinki: false,
                                            isCheckingAllergy: false,
                                            isCheckingDosage: false,
                                            isCheckingDays: true,
                                            isCheckingAge: false,
                                            isCheckingDisease: false,
                                            isCheckingInvalidData: false,
                                            isCheckingAutoCheck: false);

        try
        {
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);
            var listErrorInfo = commonMedicalCheck.CheckListOrder(hpId, ptId, sinDay, currentListOdr, checkingListOdr, new(), new(), new(), true, realtimeCheckerCondition);
            var errorDetails = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo);

            // Assert
            Assert.True(!errorDetails.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.SaveChanges();
        }

    }

    /// <summary>
    /// Test Non Error Day limit when DRUG_DAY_LIMIT table has no data
    /// </summary>
    [Test]
    public void CheckDayLimit_Service_006_NonError_WhenDRUG_DAY_LIMIThasNoData()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day006", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day006", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        //Setup Data test
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("day006", "day006");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day006");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
        dayLimitChecker.HpID = 999;
        dayLimitChecker.PtID = 111;
        dayLimitChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        dayLimitChecker.InitFinder(tenantNoTracking, cache);
        try
        {
            // Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(!result.ErrorOrderList.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Test Non Error Day limit when M10_DAY_LIMIT table has no data
    /// </summary>
    [Test]
    public void CheckDayLimit_Service_007_NonError_WhenM10_DAY_LIMIThasNoData()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day007", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day007", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>();
        odrInfoModel.Add(new OrdInfoModel(21, 0, ordInfDetails));

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        //Setup Data test
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CommonCheckerData.ReadTenMst("day007", "day007");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day007");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
        dayLimitChecker.HpID = 999;
        dayLimitChecker.PtID = 111;
        dayLimitChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        dayLimitChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(!result.ErrorOrderList.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_008_Finder_CheckDayLimit_WithUsingDayLessThanLimitDay_True()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        int hpId = 999;
        int sinDay = 20230101;
        double usingDay = 89;
        var listAddedOrderCodes = new List<ItemCodeModel>()
        {
            new ItemCodeModel("1234day008", "id1"),
        };

        var tenMsts = CommonCheckerData.ReadTenMst("day008", "day008");
        var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day008");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day008");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        // Arrange
        long ptId = 0;
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);
        try
        {
            var daylimit = realtimeCheckerFinder.CheckDayLimit(hpId, sinDay, listAddedOrderCodes, usingDay);

            Assert.True(!daylimit.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_009_Finder_CheckDayLimit_WithUsingDayEqualThanLimitDay_True()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        int hpId = 999;
        int sinDay = 20230101;
        double usingDay = 90;
        var listAddedOrderCodes = new List<ItemCodeModel>()
        {
            new ItemCodeModel("1234day009", "id1"),
        };

        var tenMsts = CommonCheckerData.ReadTenMst("day009", "day009");
        var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day009");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day009");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();

        // Arrange
        long ptId = 0;
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);
        try
        {
            var daylimit = realtimeCheckerFinder.CheckDayLimit(hpId, sinDay, listAddedOrderCodes, usingDay);

            Assert.True(!daylimit.Any());
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void Test_010_Finder_CheckDayLimit_WithUsingDayMoreThanLimitDay_True()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        int hpId = 999;
        int sinDay = 20230101;
        double usingDay = 91;
        var listAddedOrderCodes = new List<ItemCodeModel>()
        {
            new ItemCodeModel("1234day010", "id1"),
        };

        // Arrange
        long ptId = 0;
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        var tenMsts = CommonCheckerData.ReadTenMst("day010", "day010");
        var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day010");
        var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day010");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
        tenantTracking.M10DayLimit.AddRange(m10DayLimits);
        tenantTracking.SaveChanges();
        try
        {
            var daylimit = realtimeCheckerFinder.CheckDayLimit(hpId, sinDay, listAddedOrderCodes, usingDay);

            Assert.True(daylimit.Any(x => x.LimitDay == 90 && x.UsingDay == 91));
        }
        finally
        {
            //Clear Data test
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
            tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
            tenantTracking.SaveChanges();
        }
    }
}
