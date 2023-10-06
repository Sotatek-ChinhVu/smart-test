using CloudUnitTest.SampleData;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services
{
    public class DosageCheckerTest : BaseUT
    {
        //[Test]
        //public void CheckDosageChecker_001_ReturnsEmptyList_WhenFollowSettingValue()
        //{
        //    var ordInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "620160501",
        //                                itemName: "ＰＬ配合顆粒",
        //                                suryo: 100,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 2,
        //                                syohoLimitKbn: 1,
        //                                drugKbn: 1,
        //                                yohoKbn: 2,
        //                                ipnCd: "1180107D1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 21,
        //                                itemCd: "Y101",
        //                                itemName: "・・・・ｼ・・・ｵｷ・ｺ・・・・",
        //                                suryo: 1,
        //                                unitName: "・・･・・・",
        //                                termVal: 0,
        //                                syohoKbn: 0,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 0,
        //                                yohoKbn: 1,
        //                                ipnCd: "",
        //                                bunkatu: "",
        //                                masterSbt: "",
        //                                bunkatuKoui: 0),
        //    };

        //    var odrInfoModel = new List<OrdInfoModel>()
        //    {
        //        new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
        //    };

        //    var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
        //                                                            RealtimeCheckerType.Dosage, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var ptInfs = CommonCheckerData.ReadPtInf();
        //    tenantTracking.PtInfs.AddRange(ptInfs);
        //    tenantTracking.SaveChanges();
        //    var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
        //    dosageChecker.HpID = 999;
        //    dosageChecker.PtID = 1231;
        //    dosageChecker.Sinday = 20230101;
        //    dosageChecker.DataContext = TenantProvider.GetNoTrackingDataContext();

        //    //// Act
        //    var result = dosageChecker.HandleCheckOrderList(unitCheckerForOrderListResult);

        //    tenantTracking.PtInfs.RemoveRange(ptInfs);
        //    tenantTracking.SaveChanges();
        //    //// Assert
        //    Assert.True(result.ErrorOrderList.Count > 0);
        //}

        //[Test]
        //public void DosageChecker_002_CheckDosageFinder_ErrorResult()
        //{
        //    //setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var ptInfs = CommonCheckerData.ReadPtInf();
        //    tenantTracking.PtInfs.AddRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    var hpId = 999;
        //    long ptId = 1231;
        //    var sinday = 20230101;
        //    var minCheck = false;
        //    var ratioSetting = 9.9;
        //    var listItem = new List<DrugInfo>()
        //    {
        //        new DrugInfo()
        //        {
        //            Id = "",
        //            ItemCD = "620160501",
        //            ItemName = "ＰＬ配合顆粒",
        //            SinKouiKbn = 21,
        //            Suryo = 100,
        //            TermVal = 0,
        //            UnitName = "g",
        //            UsageQuantity = 1
        //        }
        //    };

        //    var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());

        //    //// Act

        //    var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, 0, 0, new(), true);

        //    tenantTracking.PtInfs.RemoveRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    //// Assert
        //    Assert.True(result.Any() && result[0].ItemCd == "620160501");
        //}

        //[Test]
        //public void DosageChecker_003_CheckDosageFinder_ErrorResult_CheckCurrentHeightIsNegative()
        //{
        //    //setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var ptInfs = CommonCheckerData.ReadPtInf();
        //    tenantTracking.PtInfs.AddRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    var hpId = 999;
        //    long ptId = 1231;
        //    var sinday = 20230101;
        //    var minCheck = false;
        //    var ratioSetting = 9.9;
        //    var currentHeight = -1;
        //    var currenWeight = 0;
        //    var listItem = new List<DrugInfo>()
        //    {
        //        new DrugInfo()
        //        {
        //            Id = "",
        //            ItemCD = "620160501",
        //            ItemName = "ＰＬ配合顆粒",
        //            SinKouiKbn = 21,
        //            Suryo = 100,
        //            TermVal = 0,
        //            UnitName = "g",
        //            UsageQuantity = 1
        //        }
        //    };

        //    var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());

        //    //// Act

        //    var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

        //    tenantTracking.PtInfs.RemoveRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    //// Assert
        //    Assert.True(result.Any() && result[0].ItemCd == "620160501");
        //}

        //[Test]
        //public void DosageChecker_003_CheckDosageFinder_ErrorResult_CheckCurrentWeightIsNegative()
        //{
        //    //setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var ptInfs = CommonCheckerData.ReadPtInf();
        //    tenantTracking.PtInfs.AddRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    var hpId = 999;
        //    long ptId = 1231;
        //    var sinday = 20230101;
        //    var minCheck = false;
        //    var ratioSetting = 9.9;
        //    var currentHeight = 0;
        //    var currenWeight = -1;
        //    var listItem = new List<DrugInfo>()
        //    {
        //        new DrugInfo()
        //        {
        //            Id = "",
        //            ItemCD = "620160501",
        //            ItemName = "ＰＬ配合顆粒",
        //            SinKouiKbn = 21,
        //            Suryo = 100,
        //            TermVal = 0,
        //            UnitName = "g",
        //            UsageQuantity = 1
        //        }
        //    };

        //    var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());

        //    //// Act

        //    var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

        //    tenantTracking.PtInfs.RemoveRange(ptInfs);
        //    tenantTracking.SaveChanges();

        //    //// Assert
        //    Assert.True(result.Any());
        //}
    }
}
