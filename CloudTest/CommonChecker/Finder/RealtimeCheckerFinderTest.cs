using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;
using Domain.Models.Diseases;
using Reporting.Calculate.Extensions;

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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckFoodAllergy(hpId, ptId, sinDay, itemCodeModelList, level, new(), isDataOfDb);

                // Assert
                Assert.True(result.Any() && result.FirstOrDefault().Id == "TC002");
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
        public void TC_005_CheckProDrug()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS();
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int level = 4;
            bool isDataOfDb = true;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2700", "Id001"),
                new ItemCodeModel("UT2700", "Id002"),
                };

            var listCompare = new List<string>() { "UT2701" };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

            try
            {
                // Act
                var result = realtimcheckerfinder.CheckProDrug(hpId, ptId, sinDay, itemCodeModelList, listCompare);

                // Assert
                Assert.True(result.Any() && result.FirstOrDefault().Id == "Id001");
            }
            catch (Exception)
            {
                tenantTracking.PtAlrgyFoods.RemoveRange(alrgyFoods);
                tenantTracking.M12FoodAlrgy.RemoveRange(m12);
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEd);
                tenantTracking.M56ProdrugCd.RemoveRange(m56Prodrugs);
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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

        /// <summary>
        /// test yohocd checking equal yohocd listCompare
        /// </summary>
        [Test]
        public void TC_006_CheckProDrugForDuplication_Test_Equal_YohoCd()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS();
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD();
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_006_CheckProDrugForDuplication_Test_ZensinsayoFlg_Is_1()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var alrgyFoods = CommonCheckerData.ReadPtAlrgyFood();
            var m12 = CommonCheckerData.ReadM12FoodAlrgy("");
            var m56ExEd = CommonCheckerData.Read_M56_EX_ED_INGREDIENTS();
            var m56Prodrugs = CommonCheckerData.READ_M56_PRODRUG_CD();
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN();
            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            tenantTracking.PtAlrgyFoods.AddRange(alrgyFoods);
            tenantTracking.M12FoodAlrgy.AddRange(m12);
            tenantTracking.M56ExEdIngredients.AddRange(m56ExEd);
            tenantTracking.M56ProdrugCd.AddRange(m56Prodrugs);
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 111;
            int sinDay = 20230101;
            int setting = 0;

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
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_007_CheckSameComponent()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = CommonCheckerData.ReadPtInf();
            var ptAlrgyDrugs = CommonCheckerData.ReadPtAlrgyDrug();
            tenantTracking.PtInfs.AddRange(ptInfs);
            tenantTracking.PtAlrgyDrugs.AddRange(ptAlrgyDrugs);
            tenantTracking.SaveChanges();

            //Setup Param
            int hpId = 1;
            long ptId = 1231;
            int sinDay = 20230101;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("613110017", "Id1"),
                };
            var listCompare = new List<string>() { "620675301" };
            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, sinDay, ptId);
            var realtimcheckerfinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_008_CheckDiseaseChecker_Test_IsDataDb_True()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("TC008", "TC008");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("TC008");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("TC008");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.SaveChanges();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 5;
            bool dataDb = true;
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936TC008", "id1"),
            new ItemCodeModel("22TC008", "id2"),
            new ItemCodeModel("101TC008", "id3"),
            new ItemCodeModel("776TC008", "id4"),
            new ItemCodeModel("717TC008", "id5"),
        };

            // Arrange
            var cache = new MasterDataCacheService(TenantProvider);
            cache.InitCache(new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_008_CheckDiseaseChecker_Test_IsDataDb_False()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("TC008", "TC008");
            var m42DisCon = CommonCheckerData.ReadM42ContaindiDisCon("TC008");
            var m42DrugMainEx = CommonCheckerData.ReadM42ContaindiDrugMainEx("TC008");
            var ptByomei = CommonCheckerData.ReadPtByomei();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M42ContraindiDisCon.AddRange(m42DisCon);
            tenantTracking.M42ContraindiDrugMainEx.AddRange(m42DrugMainEx);
            tenantTracking.PtByomeis.AddRange(ptByomei);
            tenantTracking.SaveChanges();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230505;
            int settingLevel = 5;
            bool dataDb = false;
            var listItemCode = new List<ItemCodeModel>()
        {
            new ItemCodeModel("936TC008", "id1"),
            new ItemCodeModel("22TC008", "id2"),
            new ItemCodeModel("101TC008", "id3"),
            new ItemCodeModel("776TC008", "id4"),
            new ItemCodeModel("717TC008", "id5"),
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
            cache.InitCache(new List<string>() { "620160501" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_009_CheckDuplicatedClassForDuplication()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN();
            var m56YjDrugs = CommonCheckerData.READ_M56_YJ_DRUG_CLASS();
            var m56DrugClasses = CommonCheckerData.READ_M56_DRUG_CLASS();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56YjDrugClass.AddRange(m56YjDrugs);
            tenantTracking.M56DrugClass.AddRange(m56DrugClasses);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;
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
            cache.InitCache(new List<string>() { "UT2704", "UT2705" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
        public void TC_009_CheckDuplicatedClassForDuplication_Test_Current_And_Checking_Are_Same_ItemCd()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst("", "");
            var m56ExIngrdtMains = CommonCheckerData.READ_M56_EX_INGRDT_MAIN();
            var m56YjDrugs = CommonCheckerData.READ_M56_YJ_DRUG_CLASS();
            var m56DrugClasses = CommonCheckerData.READ_M56_DRUG_CLASS();
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.M56YjDrugClass.AddRange(m56YjDrugs);
            tenantTracking.M56DrugClass.AddRange(m56DrugClasses);
            tenantTracking.M56ExIngrdtMain.AddRange(m56ExIngrdtMains);
            tenantTracking.SaveChanges();

            int hpId = 999;
            long ptId = 1231;
            int sinDate = 20230101;
            int settingLevel = 0;
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
            cache.InitCache(new List<string>() { "UT2704", "UT2704" }, sinDate, ptId);
            var realTimeCheckerFinder = new RealtimeCheckerFinder(TenantProvider.GetNoTrackingDataContext(), cache);

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
    }
}
