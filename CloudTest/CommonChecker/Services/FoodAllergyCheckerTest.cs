﻿using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services;

public class FoodAllergyCheckerTest : BaseUT
{
    /// <summary>
    /// Test Error Food Allery when Pt_Id has data in PTAlleryDrug and ItemCd contain Odr 
    /// </summary>
    [Test]
    public void FoodAllergyChecker_001_CheckError_With_ItemCd_Is_PtAlrydrug()
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
                                                                RealtimeCheckerType.FoodAllergy, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
        foodAllergyChecker.HpID = 1;
        foodAllergyChecker.PtID = 111;
        foodAllergyChecker.Sinday = 20230101;
        foodAllergyChecker.InitFinder(tenantNoTracking, cache);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 0);
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
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 3
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        //Read data test
        var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
        var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
        tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
        tenantTracking.M12FoodAlrgy.AddRange(m12);
        tenantTracking.SaveChanges();

        try
        {
            // Act
            var result = foodAllergyChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
            // Assert
            Assert.True(result.ErrorOrderList.Count == 1 && result.ErrorInfo != null);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.RemoveRange(m12);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void FoodAllergyChecker_002_CheckError_With_ItemCd_Is_PtAlrydrug()
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
                                                                RealtimeCheckerType.FoodAllergy, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
        foodAllergyChecker.HpID = 1;
        foodAllergyChecker.PtID = 111;
        foodAllergyChecker.Sinday = 20230101;
        foodAllergyChecker.InitFinder(tenantNoTracking, cache);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 0);
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
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 2,
                UpdateId = 2,
                Val = 3
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        tenantTracking.SaveChanges();

        //Read data test
        var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
        var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
        tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
        tenantTracking.M12FoodAlrgy.AddRange(m12);
        tenantTracking.SaveChanges();

        try
        {
            // Act
            var result = foodAllergyChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
            // Assert
            Assert.True(result.ErrorOrderList.Count == 1 && result.ErrorInfo != null);
        }
        finally
        {
            if (systemConf != null) systemConf.Val = temp;

            tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.RemoveRange(m12);
            tenantTracking.SaveChanges();
        }
    }
}
