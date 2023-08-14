using CommonChecker.DB;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.CommonChecker.Services
{
    public class InvalidDataOrderCheckerTest : BaseUT
    {
        [Test]
        public void CheckInvalidDataOrder_001_HandleCheckOrder()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList is null);
        }

        [Test]
        public void CheckInvalidDataOrder_002_ItemCd()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_003_DisplayedUnit()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 1, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_004_DisplayedQuantity_Null()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@BUNKATU", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@BUNKATU", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 1, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_005_DisplayedQuantity()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "@REFILL", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "@REFILL", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 1, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_006_DisplayedUnit_Null()
        {
            //Setups
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 95, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1000000000, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 96, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 1000000000, "", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_007_DisplayedUnit_Null()
        {
            //Setups
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1000000000, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 20, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 1000000000, "", 0, 0, 0, 1, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(21, 0, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }

        [Test]
        public void CheckInvalidDataOrder_008_DisplayedUnit_Null()
        {
            //Setups
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 30, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1000000000, "", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 30, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 1000000000, "", 0, 0, 0, 1, 1, "", "", "", 1),
            };

            var mock = new Mock<ISystemConfigRepository>();

            var odrInfoModel = new OrdInfoModel(30, 30, ordInfDetails);

            var unitCheckerForOrderListResult = new UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.InvaliData, odrInfoModel, 20230101, 111);
            var invalidDataOrderChecker = new InvalidDataOrderChecker<OrdInfoModel, OrdInfoDetailModel>(mock.Object);
            //Act
            var result = invalidDataOrderChecker.HandleCheckOrder(unitCheckerForOrderListResult);
            Assert.True(result.ErrorOrderList == null);
        }
    }
}
