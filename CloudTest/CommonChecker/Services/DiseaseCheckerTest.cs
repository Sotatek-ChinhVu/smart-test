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

namespace CloudUnitTest.CommonChecker.Services;

public class DiseaseCheckerTest : BaseUT
{
    /// <summary>
    /// Test DiseaseCheck With Setting Value is 5
    /// </summary>
    [Test]
    public void CheckDiseaseChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
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
                                                                RealtimeCheckerType.Disease, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var diseaseChecker = new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>();
        diseaseChecker.HpID = 1;
        diseaseChecker.PtID = 111;
        diseaseChecker.Sinday = 20230101;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
        diseaseChecker.InitFinder(tenantNoTracking, cache);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 2);
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

        if (systemConf != null) systemConf.Val = temp;
        tenantTracking.SaveChanges();

        // Act
        var result = diseaseChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

        // Assert
        Assert.True(result.ErrorOrderList.Count == 0);
    }

    [Test]
    public void CheckDiseaseChecker_002_CheckContraindicationForCurrentDisease()
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

        var tenMsts = CommonCheckerData.ReadTenMst("DIS002", "DIS002");
        var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("DIS002");
        var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS002");
        var ptByomei = CommonCheckerData.ReadPtByomei();
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
        tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
        tenantTracking.PtByomeis.AddRange(ptByomei);
        tenantTracking.SaveChanges();

        int hpId = 999;
        long ptId = 1231;
        int sinDate = 20230505;
        var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936DIS002", "id1"),
            new ItemCodeModel("22DIS002", "id2"),
            new ItemCodeModel("101DIS002", "id3"),
            new ItemCodeModel("776DIS002", "id4"),
            new ItemCodeModel("717DIS002", "id5"),
        };

        // Arrange
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "620160501" }, sinDate, ptId);
        var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

        try
        {
            //Act
            var result = realTimeCheckerFinder.CheckContraindicationForCurrentDisease(hpId, ptId, settingLevel, sinDate, listItemCode, new(), true);

            //Assert
            Assert.True(result.Any());
            if (systemConf != null) systemConf.Val = temp;
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.RemoveRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42DrugMainEx);
            tenantTracking.PtByomeis.RemoveRange(ptByomei);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckDiseaseChecker_003_CheckOrderList_With_CheckContraindicationForCurrentDisease_Any()
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

        var tenMsts = CommonCheckerData.ReadTenMst("DIS002", "DIS002");
        var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("DIS002");
        var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS002");
        var ptByomei = CommonCheckerData.ReadPtByomei();
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
        tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
        tenantTracking.PtByomeis.AddRange(ptByomei);
        tenantTracking.SaveChanges();

        var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, itemCd: "936DIS003", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, itemCd: "22DIS003", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id3", 21, itemCd: "101DIS003", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id4", 21, itemCd: "776DIS003", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id5", 21, itemCd: "717DIS003", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

        var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

        ///Act
        var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                RealtimeCheckerType.Disease, odrInfoModel, 20230505, 1231, new(new(), new(), new()), new(), new(), true);

        var diseaseChecker = new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>();
        diseaseChecker.HpID = 999;
        diseaseChecker.PtID = 1231;
        diseaseChecker.Sinday = 20230505;
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var cache = new MasterDataCacheService(TenantProvider);
        cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
        diseaseChecker.InitFinder(tenantNoTracking, cache);

        try
        {
            // Act
            var result = diseaseChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.True(result.ErrorOrderList.Any());
            if (systemConf != null) systemConf.Val = temp;
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.RemoveRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42DrugMainEx);
            tenantTracking.PtByomeis.RemoveRange(ptByomei);
            tenantTracking.SaveChanges();
        }
    }
}
