using CommonChecker.Caches;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;
using Domain.Models.SpecialNote.PatientInfo;

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
}
