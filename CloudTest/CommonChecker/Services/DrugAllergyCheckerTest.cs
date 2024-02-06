using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services
{
    public class DrugAllergyCheckerTest : BaseUT
    {
        [Test]
        public void CheckDrugAllergyChecker_001_CheckingError_WhenDrugsWithTheSameEffectOfAllergies()
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var isSameComponentChecked = SaveSystemConf(1, 2026, 2, 1);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);

            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

                // Assert
                Assert.True(result.ErrorOrderList.Count > 0 && result.ErrorOrderList[0].OrdInfDetails[0].ItemCd == "613110017");
            }
            finally
            {
                SaveSystemConf(1, 2026, 2, isSameComponentChecked);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void CheckDrugAllergyChecker_002_CheckingError_WhenNoAllergyMedicineInSpecialNote()
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);

            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

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
        public void CheckDrugAllergyChecker_003_GetDrugAllergyByPtId()
        {
            // Setup
            var hpId = 999;
            var ptId = 1231;
            var sinDate = 20230101;

            var isSameComponentChecked = SaveSystemConf(1, 2026, 2, 1);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

            try
            {
                // Act
                var result = realTimeCheckerFinder.GetDrugAllergyByPtId(hpId, ptId, sinDate, new(), true);

                /// Assert
                Assert.True(result.Count == 1);
            }
            finally
            {
                SaveSystemConf(1, 2026, 2, isSameComponentChecked);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// IsSameComponentChecked = true
        /// </summary>
        [Test]
        public void CheckDrugAllergyChecker_004_CheckingError_IsDuplicatedItemCode()
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

                // Duplicated
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails),
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var isSameComponentChecked = SaveSystemConf(1, 2026, 2, 1);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);

            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

                // Assert
                Assert.True(result.ErrorOrderList.Count > 0);
            }
            finally
            {
                SaveSystemConf(1, 2026, 2, isSameComponentChecked);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void CheckDrugAllergyChecker_005_CheckingError_IsDuplicatedItemCode()
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

                // Duplicated
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails),
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var isSameComponentChecked = SaveSystemConf(1, 2026, 2, 1);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);
            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

                // Assert
                Assert.True(result.ErrorOrderList.Any());
            }
            finally
            {
                SaveSystemConf(1, 2026, 2, isSameComponentChecked);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void CheckDrugAllergyChecker_006_HandleCheckOrder_ThrowsNotImplementedException()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            // Arrange
            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var unitChecker = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 111);

            // Act and Assert
            Assert.Throws<NotImplementedException>(() => drugAllergy.HandleCheckOrder(unitChecker));
        }

        /// <summary>
        /// IsDuplicatedComponentChecked setting = false
        /// </summary>
        [Test]
        public void CheckDrugAllergyChecker_007_CheckingError_Test_ListDuplicatedItemCode()
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

                // Duplicated
                new OrdInfoDetailModel( id: "id1",
                                        sinKouiKbn: 20,
                                        itemCd: "620675301",
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails),
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);
            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

                // Assert
                Assert.True(result.ErrorOrderList.Count == 2 &&
                            result.CheckerType == RealtimeCheckerType.DrugAllergy
                    );
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// IsDuplicatedComponentChecked setting = true
        /// IsProDrugChecked = false
        /// IsSameComponentChecked = false
        /// IsDuplicatedClassChecked = false
        /// CheckDuplicatedComponent Any()
        /// </summary>
        [Test]
        public void CheckDrugAllergyChecker_008_CheckingError_Test_IsDuplicatedComponentChecked_Is_True()
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

                // Duplicated
                new OrdInfoDetailModel( id: "id1",
                                        sinKouiKbn: 20,
                                        itemCd: "620675301",
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails),
                new OrdInfoModel(odrKouiKbn: 21,santeiKbn: 0, ordInfDetails: ordInfDetails)
            };

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.DrugAllergy, odrInfoModel, 20230101, 1231, new(new(), new(), new()), new(), new(), true);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);

            var isDuplicatedComponentChecked = SaveSystemConf(1, 2026, 0, 1);
            var isProDrugChecked = SaveSystemConf(1, 2026, 1, 0);
            var isDuplicatedClassChecked = SaveSystemConf(1, 2026, 3, 0);
            var isSameComponentChecked = SaveSystemConf(1, 2026, 2, 0);

            tenantTracking.SaveChanges();

            var drugAllergy = new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();

            drugAllergy.HpID = 999;
            drugAllergy.PtID = 1231;
            drugAllergy.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(drugAllergy.HpID, new List<string>() { "620160501" }, 20230101, 1231);
            drugAllergy.InitFinder(tenantNoTracking, cache);
            try
            {
                // Act
                var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

                // Assert
                Assert.True(result.ErrorOrderList.Count == 2 &&
                            result.CheckerType == RealtimeCheckerType.DrugAllergy);
            }
            finally
            {
                SaveSystemConf(1, 2026, 0, isDuplicatedComponentChecked);
                SaveSystemConf(1, 2026, 1, isProDrugChecked);
                SaveSystemConf(1, 2026, 3, isDuplicatedClassChecked);
                SaveSystemConf(1, 2026, 2, isSameComponentChecked);

                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Setup systemconf
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="grpCd"></param>
        /// <param name="grpEdaNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double SaveSystemConf(int hpId, int grpCd, int grpEdaNo, double value)
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == grpCd && p.GrpEdaNo == grpEdaNo);
            var val = systemConf?.Val ?? 0;
            if (systemConf != null)
            {
                systemConf.Val = value;
            }
            else
            {
                systemConf = new SystemConf
                {
                    HpId = hpId,
                    GrpCd = grpCd,
                    GrpEdaNo = grpEdaNo,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = value
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            tenantTracking.SaveChanges();

            return val;
        }
    }
}
