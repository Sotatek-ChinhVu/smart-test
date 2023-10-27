using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;

namespace CloudUnitTest.CommonChecker.Services
{
    public class DrugAllergyCheckerTest : BaseUT
    {
        [Test]
        public void CheckDrugAllergyChecker_001_CheckingError_WhenDrugsWithTheSameEffectOfAllergies()
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

            var odrInfoModel = new List<OrdInfoModel>()
            {
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

            //// Act
            var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();
            //// Assert
            Assert.True(result.ErrorOrderList.Count > 0 && result.ErrorOrderList[0].OrdInfDetails[0].ItemCd == "613110017");
        }

        [Test]
        public void CheckDrugAllergyChecker_002_CheckingError_WhenNoAllergyMedicineInSpecialNote()
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

            // Act
            var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.SaveChanges();
            // Assert
            Assert.True(result.ErrorOrderList.Count == 0);
        }

        [Test]
        public void CheckDrugAllergyChecker_003_GetDrugAllergyByPtId()
        {
            // Setup
            var hpId = 999;
            var ptId = 1231;
            var sinDate = 20230101;

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            // Act
            var result = realTimeCheckerFinder.GetDrugAllergyByPtId(hpId, ptId, sinDate, new(), true);

            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            /// Assert
            Assert.True(result.Count == 1);
        }

        [Test]
        public void CheckDrugAllergyChecker_004_CheckingError_IsDuplicatedItemCode()
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

            //// Act
            var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();
            //// Assert
            Assert.True(result.ErrorOrderList.Count > 0);
        }

        [Test]
        public void CheckDrugAllergyChecker_005_CheckingError_IsDuplicatedItemCode()
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

            // Act
            var result = drugAllergy.HandleCheckOrderList(unitCheckerForOrderListResult);

            tenantTracking.PtInfs.RemoveRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            // Assert
            Assert.True(result.ErrorOrderList.Count > 0);
        }
    }
}
