using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;
using Helper.Constants;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DuplicationChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        private readonly SystemConfig? _systemConf;
        public DuplicationChecker(SystemConfig systemConf)
        {
            _systemConf = systemConf;
        }
        public DuplicationChecker()
        {

        }
        public List<TOdrInf> CurrentListOrder = new();

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            if (CurrentListOrder == null)
            {
                return unitCheckerResult;
            }

            TOdrInf checkingOrder = unitCheckerResult.CheckingData;
            List<TOdrDetail> currentOdrDetailList = GetOdrDetailListByCondition(CurrentListOrder);
            List<string> currentOdrDetailCodeList = currentOdrDetailList.Select(o => o.ItemCd.Trim()).ToList(); ;
            List<DuplicationResultModel> listErrorInfo = new List<DuplicationResultModel>();

            //◆処方行為（ODR_INF.ODR_KOUI_CD[21..23]）のみ
            //2.同一薬剤
            //3.成分重複
            if (checkingOrder.OdrKouiKbn < 21 || 23 < checkingOrder.OdrKouiKbn)
            {
                return unitCheckerResult;
            }

            #region Check duplicated item
            int checkDuplicationSetting = _systemConf?.CheckDupicatedSetting ?? 0;
            bool allowCheckDuplicatedItemCode = checkDuplicationSetting != 2;
            bool allowCheckDuplicatedIppanCode = checkDuplicationSetting == 1;

            if (allowCheckDuplicatedItemCode)
            {
                // Check duplicated itemCode
                List<DuplicationResultModel> listDuplicatedItemCode = CheckDuplicatedItemCode(checkingOrder, currentOdrDetailCodeList);
                if (listDuplicatedItemCode != null)
                {
                    listErrorInfo.AddRange(listDuplicatedItemCode);
                }
            }

            if (allowCheckDuplicatedIppanCode)
            {
                // Check duplicated ippanCode
                List<DuplicationResultModel> listDuplicatedIppanCode = CheckDuplicatedIppanCode(checkingOrder, currentOdrDetailList);
                if (listDuplicatedIppanCode != null)
                {
                    listErrorInfo.AddRange(listDuplicatedIppanCode);
                }
            }

            #endregion

            #region Check duplicated component
            List<string> listItemCode = GetAllOdrDetailCodeByOrder(checkingOrder);
            List<string> listCheckedCode = new List<string>();

            List<DrugAllergyResultModel> listDuplicatedComponentResult = new List<DrugAllergyResultModel>();
            if (_systemConf?.IsDuplicatedComponentForDuplication ?? true && listItemCode.Count != 0)
            {
                List<DrugAllergyResultModel> checkedResultAsLevelIntoOrder = Finder.CheckDuplicatedComponentForDuplication(HpID, PtID, Sinday, listItemCode, listItemCode, _systemConf?.GetHaigouSetting ?? 0);
                listDuplicatedComponentResult.AddRange(checkedResultAsLevelIntoOrder);

                List<DrugAllergyResultModel> checkedResultAsLevel = Finder.CheckDuplicatedComponentForDuplication(HpID, PtID, Sinday, listItemCode, currentOdrDetailCodeList, _systemConf?.GetHaigouSetting ?? 0);
                listDuplicatedComponentResult.AddRange(checkedResultAsLevel);

                listCheckedCode = new List<string>();
                listCheckedCode.AddRange(checkedResultAsLevelIntoOrder.Select(r => r.ItemCd).ToList());
                listCheckedCode.AddRange(checkedResultAsLevel.Select(r => r.ItemCd).ToList());
                listItemCode = listItemCode.Where(l => !listCheckedCode.Contains(l)).ToList();
            }

            if (_systemConf?.IsProDrugForDuplication ?? true || _systemConf.IsSameComponentForDuplication)
            {
                List<DrugAllergyResultModel> checkedResultAsLevel = new List<DrugAllergyResultModel>();
                if (_systemConf?.IsProDrugForDuplication ?? true && listItemCode.Count != 0)
                {
                    checkedResultAsLevel.AddRange(Finder.CheckProDrugForDuplication(HpID, PtID, Sinday, listItemCode, listItemCode, _systemConf?.GetHaigouSetting ?? 0));
                    checkedResultAsLevel.AddRange(Finder.CheckProDrugForDuplication(HpID, PtID, Sinday, listItemCode, currentOdrDetailCodeList, _systemConf?.GetHaigouSetting ?? 0));
                }

                if (_systemConf?.IsSameComponentForDuplication ?? true && listItemCode.Count != 0)
                {
                    checkedResultAsLevel.AddRange(Finder.CheckSameComponentForDuplication(HpID, PtID, Sinday, listItemCode, listItemCode, _systemConf?.GetHaigouSetting ?? 0));
                    checkedResultAsLevel.AddRange(Finder.CheckSameComponentForDuplication(HpID, PtID, Sinday, listItemCode, currentOdrDetailCodeList, _systemConf?.GetHaigouSetting ?? 0));
                }

                listDuplicatedComponentResult.AddRange(checkedResultAsLevel);
                listCheckedCode = checkedResultAsLevel.Select(r => r.ItemCd).ToList();
                listItemCode = listItemCode.Where(l => !listCheckedCode.Contains(l)).ToList();
            }

            if (_systemConf?.IsDuplicatedClassForDuplication ?? true)
            {
                listDuplicatedComponentResult.AddRange(Finder.CheckDuplicatedClassForDuplication(HpID, PtID, Sinday, listItemCode, listItemCode, _systemConf?.GetHaigouSetting ?? 0));
                listDuplicatedComponentResult.AddRange(Finder.CheckDuplicatedClassForDuplication(HpID, PtID, Sinday, listItemCode, currentOdrDetailCodeList, _systemConf?.GetHaigouSetting ?? 0));
            }

            listDuplicatedComponentResult.ForEach((duplicatedComponent) =>
            {
                listErrorInfo.Add(new DuplicationResultModel()
                {
                    Level = duplicatedComponent.Level,
                    ItemCd = duplicatedComponent.ItemCd,
                    DuplicatedItemCd = duplicatedComponent.AllergyItemCd,
                    AllergySeibunCd = duplicatedComponent.AllergySeibunCd,
                    SeibunCd = duplicatedComponent.SeibunCd,
                    Tag = duplicatedComponent.Tag,
                    IsIppanCdDuplicated = false,
                    IsComponentDuplicated = true,
                    IppanCode = string.Empty
                });
            });

            #endregion

            return unitCheckerResult;
        }

        public List<TOdrDetail> GetOdrDetailListByCondition(List<TOdrInf> listOrder)
        {
            List<TOdrDetail> result = new List<TOdrDetail>();
            if (listOrder == null || listOrder.Count == 0)
            {
                return result;
            }

            foreach (TOdrInf odrInf in listOrder)
            {
                //◆処方行為（ODR_INF.ODR_KOUI_CD[21..23]）のみ
                //2.同一薬剤
                //3.成分重複
                if (odrInf.OdrKouiKbn < 21 || 23 < odrInf.OdrKouiKbn)
                {
                    continue;
                }

                result.AddRange
                (
                    odrInf.OdrInfDetailModelsIgnoreEmpty
                    .Where(o => o.YohoKbn == 0 && o.DrugKbn > 0)
                    .ToList()
                );
            }
            return result;
        }

        //When check duplication into checkingOrder
        //If itemDetailA found duplicated issue with itemDetailB
        //Then itemDetailB also found duplicated issue with itemDetailA
        //So need to remove one
        private List<DuplicationResultModel> RemoveDuplicatedItem(List<DuplicationResultModel> listDrugAllergyResult)
        {
            List<DuplicationResultModel> result = new List<DuplicationResultModel>();

            listDrugAllergyResult.ForEach((r) =>
            {
                var duplicatedResult = result.Where(re => (re.ItemCd == r.DuplicatedItemCd && re.DuplicatedItemCd == r.ItemCd || re.ItemCd == r.ItemCd && re.DuplicatedItemCd == r.DuplicatedItemCd) &&
                                                           re.Level == r.Level).FirstOrDefault();
                if (duplicatedResult == null)
                {
                    result.Add(r);
                }
            });

            return result;
        }

        private List<DuplicationResultModel> CheckDuplicatedItemCode(TOdrInf checkingOrder, List<string> listDrugItemCode)
        {
            List<DuplicationResultModel> listErrorInfo = new List<DuplicationResultModel>();

            List<string> listDuplicatedItemCodeIntoOrder =
                checkingOrder.OdrInfDetailModelsIgnoreEmpty
                .Where(o => o.YohoKbn == 0 && o.DrugKbn > 0)
                .GroupBy(i => i.ItemCd)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            foreach (string itemCode in listDuplicatedItemCodeIntoOrder)
            {
                var duplicatedItem = checkingOrder.OdrInfDetailModelsIgnoreEmpty.FirstOrDefault(i => i.ItemCd == itemCode);

                if (duplicatedItem != null)
                {
                    listErrorInfo.Add(new DuplicationResultModel()
                    {
                        Level = 0,
                        ItemCd = itemCode,
                        DuplicatedItemCd = duplicatedItem.ItemCd,
                        IsIppanCdDuplicated = false,
                        IsComponentDuplicated = false,
                    });
                }
            }

            List<TOdrDetail> listDuplicatedItemCode =
                checkingOrder.OdrInfDetailModelsIgnoreEmpty
                .Where(o => o.YohoKbn == 0 && o.DrugKbn > 0 && o.ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu && o.ItemCd != ItemCdConst.Con_Refill &&
                listDrugItemCode.Contains(o.ItemCd) && !listDuplicatedItemCodeIntoOrder.Contains(o.ItemCd))
                .ToList();

            if (listDuplicatedItemCode != null)
            {
                foreach (var detail in listDuplicatedItemCode)
                {
                    listErrorInfo.Add(new DuplicationResultModel()
                    {
                        Level = 0,
                        ItemCd = detail.ItemCd,
                        DuplicatedItemCd = detail.ItemCd,
                        IsIppanCdDuplicated = false,
                        IsComponentDuplicated = false,
                    });
                }
            }

            return listErrorInfo;
        }

        private List<DuplicationResultModel> CheckDuplicatedIppanCode(TOdrInf checkingOrder, List<TOdrDetail> listComparedDrug)
        {
            List<DuplicationResultModel> listErrorInfo = new List<DuplicationResultModel>();

            List<TOdrDetail> listDetailWithoutDuplication = RemoveDuplicatedDetail(checkingOrder);

            #region Fix comment 3720
            List<TOdrDetail> listDetailOrderByIppandName =
                listDetailWithoutDuplication.Where(d => d.ReleasedType == ReleasedDrugType.CommonName ||
                                                        d.ReleasedType == ReleasedDrugType.CommonName_DoNotChangeTheDosageForm ||
                                                        d.ReleasedType == ReleasedDrugType.CommonName_DoesNotChangeTheContentStandard ||
                                                        d.ReleasedType == ReleasedDrugType.CommonName_DoesNotChangeTheContentStandardOrDosageForm
                                                 ).ToList();

            List<TOdrDetail> listComparedDrugOrderByIppandName =
                listComparedDrug.Where(d => d.ReleasedType == ReleasedDrugType.CommonName ||
                                            d.ReleasedType == ReleasedDrugType.CommonName_DoNotChangeTheDosageForm ||
                                            d.ReleasedType == ReleasedDrugType.CommonName_DoesNotChangeTheContentStandard ||
                                            d.ReleasedType == ReleasedDrugType.CommonName_DoesNotChangeTheContentStandardOrDosageForm
                                      ).ToList();

            #endregion

            listDetailOrderByIppandName.ForEach((checkingOrderDetail) =>
            {
                if (checkingOrderDetail.YohoKbn == 0 &&
                    checkingOrderDetail.DrugKbn > 0 &&
                    checkingOrderDetail.ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu &&
                    checkingOrderDetail.ItemCd != ItemCdConst.Con_Refill)
                {
                    var listDuplicatedIppanCd = listDetailOrderByIppandName.Where(d => checkingOrderDetail.ItemCd != d.ItemCd &&
                                                                                        !string.IsNullOrEmpty(d.IpnCd) &&
                                                                                        checkingOrderDetail.IpnCd == d.IpnCd)
                                                        .ToList();
                    listDuplicatedIppanCd.ForEach((duplicatedItem) =>
                    {
                        listErrorInfo.Add(new DuplicationResultModel()
                        {
                            Level = 0,
                            ItemCd = checkingOrderDetail.ItemCd,
                            DuplicatedItemCd = duplicatedItem.ItemCd,
                            IsIppanCdDuplicated = true,
                            IsComponentDuplicated = false,
                            IppanCode = checkingOrderDetail.IpnCd
                        });
                    });
                }
            });

            listDetailOrderByIppandName.ToList().ForEach((checkingOrderDetail) =>
            {
                if (checkingOrderDetail.YohoKbn == 0 &&
                    checkingOrderDetail.DrugKbn > 0 &&
                    checkingOrderDetail.ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu &&
                    checkingOrderDetail.ItemCd != ItemCdConst.Con_Refill)
                {
                    var listDuplicatedIppanCd = listComparedDrugOrderByIppandName.Where(d => checkingOrderDetail.ItemCd != d.ItemCd &&
                                                                            !string.IsNullOrEmpty(d.IpnCd) &&
                                                                            checkingOrderDetail.IpnCd == d.IpnCd)
                                                        .ToList();
                    listDuplicatedIppanCd.ForEach((duplicatedItem) =>
                    {
                        listErrorInfo.Add(new DuplicationResultModel()
                        {
                            Level = 0,
                            ItemCd = checkingOrderDetail.ItemCd,
                            DuplicatedItemCd = duplicatedItem.ItemCd,
                            IsIppanCdDuplicated = true,
                            IsComponentDuplicated = false,
                            IppanCode = checkingOrderDetail.IpnCd
                        });
                    });
                }
            });

            return listErrorInfo;
        }

        private List<TOdrDetail> RemoveDuplicatedDetail(TOdrInf checkingOrder)
        {
            List<string> groupItemCode =
                checkingOrder.OdrInfDetailModelsIgnoreEmpty
                .Where(o => o.YohoKbn == 0 && o.DrugKbn > 0)
                .GroupBy(i => i.ItemCd)
                .Select(y => y.Key)
                .ToList();

            List<TOdrDetail> result = new List<TOdrDetail>();
            foreach (string itemCode in groupItemCode)
            {
                var odrDetail = checkingOrder.OdrInfDetailModelsIgnoreEmpty.FirstOrDefault(i => i.ItemCd == itemCode);
                if (odrDetail != null)
                {
                    result.Add(odrDetail);
                }
            }
            return result;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            throw new NotImplementedException();
        }
    }
}
