using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DosageChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public double CurrentHeight { get; set; }

        public double CurrentWeight { get; set; }

        public bool TermLimitCheckingOnly { get; set; }

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            bool isMinCheck = SystemConfig!.DosageMinCheckSetting;
            double ratioSetting = SystemConfig.DosageRatioSetting;
            List<DosageResultModel> resultList = new List<DosageResultModel>();
            List<TOdrInf> errorOrderList = new List<TOdrInf>();
            foreach (var checkingOrder in unitCheckerForOrderListResult.CheckingOrderList)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (checkingOrder.OdrKouiKbn == 21 && !SystemConfig.DosageDrinkingDrugSetting ||
                checkingOrder.OdrKouiKbn == 22 && !SystemConfig.DosageDrugAsOrderSetting ||
                checkingOrder.OdrKouiKbn == 23 ||
                checkingOrder.OdrKouiKbn == 28 ||
                !new List<int>() { 21, 22, 23, 28 }.Contains(checkingOrder.OdrKouiKbn) && !SystemConfig.DosageOtherDrugSetting)
                {
                    continue;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                double usageQuantity = 0;
                var usageItem = checkingOrder.OdrInfDetailModelsIgnoreEmpty.FirstOrDefault(d => d.IsStandardUsage);
                if (usageItem != null)
                {
                    usageQuantity = usageItem.Suryo;
                }
                // Get listItemCode
                List<DrugInfo> itemList = checkingOrder.OdrInfDetailModelsIgnoreEmpty
                    .Where(i => i.DrugKbn > 0)
                    .Select(i => new DrugInfo()
                    {
                        Id = i.Id,
                        ItemCD = i.ItemCd,
                        ItemName = i.ItemName,
                        Suryo = i.Suryo,
                        UnitName = i.UnitName,
                        TermVal = i.TermVal,
                        SinKouiKbn = checkingOrder.OdrKouiKbn,
                        UsageQuantity = usageQuantity
                    })
                    .ToList();

                if (itemList.Count == 0)
                {
                    continue;
                }

                List<DosageResultModel> checkedResult = Finder!.CheckDosage(HpID, PtID, Sinday, itemList, isMinCheck, ratioSetting, CurrentHeight, CurrentWeight);

                if (TermLimitCheckingOnly)
                {
                    checkedResult = checkedResult.Where(r => r.LabelChecking == DosageLabelChecking.TermLimit).ToList();
                }

                if (checkedResult.Count > 0)
                {
                    errorOrderList.Add(checkingOrder);
                    resultList.AddRange(checkedResult);
                }
            }

            if (resultList.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = resultList;
                unitCheckerForOrderListResult.ErrorOrderList = errorOrderList;
            }

            return unitCheckerForOrderListResult;
        }
    }
}
