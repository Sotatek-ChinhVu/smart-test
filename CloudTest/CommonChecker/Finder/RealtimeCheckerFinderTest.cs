﻿using CloudUnitTest.SampleData;
using CommonChecker.Caches;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;

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

        [Test]
        public void TC_006_CheckProDrugForDuplication()
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
            int setting = 0;

            var itemCodeModelList = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2700", "Id1"),
                };
            var listCompare = new List<ItemCodeModel>()
                {
                new ItemCodeModel("UT2701", "Id2"),
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
                Assert.False(result.Any());
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
    }
}
