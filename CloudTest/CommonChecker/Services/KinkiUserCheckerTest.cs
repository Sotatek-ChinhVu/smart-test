using CloudUnitTest.SampleData;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.CommonChecker.Services
{
    public class KinkiUserCheckerTest : BaseUT
    {
        ///// <summary>
        ///// Test KinkiUserChecker With Setting Value is 5
        ///// </summary>
        //[Test]
        //public void Test_001_KinkiUserCheckerTest_WhenCurrentOrderCodeConstantAcdAndCheckingOrderCodeConstantBcd()
        //{
        //    ///Setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        //    var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        //    tenantTracking.TenMsts.AddRange(tenMsts);
        //    tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        //    tenantTracking.SaveChanges();

        //    var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        //    kinkiUserChecker.HpID = 999;
        //    kinkiUserChecker.PtID = 111;
        //    kinkiUserChecker.Sinday = 20230101;
        //    kinkiUserChecker.DataContext = TenantProvider.GetNoTrackingDataContext();

        //    var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6111K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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

        //    var currentList = new List<OrdInfoModel>()
        //    {
        //        new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        //    };

        //    kinkiUserChecker.CurrentListOrder = currentList;

        //    var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6404K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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
        //    var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        //    var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
        //                                            RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);

        //    //// Act
        //    var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

        //    tenantTracking.TenMsts.RemoveRange(tenMsts);
        //    tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
        //    tenantTracking.SaveChanges();
        //    //// Assert
        //    Assert.True(result.ErrorInfo != null && result.IsError);
        //}

        //[Test]
        //public void Test_002_KinkiUserCheckerTest_WhenCurrentOrderCodeConstantBcdAndCheckingOrderCodeConstantAcd()
        //{
        //    ///Setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        //    var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        //    tenantTracking.TenMsts.AddRange(tenMsts);
        //    tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        //    tenantTracking.SaveChanges();

        //    var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        //    kinkiUserChecker.HpID = 999;
        //    kinkiUserChecker.PtID = 111;
        //    kinkiUserChecker.Sinday = 20230101;
        //    kinkiUserChecker.DataContext = TenantProvider.GetNoTrackingDataContext();

        //    var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6404K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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

        //    var currentList = new List<OrdInfoModel>()
        //    {
        //        new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        //    };

        //    kinkiUserChecker.CurrentListOrder = currentList;

        //    var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6111K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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
        //    var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        //    var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
        //                                            RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);

        //    //// Act
        //    var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

        //    tenantTracking.TenMsts.RemoveRange(tenMsts);
        //    tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
        //    tenantTracking.SaveChanges();
        //    //// Assert
        //    Assert.True(result.ErrorInfo != null && result.IsError);
        //}

        //[Test]
        //public void Test_003_KinkiUserCheckerTest_WhenSettingLevelIsFalse()
        //{
        //    ///Setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        //    var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
        //    var temp = systemConf?.Val ?? 0;
        //    int settingLevel = 6;
        //    if (systemConf != null)
        //    {
        //        systemConf.Val = settingLevel;
        //    }
        //    else
        //    {
        //        systemConf = new SystemConf
        //        {
        //            HpId = 1,
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

        //    var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        //    var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        //    tenantTracking.TenMsts.AddRange(tenMsts);
        //    var kinkiMsts = CommonCheckerData.ReadKinkiMst();
        //    tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        //    tenantTracking.SaveChanges();

        //    var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
        //    kinkiUserChecker.HpID = 999;
        //    kinkiUserChecker.PtID = 111;
        //    kinkiUserChecker.Sinday = 20230101;
        //    kinkiUserChecker.DataContext = TenantProvider.GetNoTrackingDataContext();

        //    var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6404K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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

        //    var currentList = new List<OrdInfoModel>()
        //    {
        //        new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        //    };

        //    kinkiUserChecker.CurrentListOrder = currentList;

        //    var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        //    {
        //        new OrdInfoDetailModel( id: "id1",
        //                                sinKouiKbn: 20,
        //                                itemCd: "6111K01",
        //                                itemName: "・・ｭ・・ｫ・・ｫ・・・・・ｭ・・ｼ・・ｫ・・ｫ・・・・・ｻ・・ｫ・ｼ・・ｼ・・ｼ・・ｼ・・・ｼ・・ｼ・・ｼ・・ｼ・ﾎｼ・ｽ・",
        //                                suryo: 1,
        //                                unitName: "g",
        //                                termVal: 0,
        //                                syohoKbn: 3,
        //                                syohoLimitKbn: 0,
        //                                drugKbn: 1,
        //                                yohoKbn: 0,
        //                                ipnCd: "3112004M1",
        //                                bunkatu: "",
        //                                masterSbt: "Y",
        //                                bunkatuKoui: 0),

        //        new OrdInfoDetailModel( id: "id2",
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
        //    var odrInfoModel = new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails);

        //    var unitCheckerResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
        //                                            RealtimeCheckerType.KinkiUser, odrInfoModel, 20230101, 1231);

        //    //// Act
        //    var result = kinkiUserChecker.HandleCheckOrder(unitCheckerResult);

        //    systemConf.Val = temp;

        //    tenantTracking.TenMsts.RemoveRange(tenMsts);
        //    tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
        //    tenantTracking.SaveChanges();
        //    //// Assert
        //    Assert.True(!result.IsError);
        //}

        //[Test]
        //public void Test_004_CheckKinkiUser_Finder_CurrentCodeConstantACd_AddedCodeConstantBCd()
        //{
        //    ///Setup
        //    var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        //    var tenMsts = CommonCheckerData.ReadTenMst("K01", "K01");
        //    var kinkiMsts = CommonCheckerData.ReadKinkiMst("K01");
        //    tenantTracking.TenMsts.AddRange(tenMsts);
        //    tenantTracking.KinkiMsts.AddRange(kinkiMsts);
        //    tenantTracking.SaveChanges();

        //    var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext());
        //    var hpId = 999;
        //    var settingLevel = 4;
        //    var sinDay = 20230101;
        //    var listCurrentOrderCode = new List<ItemCodeModel>()
        //    {
        //        new("6111K01", "id1")
        //    };

        //    var listAddedOrderCode = new List<ItemCodeModel>()
        //    {
        //        new("6404K01", "id1")
        //    };
        //    ///Act
        //    var result = realTimeCheckerFinder.CheckKinkiUser(hpId, settingLevel, sinDay, listCurrentOrderCode, listAddedOrderCode);

        //    tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
        //    tenantTracking.TenMsts.RemoveRange(tenMsts);
        //    tenantTracking.SaveChanges();

        //    ///Assert
        //    Assert.True(result.Count == 1);
        //}
    }
}
