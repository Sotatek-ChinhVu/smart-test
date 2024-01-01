using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.DB;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Domain.Models.Diseases;
using Entity.Tenant;
using Interactor.CommonChecker.CommonMedicalCheck;
using Moq;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;

namespace CloudUnitTest.CommonChecker.Interactor
{
    public class CommonMedicalCheckTest : BaseUT
    {
        [Test]
        public void TC_001_CheckFoodAllergy()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };
            var specialNoteItem = new SpecialNoteItem();
            var ptDiseaseModels = new List<PtDiseaseModel>();
            var familyItems = new List<FamilyItem>();
            var isDataOfDb = true;

            #region Setup data test

            //FoodAllergyLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 0);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null)
            {
                systemConf.Val = 4;
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
                    Val = 4
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Read data test
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: true,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckFoodAllergy(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.FoodAllergy);
            }
            finally
            {
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_002_CheckDrugAllergy()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
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
            var specialNoteItem = new SpecialNoteItem();
            var ptDiseaseModels = new List<PtDiseaseModel>();
            var familyItems = new List<FamilyItem>();
            var isDataOfDb = true;

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            //Read data test
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: true,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckDrugAllergy(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.DrugAllergy);
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }

        }

        /// <summary>
        /// IsCheckingAllergy == true
        /// Test CheckDrugAllergy
        /// </summary>
        [Test]
        public void TC_003_GetErrorFromListOrder_Test_IsCheckingAllergy_CheckDrugAllergy_Is_Error()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            //Read data test
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: true,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.Count > 0);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.DrugAllergy);
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }

        }

        /// <summary>
        /// IsCheckingAllergy == true
        /// Test CheckFoodAllergy 
        /// </summary>
        [Test]
        public void TC_004_GetErrorFromListOrder_Test_IsCheckingAllergy_CheckFoodAllergy_Is_Error()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
            new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var odrInfoModel = new List<OrdInfoModel>()
            {
            new OrdInfoModel(21, 0, ordInfDetails)
            };

            #region Setup data test

            //FoodAllergyLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 0);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null)
            {
                systemConf.Val = 4;
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
                    Val = 4
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Read data test
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: true,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.Count > 0);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.DrugAllergy);
            }
            finally
            {
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_005_CheckAge()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "6220816AGE", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            //Read data test
            //AgeLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 3);
            var temp = systemConf?.Val ?? 8;
            int settingLevel = 8;
            if (systemConf != null)
            {
                systemConf.Val = settingLevel;
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
                    Val = settingLevel
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            var tenMsts = CommonCheckerData.ReadTenMst("AGE008", "AGE008");
            var m14 = CommonCheckerData.ReadM14AgeCheck();
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS003");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M14AgeCheck.AddRange(m14);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: true,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckAge(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Age);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M14AgeCheck.RemoveRange(m14);
                if (systemConf != null) systemConf.Val = temp;
                tenantTracking.SaveChanges();
            }

        }

        /// <summary>
        /// IsCheckingAge = true
        /// </summary>
        [Test]
        public void TC_006_GetErrorFromListOrder_IsCheckingAge_Is_True()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "6220816AGE", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            //Read data test
            //AgeLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 3);
            var temp = systemConf?.Val ?? 8;
            int settingLevel = 8;
            if (systemConf != null)
            {
                systemConf.Val = settingLevel;
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
                    Val = settingLevel
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            var tenMsts = CommonCheckerData.ReadTenMst("AGE008", "AGE008");
            var m14 = CommonCheckerData.ReadM14AgeCheck();
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS003");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M14AgeCheck.AddRange(m14);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: true,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Age);
                Assert.True(result.Count > 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M14AgeCheck.RemoveRange(m14);
                if (systemConf != null) systemConf.Val = temp;
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckDisease CommonMedicalCheck
        [Test]
        public void TC_007_CheckDisease()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, itemCd: "937DIS008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, itemCd: "22DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id3", 21, itemCd: "101DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id4", 21, itemCd: "776DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id5", 21, itemCd: "717DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

            #region Setup data test

            //DiseaseLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 2);
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
                    HpId = 1,
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

            var tenMsts = CommonCheckerData.ReadTenMst("DIS008", "DIS008");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("DIS008");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS008");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            var ptFamilyReki = CommonCheckerData.ReadPtFamilyReki();
            var ptFamilies = CommonCheckerData.ReadPtFamily();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.PtFamilyRekis.AddRange(ptFamilyReki);
            tenantTracking.PtFamilys.AddRange(ptFamilies);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckDisease(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Disease);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42DisCon);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42DrugMainEx);
                tenantTracking.PtByomeis.RemoveRange(ptByomei);
                tenantTracking.PtFamilyRekis.RemoveRange(ptFamilyReki);
                tenantTracking.PtFamilys.RemoveRange(ptFamilies);
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromListOrder CommonMedicalCheck
        //Test IsCheckingDisease = true
        [Test]
        public void TC_008_GetErrorFromListOrder_CheckDisease()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, itemCd: "937DIS008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, itemCd: "22DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id3", 21, itemCd: "101DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id4", 21, itemCd: "776DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            new OrdInfoDetailModel("id5", 21, itemCd: "717DIS008", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails)
        };

            #region Setup data test

            //DiseaseLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 2);
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
                    HpId = 1,
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

            var tenMsts = CommonCheckerData.ReadTenMst("DIS008", "DIS008");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("DIS008");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("DIS008");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            var ptFamilyReki = CommonCheckerData.ReadPtFamilyReki();
            var ptFamilies = CommonCheckerData.ReadPtFamily();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.PtFamilyRekis.AddRange(ptFamilyReki);
            tenantTracking.PtFamilys.AddRange(ptFamilies);
            tenantTracking.SaveChanges();
            var foodAllergyChecker = new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>();
            var cache = new MasterDataCacheService(TenantProvider);
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            foodAllergyChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: true,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Disease);
                Assert.True(result.Count > 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42DisCon);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42DrugMainEx);
                tenantTracking.PtByomeis.RemoveRange(ptByomei);
                tenantTracking.PtFamilyRekis.RemoveRange(ptFamilyReki);
                tenantTracking.PtFamilys.RemoveRange(ptFamilies);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckKinkiTain CommonMedicalCheck
        [Test]
        public void TC_009_CheckKinkiTain()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails)
        };

            #region Setup data test

            //DiseaseLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup KinkiLevelSetting 
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null)
            {
                systemConf.Val = 4;
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
                    Val = 4
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Setup M01_KINKI
            var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
            var m01Kinki = new M01Kinki();
            if (m01 == null)
            {
                m01Kinki.ACd = "1190700";
                m01Kinki.BCd = "1190700";
                m01Kinki.CmtCd = "D006";
                m01Kinki.SayokijyoCd = "S2001";
                m01Kinki.KyodoCd = "";
                m01Kinki.Kyodo = "3";
                m01Kinki.DataKbn = "1";

                tenantTracking.M01Kinki.Add(m01Kinki);
            }

            tenantTracking.SaveChanges();

            int ptId = 1233;
            var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(ptId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "6220816T3" }, 20230505, ptId);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckKinkiTain(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.KinkiTain);
            }
            finally
            {
                systemConf.Val = temp;
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                if (m01 == null)
                {
                    tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                }
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromListOrder CommonMedicalCheck
        //Test IsCheckingKinki = true
        [Test]
        public void TC_010_GetErrorFromListOrder_CheckKinkiTain_IsError_IsCheckingKinki_True()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails)
        };

            #region Setup data test

            //DiseaseLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup KinkiLevelSetting 
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null)
            {
                systemConf.Val = 4;
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
                    Val = 4
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Setup M01_KINKI
            var m01 = tenantTracking.M01Kinki.FirstOrDefault(p => p.ACd == "1190700" && p.BCd == "1190700" && p.CmtCd == "D006" && p.SayokijyoCd == "S2001");
            var m01Kinki = new M01Kinki();
            if (m01 == null)
            {
                m01Kinki.ACd = "1190700";
                m01Kinki.BCd = "1190700";
                m01Kinki.CmtCd = "D006";
                m01Kinki.SayokijyoCd = "S2001";
                m01Kinki.KyodoCd = "";
                m01Kinki.Kyodo = "3";
                m01Kinki.DataKbn = "1";

                tenantTracking.M01Kinki.Add(m01Kinki);
            }

            tenantTracking.SaveChanges();

            int ptId = 1233;
            var tenMsts = CommonCheckerData.ReadTenMst("T3", "");
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(ptId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "6220816T3" }, 20230505, ptId);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.KinkiTain);
                Assert.True(result.Count > 0);
            }
            finally
            {
                systemConf.Val = temp;
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                if (m01 == null)
                {
                    tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                }
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckKinkiOTC CommonMedicalCheck
        [Test]
        public void TC_011_CheckKinkiOTC()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
            var temp = systemConf?.Val ?? 0;
            int settingLevel = 5;
            if (systemConf != null)
            {
                systemConf.Val = settingLevel;
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
                    Val = settingLevel
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }
            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            var prOtcDrugs = CommonCheckerData.ReadPtOtcDrug();
            tenantTracking.PtOtcDrug.AddRange(prOtcDrugs);
            var m38Ingredients = CommonCheckerData.ReadM38Ingredients("");
            tenantTracking.M38Ingredients.AddRange(m38Ingredients);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "611170008" }, 20230505, 1231);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckKinkiOTC(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.KinkiOTC);
            }
            finally
            {
                systemConf.Val = temp;
                tenantTracking.PtOtcDrug.RemoveRange(prOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38Ingredients);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }


        //Test GetErrorFromListOrder CommonMedicalCheck
        //Test IsCheckingKinki = true
        //Test CheckKinkiOTC Error
        [Test]
        public void TC_012_GetErrorFromListOrder_CheckKinkiOTC_IsError_IsCheckingKinki_True()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);
            var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6220816T3",
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
            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: addedOrdInfDetails)
        };

            #region Setup data test

            //DiseaseLevelSetting
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
            var temp = systemConf?.Val ?? 0;
            int settingLevel = 5;
            if (systemConf != null)
            {
                systemConf.Val = settingLevel;
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
                    Val = settingLevel
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }
            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            var prOtcDrugs = CommonCheckerData.ReadPtOtcDrug();
            tenantTracking.PtOtcDrug.AddRange(prOtcDrugs);
            var m38Ingredients = CommonCheckerData.ReadM38Ingredients("");
            tenantTracking.M38Ingredients.AddRange(m38Ingredients);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "6220816T3", "Y101" }, 20230505, 1231);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.KinkiOTC);
                Assert.True(result.Count > 0);
            }
            finally
            {
                systemConf.Val = temp;
                tenantTracking.PtOtcDrug.RemoveRange(prOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38Ingredients);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckKinkiSupple CommonMedicalCheck
        [Test]
        public void TC_013_CheckKinkiSupple()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

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

            #region Setup data test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            //setup SystemCof KinkiLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
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
                    GrpEdaNo = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 3
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Setup Data test
            var ptSupples = CommonCheckerData.ReadPtSupple();
            var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef();
            var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode();
            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
            var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
            kinkiSuppleChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckKinkiSupple(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.KinkiSupplement);
            }
            finally
            {
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromListOrder CommonMedicalCheck
        //Test CheckKinkiSupple Is Error
        [Test]
        public void TC_014_GetErrorFromListOrder_CheckKinkiSupple_IsError_IsCheckingKinki_True()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

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

            #region Setup data test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            //setup SystemCof KinkiLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
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
                    GrpEdaNo = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 3
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            //Setup Data test
            var ptSupples = CommonCheckerData.ReadPtSupple();
            var m41IndexDef = CommonCheckerData.ReadM41SuppleIndexdef();
            var m41IndexCode = CommonCheckerData.ReadM41SuppleIndexcode();
            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41IndexDef);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41IndexCode);
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "936DIS003" }, 20230505, 1231);
            var kinkiSuppleChecker = new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>();
            kinkiSuppleChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.KinkiSupplement);
                Assert.True(result.Count > 0);
            }
            finally
            {
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41IndexDef);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41IndexCode);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }

        //Test IsCheckingDays
        //Test CheckDayLimit CommonMedicalCheck

        [Test]
        public void TC_015_CheckDayLimit()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day013", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day013", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(20, 0, ordInfDetails),
        };

            #region Setup data test
            //Setup Data test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("day013", "day013");
            var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day013");
            var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day013");

            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = 999;
            dayLimitChecker.PtID = 111;
            dayLimitChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dayLimitChecker.InitFinder(tenantNoTracking, cache);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: true,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckDayLimit(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Days);
            }
            finally
            {
                //Clear Data test
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.SaveChanges();
            }

        }

        //Test IsCheckingDays
        //Test GetErrorFromListOrder CommonMedicalCheck
        //Test CheckDayLimit Error

        [Test]
        public void TC_016_GetErrorFromListOrder_CheckDayLimit()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day013", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day013", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new List<OrdInfoModel>()
        {
            new OrdInfoModel(20, 0, ordInfDetails),
        };

            #region Setup data test
            //Setup Data test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("day013", "day013");
            var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day013");
            var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day013");

            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = 999;
            dayLimitChecker.PtID = 111;
            dayLimitChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dayLimitChecker.InitFinder(tenantNoTracking, cache);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: true,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Days);
                Assert.True(result.Count > 0);
            }
            finally
            {
                //Clear Data test
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckDosage
        [Test]
        public void TC_017_CheckDosage()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);

            //DosageDrinkingDrugSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
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
                    GrpCd = 2023,
                    GrpEdaNo = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 1
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            tenantTracking.SaveChanges();
            var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
            dosageChecker.HpID = 999;
            dosageChecker.PtID = 1231;
            dosageChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dosageChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: true,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckDosage(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Dosage);
            }
            finally
            {
                //Clear Data test
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }

        //Test IsCheckingDosage
        //Test IsCheckingDosage = true
        //Test GetErrorFromListOrder CheckDosage is error
        [Test]
        public void TC_018_GetErrorFromListOrder_CheckDosage_IsError()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "620160501",
                                    itemName: "ＰＬ配合顆粒",
                                    suryo: 100,
                                    unitName: "g",
                                    termVal: 0,
                                    syohoKbn: 2,
                                    syohoLimitKbn: 1,
                                    drugKbn: 1,
                                    yohoKbn: 2,
                                    ipnCd: "1180107D1",
                                    bunkatu: "",
                                    masterSbt: "Y",
                                    bunkatuKoui: 0),

            new OrdInfoDetailModel( id: "id1",
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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);

            //DosageDrinkingDrugSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2023 && p.GrpEdaNo == 2);
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
                    GrpCd = 2023,
                    GrpEdaNo = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 1
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }

            tenantTracking.SaveChanges();
            var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
            dosageChecker.HpID = 999;
            dosageChecker.PtID = 1231;
            dosageChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dosageChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: true,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromListOrder(odrInfoModel, new(), new(), new(), true);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Dosage);
                Assert.True(result.Count > 0);
            }
            finally
            {
                //Clear Data test
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckDuplication
        [Test]
        public void TC_019_CheckDuplication()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
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

            var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);

            //DosageDrinkingDrugSetting
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

            tenantTracking.SaveChanges();

            var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
            dosageChecker.HpID = 999;
            dosageChecker.PtID = 1231;
            dosageChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dosageChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: true,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckDuplication(currentList, odrInfoModel);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Duplication);
            }
            finally
            {
                //Clear Data test
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromOrder and CheckDuplication Error
        //Test IsCheckingDuplication = true
        [Test]
        public void TC_020_GetErrorFromListOrder_CheckDuplication()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
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

            var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
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

            #region Setup data test

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);

            //DosageDrinkingDrugSetting
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

            tenantTracking.SaveChanges();

            var dosageChecker = new DosageChecker<OrdInfoModel, OrdInfoDetailModel>();
            dosageChecker.HpID = 999;
            dosageChecker.PtID = 1231;
            dosageChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            dosageChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: true,
                                                      isCheckingKinki: false,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromOrder(currentList, odrInfoModel);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Duplication);
                Assert.True(result.Count > 0);
            }
            finally
            {
                //Clear Data test
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckKinki
        [Test]
        public void TC_021_CheckKinki()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61UTKINKI3", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var currentOdrInfoDetailModels = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61UTKINKI3", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var currentOdrInfoModel = new List<OrdInfoModel>()
        {
           new OrdInfoModel (21, 0, currentOdrInfoDetailModels)
        };

            #region Setup data test
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
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
            systemConf.Val = temp;

            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            var tenMsts = CommonCheckerData.ReadTenMst("KINKI3", "");
            tenantTracking.TenMsts.AddRange(tenMsts);

            tenantTracking.SaveChanges();

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckKinki(currentOdrInfoModel, odrInfoModel);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Kinki);
            }
            finally
            {
                //Clear Data test
                systemConf.Val = temp;
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromOrder and CheckKinki Error
        //Test IsCheckingKinki = true
        [Test]
        public void TC_022_GetErrorFromOrder_CheckKinki_IsError_IsCheckingKinki()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61UTKINKI3", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var currentOdrInfoDetailModels = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61UTKINKI3", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var currentOdrInfoModel = new List<OrdInfoModel>()
        {
           new OrdInfoModel (21, 0, currentOdrInfoDetailModels)
        };

            #region Setup data test
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
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
            systemConf.Val = temp;

            var m01Kinki = CommonCheckerData.ReadM01Kinki();
            tenantTracking.M01Kinki.AddRange(m01Kinki);
            var tenMsts = CommonCheckerData.ReadTenMst("KINKI3", "");
            tenantTracking.TenMsts.AddRange(tenMsts);

            tenantTracking.SaveChanges();

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromOrder(currentOdrInfoModel, odrInfoModel);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Kinki);
                Assert.True(result.Count > 0);
            }
            finally
            {
                //Clear Data test
                systemConf.Val = temp;
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinki);
                tenantTracking.SaveChanges();
            }

        }

        //Test CheckKinkiUser
        [Test]
        public void TC_023_CheckKinkiUser()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K08",
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

            var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

            var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K08",
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

            #region Setup data test
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("K08", "K08");
            var kinkiMsts = CommonCheckerData.ReadKinkiMst("K08");
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.KinkiMsts.AddRange(kinkiMsts);

            //KinkiLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
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
                    GrpEdaNo = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 3
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }
            tenantTracking.SaveChanges();

            var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
            kinkiUserChecker.HpID = 999;
            kinkiUserChecker.PtID = 111;
            kinkiUserChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "6404K08", "6111K08" }, 20230505, 1231);
            kinkiUserChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.CheckKinkiUser(currentList, odrInfoModel);

                // Assert
                Assert.True(result.IsError == true);
                Assert.True(result.CheckerType == RealtimeCheckerType.Kinki);
            }
            finally
            {
                //Clear Data test
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
                tenantTracking.SaveChanges();
            }

        }

        //Test GetErrorFromOrder and CheckKinkiUser Error
        //Test IsCheckingKinki = true
        [Test]
        public void TC_024_CheckKinkiUser_IsCheckingKinki_IsTrue_CheckKinkiUser_Is_Error()
        {
            //Mock
            var realtimeOrderErrorFinder = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, realtimeOrderErrorFinder.Object);

            //setup
            var currentOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6111K08",
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

            var currentList = new List<OrdInfoModel>()
        {
            new OrdInfoModel(odrKouiKbn: 21, santeiKbn: 0, ordInfDetails: currentOrdInfDetails)
        };

            var addedOrdInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel( id: "id1",
                                    sinKouiKbn: 20,
                                    itemCd: "6404K08",
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

            #region Setup data test
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("K08", "K08");
            var kinkiMsts = CommonCheckerData.ReadKinkiMst("K08");
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.KinkiMsts.AddRange(kinkiMsts);

            //KinkiLevelSetting
            var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2027 && p.GrpEdaNo == 1);
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
                    GrpEdaNo = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 3
                };
                tenantTracking.SystemConfs.Add(systemConf);
            }
            tenantTracking.SaveChanges();

            var kinkiUserChecker = new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>();
            kinkiUserChecker.HpID = 999;
            kinkiUserChecker.PtID = 111;
            kinkiUserChecker.Sinday = 20230101;
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "6404K08", "6111K08" }, 20230505, 1231);
            kinkiUserChecker.InitFinder(tenantNoTracking, cache);

            #endregion

            // Set up your CheckerCondition

            commonMedicalCheck.CheckerCondition = new RealTimeCheckerCondition(
                                                      isCheckingDuplication: false,
                                                      isCheckingKinki: true,
                                                      isCheckingAllergy: false,
                                                      isCheckingDosage: false,
                                                      isCheckingDays: false,
                                                      isCheckingAge: false,
                                                      isCheckingDisease: false,
                                                      isCheckingInvalidData: false,
                                                      isCheckingAutoCheck: false
                                                      );

            try
            {
                // Act
                var result = commonMedicalCheck.GetErrorFromOrder(currentList, odrInfoModel);

                // Assert
                Assert.True(result.First().IsError == true);
                Assert.True(result.First().CheckerType == RealtimeCheckerType.Kinki);
                Assert.True(result.Count > 0);
            }
            finally
            {
                //Clear Data test
                if (systemConf != null) systemConf.Val = temp;

                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
                tenantTracking.SaveChanges();
            }

        }
    }
}
