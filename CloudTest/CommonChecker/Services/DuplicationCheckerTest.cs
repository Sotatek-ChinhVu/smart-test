using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services;

public class DuplicationCheckerTest : BaseUT
{
    [Test]
    public void DuplicationCheck_001_HandleCheckOrder_CheckDuplicationWhenCurrentListOrderIsNullOrEmpty()
    {
        int hpId = 999;
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
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
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
        int hpId = 999;
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
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
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
        int hpId = 999;
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
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
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
        int hpId = 999;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //Setup CheckDupicatedSetting
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 4);
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
                GrpCd = 2027,
                GrpEdaNo = 4,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

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
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.True(result.ErrorInfo != null && result.IsError && result.CheckerType == RealtimeCheckerType.Duplication);
        }
        finally
        {

            if (systemConf != null) systemConf.Val = temp;

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
    public void DuplicationCheckerTest_006_CheckDuplicatedItemCode_TestDuplicatedError()
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

    [Test]
    public void DuplicationCheck_007_HandleCheckOrder_TestCurrentListOrder_Is_Null()
    {

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, null, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        duplicationChecker.CurrentListOrder = null;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        // Act
        try
        {
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.False(result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_008_HandleCheckOrder_TestCurrentListOrder_Coun_Is_0()
    {
        int hpId = 999;
        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, null, 20230101, 1231);

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
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        // Act
        try
        {
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.False(result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_009_HandleCheckOrder_Test_OdrKouiKbn_Less_Than_21()
    {
        int hpId = 999;
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

        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 20, santeiKbn: 0, ordInfDetails: ordInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 20, santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        duplicationChecker.CurrentListOrder = currentList;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        // Act
        try
        {
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.False(result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheck_010_HandleCheckOrder_Test_OdrKouiKbn_More_Than_23()
    {
        int hpId = 999;
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

        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 24, santeiKbn: 0, ordInfDetails: ordInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var ptInfs = CommonCheckerData.ReadPtInf();
        tenantTracking.PtInfs.AddRange(ptInfs);
        tenantTracking.SaveChanges();

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 24, santeiKbn: 0, ordInfDetails: ordInfDetails)
        };

        duplicationChecker.CurrentListOrder = currentList;
        duplicationChecker.HpID = 999;
        duplicationChecker.PtID = 1231;
        duplicationChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(duplicationChecker.HpID, new List<string>() { "620160501" }, 20230101, 1231);
        duplicationChecker.InitFinder(tenantNoTracking, cache);

        // Act
        try
        {
            var result = duplicationChecker.HandleCheckOrder(unitCheckerResult);

            // Assert
            Assert.False(result.IsError);
        }
        finally
        {
            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void DuplicationCheckerTest_011_Test_ListOrder_IsNull()
    {
        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, null, 20230101, 1231);

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var checkingOrder = unitCheckerResult.CheckingData;
        // Act
        var result = duplicationChecker.GetOdrDetailListByCondition(null);

        //Assert
        Assert.True(result.Count == 0);
    }

    [Test]
    public void DuplicationCheckerTest_012_Test_ListOrder_Coun_Is_0()
    {
        var currentOrdInfDetails = new List<OrdInfoDetailModel>();
        var addedOrdInfDetails = new List<OrdInfoDetailModel>();
        var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                RealtimeCheckerType.Duplication, odrInfoModel, 20230101, 1231);

        var duplicationChecker = new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>();

        var currentList = new List<OrdInfoModel>();

        var checkingOrder = unitCheckerResult.CheckingData;
        var currentOdrDetailList = duplicationChecker.GetOdrDetailListByCondition(currentList);

        // Act
        var result = duplicationChecker.CheckDuplicatedItemCode(checkingOrder, currentOdrDetailList.Select(o => new ItemCodeModel(o.ItemCd, o.Id)).ToList());

        //Assert
        Assert.True(result.Count == 0);
    }

    [Test]
    public void DuplicationCheckerTest_013_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_Common()
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
    public void DuplicationCheckerTest_014_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_CommonName_Equal_DoNotChangeTheDosageForm()
    {
        ////Setup
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
                                    itemCd: "12345",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 3,
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
    public void DuplicationCheckerTest_015_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_CommonName_Equal_DoesNotChangeTheContentStandard()
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
                                    syohoLimitKbn: 2,
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
                                    syohoLimitKbn: 2,
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
    public void DuplicationCheckerTest_016_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_CommonName_Equal_DoesNotChangeTheContentStandardOrDosageForm()
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
                                    syohoLimitKbn: 3,
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
                                    syohoLimitKbn: 3,
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
    public void DuplicationCheckerTest_017_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_CommonName_Equal_DoNotChangeTheDosageForm()
    {
        ////Setup
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
                                    syohoLimitKbn: 3,
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
                                    syohoLimitKbn: 3,
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

    public void DuplicationCheckerTest_018_CheckDuplicatedIppanCode_TestDuplicatedError_ReleasedDrugType_Is_CommonName_Equal_CommonName_DoNotChangeTheDosageForm()
    {
        ////Setup
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
                                    itemCd: "12345",
                                    itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
                                    suryo: 1,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 3,
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
    public void DuplicationCheckerTest_019_CheckDuplicatedIppanCode_Test_DrugKbn_Is_0()
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
                                    drugKbn: 0,
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
                                    drugKbn: 0,
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

        Assert.False(result.Count == 1 && result[0].Id == "id1" && result[0].IsIppanCdDuplicated);
    }

    [Test]
    public void DuplicationCheckerTest_020_CheckDuplicatedIppanCode_Test_ItemCd_Is_BUNKATU()
    {
        ////Setup
        ///
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "@BUNKATU",
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
                                    itemCd: "@BUNKATU",
                                    itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
                                    suryo: 1,
                                    unitName: "・・･・・・",
                                    termVal: 0,
                                    syohoKbn: 0,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
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
                                    itemCd: "@BUNKATU",
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
                                    itemCd: "@BUNKATU",
                                    itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
                                    suryo: 1,
                                    unitName: "・・･・・・",
                                    termVal: 0,
                                    syohoKbn: 0,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
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

        Assert.False(result.Count == 1 && result[0].Id == "id1" && result[0].IsIppanCdDuplicated);
    }

    [Test]
    public void DuplicationCheckerTest_021_CheckDuplicatedIppanCode_Test_ItemCd_Is_REFILL()
    {
        ////Setup
        ///
        var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "@REFILL",
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
                                    itemCd: "@REFILL",
                                    itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
                                    suryo: 1,
                                    unitName: "・・･・・・",
                                    termVal: 0,
                                    syohoKbn: 0,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
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
                                    itemCd: "@REFILL",
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
                                    itemCd: "@REFILL",
                                    itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
                                    suryo: 1,
                                    unitName: "・・･・・・",
                                    termVal: 0,
                                    syohoKbn: 0,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
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

        Assert.False(result.Count == 1 && result[0].Id == "id1" && result[0].IsIppanCdDuplicated);
    }

    [Test]
    public void DuplicationCheckerTest_022_CheckDuplicatedIppanCode_Test_IPNCD_Is_Empty()
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
                                    ipnCd: "",
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
                                    drugKbn: 1,
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
                                    syohoKbn: 3,
                                    syohoLimitKbn: 0,
                                    drugKbn: 1,
                                    yohoKbn: 0,
                                    ipnCd: "",
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
                                    drugKbn: 1,
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

        Assert.False(result.Count == 1 && result[0].Id == "id1" && result[0].IsIppanCdDuplicated);
    }


}
