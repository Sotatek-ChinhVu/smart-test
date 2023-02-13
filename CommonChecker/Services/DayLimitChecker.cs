using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DayLimitChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            List<DayLimitResultModel> resultList = new List<DayLimitResultModel>();
            List<TOdrInf> errorOrderList = new List<TOdrInf>();
            foreach (var checkingOrder in unitCheckerForOrderListResult.CheckingOrderList)
            {
                //◆内服（ODR_INF.ODR_KOUI_CD[21]）のみ
                //9.投与日数
                if (checkingOrder.OdrKouiKbn != 21)
                {
                    continue;
                }

                double usingDay = GetUsingDay(checkingOrder);

                if (usingDay > 0)
                {
                    List<ItemCodeModel> listItemCode = GetAllOdrDetailCodeByOrder(checkingOrder);
                    List<DayLimitResultModel> checkedResult = Finder.CheckDayLimit(HpID, Sinday, listItemCode, usingDay);

                    if (checkedResult != null && checkedResult.Count > 0)
                    {
                        resultList.AddRange(checkedResult);
                        errorOrderList.Add(checkingOrder);
                    }
                }
            }

            if (resultList.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = resultList;
                unitCheckerForOrderListResult.ErrorOrderList = errorOrderList;
            }

            return unitCheckerForOrderListResult;
        }

        private double GetUsingDay(TOdrInf order)
        {
            var usingInfo = order.OdrInfDetailModelsIgnoreEmpty.FirstOrDefault(o => o.IsDrugUsage);

            if (usingInfo == null)
            {
                return -1;
            }
            return usingInfo?.Suryo ?? 0;
        }
    }
}
