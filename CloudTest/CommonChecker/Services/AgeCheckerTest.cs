using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Services
{
    public class AgeCheckerTest : BaseUT
    {
        [Test]
        public void HandleCheckOrderList_InvalidSettingLevel_ReturnsOriginalResult()
        {
            var odrInf = new List<OrdInfoModel>();
            int sinDay = 20230603;
            int ptId = 1231;
            bool isDataOfDb = false;
            // Arrange
            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(RealtimeCheckerType.Age, odrInf, sinDay, 1231, new(new(), new(), new()), new(), new(), isDataOfDb);
            unitCheckerForOrderListResult.CheckingOrderList = new List<OrdInfoModel>();

            var orderListHandler = new OrderListHandler();

            // Act
            var result = orderListHandler.HandleCheckOrderList(unitCheckerForOrderListResult);

            // Assert
            Assert.AreEqual(unitCheckerForOrderListResult, result);
        }

        [Test]
        public void HandleCheckOrderList_ValidSettingLevel_ReturnsUpdatedResult()
        {
            // Arrange
            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>();
            unitCheckerForOrderListResult.CheckingOrderList = new List<TOdrInf> { new TOdrInf() };

            var orderListHandler = new OrderListHandler();
            orderListHandler.GetSettingLevel = () => 5;

            var result = orderListHandler.HandleCheckOrderList(unitCheckerForOrderListResult);

            Assert.IsNotNull(result.ErrorInfo);
            Assert.IsNotNull(result.ErrorOrderList);
        }

        [Test]
        public void GetErrorOrderList_WithErrors_ReturnsCorrectErrorOrderList()
        {
            // Arrange
            var checkingOrderList = new List<TOdrInf>
        {
            new TOdrInf
            {
                OdrInfDetailModelsIgnoreEmpty = new List<OdrInfDetailModel>
                {
                    new OdrInfDetailModel { ItemCd = "Item1" },
                    new OdrInfDetailModel { ItemCd = "Item2" },
                }
            },
            new TOdrInf
            {
                OdrInfDetailModelsIgnoreEmpty = new List<OdrInfDetailModel>
                {
                    new OdrInfDetailModel { ItemCd = "Item3" },
                    new OdrInfDetailModel { ItemCd = "Item4" },
                }
            }
        };

            var checkedResultList = new List<AgeResultModel>
        {
            new AgeResultModel { ItemCd = "Item2" },
            new AgeResultModel { ItemCd = "Item4" }
        };

            var orderListHandler = new OrderListHandler();

            // Act
            var result = orderListHandler.GetErrorOrderList(checkingOrderList, checkedResultList);

            // Assert
            Assert.AreEqual(2, result.Count);
            // Perform assertions on the result as per your requirements
            // For example, you can assert that the error order list contains the expected order items
            Assert.IsTrue(result.Any(o => o.OdrInfDetailModelsIgnoreEmpty.Any(d => d.ItemCd == "Item2")));
            Assert.IsTrue(result.Any(o => o.OdrInfDetailModelsIgnoreEmpty.Any(d => d.ItemCd == "Item4")));
        }
    }
}
