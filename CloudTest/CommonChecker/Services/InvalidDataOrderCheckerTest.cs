using CommonChecker.DB;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using DocumentFormat.OpenXml.Bibliography;
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
    }
}
