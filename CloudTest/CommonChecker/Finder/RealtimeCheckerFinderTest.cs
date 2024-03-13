using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Caches.Interface;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;
using Helper.Common;
using Moq;
using PtAlrgyDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyDrugModel;
using PtAlrgyFoodModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyFoodModel;
using PtKioRekiModelStandard = Domain.Models.SpecialNote.ImportantNote.PtKioRekiModel;
using PtOtcDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtcDrugModel;
using PtOtherDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtherDrugModel;
using PtSuppleModelStandard = Domain.Models.SpecialNote.ImportantNote.PtSuppleModel;

namespace CloudUnitTest.CommonChecker.Finder
{
    public class RealtimeCheckerFinderTest : BaseUT
    {
        [Test]
        public void TC_001_CheckFoodAllergy_Test_TenpuLevel()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int level = 0;
            bool isDataOfDb = true;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("610406404", "TC001"),
                new ItemCodeModel("611170008", "TC001"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckFoodAllergy(hpId, ptId, sinDay, itemCodeModelList, level, new(), isDataOfDb);

                // Assert
                Assert.True(!result.Any());
            }
            catch (Exception)
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_002_CheckFoodAllergy_Test_Check_NormalCase()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int level = 4;
            bool isDataOfDb = true;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("610406404", "TC001"),
                new ItemCodeModel("611170008", "TC002"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckFoodAllergy(hpId, ptId, sinDay, itemCodeModelList, level, new(), isDataOfDb);

                // Assert
                Assert.True(result.Any() && result.First().Id == "TC002");
            }
            catch (Exception)
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_003_CheckDuplicatedComponentForDuplication_Test_Duplicated()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("613110017", "TC001"),
                new ItemCodeModel("613110018", "TC002"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckDuplicatedComponentForDuplication(hpId, ptId, sinDay, itemCodeModelList, itemCodeModelList, setting);

                // Assert
                Assert.True(result.Any() && result.Count() == 2 && result.First().Id == "TC001" && result.Last().Id == "TC002");
            }
            catch (Exception)
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_004_CheckDuplicatedComponentForDuplication_Test_ItemCodeList_NotDuplicated()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("613110019", "TC001"),
                new ItemCodeModel("613110020", "TC002"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckDuplicatedComponentForDuplication(hpId, ptId, sinDay, itemCodeModelList, itemCodeModelList, setting);

                // Assert
                Assert.False(result.Any());
            }
            catch (Exception)
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }

        }


        [Test]
        public void TC_005_CheckProDrug()
        {
            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();


            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2708", "Id001"),
                new ItemCodeModel("UT2708", "Id002"),
                };

            var listCompare = new List<string>() { "UT2701" };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckProDrug(hpId, ptId, sinDay, itemCodeModelList, listCompare);

                // Assert
                Assert.True(result.Any() && result.First().Id == "Id001");
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEd);
                tenantTracking.M56ProdrugCd.RemoveRange(m56Prodrugs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// test yohocd checking equal yohocd listCompare
        /// </summary>
        [Test]
        public void TC_006_CheckProDrugForDuplication_Test_Equal_YohoCd()
        {
            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD(hpId);
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2701", "Id1"),
                };
            var listCompare = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2700", "Id2"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckProDrugForDuplication(hpId, ptId, sinDay, itemCodeModelList, listCompare, setting);

                // Assert
                Assert.True(result.Any() && result.First().ItemCd == "UT2701" && result.First().Level == 2);
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEd);
                tenantTracking.M56ProdrugCd.RemoveRange(m56Prodrugs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);
                tenantTracking.SaveChanges();
            }

        }

        /// <summary>
        /// test yohocd checking equal yohocd listCompare
        /// </summary>
        [Test]
        public void TC_007_CheckProDrugForDuplication_Test_ZensinsayoFlg_Is_1()
        {
            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD(hpId);
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2702", "Id1"),
                };
            var listCompare = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2703", "Id2"),
                };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckProDrugForDuplication(hpId, ptId, sinDay, itemCodeModelList, listCompare, setting);

                // Assert
                Assert.True(result.Any() && result.First().ItemCd == "UT2702" && result.First().Level == 2);
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEd);
                tenantTracking.M56ProdrugCd.RemoveRange(m56Prodrugs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_008_CheckSameComponent()
        {
            //Setup Param
            int hpId = 1;
            long ptId = 1231;
            int sinDay = 20230101;

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug(hpId);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("613110017", "Id1"),
                };
            var listCompare = new List<string>() { "620675301" };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckSameComponent(hpId, ptId, sinDay, itemCodeModelList, listCompare);

                // Assert
                Assert.True(result.Any() && result.First().ItemCd == "613110017" && result.First().Level == 3);
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlrgyDrugs);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_009_CheckDiseaseChecker_Test_IsDataDb_True()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 5;
            bool dataDb = true;

            var tenMsts = CommonCheckerData.ReadTenMst("TC009", "TC009");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "TC009");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "TC009");
            var ptByomei = CommonCheckerData.ReadPtByomei(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936TC009", "id1"),
            new ItemCodeModel("22TC009", "id2"),
            new ItemCodeModel("101TC009", "id3"),
            new ItemCodeModel("776TC009", "id4"),
            new ItemCodeModel("717TC009", "id5"),
        };

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForCurrentDisease(hpId, ptId, settingLevel, sinDate, listItemCode, new(), dataDb);

                //Assert
                Assert.True(result.Count == 5);
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
        public void TC_010_CheckDiseaseChecker_Test_IsDataDb_False()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 5;
            bool dataDb = false;

            var tenMsts = CommonCheckerData.ReadTenMst("TC010", "TC010");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "TC010");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "TC010");
            var ptByomei = CommonCheckerData.ReadPtByomei(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936TC010", "id1"),
            new ItemCodeModel("22TC010", "id2"),
            new ItemCodeModel("101TC010", "id3"),
            new ItemCodeModel("776TC010", "id4"),
            new ItemCodeModel("717TC010", "id5"),
        };


            //setup List Disease
            var ptDiseases = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId:999, ptId: 1231, seqNo: 0, byomeiCd:"250001" , sortNo: 1, new(), byomei: "UT",
                                   startDate:20190101, tenkiKbn: 0, tenkiDate: 20190202,
                                   syubyoKbn: 0,  sikkanKbn:0,  nanbyoCd:0,  isNodspRece:0,  isNodspKarte:0,
                                   isDeleted:0,  id:1,  isImportant:0,  sinDate: 20230101,  icd10:"",  icd102013:"",
                                   icd1012013:"", icd1022013:"",  hokenPid:0,  hosokuCmt:"",  togetuByomei:0,  delDate:0
                                   ),
                new PtDiseaseModel(hpId:999, ptId: 1231, seqNo: 0, byomeiCd:"493900" , sortNo: 1, new(), byomei: "UT",
                                   startDate:20190101, tenkiKbn: 1, tenkiDate: 20230606,
                                   syubyoKbn: 0,  sikkanKbn:0,  nanbyoCd:0,  isNodspRece:0,  isNodspKarte:0,
                                   isDeleted:0,  id:1,  isImportant:0,  sinDate: 20200101,  icd10:"",  icd102013:"",
                                   icd1012013:"", icd1022013:"",  hokenPid:0,  hosokuCmt:"",  togetuByomei:0,  delDate:0
                                   ),
            };

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForCurrentDisease(hpId, ptId, settingLevel, sinDate, listItemCode, ptDiseases, dataDb);

                //Assert
                Assert.True(result.Count == 3);
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
        public void TC_011_CheckDuplicatedClassForDuplication()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var m56YjDrugs = CommonCheckerData.READ_M56_YJ_DRUG_CLASS(hpId);
            var m56DrugClasses = CommonCheckerData.READ_M56_DRUG_CLASS(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56YjDrugClass.AddRange(m56YjDrugs);
            tenantTracking.M56DrugClass.AddRange(m56DrugClasses);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2704", "id1"),
        };

            var compareItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2705", "id1"),
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2704", "UT2705" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedClassForDuplication(hpId, ptId, sinDate, listItemCode, compareItemCode, settingLevel);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2704");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56YjDrugClass.RemoveRange(m56YjDrugs);
                tenantTracking.M56DrugClass.RemoveRange(m56DrugClasses);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_012_CheckDuplicatedClassForDuplication_Test_Current_And_Checking_Are_Same_ItemCd()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var m56YjDrugs = CommonCheckerData.READ_M56_YJ_DRUG_CLASS(hpId);
            var m56DrugClasses = CommonCheckerData.READ_M56_DRUG_CLASS(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56YjDrugClass.AddRange(m56YjDrugs);
            tenantTracking.M56DrugClass.AddRange(m56DrugClasses);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2704", "id1"),
        };

            var compareItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2704", "id1"),
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2704", "UT2704" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedClassForDuplication(hpId, ptId, sinDate, listItemCode, compareItemCode, settingLevel);

                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56YjDrugClass.RemoveRange(m56YjDrugs);
                tenantTracking.M56DrugClass.RemoveRange(m56DrugClasses);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_013_CheckSameComponentForDuplication_Test_Checking_And_Current_YohoCd_Are_Equal()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var m56ExAnalogues = CommonCheckerData.READ_M56_EX_ANALOGUE(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.M56ExAnalogue.AddRange(m56ExAnalogues);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2704", "id1"),
        };

            var compareItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2705", "id1"),
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2704", "UT2705" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckSameComponentForDuplication(hpId, ptId, sinDate, listItemCode, compareItemCode, settingLevel);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2704" && result.First().Level == 3);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);
                tenantTracking.M56ExAnalogue.RemoveRange(m56ExAnalogues);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_014_CheckSameComponentForDuplication_Test_Checking_ZensinsayoFlg_Is_1()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN(hpId);
            var m56ExAnalogues = CommonCheckerData.READ_M56_EX_ANALOGUE(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.M56ExAnalogue.AddRange(m56ExAnalogues);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2706", "id1"),
        };

            var compareItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2707", "id2"),
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2706", "UT2707" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckSameComponentForDuplication(hpId, ptId, sinDate, listItemCode, compareItemCode, settingLevel);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2706" && result.First().Level == 3);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);
                tenantTracking.M56ExAnalogue.RemoveRange(m56ExAnalogues);
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56ExIngrdtMains);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_015_CheckDuplicatedClass()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56AlrgyDerivatives = CommonCheckerData.READ_M56_ALRGY_DERIVATIVES(hpId);
            var m56DrvalrgyCodes = CommonCheckerData.READ_M56_DRVALRGY_CODE(hpId);
            tenantTracking.M56AlrgyDerivatives.AddRange(m56AlrgyDerivatives);
            tenantTracking.M56DrvalrgyCode.AddRange(m56DrvalrgyCodes);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2706", "id1"),
        };

            var compareItemCode = new List<string>()
        {
            "UT2707",
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2706", "UT2707" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedClass(hpId, ptId, sinDate, listItemCode, compareItemCode);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2706" && result.First().Level == 4);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56AlrgyDerivatives.RemoveRange(m56AlrgyDerivatives);
                tenantTracking.M56DrvalrgyCode.RemoveRange(m56DrvalrgyCodes);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_016_CheckDuplicatedComponent_Test_GetM56ExEdIngredientList_Test_Sbt_Is_1()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2709", "id1"),
        };

            var compareItemCode = new List<string>()
        {
            "UT2710",
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2709", "UT2710" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedComponent(hpId, ptId, sinDate, listItemCode, compareItemCode);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2709" && result.First().Level == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_017_CheckDuplicatedComponent_Test_GetM56ExEdIngredientList_Test_Sbt_Is_2()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2711", "id1"),
        };

            var compareItemCode = new List<string>()
        {
            "UT2712",
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2711", "UT2712" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedComponent(hpId, ptId, sinDate, listItemCode, compareItemCode);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2711" && result.First().Level == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_018_CheckDuplicatedComponent_Test_GetM56ExEdIngredientList_Test_TenkabutuCheck_Not_Is_1()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2711", "id1"),
        };

            var compareItemCode = new List<string>()
        {
            "UT2713",
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2711", "UT2713" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedComponent(hpId, ptId, sinDate, listItemCode, compareItemCode);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2711" && result.First().Level == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_019_CheckDuplicatedComponent_Test_GetM56ExEdIngredientList_Test_Checking_SeqNo_Is_000()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExEdIngredients = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS(hpId);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2714", "id1"),
        };

            var compareItemCode = new List<string>()
        {
            "UT2711",
        };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2711", "UT2714" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckDuplicatedComponent(hpId, ptId, sinDate, listItemCode, compareItemCode);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2714" && result.First().Level == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_020_CheckContraindicationForHistoryDisease_Test_DataDb_True()
        {
            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230101;
            int level = 5;
            bool isDataDb = true;

            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var ptKioRekis = CommonCheckerData.ReadPtKioReki(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m42Contraindis = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "");
            var m42ContraindiDisCons = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "");

            try
            {
                tenantTracking.PtKioRekis.AddRange(ptKioRekis);
                tenantTracking.TenMsts.AddRange(tenMsts);
                tenantTracking.M42ContraindiDrugMainEx.AddRange(m42Contraindis);
                tenantTracking.M42ContraindiDisCon.AddRange(m42ContraindiDisCons);
                tenantTracking.SaveChanges();
                var listItemCode = new List<ItemCodeModel>()
                {
                    new ItemCodeModel("937", "id1"),
                };

                // Arrange
                var cache = new MasterDataCacheService(TenantProvider);
                cache.InitCache(hpId, new List<string>() { "937" }, sinDate, ptId);
                var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);


                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForHistoryDisease(hpId, ptId, level, sinDate, listItemCode, new(), isDataDb);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "937" && result.First().ByotaiCd == "3");
            }
            finally
            {
                tenantTracking.PtKioRekis.RemoveRange(ptKioRekis);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42Contraindis);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42ContraindiDisCons);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_021_CheckContraindicationForHistoryDisease_Test_DataDb_False()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230101;
            int level = 5;
            bool isDataDb = false;

            var ptKioRekis = CommonCheckerData.ReadPtKioReki(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m42Contraindis = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "");
            var m42ContraindiDisCons = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "");
            tenantTracking.PtKioRekis.AddRange(ptKioRekis);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42Contraindis);
            tenantTracking.M42ContraindiDisCon.AddRange(m42ContraindiDisCons);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("937", "id1"),
        };

            var ptKioRekiModels = new List<PtKioRekiModelStandard>()
            {
                new PtKioRekiModelStandard(hpId, ptId, 0, 0, "250001", "", "", 20220101, "", 0),
            };

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "937" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForHistoryDisease(hpId, ptId, level, sinDate, listItemCode, ptKioRekiModels, isDataDb);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "937" && result.First().ByotaiCd == "3");
            }
            finally
            {
                tenantTracking.PtKioRekis.RemoveRange(ptKioRekis);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42Contraindis);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42ContraindiDisCons);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_022_CheckContraindicationForFamilyDisease_Test_IsDataDb_True()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230101;
            int level = 5;
            bool isDataDb = true;

            var ptKioRekis = CommonCheckerData.ReadPtKioReki(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m42Contraindis = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "");
            var m42ContraindiDisCons = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "");
            var ptFamilyRekis = CommonCheckerData.ReadPtFamilyReki(hpId);
            var ptFamilies = CommonCheckerData.ReadPtFamily(hpId);
            tenantTracking.PtKioRekis.AddRange(ptKioRekis);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42Contraindis);
            tenantTracking.M42ContraindiDisCon.AddRange(m42ContraindiDisCons);
            tenantTracking.PtFamilys.AddRange(ptFamilies);
            tenantTracking.PtFamilyRekis.AddRange(ptFamilyRekis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("937", "id1"),
        };


            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "937" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForFamilyDisease(hpId, ptId, level, sinDate, listItemCode, new(), isDataDb);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "937" && result.First().ByotaiCd == "3" && result.First().TenpuLevel == 2);
            }
            finally
            {
                tenantTracking.PtKioRekis.RemoveRange(ptKioRekis);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42Contraindis);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42ContraindiDisCons);
                tenantTracking.PtFamilyRekis.RemoveRange(ptFamilyRekis);
                tenantTracking.PtFamilys.RemoveRange(ptFamilies);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_023_CheckContraindicationForFamilyDisease_Test_IsDataDb_False()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int level = 5;
            bool isDataDb = false;
            var ptKioRekis = CommonCheckerData.ReadPtKioReki(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "", hpId);
            var m42Contraindis = CommonCheckerData.ReadM42ContaindiDrugMainEx(hpId, "");
            var m42ContraindiDisCons = CommonCheckerData.ReadM42ContaindiDisCon(hpId, "");
            var ptFamilyRekis = CommonCheckerData.ReadPtFamilyReki(hpId);
            var ptFamilies = CommonCheckerData.ReadPtFamily(hpId);

            tenantTracking.PtKioRekis.AddRange(ptKioRekis);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42Contraindis);
            tenantTracking.M42ContraindiDisCon.AddRange(m42ContraindiDisCons);
            tenantTracking.PtFamilys.AddRange(ptFamilies);
            tenantTracking.PtFamilyRekis.AddRange(ptFamilyRekis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("937", "id1"),
        };

            var families = new List<FamilyModel>
            {
                new FamilyModel(99999, ptId, 0, "UT", 99999, ptId, "UT", "kana name", 0, 19981212, 25, 0, 0, "Biko", 1,
                                new List<PtFamilyRekiModel>
                                {
                                    new PtFamilyRekiModel(1, "250001", "Unit Test", "CMT", 1),
                                },
                                "Disease Name"
                                )
            };

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "937" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckContraindicationForFamilyDisease(hpId, ptId, level, sinDate, listItemCode, families, isDataDb);

                //Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "937" && result.First().ByotaiCd == "3" && result.First().TenpuLevel == 2);
            }
            finally
            {
                tenantTracking.PtKioRekis.RemoveRange(ptKioRekis);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M42ContraindiDrugMainEx.RemoveRange(m42Contraindis);
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42ContraindiDisCons);
                tenantTracking.PtFamilyRekis.RemoveRange(ptFamilyRekis);
                tenantTracking.PtFamilys.RemoveRange(ptFamilies);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_024_CheckKinki_Test_Kyodo_Greater_Than_Level()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 2;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2714", "id1"),
            new ItemCodeModel("UT2715", "id2"),
        };

            var listDrugItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2715", "id3"),
            new ItemCodeModel("UT2716", "id4"),
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            ///Act
            try
            {
                var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

                ///Assert
                Assert.True(!result.Any());
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_025_CheckKinki_Test_Kyodo_Equal_Than_Level()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 3;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2714", "id1"),
            new ItemCodeModel("UT2715", "id2"),
        };

            var listDrugItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2715", "id3"),
            new ItemCodeModel("UT2716", "id4"),
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            ///Act
            try
            {
                var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

                ///Assert
                Assert.True(result.Any());
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_026_CheckKinki_Test_Kyodo_Less_Than_Level()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 4;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2714", "id1"),
            new ItemCodeModel("UT2715", "id2"),
        };

            var listDrugItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2715", "id3"),
            new ItemCodeModel("UT2716", "id4"),
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            ///Act
            try
            {
                var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

                ///Assert
                Assert.True(result.Any());
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_027_CheckKinki_Test_AddedOrderSubYjCode_IsNull()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 4;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT777777", "id1"),
        };

            var listDrugItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2715", "id3"),
            new ItemCodeModel("UT2716", "id4"),
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            ///Act
            try
            {
                var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

                ///Assert
                Assert.True(!result.Any());
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_028_CheckKinki_Test_currentOrderSubYjCode()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 4;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT2714", "id1"),
            new ItemCodeModel("UT2715", "id2"),
        };

            var listDrugItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("UT777777", "id3")
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            ///Act
            try
            {
                var result = realTimeCheckerFinder.CheckKinki(hpId, settingLevel, sinDate, listDrugItemCode, listItemCode);

                ///Assert
                Assert.True(!result.Any());
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_029_CheckKinkiUser_Test_KINKI_MST()
        {
            var hpId = 999;

            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("029", "029", hpId);
            var kinkiMsts = CommonCheckerData.ReadKinkiMst("029", hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.KinkiMsts.AddRange(kinkiMsts);
            tenantTracking.SaveChanges();

            var ptId = 1231;
            var settingLevel = 4;
            var sinDay = 20230101;
            var listCurrentOrderCode = new List<ItemCodeModel>()
        {
            new("6111029", "id1")
        };

            var listAddedOrderCode = new List<ItemCodeModel>()
        {
            new("6404029", "id1")
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "6111029", "6404029" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);
            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiUser(hpId, settingLevel, sinDay, listCurrentOrderCode, listAddedOrderCode);

                //Assert
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_030_CheckKinkiUser_Test_LEVEL_IS_0_RETURN_NEW()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("029", "029");
            var kinkiMsts = CommonCheckerData.ReadKinkiMst("029");
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.KinkiMsts.AddRange(kinkiMsts);
            tenantTracking.SaveChanges();

            var hpId = 999;
            var ptId = 1231;
            var settingLevel = 0;
            var sinDay = 20230101;
            var listCurrentOrderCode = new List<ItemCodeModel>()
        {
            new("6111029", "id1")
        };

            var listAddedOrderCode = new List<ItemCodeModel>()
        {
            new("6404029", "id1")
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "6111029", "6404029" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);
            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiUser(hpId, settingLevel, sinDay, listCurrentOrderCode, listAddedOrderCode);

                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.KinkiMsts.RemoveRange(kinkiMsts);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_031_Finder_CheckKinkiTain_Test_IsDataDb_True()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 1;
            var settingLevel = 4;
            var sinDay = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var ptId = 1231;
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var addedItemCodes = new List<ItemCodeModel>()
        {
            new("UT2714", "id1")
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2714" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, new(), true);

                //Assert
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_032_Finder_CheckKinkiTain_Test_IsDataDb_True_Kyodo_Greater_Than_Level()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 1;
            var settingLevel = 0;
            var sinDay = 20230101;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var ptId = 1231;
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var addedItemCodes = new List<ItemCodeModel>()
        {
            new("UT2714", "id1")
        };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2714" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, new(), true);

                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_031_Finder_CheckKinkiTain_Test_IsDataDb_False()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 1;
            var settingLevel = 4;
            var sinDay = 20230101;
            var isDataDb = false;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var ptId = 1231;
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var addedItemCodes = new List<ItemCodeModel>()
            {
            new("UT2714", "id1")
            };

            var ptOtherDrugModel = new List<PtOtherDrugModelStandard>()
            {
                new PtOtherDrugModelStandard(hpId, ptId, 0 , 0, "UT2714", "DRUG NAME UT", 0, 99999999, "COMMENT UT", 0),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2714" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, ptOtherDrugModel, isDataDb);

                //Assert
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_032_Finder_CheckKinkiTain_Test_AddedOrderSubYjCode_IsNull()
        {
            ///Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 1;
            var settingLevel = 4;
            var sinDay = 20230101;
            var isDataDb = false;

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var ptId = 1231;
            var ptOtherDrugs = CommonCheckerData.ReadPtOtherDrug(hpId, ptId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtOtherDrug.AddRange(ptOtherDrugs);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();
            var addedItemCodes = new List<ItemCodeModel>()
            {
            new("UT777777", "id1")
            };

            var ptOtherDrugModel = new List<PtOtherDrugModelStandard>()
            {
                new PtOtherDrugModelStandard(hpId, ptId, 0 , 0, "UT777777", "DRUG NAME UT", 0, 99999999, "COMMENT UT", 0),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT777777" }, sinDay, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                //Act
                var result = realTimeCheckerFinder.CheckKinkiTain(hpId, ptId, sinDay, settingLevel, addedItemCodes, ptOtherDrugModel, isDataDb);

                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtOtherDrug.RemoveRange(ptOtherDrugs);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_033TestAge_Test_DataDb_Is_False()
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

            var kensaInfDetailModels = new List<KensaInfDetailModel>
            {
                new KensaInfDetailModel(hpId, ptId, 0, 0, 20230101, 0, "V0002", "50", "","",0,"cmtCd1","cmtCd2", DateTime.UtcNow, "", "UT KensaName", 0, string.Empty),
            };
            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

            // Assert
            Assert.True(result.Any());
        }

        [Test]
        public void TC_034_TestAge_Return_Empty_When_Weight_And_Age_Less_Than_0()
        {
            //Setup
            int hpId = 1;
            long ptId = 99999637;
            int sinDay = 0;
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

            var kensaInfDetailModels = new List<KensaInfDetailModel>
            {
                new KensaInfDetailModel(hpId, ptId, 0, 0, -1, 0, "V0002", "-1", "","",0,"cmtCd1","cmtCd2", DateTime.UtcNow, "", "UT KensaName", 0, string.Empty),
            };
            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

            // Assert
            Assert.True(result.Count == 0);
        }

        /// <summary>
        /// Kyodo = 3
        /// SettingLevel = 3
        /// isDataOfDb = true
        /// </summary>
        [Test]
        public void TC_035_CheckKinkiOTC_Kyodo_Equal_SettingLevel()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 3;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2714", "Id1"),
            new ItemCodeModel("UT2713", "Id2"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 4);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Kyodo = 3
        /// SettingLevel = 2
        /// isDataOfDb = true
        /// </summary>
        [Test]
        public void TC_036_CheckKinkiOTC_Kyodo_Greater_Than_SettingLevel()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 2;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2714", "Id1"),
            new ItemCodeModel("UT2713", "Id2"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Kyodo = 3
        /// SettingLevel = 4
        /// </summary>
        [Test]
        public void TC_037_CheckKinkiOTC_Kyodo_Less_Than_SettingLevel()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 4;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2714", "Id1"),
            new ItemCodeModel("UT2713", "Id2"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 4);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Kyodo = 3
        /// SettingLevel = 4
        /// isDataOfDb = true
        /// Result Empty List
        /// </summary>
        [Test]
        public void TC_038_CheckKinkiOTC_Test_AddedOrderSubYjCode_IsNull()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 4;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT777777", "id1"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = false
        /// </summary>
        [Test]
        public void TC_039_CheckKinkiOTC_Test_IsDataDb_False()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 4;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2714", "Id1"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>
            {
                new PtOtcDrugModelStandard(hpId, ptId, 0, 0, 99999, "Trade Name", 20221212, 20231212, "UT Cmt", 0),
            };

            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2714");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = false
        /// s.Otc7 == c.SubAYjCd && c.IsNeedToReplace
        /// </summary>
        [Test]
        public void TC_040_CheckKinkiOTC_Test_IsNeedToReplace_IsTrue_And_Otc7_Equal_SubAYjCd()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 4;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptOtcDrugs = CommonCheckerData.ReadPtOtcDrug(hpId);
            var m38 = CommonCheckerData.ReadM38Ingredients(hpId, "");
            var tenMst = CommonCheckerData.ReadTenMst("", "");
            var ptInfs = CommonCheckerData.ReadPtInf();
            var m01 = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMst);
            tenantTracking.PtOtcDrug.AddRange(ptOtcDrugs);
            tenantTracking.M38Ingredients.AddRange(m38);
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.M01Kinki.AddRange(m01);
            tenantTracking.SaveChanges();

            var ptOtcDrugModels = new List<PtOtcDrugModelStandard>
            {
                new PtOtcDrugModelStandard(hpId, ptId, 0, 0, 99999, "Trade Name", 20221212, 20231212, "UT Cmt", 0),
            };

            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiOTC(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptOtcDrugModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2719");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMst);
                tenantTracking.PtOtcDrug.RemoveRange(ptOtcDrugs);
                tenantTracking.M38Ingredients.RemoveRange(m38);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.M01Kinki.RemoveRange(m01);
                tenantTracking.SaveChanges();
            }
        }


        /// <summary>
        /// isDataOfDb = true
        /// Kyodo = 3
        /// level = 4
        /// </summary>
        [Test]
        public void TC_041_CheckKinkiSupple_Test_IsDataOfDb_And_Kyodo_Less_Than_LevelSetting()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 4;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2719" && result.First().SeibunCd == "UT00002");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = true
        /// Kyodo = 3
        /// level = 3
        /// </summary>
        [Test]
        public void TC_042_CheckKinkiSupple_Test_IsDataOfDb_And_Kyodo_Equal_LevelSetting()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 3;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2719" && result.First().SeibunCd == "UT00002");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = true
        /// Kyodo = 3
        /// level = 2
        /// Return EmptyList
        /// </summary>
        [Test]
        public void TC_043_CheckKinkiSupple_Test_IsDataOfDb_And_Kyodo_Greater_Than_LevelSetting()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 2;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = true
        /// Kyodo = 3
        /// level = 3
        /// addedOrderSubYjCode is null
        /// Return EmptyList
        /// </summary>
        [Test]
        public void TC_044_CheckKinkiSupple_Test_AddedOrderSubYjCode_IsNull()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 3;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT777777", "id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>();

            bool isDataOfDb = true;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = false
        /// Kyodo = 3
        /// level = 3
        /// StartDate = EndDate = Sinday
        /// </summary>
        [Test]
        public void TC_045_CheckKinkiSupple_Test_IsDataOfDb_Is_False_Test_Sinday_Equal_StartDate_And_EndDate()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 3;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>
            {
                new PtSuppleModelStandard(hpId, ptId, 0 , 0, "", "UNIT-TEST", sinDay, sinDay, "CMT", 0),
            };

            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2719" && result.First().SeibunCd == "UT00002");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// isDataOfDb = false
        /// Kyodo = 3
        /// level = 3
        /// StartDate < Sinday < EndDate
        /// </summary>
        [Test]
        public void TC_046_CheckKinkiSupple_Test_IsDataOfDb_Is_False_Test_Sinday_LessThan_StartDate_LessThan_EndDate()
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            //Setup
            int hpId = 999;
            long ptId = 1231;
            int sinDay = 20230101;
            int settingLevel = 3;
            var addedOrderItemCodeList = new List<ItemCodeModel>()
            {
            new ItemCodeModel("UT2719", "Id1"),
            };

            var ptSupples = CommonCheckerData.ReadPtSupple(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m41SuppleIndexdefs = CommonCheckerData.ReadM41SuppleIndexdef(hpId);
            var m41SuppleIndexcodes = CommonCheckerData.ReadM41SuppleIndexcode(hpId);
            var m01Kinkis = CommonCheckerData.ReadM01Kinki(hpId);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.PtSupples.AddRange(ptSupples);
            tenantTracking.M41SuppleIndexdefs.AddRange(m41SuppleIndexdefs);
            tenantTracking.M41SuppleIndexcodes.AddRange(m41SuppleIndexcodes);
            tenantTracking.M01Kinki.AddRange(m01Kinkis);
            tenantTracking.SaveChanges();

            var ptSuppleModels = new List<PtSuppleModelStandard>
            {
                new PtSuppleModelStandard(hpId, ptId, 0 , 0, "", "UNIT-TEST", 20221231, 20230102, "CMT", 0),
            };

            bool isDataOfDb = false;
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckKinkiSupple(hpId, ptId, sinDay, settingLevel, addedOrderItemCodeList, ptSuppleModels, isDataOfDb);

                // Assert
                Assert.True(result.Count == 1 && result.First().ItemCd == "UT2719" && result.First().SeibunCd == "UT00002");
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.PtSupples.RemoveRange(ptSupples);
                tenantTracking.M41SuppleIndexdefs.RemoveRange(m41SuppleIndexdefs);
                tenantTracking.M41SuppleIndexcodes.RemoveRange(m41SuppleIndexcodes);
                tenantTracking.M01Kinki.RemoveRange(m01Kinkis);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_047_CheckDosage_When_PatientInfoIsNull_ReturnsEmptyList()
        {
            // Arrange
            var mockTenMstCacheService = new Mock<IMasterDataCacheService>();
            var cache = new MasterDataCacheService(TenantProvider);
            // cache.InitCache(hpId,new List<string>() { }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider, cache);

            mockTenMstCacheService.Setup(x => x.GetPtInf()).Returns((PtInf?)null);

            // Act
            var result = realtimcheckerfinder.CheckDosage(1, 2, 3, new List<DrugInfo>(), true, 1.0, 70.0, 160.0, new List<KensaInfDetailModel>(), false);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_048_CheckDosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.SaveChanges();

            var hpId = 1;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;
            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "620160501",
                    ItemName = "ＰＬ配合顆粒",
                    SinKouiKbn = 21,
                    Suryo = 100,
                    TermVal = 0,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "620160501" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// UnitName = mL
        /// YAKKA_UNIT = mL
        /// </summary>
        [Test]
        public void TC_049_CheckDosage_TEST_UnitName_Equal_YAKKAUNIT()
        {
            //setup
            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var dosageMsts = CommonCheckerData.READ_DOSAGE_MST(hpId);
            var dosageDrugs = CommonCheckerData.READ_M46_DOSAGE_DRUG(hpId);
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();
            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 100,
                    TermVal = 0,
                    UnitName = "mL",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// UnitName = g
        /// RIKIKA_UNIT = g
        /// </summary>
        [Test]
        public void TC_050_CheckDosage_TEST_UnitName_Equal_YAKKAUNIT()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 999,
                    OnceLimit = 999,
                    OnceUnit = 1,
                    DayMin = 1,
                    DayMax = 1,
                    DayLimit = 1,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 100,
                    TermVal = 0,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// UnitName not equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// </summary>
        [Test]
        public void TC_051_CheckDosage_TEST_TermVal_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 999,
                    OnceLimit = 999,
                    OnceUnit = 1,
                    DayMin = 1,
                    DayMax = 1,
                    DayLimit = 1,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 100,
                    TermVal = 1,
                    UnitName = "TEST",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit is empty
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// </summary>
        [Test]
        public void TC_052_CheckDosage_TEST_RikikaUnit_Is_Empty()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 999,
                    OnceLimit = 999,
                    OnceUnit = 1,
                    DayMin = 1,
                    DayMax = 1,
                    DayLimit = 1,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 100,
                    TermVal = 1,
                    UnitName = "",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 0
        /// </summary>
        [Test]
        public void TC_053_CheckDosage_TEST_Dosage_Is_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 999,
                    OnceLimit = 999,
                    OnceUnit = 1,
                    DayMin = 1,
                    DayMax = 1,
                    DayLimit = 1,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 0,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin > 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 1
        /// </summary>
        [Test]
        public void TC_054_CheckDosage_TEST_OnceMin_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 1,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin > 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = false
        /// OnceUnit = 1
        /// </summary>
        [Test]
        public void TC_055_CheckDosage_TEST_OnceMin_Greater_Than_0_MinChcek_Is_False()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 1,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin > 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 1
        /// </summary>
        [Test]
        public void TC_056_CheckDosage_TEST_OnceMin_Greater_Than_0_MinChcek_SinKouiKbn_Not_Is_22()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 0;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 1,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 1,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin > 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 2
        /// weight = -1
        /// bodySize = 1
        /// </summary>
        [Test]
        public void TC_057_CheckDosage_TEST_OnceUnit_Is_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 2;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 10000,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 2,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 2
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 3
        /// weight = 2
        /// </summary>
        [Test]
        public void TC_058_CheckDosage_TEST_OnceUnit_Is_Not_Equal_1_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 2;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 2,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 3,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 3
        /// OnceMin = 0
        /// OnceMax = 1
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 1
        /// weight = 2
        /// </summary>
        [Test]
        public void TC_059_CheckDosage_TEST_OnceMax_Greater_Than_0_OnceUnit_Is_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 0;
            var currentHeight = 0;
            var currenWeight = 2;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 1,
                    OnceLimit = 0,
                    OnceUnit = 1,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 1
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 2
        /// </summary>
        [Test]
        public void TC_060_CheckDosage_TEST_OnceMax_Greater_Than_0_OnceUnit_Is_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;
            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 1,
                    OnceLimit = 0,
                    OnceUnit = 2,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 1
        /// OnceLimit = 0
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 3
        /// </summary>
        [Test]
        public void TC_061_CheckDosage_TEST_OnceMax_Greater_Than_0_OnceUnit_Is_Not_Equal_1_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;
            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 1,
                    OnceLimit = 0,
                    OnceUnit = 3,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 2
        /// DayMin = 0
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// </summary>
        [Test]
        public void TC_062_CheckDosage_TEST_OnceLimit_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;
            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 2,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 2
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 1
        /// </summary>
        [Test]
        public void TC_063_CheckDosage_TEST_DayMin_Greater_Than_0_And_Day_Unit_Is_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 80;
            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 2,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 1,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 2
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 2
        /// </summary>
        [Test]
        public void TC_064_CheckDosage_TEST_DayMin_Greater_Than_0_And_Day_Unit_Is_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;
            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 2,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 2,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 1
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 4
        /// DayMax = 0
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3
        /// </summary>
        [Test]
        public void TC_065_CheckDosage_TEST_DayMin_Greater_Than_0_And_Day_Unit_Is_Not_Equal_1_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 4,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 161
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayMax = 2
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 1
        /// </summary>
        [Test]
        public void TC_066_CheckDosage_TEST_DayMax_Greater_Than_0_And_Day_Unit_Is_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 80;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 1,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 161,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 161
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayMax = 2
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 2
        /// </summary>
        [Test]
        public void TC_067_CheckDosage_TEST_DayMax_Greater_Than_0_And_Day_Unit_Is_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = 180;
            var currenWeight = 80;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 2,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 161
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayMax = 2
        /// DayLimit = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3
        /// </summary>
        [Test]
        public void TC_068_CheckDosage_TEST_DayMax_Greater_Than_0_And_Day_Unit_Is_Not_Equal_1_2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 2 => LimitDay = 2
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3 DayMax = 2 => maxDay = 2
        /// </summary>
        [Test]
        public void TC_069_CheckDosage_TEST_LimitDay_And_MaxDay_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 2,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 161
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 2 => LimitDay = 2
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3 DayMax = 0 => maxDay = 0
        /// </summary>
        [Test]
        public void TC_070_CheckDosage_TEST_LimitDay_Greater_Than_0_And_MaxDay_Is_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 2,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 161
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 0 => LimitDay = 0
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3 DayMax = 2 => maxDay = 2
        /// </summary>
        [Test]
        public void TC_071_CheckDosage_TEST_LimitDay_Is_0_And_MaxDay_Is_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 2,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 2 => LimitDay = 2
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3 DayMax = 3 => maxDay = 3
        /// </summary>
        [Test]
        public void TC_072_CheckDosage_TEST_LimitDay_And_MaxDay_Greater_Than_0_LimitDay_LessThan_MaxDay()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 3,
                    DayLimit = 2,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 2 => LimitDay = 2
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 3 DayMax = 0 => maxDay = 0
        /// </summary>
        [Test]
        public void TC_073_CheckDosage_TEST_LimitDay_Greater_Than_0_And_MaxDay_Is_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 2,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 3 => maxOnce = 3
        /// OnceLimit = 2 => limitOnce = 2
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// </summary>
        [Test]
        public void TC_074_CheckDosage_TEST_LimitOnce_And_MaxOnce_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 3,
                    OnceLimit = 2,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 3 => maxOnce = 3
        /// OnceLimit = 3 => limitOnce = 3
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// </summary>
        [Test]
        public void TC_075_CheckDosage_TEST_LimitOnce_And_MaxOnce_Greater_Than_0_And_limitOnce_Equal_MaxOnce()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 3,
                    OnceLimit = 3,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 0 => maxOnce = 0
        /// OnceLimit = 3 => limitOnce = 3
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// </summary>
        [Test]
        public void TC_076_CheckDosage_TEST_LimitOnce_And_MaxOnce_Is_0_And_limitOnce_Is_Greater_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 3,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4
        /// OnceMin = 0
        /// OnceMax = 2 => maxOnce = 2
        /// OnceLimit = 0 => limitOnce = 0
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// </summary>
        [Test]
        public void TC_077_CheckDosage_TEST_LimitOnce_And_MaxOnce_Is_Greater_Than_0_And_limitOnce_Is_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 2,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 3,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 2 
        /// OnceLimit = 0 
        /// DayMin = 5 => minDay = 5
        /// DayLimit = 0 
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// minday > Dosage
        /// </summary>
        [Test]
        public void TC_078_CheckDosage_TEST_SinKouiKbn_Is_21_And_MinCheck_Is_True_And_MinDay_GreaterThan_Dosage_And_MinDay_NotEqual_Negative_Number_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 2,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 5,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 0 
        /// OnceLimit = 3 => limitOnce = 3
        /// DayMin = 0 => minDay = 0
        /// DayLimit = 0 
        /// SinKouiKbn = 21
        /// minCheck = false
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 3  => maxByDayToCheck = maxDay
        /// minday > Dosage
        /// </summary>
        [Test]
        public void TC_079_CheckDosage_TEST_SinKouiKbn_Is_21_And_MaxByDayToCheck_Greater_Than_0_And_MaxByDayToCheck_Less_Than_Dosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 3,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 3,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 2 => Dosage = 2
        /// OnceMin = 3 => MinOnce = 3
        /// OnceMax = 0
        /// OnceLimit = 0 
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// minOnce = 3 & dosage = 2
        /// </summary>
        [Test]
        public void TC_080_CheckDosage_TEST_SinKouiKbn_Is_22_And_MinCheck_Is_True_And_MinOnce_GreaterThan_Dosage_And_MinOnce_NotEqual_Negative_Number_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 3,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 2,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 3 => MaxOnce =3
        /// OnceLimit = 0 
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 22
        /// minCheck = false
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// maxByOnceToCheck = 3 & dosage = 4
        /// </summary>
        [Test]
        public void TC_081_CheckDosage_TEST_SinKouiKbn_Is_22_And_MaxByOnceToCheck_Greater_Than_0_And_MaxByOnceToCheck_Less_Than_Dosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 3,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 2 => Dosage = 2
        /// OnceMin = 3 => MinOnce = 4
        /// OnceMax = 0
        /// OnceLimit = 0 
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 23
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// minOnce = 3 & dosage = 2
        /// </summary>
        [Test]
        public void TC_082_CheckDosage_TEST_SinKouiKbn_NotEqual_21_22_MinCheck_Is_True_And_MinOnce_GreaterThan_Dosage_And_MinOnce_Not_IsNegative_Number_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 3,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 2,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 3 => MaxOnce =3
        /// OnceLimit = 0 
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 23
        /// minCheck = false
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// maxByOnceToCheck = 3 & dosage = 4
        /// </summary>
        [Test]
        public void TC_083_CheckDosage_TEST_SinKouiKbn_NotEqual_21_22_And_MaxByOnceToCheck_Greater_Than_0_And_MaxByOnceToCheck_Less_Than_Dosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = false;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 3,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 0
        /// OnceLimit = 0 
        /// DayMin = 5 => Minday = 5 
        /// DayLimit = 0 
        /// SinKouiKbn = 23
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 0 
        /// maxByOnceToCheck = 3 & dosage = 4
        /// </summary>
        [Test]
        public void TC_084_CheckDosage_TEST_SinKouiKbn_NotEqual_21_22_And_MinCheck_IsTrue_Minday_GreaterThan_Dosage_And_MinDay_NotEqual_NegativeNumber1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 5,
                    DayMax = 0,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 999,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// UnitName equal RIKIKA_UNIT & YAKKA_UNIT
        /// TermVal > 0
        /// Suryo = 4 => Dosage = 4
        /// OnceMin = 0
        /// OnceMax = 0 
        /// OnceLimit = 0 => limitOnce = 0
        /// DayMin = 0 => minDay = 0
        /// DayLimit = 0 
        /// SinKouiKbn = 999
        /// minCheck = false
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 3  => maxByDayToCheck = maxDay
        /// minday > Dosage
        /// </summary>
        [Test]
        public void TC_085_CheckDosage_TEST_SinKouiKbn_NotEqual_21_22_And_MaxByDayToCheck_Greater_Than_0_And_MaxByDayToCheck_Less_Than_Dosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = -1;

            var dosageMsts = new List<DosageMst>()
            {
                new DosageMst()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    SeqNo = 999999,
                    OnceMin = 0,
                    OnceMax = 0,
                    OnceLimit = 0,
                    OnceUnit = 0,
                    DayMin = 0,
                    DayMax = 3,
                    DayLimit = 0,
                    DayUnit = 0,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 0,
                    UpdateId = 0
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageMsts.AddRange(dosageMsts);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 999,
                    Suryo = 4,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageMsts.RemoveRange(dosageMsts);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// ItemCd = UT2720
        /// RikikaUnit = g
        /// RIKIKA_UNIT = g
        /// UnitName = g
        /// TermVal > 0
        /// Suryo = 1000 => Dosage = 1000
        /// OnceMin = 0
        /// OnceMax = 0 
        /// OnceLimit = 0
        /// DayMin = 0 
        /// DayLimit = 0 
        /// SinKouiKbn = 21
        /// minCheck = true
        /// OnceUnit = 0
        /// DayUnit = 0
        /// DayMax = 3  
        /// </summary>
        [Test]
        public void TC_086_CheckDosage_TEST_MasterData()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }



        [Test]
        public void TC_087_CheckDayLimit_TEST_DayLimitInfoByUser_UsingDay_LessThan_LimitDay()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var usingDay = 1.0;

            var drugDayLimit = new List<DrugDayLimit>()
            {
                new DrugDayLimit()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    StartDate = 20230101,
                    EndDate = 20231212,
                    LimitDay = 2,
                    IsDeleted= 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 1,
                    UpdateId = 1
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimit);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimit);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_088_CheckDayLimit_TEST_DayLimitInfoByUser_UsingDay_Equal_LimitDay()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var usingDay = 9.0;

            var drugDayLimit = new List<DrugDayLimit>()
            {
                new DrugDayLimit()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    StartDate = 20230101,
                    EndDate = 20231212,
                    LimitDay = 9,
                    IsDeleted= 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 1,
                    UpdateId = 1
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimit);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimit);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_089_CheckDayLimit_TEST_DayLimitInfoByUser_UsingDay_GreaterThan_LimitDay()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var usingDay = 10.0;

            var drugDayLimit = new List<DrugDayLimit>()
            {
                new DrugDayLimit()
                {
                    HpId = hpId,
                    ItemCd = "UT2720",
                    StartDate = 20230101,
                    EndDate = 20231212,
                    LimitDay = 9,
                    IsDeleted= 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 1,
                    UpdateId = 1
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimit);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.True(result.First().ItemCd == "UT2720" && result.First().LimitDay == 9 && result.First().UsingDay == 10 && result.First().YjCd == "UT271026");
            }
            finally
            {
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimit);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_090_CheckDayLimit_TEST_StartDate_And_EndDate_Is_Empty_And_UsingDay_LessThan_LitmitDay()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var usingDay = 1.0;

            var m10DayLimits = new List<M10DayLimit>()
            {
                new M10DayLimit()
                {
                    HpId = 1,
                    YjCd = "UT271026",
                    SeqNo = 1,
                    LimitDay = 2,
                    StDate = "",
                    EdDate = "",
                    Cmt  = ""
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_091_CheckDayLimit_TEST_Sinday_LessThan_StartDate()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var m10DayLimits = new List<M10DayLimit>()
            {
                new M10DayLimit()
                {
                    HpId = 1,
                    YjCd = "UT271026",
                    SeqNo = 1,
                    LimitDay = 2,
                    StDate = "20240101",
                    EdDate = "",
                    Cmt  = ""
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20231231;
            var usingDay = 9.0;
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_092_CheckDayLimit_TEST_EndDate_LessThan_Sinday()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var m10DayLimits = new List<M10DayLimit>()
            {
                new M10DayLimit()
                {
                    HpId = 1,
                    YjCd = "UT271026",
                    SeqNo = 1,
                    LimitDay = 2,
                    StDate = "20240101",
                    EdDate = "20251231",
                    Cmt  = ""
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20260101;
            var usingDay = 9.0;
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(0));
            }
            finally
            {
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_093_CheckDayLimit_TEST_UsingDay_GreaterThan_LitmitDay_StartDate_And_EndDate_IsEmpty()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20260101;
            var usingDay = 9.0;

            var m10DayLimits = new List<M10DayLimit>()
            {
                new M10DayLimit()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    SeqNo = 1,
                    LimitDay = 2,
                    StDate = "",
                    EdDate = "",
                    Cmt  = ""
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.True(result.First().YjCd == "UT271026" && result.First().ItemCd == "UT2720");
            }
            finally
            {
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Sinday > StartDate 
        /// EndDate > Sindate
        /// Sinday = 20230102, StartDate = 20230101, Endate = 20230103
        /// </summary>
        [Test]
        public void TC_094_CheckDayLimit_TEST_UsingDay_GreaterThan_LitmitDay_StartDate_LessThan_Sinday_LessThan_EndDate()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230102;
            var usingDay = 9.0;

            var m10DayLimits = new List<M10DayLimit>()
            {
                new M10DayLimit()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    SeqNo = 1,
                    LimitDay = 2,
                    StDate = "20230101",
                    EdDate = "20230103",
                    Cmt  = ""
                },
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2719" , "id1"),
                new ItemCodeModel("UT2720" , "id2"),
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Arrange
                var mockTenMstCacheService = new Mock<RealtimeCheckerFinder>();

                // Act
                var result = realtimeCheckerFinder.CheckDayLimit(hpId, sinday, listItem, usingDay);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.True(result.First().YjCd == "UT271026" && result.First().ItemCd == "UT2720");
            }
            finally
            {
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_096_TEST_GetYjCdListByItemCdList()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var hpId = 1;
            var sinday = 20231212;
            var ptId = 1231;
            var listItem = new List<ItemCodeModel>()
            {
                new ItemCodeModel("UT2720", "id1"),
                new ItemCodeModel("UT2719", "id2"),
                new ItemCodeModel("UT2718", "id3"),
            };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetYjCdListByItemCdList(hpId, listItem, sinday);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_097_TEST_GetFoodAllergyByPtId_Test_IsDataOfDb_True()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 99999;
            var sinday = 20230101;
            var ptId = 1231;
            var isDataOfDb = true;

            var ptAlrgyFoods = new List<PtAlrgyFood>()
            {
                new PtAlrgyFood()
                {
                    HpId = 99999,
                    PtId = 1231,
                    SortNo = 1,
                    IsDeleted = 0,
                    StartDate = 20230101,
                    EndDate = 20230101
                },
                new PtAlrgyFood()
                {
                    HpId = 99999,
                    PtId = 1231,
                    SortNo = 2,
                    IsDeleted = 0,
                    AlrgyKbn = "1",
                    Cmt = "CMT TEST",
                    StartDate = 20221231,
                    EndDate = 20230102
                }
            };

            tenantTracking.PtAlrgyFoods.AddRange(ptAlrgyFoods);
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetFoodAllergyByPtId(hpId, ptId, sinday, new(), isDataOfDb);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(ptAlrgyFoods);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_098_TEST_GetFoodAllergyByPtId_Test_IsDataOfDb_False()
        {
            //setup
            var hpId = 99999;
            var sinday = 20230101;
            var ptId = 1231;
            var isDataOfDb = false;

            var ptAlrgyFoodModels = new List<PtAlrgyFoodModelStandard>()
            {
              new PtAlrgyFoodModelStandard(hpId, ptId, 111 , 0, "", 20230101, 20230101, "", 0, string.Empty),
              new PtAlrgyFoodModelStandard(hpId, ptId, 111 , 0, "", 20221231, 20230102, "", 0, string.Empty),
            };

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);


            // Act
            var result = realtimeCheckerFinder.GetFoodAllergyByPtId(hpId, ptId, sinday, ptAlrgyFoodModels, isDataOfDb);

            // Assert
            Assert.AreEqual(2, result.Count);

        }

        [Test]
        public void TC_099_TEST_GetDrugAllergyByPtId_Test_IsDataOfDb_True()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 99999;
            var sinday = 20230102;
            var ptId = 1231;
            var isDataOfDb = true;

            var ptAlgryDrugs = new List<PtAlrgyDrug>()
            {
                new PtAlrgyDrug()
                {
                    HpId = hpId,
                    PtId = ptId,
                    SortNo = 1,
                    ItemCd = "UNIT-TEST",
                    DrugName = "DrugName Test",
                    StartDate = 20230101,
                    EndDate = 20230103,
                    Cmt = "",
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow ,
                    CreateId = 2,
                    UpdateId = 2
                },

                new PtAlrgyDrug()
                {
                    HpId = hpId,
                    PtId = ptId,
                    SortNo = 2,
                    ItemCd = "UNIT-TEST2",
                    DrugName = "DrugName Test",
                    StartDate = 20230102,
                    EndDate = 20230102,
                    Cmt = "",
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow ,
                    CreateId = 2,
                    UpdateId = 2
                },
            };

            tenantTracking.PtAlrgyDrugs.AddRange(ptAlgryDrugs);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugAllergyByPtId(hpId, ptId, sinday, new(), isDataOfDb);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.PtAlrgyDrugs.RemoveRange(ptAlgryDrugs);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_100_TEST_GetDrugAllergyByPtId_Test_IsDataOfDb_False()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 99999;
            var sinday = 20230102;
            var ptId = 1231;
            var isDataOfDb = false;

            var ptAlrgyDrugModels = new List<PtAlrgyDrugModelStandard>()
            {
                new PtAlrgyDrugModelStandard (hpId, ptId, 111, 1, "11001100", "drug name", 20230101, 20230103, "", 0),
                new PtAlrgyDrugModelStandard (hpId, ptId, 112, 2, "11001101", "drug name", 20230102, 20230102, "", 0),
            };

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetDrugAllergyByPtId(hpId, ptId, sinday, ptAlrgyDrugModels, isDataOfDb);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void TC_101_TEST_GetBodyInfo()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 99999;
            var sinday = 20230105;
            var ptId = 1231;
            var kensaItemCd = "ItemCd9999";

            var kensaInfs = new List<KensaInfDetail>()
            {
                new KensaInfDetail()
                {
                    HpId = hpId,
                    PtId = ptId,
                    IraiDate = 20230101,
                    RaiinNo = 99999999,
                    IraiCd = 01234,
                    KensaItemCd = kensaItemCd,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId  = 2,
                    UpdateId = 2,
                    ResultVal = "9"
                },

                new KensaInfDetail()
                {
                    HpId = hpId,
                    PtId = ptId,
                    IraiDate = 20230202,
                    RaiinNo = 99999998,
                    IraiCd = 01233,
                    KensaItemCd = kensaItemCd,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId  = 2,
                    UpdateId = 2,
                    ResultVal = "9"
                },
            };

            tenantTracking.KensaInfDetails.AddRange(kensaInfs);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetBodyInfo(hpId, ptId, sinday, kensaItemCd);

                // Assert
                Assert.True(result.IraiCd == 01234);
            }
            finally
            {
                tenantTracking.KensaInfDetails.RemoveRange(kensaInfs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_102_TEST_GetCommonBodyInfo()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var birthDay = 19981212;
            var sinday = 21990105;

            var physicalAverages = new List<PhysicalAverage>()
            {
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2199,
                    AgeYear = 200,
                    AgeMonth = 0,
                    AgeDay = 24,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 60,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2198,
                    AgeYear = 199,
                    AgeMonth = -1,
                    AgeDay = 23,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 60,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },

            };

            tenantTracking.PhysicalAverage.AddRange(physicalAverages);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetCommonBodyInfo(1, birthDay, sinday);

                // Assert
                Assert.True(result.JissiYear == 2199 && result.AgeYear == 200);
            }
            finally
            {
                tenantTracking.PhysicalAverage.RemoveRange(physicalAverages);
                tenantTracking.SaveChanges(true);
            }
        }

        [Test]
        public void TC_103_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271025",
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 0;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_104_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_1_And_HaigouFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     HaigouFlg = "",
                     YuekiFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     HaigouFlg = "1",
                     YuekiFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271025",
                     YuekiFlg = "1"
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 1;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_105_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_1_And_YuekiFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     YuekiFlg = "1",
                     HaigouFlg = "1",
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     YuekiFlg = "",
                     HaigouFlg = "1",
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271025",
                     HaigouFlg = "1",
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 1;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_106_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_2_And_HaigouFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     HaigouFlg = "1",
                     KanpoFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     HaigouFlg = "",
                     KanpoFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271025",
                     HaigouFlg = "2",
                     KanpoFlg = "1"
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 2;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_107_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_2_And_KanpoFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     KanpoFlg = "1",
                     HaigouFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     KanpoFlg = "",
                     HaigouFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     KanpoFlg = "3",
                     YjCd = "UT271025",
                     HaigouFlg = "1"
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 2;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_108_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_3_And_HaigouFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     HaigouFlg = "1",
                     YuekiFlg = "1",
                     KanpoFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     HaigouFlg = "",
                     YuekiFlg = "1",
                     KanpoFlg = "1"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     HaigouFlg = "3",
                     YjCd = "UT271025",
                     YuekiFlg = "1",
                     KanpoFlg = "1"
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 3;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(2, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_109_TEST_GetDrugTypeInfo_Test_HaigouSetting_Is_3_And_YuekiFlg_And_KanpoFlg_NotEqual_1()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            int hpId = tenMsts.FirstOrDefault()?.HpId ?? 1;

            var m56 = new List<M56ExIngrdtMain>()
            {
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271023",
                     KanpoFlg = "1",
                     YuekiFlg = "2",
                     HaigouFlg = "1",
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271024",
                     HaigouFlg = "1",
                     YuekiFlg = "2",
                     KanpoFlg = "3"
                 },
                 new M56ExIngrdtMain()
                 {
                     HpId = hpId,
                     YjCd = "UT271025",
                     KanpoFlg = "2",
                     YuekiFlg = "1",
                     HaigouFlg = "1",
                 }
            };
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56);

            tenantTracking.SaveChanges();

            var haigouSetting = 3;

            /// YJCD = UT271023 , UT271024, UT271025
            var itemCodes = new List<string>() { "UT2716", "UT2717", "UT2718" };

            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetDrugTypeInfo(hpId, haigouSetting, itemCodes);

                // Assert
                Assert.AreEqual(1, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngrdtMain.RemoveRange(m56);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_110_TEST_GetRatio_Test_YYYY_Is_0_MM_Is_0()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now);
            var today = CIUtil.DateTimeToInt(DateTime.Now);
            // YYYY = 0 , MM = 0
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(1.0 / 8, result);
        }

        [Test]
        public void TC_111_TEST_GetRatio_Test_YYYY_Is_0_MM_LessThan_4()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now);
            var today = CIUtil.DateTimeToInt(DateTime.Now.AddMonths(3));
            // YYYY = 0 , MM = 3
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(1.0 / 6, result);
        }

        [Test]
        public void TC_112_TEST_GetRatio_Test_YYYY_Is_0_MM_LessThan_4()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now);
            var today = CIUtil.DateTimeToInt(DateTime.Now.AddMonths(3));
            // YYYY = 0 , MM = 3
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(1.0 / 6, result);
        }

        [Test]
        public void TC_113_TEST_GetRatio_Test_YYYY_Is_0_MM_Is_4()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now);
            var today = CIUtil.DateTimeToInt(DateTime.Now.AddMonths(4));
            // YYYY = 0 , MM = 4
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(1.0 / 5, result);
        }

        [Test]
        public void TC_114_TEST_GetRatio_Test_YYYY_GreaterThan0_And_LessThan_15_MM_Is_4()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now.AddYears(-14));
            var today = CIUtil.DateTimeToInt(DateTime.Now.AddMonths(4));
            // YYYY = 14 , MM = 4
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(((double)(14 * 4 + 20)) / 100, result);
        }

        [Test]
        public void TC_115_TEST_GetRatio_Test_YYYY_Is_15_MM_Is_4()
        {
            int hpId = 1;
            //setup
            var fromDay = CIUtil.DateTimeToInt(DateTime.Now.AddYears(-15));
            var today = CIUtil.DateTimeToInt(DateTime.Now.AddMonths(4));
            // YYYY = 14 , MM = 4
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(1, new List<string>() { "UT2716", "UT2717", "UT2718" }, 20230101, 0);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.GetRatio(fromDay, today);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TC_116_TEST_GetCommonWeight_Test_Sex_Is_Male()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var birthDay = 19981212;
            var sinday = 21990105;
            var sex = 1;

            var physicalAverages = new List<PhysicalAverage>()
            {
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2199,
                    AgeYear = 200,
                    AgeMonth = 0,
                    AgeDay = 24,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 60,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2198,
                    AgeYear = 199,
                    AgeMonth = -1,
                    AgeDay = 23,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },

            };

            tenantTracking.PhysicalAverage.AddRange(physicalAverages);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetCommonWeight(1, birthDay, sinday, sex);

                // Assert
                Assert.AreEqual(60, result);
            }
            finally
            {
                tenantTracking.PhysicalAverage.RemoveRange(physicalAverages);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_117_TEST_GetCommonWeight_Test_Sex_Is_Female()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var birthDay = 19981212;
            var sinday = 21990105;

            var physicalAverages = new List<PhysicalAverage>()
            {
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2199,
                    AgeYear = 200,
                    AgeMonth = 0,
                    AgeDay = 24,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2198,
                    AgeYear = 199,
                    AgeMonth = -1,
                    AgeDay = 23,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 180,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },

            };

            tenantTracking.PhysicalAverage.AddRange(physicalAverages);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var sex = 2;
                var result1 = realtimeCheckerFinder.GetCommonWeight(1, birthDay, sinday, sex);
                sex = 0;
                var result2 = realtimeCheckerFinder.GetCommonWeight(1, birthDay, sinday, sex);

                // Assert
                Assert.AreEqual(99, result1);
                Assert.AreEqual(99, result2);
            }
            finally
            {
                tenantTracking.PhysicalAverage.RemoveRange(physicalAverages);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_118_TEST_GetCommonHeight_Test_Sex_Is_Female()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var birthDay = 19981212;
            var sinday = 21990105;

            var physicalAverages = new List<PhysicalAverage>()
            {
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2199,
                    AgeYear = 200,
                    AgeMonth = 0,
                    AgeDay = 24,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 140,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2198,
                    AgeYear = 199,
                    AgeMonth = -1,
                    AgeDay = 23,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 140,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },

            };

            tenantTracking.PhysicalAverage.AddRange(physicalAverages);

            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var sex = 2;
                var result1 = realtimeCheckerFinder.GetCommonHeight(1, birthDay, sinday, sex);
                sex = 0;
                var result2 = realtimeCheckerFinder.GetCommonHeight(1, birthDay, sinday, sex);

                // Assert
                Assert.AreEqual(140, result1);
                Assert.AreEqual(140, result2);
            }
            finally
            {
                tenantTracking.PhysicalAverage.RemoveRange(physicalAverages);

                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_119_TEST_GetCommonHeight_Test_Sex_Is_Male()
        {

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var birthDay = 19981212;
            var sinday = 21990105;

            var physicalAverages = new List<PhysicalAverage>()
            {
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2199,
                    AgeYear = 200,
                    AgeMonth = 0,
                    AgeDay = 24,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 140,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },
                new PhysicalAverage()
                {
                    HpId = 1,
                    JissiYear = 2198,
                    AgeYear = 199,
                    AgeMonth = -1,
                    AgeDay = 23,
                    MaleHeight = 180,
                    MaleWeight = 60,
                    MaleChest = 90,
                    MaleHead = 50,
                    FemaleHeight = 140,
                    FemaleWeight = 99,
                    FemaleChest = 90,
                    FemaleHead = 50,
                    CreateDate = DateTime.UtcNow
                },

            };

            tenantTracking.PhysicalAverage.AddRange(physicalAverages);

            tenantTracking.SaveChanges();

            try
            {

                var cache = new MasterDataCacheService(TenantProvider);
                var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

                // Act
                var sex = 1;
                var result = realtimeCheckerFinder.GetCommonHeight(1, birthDay, sinday, sex);

                // Assert
                Assert.AreEqual(180, result);
            }
            finally
            {
                tenantTracking.PhysicalAverage.RemoveRange(physicalAverages);

                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_120_TEST_GetPatientWeight_IsDataDb_True()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var hpId = 99999;
            var sinday = 20230105;
            long ptId = 1231;
            var kensaItemCd = "V0002";

            var kensaInfs = new List<KensaInfDetail>()
            {
                new KensaInfDetail()
                {
                    HpId = hpId,
                    PtId = ptId,
                    IraiDate = 20230101,
                    RaiinNo = 99999999,
                    IraiCd = 01234,
                    KensaItemCd = kensaItemCd,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId  = 2,
                    UpdateId = 2,
                    ResultVal = "9"
                },

                new KensaInfDetail()
                {
                    HpId = hpId,
                    PtId = ptId,
                    IraiDate = 20230202,
                    RaiinNo = 99999998,
                    IraiCd = 01233,
                    KensaItemCd = kensaItemCd,
                    IsDeleted = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId  = 2,
                    UpdateId = 2,
                    ResultVal = "9"
                },
            };

            tenantTracking.KensaInfDetails.AddRange(kensaInfs);

            tenantTracking.SaveChanges();

            var isDataDb = true;
            var sex = 1;

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetPatientWeight(hpId, ptId, 20000101, sinday, sex, new(), isDataDb);

                // Assert
                Assert.AreEqual(9, result);
            }
            finally
            {
                tenantTracking.KensaInfDetails.RemoveRange(kensaInfs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_121_TEST_GetPatientWeight_IsDataDb_False()
        {
            //setup
            var hpId = 99999;
            var sinday = 20230105;
            long ptId = 1231;
            var kensaItemCd = "V0002";

            var kensaInfDetailModels = new List<KensaInfDetailModel>()
            {
                new KensaInfDetailModel(hpId, ptId, 1234, 0, 20230101, 99999, kensaItemCd, "4", "","", 0 , "","", DateTime.UtcNow, "","",1, string.Empty),
            };

            var isDataDb = false;
            var sex = 1;

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.GetPatientWeight(hpId, ptId, 20000101, sinday, sex, kensaInfDetailModels, isDataDb);

                // Assert
                Assert.AreEqual(4, result);
            }
            finally
            {
            }
        }

        [Test]
        public void TC_122_TEST_GetBodySize_Test_Age_GreaterThan_5()
        {
            //setup

            double weight = 69.7; double height = 176.5;

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            double age = 6;
            // Act
            var result1 = realtimeCheckerFinder.GetBodySize(weight, height, age);
            age = 7;
            var result2 = realtimeCheckerFinder.GetBodySize(weight, height, age);

            // Assert
            Assert.AreEqual(Math.Pow(weight, 0.444) * Math.Pow(height, 0.663) * 0.008883, result1);
            Assert.AreEqual(Math.Pow(weight, 0.444) * Math.Pow(height, 0.663) * 0.008883, result2);

        }

        [Test]
        public void TC_122_TEST_GetBodySize_Test_Age_GreaterThan_0_And_LessThan_6()
        {
            //setup

            double weight = 69.7; double height = 176.5;

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);


            // Act
            double age = 5;
            var result1 = realtimeCheckerFinder.GetBodySize(weight, height, age);
            age = 1;
            var result2 = realtimeCheckerFinder.GetBodySize(weight, height, age);

            // Assert
            Assert.AreEqual(Math.Pow(weight, 0.423) * Math.Pow(height, 0.362) * 0.038189, result1);
            Assert.AreEqual(Math.Pow(weight, 0.423) * Math.Pow(height, 0.362) * 0.038189, result2);

        }

        [Test]
        public void TC_123_TEST_GetBodySize_Test_Age_LessThan_1()
        {
            //setup

            double weight = 69.7; double height = 176.5;

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);


            // Act
            double age = 0;
            var result1 = realtimeCheckerFinder.GetBodySize(weight, height, age);
            age = -1;
            var result2 = realtimeCheckerFinder.GetBodySize(weight, height, age);

            // Assert
            Assert.AreEqual(Math.Pow(weight, 0.473) * Math.Pow(height, 0.655) * 0.009568, result1);
            Assert.AreEqual(Math.Pow(weight, 0.473) * Math.Pow(height, 0.655) * 0.009568, result2);

        }

        [Test]
        public void TC_124_TEST_GetPatientHeight()
        {
            //setup

            var hpId = 9999;
            var ptId = 19999;
            var birthDay = 19981212;
            var sinDay = CIUtil.DateTimeToInt(DateTime.Now);

            var kensaInfDetailModels = new List<KensaInfDetailModel>
            {
                new KensaInfDetailModel(hpId, ptId, 0, 0, 20230101, 0, "V0001", "50", "","",0,"cmtCd1","cmtCd2", DateTime.UtcNow, "", "UT KensaName", 0, string.Empty),
                new KensaInfDetailModel(hpId, ptId, 0, 0, 20230103, 0, "V0002", "60", "","",0,"cmtCd2","cmtCd3", DateTime.UtcNow, "", "UT KensaName2", 1, string.Empty),
            };

            var cache = new MasterDataCacheService(TenantProvider);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);


            // Act
            var sex = 1;
            var result = realtimeCheckerFinder.GetPatientHeight(hpId, ptId, birthDay, sinDay, sex, kensaInfDetailModels);

            // Assert
            Assert.AreEqual(50, result);

        }
        [Test]
        public void TC_095_CheckDosage_TEST_MasterData_No_Fake_Data()
        {
            //setup
            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT27201",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT27201" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            // Act
            var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

            // Assert
            Assert.That(result.Count == 0);
        }

        [Test]
        public void TC_096_CheckDosage_TEST_MasterData_UnitName_Equal_YakkaUnit()
        {
            //setup

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "a",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_097_CheckDosage_TEST_MasterData_TermVal_More_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "a",
                    RikikaRate = 0,
                    RikikaUnit = "a",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_098_CheckDosage_TEST_MasterData_Dogase_More_Than_0()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_099_CheckDosage_TEST_MasterData_OnceUnit_Kg()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "/kg"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_100_CheckDosage_TEST_MasterData_OnceUnit_m2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "/m2"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_101_CheckDosage_TEST_MasterData_OnceUnit_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "other"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_102_CheckDosage_TEST_MasterData_OnceUnitLimit_kg()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/kg"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_103_CheckDosage_TEST_MasterData_OnceUnitLimit_m2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/m2"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_104_CheckDosage_TEST_MasterData_OnceUnitLimit_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_105_CheckDosage_TEST_MasterData_DayUnit()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_106_CheckDosage_TEST_MasterData_DayUnit_m2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/m2",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_107_CheckDosage_TEST_MasterData_DayUnit_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_108_CheckDosage_TEST_MasterData_DayUnitLimit_m2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/m2"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_109_CheckDosage_TEST_MasterData_DayUnitLimit_kg()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/kg"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_110_CheckDosage_TEST_MasterData_DayUnitLimit_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_111_CheckDosage_TEST_MasterData_TermCheck()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "d"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_112_CheckDosage_TEST_MasterData_TermCheck_DosageLimitUnit_w()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "w"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_113_CheckDosage_TEST_MasterData_TermCheck_DosageLimitUnit_m()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "m"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_114_CheckDosage_TEST_MasterData_TermCheck_DosageLimitUnit_y()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_115_CheckDosage_TEST_MasterData_TermCheck_UnittermUnit_kg()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/kg"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_116_CheckDosage_TEST_MasterData_TermCheck_UnittermUnit_m2()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/m2"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 0);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_117_CheckDosage_TEST_MasterData_TermCheck_UnittermUnit_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "/abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_118_CheckDosage_TEST_MasterData_TermCheck_IsNotDayLimitFoundIntoAnyRecord()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc"
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_119_CheckDosage_TEST_MasterData_TermCheck_IsOnceLimitFoundIntoAllOfRecord()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "other",
                    OnceLimitUnit = "/abc",
                    DayLimitUnit = "",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_120_CheckDosage_TEST_MasterData_TermCheck_IsNotOnceLimitFoundIntoAnyRecord()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "/kg",
                    OnceLimitUnit = "",
                    DayLimitUnit = "",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMax = 100
                },
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_121_CheckDosage_TEST_MasterData_TermCheck_IsDayLimitFoundIntoAllOfRecord()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMax = 100,
                    DayLimit = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_122_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_21()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 1,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_123_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_21_Dogase_Morethan_MaxByDayToCheck()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_124_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_21_DosageResultModelIsNotNull()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 21,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 2);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_125_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_22()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "/kg",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 10000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }


        [Test]
        public void TC_126_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_22_MinPerOnce_MoreThan_Dosage()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "/kg",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 22,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_127_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "/kg",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 3,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_128_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_Other_MaxByOnceToCheck_MoreThan_Dogase()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/abc",
                    DayMax = 3,
                    OnceUnit = "/kg",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 100,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 10000,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_129_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_Other_MinPerDay_MoreThan_Dogase()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 0,
                    DayLimit = 2,
                    DayMin = 2
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 19,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_130_CheckDosage_TEST_MasterData_TermCheck_SinKouiKbn_Other_Other()
        {
            //setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");

            var hpId = 999;
            long ptId = 1231;
            var sinday = 20230101;
            var minCheck = true;
            var ratioSetting = 9.9;
            var currentHeight = -1;
            var currenWeight = 10;

            var dosageDosage = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = hpId,
                    DoeiCd = "UT9898",
                    DoeiSeqNo = 999999,
                    KonokokaCd = "",
                    KensaPcd = "",
                    AgeOver = 0,
                    AgeUnder = 0,
                    AgeCd = "1",
                    WeightOver = 0,
                    WeightUnder = 0,
                    BodyOver = 0,
                    BodyUnder = 0,
                    DrugRoute = "UTDrugRoute",
                    UseFlg = "",
                    DrugCondition = "",
                    DosageCheckFlg = "1",
                    KyugenCd = "",
                    DayUnit = "/kg",
                    DayMax = 3,
                    OnceUnit = "",
                    OnceLimitUnit = "",
                    DayLimitUnit = "abc",
                    DosageLimitTerm = 1,
                    UnittermLimit = 1,
                    DosageLimitUnit = "y",
                    UnittermUnit = "/abc",
                    OnceLimit = 999,
                    OnceMin = 100,
                    OnceMax = 0,
                    DayLimit = 2,
                    DayMin = 1
                }
            };
            var dosageDrugs = new List<DosageDrug>
            {
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = "UT271026",
                    DoeiCd = "UT9898",
                    DgurKbn = "",
                    KikakiUnit = "g",
                    YakkaiUnit = "g",
                    RikikaRate = 0,
                    RikikaUnit = "g",
                    YoukaiekiCd = ""
                },
            };

            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.DosageDosages.AddRange(dosageDosage);
            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            var listItem = new List<DrugInfo>()
            {
                new DrugInfo()
                {
                    Id = "",
                    ItemCD = "UT2720",
                    ItemName = "UNIT_TEST",
                    SinKouiKbn = 23,
                    Suryo = 19,
                    TermVal = 1,
                    UnitName = "g",
                    UsageQuantity = 1000,
                }
            };
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(hpId, new List<string>() { "UT2720" }, sinday, ptId);
            var realtimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider, cache);

            try
            {
                // Act
                var result = realtimeCheckerFinder.CheckDosage(hpId, ptId, sinday, listItem, minCheck, ratioSetting, currentHeight, currenWeight, new(), true);

                // Assert
                Assert.That(result.Count == 1);
            }
            finally
            {
                tenantTracking.DosageDosages.RemoveRange(dosageDosage);
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.PtInfs.RemoveRange(ptInfs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }
    }
}
