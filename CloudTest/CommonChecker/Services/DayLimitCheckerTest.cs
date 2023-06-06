using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;

namespace CloudUnitTest.CommonChecker.Services
{
    public class DayLimitCheckerTest : BaseUT
    {
        /// <summary>
        /// Test Is Error Order with ItemCd = 2
        /// </summary>
        [Test]
        public void CheckDayLimit_001_DayLimitError_WhenDrugExpired()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "611170008", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var odrInfoModel = new List<OrdInfoModel>();
            odrInfoModel.Add(new OrdInfoModel(21, 0, ordInfDetails));

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = 1;
            dayLimitChecker.PtID = 111;
            dayLimitChecker.Sinday = 20230101;
            dayLimitChecker.DataContext = TenantProvider.GetNoTrackingDataContext();
            //// Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
            //// Assert
            Assert.True(result.ErrorOrderList.Count == 1);
        }

        /// <summary>
        /// Test OdrKouiKbn = 21
        /// </summary>
        [Test]
        public void CheckDayLitmit_002_WhenCheckingOderWithOdrKouiKbnIs21()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var odrInfoModel = new List<OrdInfoModel>();
            odrInfoModel.Add(new OrdInfoModel(21, 0, ordInfDetails));

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = 1;
            dayLimitChecker.PtID = 111;
            dayLimitChecker.Sinday = 20230101;
            dayLimitChecker.DataContext = TenantProvider.GetNoTrackingDataContext();
            //// Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
            //// Assert
            Assert.True(!result.ErrorOrderList.Any());
        }

        /// <summary>
        ///Test Order with IsDrugUsage is false
        ///YohoKbn <= 0 and ItemCd != ItemCdConst.TouyakuChozaiNaiTon || ItemCd != ItemCdConst.TouyakuChozaiGai  -> !IsDrugUsage
        /// </summary>
        [Test]
        public void CheckDayLitmit_003_WhenCheckingOderWithOdrHasIsDrugUsageIsFalse()
        {
            //Setup
            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id2", 21, "Y101", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 0, "", "", "", 1),
            };

            var odrInfoModel = new List<OrdInfoModel>();
            odrInfoModel.Add(new OrdInfoModel(21, 0, ordInfDetails));

            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(
                                                                    RealtimeCheckerType.Days, odrInfoModel, 20230101, 111, new(new(), new(), new()), new(), new(), true);

            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = 1;
            dayLimitChecker.PtID = 111;
            dayLimitChecker.Sinday = 20230101;
            dayLimitChecker.DataContext = TenantProvider.GetNoTrackingDataContext();
            //// Act
            var result = dayLimitChecker.HandleCheckOrderList(unitCheckerForOrderListResult);
            //// Assert
            Assert.True(!result.ErrorOrderList.Any());
        }
    }
}
