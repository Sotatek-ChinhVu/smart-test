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
    }
}
