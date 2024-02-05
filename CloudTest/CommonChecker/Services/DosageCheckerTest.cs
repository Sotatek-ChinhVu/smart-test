using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services;

public class DosageCheckerTest : BaseUT
{
    [Test]
    public void CheckDosageChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageDrinkingDrugSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 1;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2023,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count > 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DosageChecker_002_CheckDosageFinder_ErrorResult()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var hpId = 999;
        long ptId = 1231;
        var sinday = 20230101;
        var minCheck = false;
        var ratioSetting = 9.9;
        var listItem = new List<DrugInfo>()
        {
            new DrugInfo()
            {
                Id = "",
                ItemCD = "620160501",
                ItemName = "ＰＬ配合顆粒",
                SinKouiKbn = 21,
                Suryo = 100,
                TermVal = 0,
                UnitName = "g",
                UsageQuantity = 1
            }
        };
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "620160501" }, sinday, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            // Act
            var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, 0, 0, new(), true);

            // Assert
            Assert.True(result.Any() && result[0].ItemCd == "620160501");
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DosageChecker_003_CheckDosageFinder_ErrorResult_CheckCurrentHeightIsNegative()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var hpId = 999;
        long ptId = 1231;
        var sinday = 20230101;
        var minCheck = false;
        var ratioSetting = 9.9;
        var currentHeight = -1;
        var currenWeight = 0;
        var listItem = new List<DrugInfo>()
        {
            new DrugInfo()
            {
                Id = "",
                ItemCD = "620160501",
                ItemName = "ＰＬ配合顆粒",
                SinKouiKbn = 21,
                Suryo = 100,
                TermVal = 0,
                UnitName = "g",
                UsageQuantity = 1
            }
        };
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "620160501" }, sinday, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            // Act
            var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

            // Assert
            Assert.True(result.Any() && result[0].ItemCd == "620160501");
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DosageChecker_004_CheckDosageFinder_ErrorResult_CheckCurrentWeightIsNegative()
    {
        //setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var hpId = 999;
        long ptId = 1231;
        var sinday = 20230101;
        var minCheck = false;
        var ratioSetting = 9.9;
        var currentHeight = 0;
        var currenWeight = -1;
        var listItem = new List<DrugInfo>()
        {
            new DrugInfo()
            {
                Id = "",
                ItemCD = "620160501",
                ItemName = "ＰＬ配合顆粒",
                SinKouiKbn = 21,
                Suryo = 100,
                TermVal = 0,
                UnitName = "g",
                UsageQuantity = 1
            }
        };
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(hpId, new List<string>() { "620160501" }, sinday, ptId);
        var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            // Act
            var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

            // Assert
            Assert.True(result.Any());
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DosageChecker_005_HandleCheckOrder_ThrowsNotImplementedException()
    {
        //Setup
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

        // Arrange
        var ageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        var unitChecker = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 111);

        // Act and Assert
        Assert.Throws<NotImplementedException>(() => ageChecker.HandleCheckOrder(unitChecker));
    }

    [Test]
    public void CheckDosageChecker_006_HandleCheckOrderList_DosageDrinkingDrugSetting_Is_False()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageDrinkingDrugSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
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
                GrpCd = 2023,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 5
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);

            if (systemConf != null) systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// odrKouiKbn = 22
    /// DosageDrugAsOrderSetting = false
    /// </summary>
    [Test]
    public void CheckDosageChecker_007_HandleCheckOrderList_DosageDrugAsOrderSetting_Is_True()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 22,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageDrugAsOrderSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 3);
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
                GrpCd = 2023,
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
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);

            if (systemConf != null) systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// odrKouiKbn = 23
    /// </summary>
    [Test]
    public void CheckDosageChecker_008_HandleCheckOrderList_OdrKouiKbn_Is_23()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 23,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);

            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// odrKouiKbn = 28
    /// </summary>
    [Test]
    public void CheckDosageChecker_009_HandleCheckOrderList_OdrKouiKbn_Is_28()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 28,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);

            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// odrKouiKbn = 30
    /// DosageOtherDrugSetting = false
    /// </summary>
    [Test]
    public void CheckDosageChecker_010_HandleCheckOrderList_Test_DosageOtherDrugSetting()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 30,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageOtherDrugSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 4);
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
                GrpCd = 2023,
                GrpEdaNo = 4,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);

            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckDosageChecker_011_ReturnsEmptyList_OdrInfDetails_Is_Empty()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageDrinkingDrugSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 1;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2023,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckDosageChecker_012_Test_TermLimitCheckingOnly_Is_True()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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
            new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);

        //DosageDrinkingDrugSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null)
        {
            systemConf.Val = 1;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2023,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        tenantTracking.SaveChanges();
        var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        dosageChecker.HpID = 999;
        dosageChecker.PtID = 1231;
        dosageChecker.Sinday = 20230101;
        dosageChecker.TermLimitCheckingOnly = true;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(dosageChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        dosageChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }
}
