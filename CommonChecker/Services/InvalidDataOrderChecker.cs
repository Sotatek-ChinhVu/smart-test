using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;
using Helper.Constants;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class InvalidDataOrderChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {

        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;

        public InvalidDataOrderChecker(ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _systemGenerationConfRepository = systemGenerationConfRepository;
        }
        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {

            TOdrInf checkingOrder = unitCheckerResult.CheckingData;

            List<InvalidDataOrder> checkedResult = new List<InvalidDataOrder>();
            foreach (var detail in checkingOrder.OrdInfDetails)
            {
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(detail.DisplayedUnit))
                {
                    if (string.IsNullOrEmpty(detail.DisplayedQuantity))
                    {
                        InvalidDataOrder error = new InvalidDataOrder()
                        {
                            ErrorType = ErrorType.Quantity,
                            ItemName = detail.DisplayItemName
                        };
                        checkedResult.Add(error);
                    }
                    else if (detail.ItemCd == ItemCdConst.Con_Refill && detail.Suryo > _systemGenerationConfRepository.RefillSetting(unitCheckerResult.Sinday))
                    {
                        InvalidDataOrder error = new InvalidDataOrder()
                        {
                            ErrorType = ErrorType.RefillQuantityLimit,
                            ItemName = "'" + detail.ItemName + "'の数量は" + _systemGenerationConfRepository.RefillSetting(unitCheckerResult.Sinday) + "以下を入力してください。"
                        };
                        checkedResult.Add(error);
                    }
                }
                else if ((detail.SinKouiKbn == 95 || detail.SinKouiKbn == 96) && checkingOrder.SanteiKbn != 2)
                {
                    if (detail.Suryo.ToString().Length > 9)
                    {
                        InvalidDataOrder error = new InvalidDataOrder()
                        {
                            ErrorType = ErrorType.QuantityLimit,
                            ItemName = detail.DisplayItemName
                        };
                        checkedResult.Add(error);
                    }
                }
            }

            // When drag from super set, incase drug only (doesn't contains usage), OdrKouiKbn = 20 (IsDrug = false)
            if (checkingOrder.OdrKouiKbn >= 20 && checkingOrder.OdrKouiKbn <= 23 && checkingOrder.OrdInfDetails.Any(d => d.IsDrug))
            {
                var usage = checkingOrder.OrdInfDetails.FirstOrDefault(d => d.IsUsage);
                if (usage == null)
                {
                    InvalidDataOrder error = new InvalidDataOrder()
                    {
                        ErrorType = ErrorType.Usage,
                    };
                    checkedResult.Add(error);
                }
                else
                {
                    //Check Bukantu items
                    var bunkatuItem = checkingOrder.OrdInfDetails.Where(item => item.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu).FirstOrDefault();
                    if (bunkatuItem != null)
                    {
                        if (usage.Suryo != bunkatuItem.Suryo)
                        {
                            InvalidDataOrder error = new InvalidDataOrder()
                            {
                                ErrorType = ErrorType.BukantuItem,
                            };
                            checkedResult.Add(error);
                        }
                    }
                }
            }

            if (checkingOrder.IsInjection && checkingOrder.OrdInfDetails.Any(d => d.IsInjection))
            {
                var usage = checkingOrder.OrdInfDetails.FirstOrDefault(d => d.IsUsage);
                if (usage == null)
                {
                    InvalidDataOrder error = new InvalidDataOrder()
                    {
                        ErrorType = ErrorType.InjectionUsage,
                    };
                    checkedResult.Add(error);
                }
            }

            return unitCheckerResult;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            throw new NotImplementedException();
        }
    }
}
