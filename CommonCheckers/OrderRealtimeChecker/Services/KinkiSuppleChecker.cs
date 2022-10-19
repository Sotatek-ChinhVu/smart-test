using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class KinkiSuppleChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public List<PtSupple> ListPtSupple;

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        private int GetSettingLevel()
        {
            return SystemConfig.Instance.KinkiLevelSetting;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            // Read setting from SystemConfig
            int settingLevel = GetSettingLevel();
            if (settingLevel <= 0 || settingLevel >= 5)
            {
                return unitCheckerForOrderListResult;
            }

            // Get listItemCode
            List<TOdrInf> checkingOrderList = unitCheckerForOrderListResult.CheckingOrderList;
            List<string> listItemCode = GetAllOdrDetailCodeByOrderList(checkingOrderList);

            List<KinkiResultModel> checkedResult = Finder.CheckKinkiSupple(HpID, PtID, Sinday, settingLevel, listItemCode, ListPtSupple);

            if (checkedResult != null && checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
                unitCheckerForOrderListResult.ErrorOrderList = GetErrorOrderList(checkingOrderList, checkedResult);
            }

            return unitCheckerForOrderListResult;
        }

        private List<TOdrInf> GetErrorOrderList(List<TOdrInf> checkingOrderList, List<KinkiResultModel> checkedResultList)
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
    }
}
