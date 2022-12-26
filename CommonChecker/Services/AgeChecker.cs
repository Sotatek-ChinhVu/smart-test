using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class AgeChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        private int GetSettingLevel()
        {
            return SystemConfig.Instance.AgeLevelSetting;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            // Read setting from SystemConfig
            int settingLevel = GetSettingLevel();
            if (settingLevel < 1 || settingLevel > 10)
            {
                return unitCheckerForOrderListResult;
            }

            // Get listItemCode
            List<TOdrInf> checkingOrderList = unitCheckerForOrderListResult.CheckingOrderList;
            List<string> listItemCode = GetAllOdrDetailCodeByOrderList(checkingOrderList);
            int ageTypeCheckSetting = SystemConfig.Instance.AgeTypeCheckSetting;

            List<AgeResultModel> checkedResult = Finder.CheckAge(HpID, PtID, Sinday, settingLevel, ageTypeCheckSetting, listItemCode);

            if (checkedResult != null && checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
            }

            return unitCheckerForOrderListResult;
        }
    }
}
