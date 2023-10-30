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
using Entity.Tenant;
using Moq;

namespace CloudUnitTest.CommonChecker.Services;

public class FoodAllergyCheckerTest : BaseUT
{
    /// <summary>
    /// Test FoodAllergyChecker With Setting Value is 5
    /// </summary>
    [Test]
    public void FoodAllergyChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
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

        var mock = new Mock<ISystemConfigRepository>();

        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.FoodAllergy, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var FoodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
        FoodAllergyChecker.HpID = 1;
        FoodAllergyChecker.PtID = 111;
        FoodAllergyChecker.Sinday = 20230101;

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
        var result = FoodAllergyChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
        // Assert
        Assert.True(result.ErrorOrderList.Count == 0);
    }

    [Test]
    public void FoodAllergyChecker_002_CheckFoodAllergy()
    {
        //Setup
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 999 && p.GrpCd == 2027 && p.GrpEdaNo == 2);
        var temp = systemConf?.Val ?? 0;
        int settingLevel = 3;
        if (systemConf != null)
        {
            systemConf.Val = settingLevel;
        }
        else
        {
            systemConf = new SystemConf
            {
                HpId = 999,
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
        tenantTracking.SaveChanges();

        var m12FoodAlrgys = CommonCheckerData.ReadM12FoodAlrgy("");
        tenantTracking.M12FoodAlrgy.AddRange(m12FoodAlrgys);
        tenantTracking.SaveChanges();

        int hpId = 999;
        long ptId = 1231;
        int sinDate = 20230505;

        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDate, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936DIS002", "id1"),
            new ItemCodeModel("22DIS002", "id2"),
            new ItemCodeModel("101DIS002", "id3"),
            new ItemCodeModel("776DIS002", "id4"),
            new ItemCodeModel("717DIS002", "id5"),
        };

        //Act
        try
        {
            var result = realTimeCheckerFinder.CheckFoodAllergy(hpId, ptId, sinDate, listItemCode, settingLevel, new(), true);

            //Assert
            Assert.True(!result.Any());
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;
            tenantTracking.M12FoodAlrgy.RemoveRange(m12FoodAlrgys);
            tenantTracking.SaveChanges();
        }
    }
}
