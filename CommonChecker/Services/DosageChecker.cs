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

            List<DrugInfo> itemList = new List<DrugInfo>();

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
                itemList.AddRange(checkingOrder.OdrInfDetailModelsIgnoreEmpty
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
                    .ToList());
            }

            List<DosageResultModel> checkedResult = Finder!.CheckDosage(HpID, PtID, Sinday, itemList, isMinCheck, ratioSetting, CurrentHeight, CurrentWeight);

            if (TermLimitCheckingOnly)
            {
                checkedResult = checkedResult.Where(r => r.LabelChecking == DosageLabelChecking.TermLimit).ToList();
            }

            if (checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
                unitCheckerForOrderListResult.ErrorOrderList = GetErrorOrderList(unitCheckerForOrderListResult.CheckingOrderList, checkedResult);
            }

            return unitCheckerForOrderListResult;
        }

        private List<TOdrInf> GetErrorOrderList(List<TOdrInf> checkingOrderList, List<DosageResultModel> checkedResultList)
        {
            List<string> listErrorItemCode = checkedResultList.Select(r => r.ItemCd).ToList();
            List<double> suryoErrorList = checkedResultList.Select(r => r.CurrentValue).ToList();

            List<TOdrInf> resultList = new List<TOdrInf>();
            foreach (var checkingOrder in checkingOrderList)
            {
                var existed = checkingOrder.OdrInfDetailModelsIgnoreEmpty.Any(o => listErrorItemCode.Contains(o.ItemCd) && suryoErrorList.Contains(o.Suryo));
                if (existed)
                {
                    resultList.Add(checkingOrder);
                }
            }

            return resultList;
        }
    }
}
