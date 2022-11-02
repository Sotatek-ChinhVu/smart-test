using CommonCheckers.OrderRealtimeChecker.Models;
using CommonChecker.Types;
using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class FoodAllergyChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        private readonly SystemConfig? _systemConf;
        public FoodAllergyChecker(SystemConfig systemConf)
        {
            _systemConf = systemConf;
        }
        public FoodAllergyChecker()
        {

        }
        public List<PtAlrgyFoodModel> ListPtAlrgyFoods { get; set; } = new List<PtAlrgyFoodModel>();

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            // Read setting from SystemConfig
            int settingLevel = GetSettingLevel();
            if (settingLevel <= 0 || settingLevel >= 4)
            {
                return unitCheckerForOrderListResult;
            }

            // Get listItemCode
            List<TOdrInf> checkingOrderList = unitCheckerForOrderListResult.CheckingOrderList;
            List<string> listItemCode = GetAllOdrDetailCodeByOrderList(checkingOrderList);

            List<FoodAllergyResultModel> checkingResult = Finder.CheckFoodAllergy(HpID, PtID, Sinday, listItemCode, settingLevel, ListPtAlrgyFoods);
            if (!(checkingResult == null || checkingResult.Count == 0))
            {
                unitCheckerForOrderListResult.ErrorInfo = checkingResult;
                unitCheckerForOrderListResult.ErrorOrderList = GetErrorOrderList(checkingOrderList, checkingResult);
            }
            return unitCheckerForOrderListResult;
        }

        private List<TOdrInf> GetErrorOrderList(List<TOdrInf> checkingOrderList, List<FoodAllergyResultModel> checkedResultList)
        {
            List<string> listErrorItemCode = checkedResultList.Select(r => r.ItemCd).ToList();

            List<TOdrInf> resultList = new List<TOdrInf>();
            foreach (var checkingOrder in checkingOrderList)
            {
                var existed = checkingOrder.OdrInfDetailModelsIgnoreEmpty.Any(o => listErrorItemCode.Contains(o.ItemCd));
                if (existed)
                {
                    resultList.Add(checkingOrder);
                }
            }

            return resultList;
        }

        private int GetSettingLevel()
        {
            return _systemConf?.FoodAllergyLevelSetting ?? 0;
        }
    }
}
