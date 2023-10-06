using CloudUnitTest.SampleData;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services
{
    public class KinkiCheckerTest : BaseUT
    {
        ///// <summary>
        ///// Test KinkiChecker With Setting Value is 5
        ///// </summary>
        //[Test]
        //public void CheckKinkiChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
        //{
        //    //setup
        //    var ordInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
        //        new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        //    };

        //    var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);


        //    var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
        //                                                            RealtimeCheckerType.Kinki, odrInfoModel, 20230101, 111);

        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var kinkiChecker = new KinkiChecker<OrdInfoModel, OrdInfoDetailModel>();
        //    kinkiChecker.HpID = 1;
        //    kinkiChecker.PtID = 111;
        //    kinkiChecker.Sinday = 20230101;
        //    kinkiChecker.DataContext = TenantProvider.GetNoTrackingDataContext();

        //    var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        //    var temp = systemConf?.Val ?? 0;
        //    if (systemConf != null)
        //    {
        //        systemConf.Val = 5;
        //    }
        //    else
        //    {
        //        systemConf = new SystemConf
        //        {
        //            HpId = 1,
        //            GrpCd = 2027,
        //            GrpEdaNo = 1,
        //            CreateDate = DateTime.UtcNow,
        //            UpdateDate = DateTime.UtcNow,
        //            CreateId = 2,
        //            UpdateId = 2,
        //            Val = 5
        //        };
        //        tenantTracking.SystemConfs.Add(systemConf);
        //    }
        //    tenantTracking.SaveChanges();

        //    if (systemConf != null) systemConf.Val = temp;
        //    tenantTracking.SaveChanges();

        //    //// Act
        //    var result = kinkiChecker.HandleCheckOrder(unitCheckerForOrderListResult);
        //    //// Assert
        //    Assert.True(result.ErrorOrderList is null);
        //}

        //[Test]

        //public void CheckKinkiChecker_002_CheckKinki()
        //{
        //    //Setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 999 && p.GrpCd == 2027 && p.GrpEdaNo == 2);
        //    var temp = systemConf?.Val ?? 0;
        //    int settingLevel = 3;
        //    if (systemConf != null)
        //    {
        //        systemConf.Val = settingLevel;
        //    }
        //    else
        //    {
        //        systemConf = new SystemConf
        //        {
        //            HpId = 999,
        //            GrpCd = 2027,
        //            GrpEdaNo = 2,
        //            CreateDate = DateTime.UtcNow,
        //            UpdateDate = DateTime.UtcNow,
        //            CreateId = 2,
        //            UpdateId = 2,
        //            Val = settingLevel
        //        };
        //        tenantTracking.SystemConfs.Add(systemConf);
        //    }
        //    tenantTracking.SaveChanges();

        //    var tenMsts = CommonCheckerData.ReadTenMst("DIS002", "DIS002");
        //    tenantTracking.TenMsts.AddRange(tenMsts);
        //    tenantTracking.SaveChanges();

        //    var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());

        //    int hpId = 999;
        //    long ptId = 1231;
        //    int sinDate = 20230505;
        //    var listItemCode = new List<ItemCodeModel>()
        //    {
        //        new ItemCodeModel("936DIS002", "id1"),
        //        new ItemCodeModel("22DIS002", "id2"),
        //        new ItemCodeModel("101DIS002", "id3"),
        //        new ItemCodeModel("776DIS002", "id4"),
        //        new ItemCodeModel("717DIS002", "id5"),
        //    };

        //    var listDrugItemCode = new List<ItemCodeModel>()
        //    {
        //        new ItemCodeModel("936DIS002", "id1"),
        //        new ItemCodeModel("22DIS002", "id2"),
        //        new ItemCodeModel("101DIS002", "id3"),
        //        new ItemCodeModel("776DIS002", "id4"),
        //        new ItemCodeModel("717DIS002", "id5"),
        //    };

        //    ///Act
        //    var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

        //    if (systemConf != null) systemConf.Val = temp;

        //    tenantTracking.TenMsts.RemoveRange(tenMsts);
        //    tenantTracking.SaveChanges();

        //    ///Assert
        //    Assert.True(!result.Any());
        //}
    }
}
