using CommonChecker.Caches;
using CommonChecker.DB;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonChecker.Services;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using DocumentFormat.OpenXml.Bibliography;
using Entity.Tenant;
using Moq;
using System.Text.Json;

namespace CloudUnitTest.CommonChecker.Services
{
    public class InvalidDataOrderCheckerTest : BaseUT
    {
        /// <summary>
        /// Quantity Error
        /// Test error when DisplayedQuantity is empty & DisplayUnit != null or empty
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_001_HandleCheckOrder_Quantity_Error()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@BUNKATU", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@BUNKATU", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true && 
                result.CheckerType == RealtimeCheckerType.InvaliData && 
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// RefillQuantityLimit Error
        /// Test error when ItemCd = @REFILL & Suryo > Refill setting
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_002_HandleCheckOrder_RefillQuantityLimit()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@REFILL", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// QuantityLimit Error
        /// Test error QuantityLimit  with KouiKbn = 95
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_003_HandleCheckOrder_QuantityLimit()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 95, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// QuantityLimit Error
        /// Test error QuantityLimit with KouiKbn = 96
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_004_HandleCheckOrder_QuantityLimit()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// Usage Error
        /// Test error When drag from super set, incase drug only (doesn't contains usage), OdrKouiKbn = 20 (IsDrug = false)
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_005_HandleCheckOrder_Usage_Error()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        [Test]
        public void CheckInvalidData_006_HandleCheckOrderList_ThrowsNotImplementedException()
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

            var mock = new Mock<ISystemConfigRepository>();

            // Arrange
            var ageChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            var unitChecker = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                     RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

            // Act and Assert
            Assert.Throws<NotImplementedException>(() => ageChecker.HandleCheckOrderList(unitChecker));
        }

        [Test]
        public void CheckInvalidDataOrder_007_HandleCheckOrder_Test_ItemCd_Is_Empty()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == false &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// Suryo > RefillSetting
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_008_HandleCheckOrder_Test_ItemCd_Is_Con_Refill()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 3, "UnitName", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2002 && p.GrpEdaNo == 0);

            var temp = systemGenerationConf?.Val ?? 0;
            if (systemGenerationConf != null)
            {
                systemGenerationConf.Val = 2;
            }
            else
            {
                systemGenerationConf = new SystemGenerationConf
                {
                    HpId = 1,
                    GrpCd = 2002,
                    GrpEdaNo = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 2
                };
                tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
            }
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);

            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            invalidDataOrderChecker.InitFinder(tenantNoTracking, cache);

            try
            {
                //Act
                var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

                Assert.True(
                    result.IsError == true &&
                    result.CheckerType == RealtimeCheckerType.InvaliData &&
                    result.ErrorInfo != null &&
                    result.PtId == 111 &&
                    result.Sinday == 20230101
                    );
            }
            finally
            {
                if (systemGenerationConf != null) systemGenerationConf.Val = temp;
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Suryo < RefillSetting
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_009_HandleCheckOrder_Test_ItemCd_Is_Con_Refill()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 3, "UnitName", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2002 && p.GrpEdaNo == 0);

            var temp = systemGenerationConf?.Val ?? 0;
            if (systemGenerationConf != null)
            {
                systemGenerationConf.Val = 4;
            }
            else
            {
                systemGenerationConf = new SystemGenerationConf
                {
                    HpId = 1,
                    GrpCd = 2002,
                    GrpEdaNo = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 4
                };
                tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
            }
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);

            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            invalidDataOrderChecker.InitFinder(tenantNoTracking, cache);

            try
            {
                //Act
                var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

                Assert.True(
                    result.IsError == true &&
                    result.CheckerType == RealtimeCheckerType.InvaliData &&
                    result.ErrorInfo != null &&
                    result.PtId == 111 &&
                    result.Sinday == 20230101
                    );
            }
            finally
            {
                if (systemGenerationConf != null) systemGenerationConf.Val = temp;
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// Suryo > RefillSetting
        /// ItemCd != @REFILL
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_010_HandleCheckOrder_Test_RefillSetting()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 3, "UnitName", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1 && p.GrpCd == 2002 && p.GrpEdaNo == 0);

            var temp = systemGenerationConf?.Val ?? 0;
            if (systemGenerationConf != null)
            {
                systemGenerationConf.Val = 1;
            }
            else
            {
                systemGenerationConf = new SystemGenerationConf
                {
                    HpId = 1,
                    GrpCd = 2002,
                    GrpEdaNo = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 2,
                    UpdateId = 2,
                    Val = 1
                };
                tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
            }
            tenantTracking.SaveChanges();

            var cache = new MasterDataCacheService(TenantProvider);

            cache.InitCache(new List<string>() { "620160501" }, 20230101, 1231);
            invalidDataOrderChecker.InitFinder(tenantNoTracking, cache);

            try
            {
                //Act
                var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

                Assert.False(
                    result.IsError == true &&
                    result.CheckerType == RealtimeCheckerType.InvaliData &&
                    result.PtId == 111 &&
                    result.Sinday == 20230101
                    );
            }
            finally
            {
                if (systemGenerationConf != null) systemGenerationConf.Val = temp;
                tenantTracking.SaveChanges();
            }
        }

        /// <summary>
        /// SinkouiKbn = 95
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_011_HandleCheckOrder_Error_SinkouiKbn_Is_95()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 95, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// SinkouiKbn = 95
        /// SanteiKbn = 2
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_012_HandleCheckOrder_SanteiKbn_Is_2()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 95, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1234567890, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 2, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.False(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// SinkouiKbn = 96
        /// Suryo.Length = 9
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_013_HandleCheckOrder_Error_SinkouiKbn_Is_95()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 96, "UNITTEST", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 123456789, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.False(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OdrKouiKbn = 20
        /// OrdInfDetails IsDrug = true
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_014_HandleCheckOrder_Test_OdrKouiKbn()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@REFILL", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(20, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OdrKouiKbn = 23
        /// OrdInfDetails IsDrug = true
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_015_HandleCheckOrder_Test_OdrKouiKbn_Is_23()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@REFILL", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(23, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OdrKouiKbn = 20
        /// OrdInfDetails IsDrug = true
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_016_HandleCheckOrder_Test_OdrKouiKbn_Is_20()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@REFILL", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(20, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.True(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OdrKouiKbn = 19
        /// OrdInfDetails IsDrug = true
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_017_HandleCheckOrder_Test_OdrKouiKbn_Is_20()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "UT", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "UT", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(19, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.False(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OdrKouiKbn = 24
        /// OrdInfDetails IsDrug = true
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_018_HandleCheckOrder_Test_OdrKouiKbn_Is_24()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "UT", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "UT", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(24, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.False(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }

        /// <summary>
        /// OrdInfDetails IsDrug = false
        /// </summary>
        [Test]
        public void CheckInvalidDataOrder_019_HandleCheckOrder_Test_OdrKouiKbn()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
        new OrdInfoDetailModel("id1", 19, "IsDrugFalse", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
        new OrdInfoDetailModel("id2", 19, "IsDrugFalse", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);

            invalidDataOrderChecker.HpID = 1;
            invalidDataOrderChecker.PtID = 111;
            invalidDataOrderChecker.Sinday = 20230101;

            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);

            Assert.False(
                result.IsError == true &&
                result.CheckerType == RealtimeCheckerType.InvaliData &&
                result.ErrorInfo != null &&
                result.PtId == 111 &&
                result.Sinday == 20230101
                );
        }
    }
}
