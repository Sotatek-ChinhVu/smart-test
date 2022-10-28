using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class AgeChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        private readonly SystemConfig? _systemConf;

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }
        public AgeChecker(SystemConfig systemConf)
        {
            _systemConf = systemConf;
        }
        public AgeChecker() { }
        private int GetSettingLevel()
        {
            return _systemConf?.AgeLevelSetting ?? default;
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
            int ageTypeCheckSetting = _systemConf?.AgeTypeCheckSetting ?? 0;

            List<AgeResultModel> checkedResult = Finder.CheckAge(HpID, PtID, Sinday, settingLevel, ageTypeCheckSetting, listItemCode);

            if (checkedResult != null && checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
            }

            return unitCheckerForOrderListResult;
        }
    }
}
