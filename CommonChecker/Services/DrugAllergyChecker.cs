using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DrugAllergyChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public List<string> ListPtAlrgyDrugCode { private get; set; } = new List<string>();

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            // Get listItemCode
            List<TOdrInf> checkingOrderList = unitCheckerForOrderListResult.CheckingOrderList;
            List<ItemCodeModel> listItemCode = GetAllOdrDetailCodeByOrderList(checkingOrderList);

            List<DrugAllergyResultModel> checkedResult = new List<DrugAllergyResultModel>();

            #region Handle duplication itemCode case

            List<ItemCodeModel> listDuplicatedItemCode = listItemCode.Where(i => ListPtAlrgyDrugCode.Contains(i.ItemCd)).ToList();

            if (listDuplicatedItemCode.Count > 0)
            {
                var yjCdList = Finder.GetYjCdListByItemCdList(HpID, listDuplicatedItemCode, Sinday);

                listDuplicatedItemCode.ForEach((i) =>
                {
                    string yjCd = string.Empty;
                    if (yjCdList.ContainsKey(i.ItemCd))
                    {
                        yjCd = yjCdList[i.ItemCd];
                    }
                    checkedResult.Add(new DrugAllergyResultModel()
                    {
                        Id = i.Id,
                        Level = 0,
                        ItemCd = i.ItemCd,
                        AllergyItemCd = i.ItemCd,
                        YjCd = yjCd,
                        AllergyYjCd = yjCd
                    });
                });

                listItemCode = listItemCode.Where(i => !listDuplicatedItemCode.Contains(i)).ToList();
            }

            #endregion

            if (SystemConfig.IsDuplicatedComponentChecked && listItemCode.Count != 0)
            {
                List<DrugAllergyResultModel> checkedResultAsLevel = Finder.CheckDuplicatedComponent(HpID, PtID, Sinday, listItemCode, ListPtAlrgyDrugCode);
                checkedResult.AddRange(checkedResultAsLevel);
                List<string> listCheckedCode = checkedResultAsLevel.Select(r => r.ItemCd).ToList();
                listItemCode = listItemCode.Where(l => !listCheckedCode.Contains(l.ItemCd)).ToList();
            }

            if ((SystemConfig.IsProDrugChecked || SystemConfig.IsSameComponentChecked) && listItemCode.Count != 0)
            {
                List<DrugAllergyResultModel> checkedResultAsLevel = new List<DrugAllergyResultModel>();
                if (SystemConfig.IsProDrugChecked && listItemCode.Count != 0)
                {
                    checkedResultAsLevel.AddRange(Finder.CheckProDrug(HpID, PtID, Sinday, listItemCode, ListPtAlrgyDrugCode));
                }

                if (SystemConfig.IsSameComponentChecked && listItemCode.Count != 0)
                {
                    checkedResultAsLevel.AddRange(Finder.CheckSameComponent(HpID, PtID, Sinday, listItemCode, ListPtAlrgyDrugCode));
                }

                checkedResult.AddRange(checkedResultAsLevel);
                List<string> listCheckedCode = checkedResultAsLevel.Select(r => r.ItemCd).ToList();
                listItemCode = listItemCode.Where(l => !listCheckedCode.Contains(l.ItemCd)).ToList();
            }

            if (SystemConfig.IsDuplicatedClassChecked && listItemCode.Count != 0)
            {
                checkedResult.AddRange(Finder.CheckDuplicatedClass(HpID, PtID, Sinday, listItemCode, ListPtAlrgyDrugCode));
            }

            if (checkedResult != null && checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
                unitCheckerForOrderListResult.ErrorOrderList = GetErrorOrderList(checkingOrderList, checkedResult);
            }

            return unitCheckerForOrderListResult;
        }

        private List<TOdrInf> GetErrorOrderList(List<TOdrInf> checkingOrderList, List<DrugAllergyResultModel> checkedResultList)
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
