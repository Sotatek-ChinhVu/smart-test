using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;

namespace CloudUnitTest.CommonChecker.Services;

public class DuplicationCheckerTest : BaseUT
{
    [Test]
    public void DuplicationCheck_001_HandleCheckOrder_CheckDuplicationWhenCurrentListOrderIsNullOrEmpty()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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

        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: ordInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        duplicationChecker.CurrentListOrder = new();
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorOrderList is null);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_002_HandleCheckOrder_CheckDuplicationWhenCurrentListOrderAndAddedListOrderIsDuplicatedItemCode()
    {
        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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

        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: ordInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        duplicationChecker.CurrentListOrder = currentList;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        // Act
        try
        {
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_003_HandleCheckOrder_CheckDuplicationWhenCurrentListOrderAndAddedListOrderIsDuplicatedIppanCode()
    {
        //Setup
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        duplicationChecker.CurrentListOrder = currentList;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_004_HandleCheckOrder_CheckDuplicationWhenCurrentListOrderAndAddedListOrderIsDuplicatedIppanCode()
    {
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
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

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "12345",
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
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        duplicationChecker.CurrentListOrder = currentList;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheckerTest_005_CheckDuplicatedIppanCode_TestDuplicatedError()
    {
        ////Setup
        ///
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
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

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "12345",
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
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var checkingOrder = unitCheckerResult.CheckingData;

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        var currentOdrDetailList = duplicationChecker.GetOdrDetailListByCondition(currentList);
        // Act
        var result = duplicationChecker.CheckDuplicatedIppanCode(checkingOrder, currentOdrDetailList);

        //Assert

        Assert.True(result.Count == 1 && result[0].Id == "id1" && result[0].IsIppanCdDuplicated);
    }

    [Test]
    public void DuplicationCheckerTest_007_CheckDuplicatedItemCode_TestDuplicatedError()
    {
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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

        var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "613110017",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
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
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

        var checkingOrder = unitCheckerResult.CheckingData;
        var currentOdrDetailList = duplicationChecker.GetOdrDetailListByCondition(currentList);

        // Act
        var result = duplicationChecker.CheckDuplicatedItemCode(checkingOrder, currentOdrDetailList.Select(o => new ItemCodeModel(o.ItemCd, o.Id)).ToList());

        //Assert
        Assert.True(result.Count == 1 && result[0].Id == "id1" && result[0].DuplicatedItemCd == "613110017");
    }
}
