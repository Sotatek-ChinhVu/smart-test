using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services.Interface;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Types;
using Entity.Tenant;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class RealtimeChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public RealtimeChecker()
        {
             
        }

        public void InjectProperties(int hpID, long ptID, int sinday, bool termLimitCheckingOnly = false)
        {
            _hpID = hpID;
            _ptID = ptID;
            _sinday = sinday;
            _termLimitCheckingOnly = termLimitCheckingOnly;
        }

        public void InjectHandler(IHandler<TOdrInf, TOdrDetail> handler)
        {
            _handler = handler;
        }

        public void InjectFinder(IRealtimeCheckerFinder finder)
        {
            _finder = finder;
        }

        #region properties

        private IHandler<TOdrInf, TOdrDetail> _handler;
        private IRealtimeCheckerFinder _finder;
        private int _hpID;
        private long _ptID;
        private int _sinday;
        private bool _termLimitCheckingOnly;
        #endregion

        #region CheckMode properties

        // Default check all condition
        public RealTimeCheckerCondition CheckerCondition { get; set; } = new RealTimeCheckerCondition();

        public bool IsOrderChecking { get; set; } = true;

        #endregion

        #region UnitCheck
        private void InitUnitCheck(UnitChecker<TOdrInf, TOdrDetail> unitChecker)
        {
            unitChecker.Handler = _handler;
            unitChecker.Finder = _finder;
            unitChecker.HpID = _hpID;
            unitChecker.PtID = _ptID;
            unitChecker.Sinday = _sinday;
        }

        private UnitCheckerResult<TOdrInf, TOdrDetail> CheckKinki(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> kinkiChecker =
                new KinkiChecker<TOdrInf, TOdrDetail>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.Kinki
                };
            InitUnitCheck(kinkiChecker);

            return kinkiChecker.CheckOrder(checkingOrder);
        }

        private UnitCheckerResult<TOdrInf, TOdrDetail> CheckKinkiUser(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> kinkiUserChecker =
                new KinkiUserChecker<TOdrInf, TOdrDetail>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.KinkiUser
                };
            InitUnitCheck(kinkiUserChecker);

            return kinkiUserChecker.CheckOrder(checkingOrder);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckKinkiOTC(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> kinkiOTCChecker =
                new KinkiOTCChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.KinkiOTC,
                    ListPtOtcDrug = _listPtOtcDrug
                };
            InitUnitCheck(kinkiOTCChecker);

            return kinkiOTCChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckKinkiTain(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> kinkiTainChecker =
                new KinkiTainChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.KinkiTain,
                    ListPtOtherDrug = _listPtOtherDrug
                };
            InitUnitCheck(kinkiTainChecker);

            return kinkiTainChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckKinkiSupple(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> kinkiSuppleChecker =
                new KinkiSuppleChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.KinkiSupplement,
                    ListPtSupple = _listPtSupple
                };
            InitUnitCheck(kinkiSuppleChecker);

            return kinkiSuppleChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerResult<TOdrInf, TOdrDetail> CheckDuplication(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> duplicationChecker =
                new DuplicationChecker<TOdrInf, TOdrDetail>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.Duplication
                };
            InitUnitCheck(duplicationChecker);

            return duplicationChecker.CheckOrder(checkingOrder);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckFoodAllergy(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> foodAllergyChecker =
                new FoodAllergyChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.FoodAllergy,
                    ListPtAlrgyFoods = _listPtAlrgyFood
                };
            InitUnitCheck(foodAllergyChecker);

            return foodAllergyChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckDrugAllergy(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> drugAllergyChecker =
                new DrugAllergyChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.DrugAllergy,
                    ListPtAlrgyDrugCode = ListPtAlrgyDrugCode
                };
            InitUnitCheck(drugAllergyChecker);

            return drugAllergyChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckAge(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> ageChecker =
                new AgeChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.Age
                };
            InitUnitCheck(ageChecker);

            return ageChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckDayLimit(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> dayLimitChecker =
                new DayLimitChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.Days
                };
            InitUnitCheck(dayLimitChecker);

            return dayLimitChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckDosage(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> dosageChecker =
                new DosageChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.Dosage,
                    CurrentHeight = _currentHeight,
                    CurrentWeight = _currentWeight,
                    TermLimitCheckingOnly = _termLimitCheckingOnly
                };
            InitUnitCheck(dosageChecker);

            return dosageChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckDisease(List<TOdrInf> checkingOrderList)
        {
            UnitChecker<TOdrInf, TOdrDetail> diseaseChecker =
                new DiseaseChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.Disease,
                    ListDiseaseCode = _listDiseaseCode,
                    ListPtKioReki = _listPtKioReki,
                };
            InitUnitCheck(diseaseChecker);

            return diseaseChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerResult<TOdrInf, TOdrDetail> CheckValidDataOrder(TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> validDataChecker =
                new InvalidDataOrderChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.InvaliData,
                };
            InitUnitCheck(validDataChecker);

            return validDataChecker.CheckOrder(checkingOrder);
        }

        public UnitCheckerResult<TOdrInf, TOdrDetail> AutoCheck(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> autoCheckChecker =
                new AutoCheckChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.AutoCheck,
                    CurrentListOrder = currentListOdr
                };
            InitUnitCheck(autoCheckChecker);

            UnitCheckerResult<TOdrInf, TOdrDetail> autoCheckResult = autoCheckChecker.CheckOrder(checkingOrder);

            return autoCheckResult;
        }

        #endregion

        private List<PtAlrgyDrugModel> _listPtAlrgyDrug;
        private List<PtAlrgyFoodModel> _listPtAlrgyFood;
        private List<PtOtherDrugModel> _listPtOtherDrug;
        private List<PtOtcDrugModel> _listPtOtcDrug;
        private List<PtSuppleModel> _listPtSupple;
        private List<PtKioRekiModel> _listPtKioReki;
        private List<string> _listDiseaseCode;
        private double _currentHeight = 0;
        private double _currentWeight = 0;

        private List<string> _listPtAlrgyDrugCode;
        public List<string> ListPtAlrgyDrugCode
        {
            get
            {
                if (_listPtAlrgyDrugCode == null)
                {
                    if (_listPtAlrgyDrug != null)
                    {
                        _listPtAlrgyDrugCode = _listPtAlrgyDrug.Select(p => p.ItemCd).ToList();
                    }
                    else
                    {
                        List<PtAlrgyDrugModel> drugAllergyAsPatient = _finder.GetDrugAllergyByPtId(_hpID, _ptID, _sinday);
                        _listPtAlrgyDrugCode = drugAllergyAsPatient.Select(dr => dr.ItemCd).ToList();
                    }
                }
                return _listPtAlrgyDrugCode;
            }
        }

        public void UpdateCurrentData(SpecialNoteModel specialNoteModel, List<string> listDiseaseCode)
        {
            if (specialNoteModel != null)
            {
                _listPtAlrgyDrug = specialNoteModel.ListPtAlrgyDrug;
                _listPtAlrgyFood = specialNoteModel.ListPtAlrgyFood;
                _listPtOtherDrug = specialNoteModel.ListPtOtherDrug;
                _listPtOtcDrug = specialNoteModel.ListPtOtcDrug;
                _listPtSupple = specialNoteModel.ListPtSupple;
                _listPtKioReki = specialNoteModel.ListPtKioReki;
                _currentHeight = specialNoteModel.Height;
                _currentWeight = specialNoteModel.Weight;
            }
            _listDiseaseCode = listDiseaseCode;
        }

        // For this checking then don't need to show error message
        public List<DayLimitResultModel> CheckOnlyDayLimit(TOdrInf checkingOrder)
        {
            UnitChecker<TOdrInf, TOdrDetail> dayLimitChecker =
                new DayLimitChecker<TOdrInf, TOdrDetail>()
                {
                    CheckType = RealtimeCheckerType.Days
                };
            InitUnitCheck(dayLimitChecker);

            UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> checkedResult = dayLimitChecker.CheckOrderList(new List<TOdrInf>() { checkingOrder });
            List<DayLimitResultModel> result = checkedResult.ErrorInfo as List<DayLimitResultModel>;
            return result ?? new List<DayLimitResultModel>();
        }

        private TOdrInf HandleValidDataOrder(TOdrInf checkingOrder)
        {
            TOdrInf result = checkingOrder;
            while (true)
            {
                if (CheckerCondition.IsCheckingInvalidData)
                {
                    UnitCheckerResult<TOdrInf, TOdrDetail> invalidDataCheckResult = CheckValidDataOrder(result);

                    if (invalidDataCheckResult.IsError)
                    {
                        invalidDataCheckResult.ActionType = _handler.ShowOrderErrorInfo(invalidDataCheckResult);

                        if (invalidDataCheckResult.ActionType == ActionResultType.Edit)
                        {
                            TOdrInf newData = _handler.EditOrder(invalidDataCheckResult.CheckingData);
                            invalidDataCheckResult.NewData = newData;
                        }

                        if (invalidDataCheckResult.ActionType == ActionResultType.Abort ||
                            (invalidDataCheckResult.ActionType == ActionResultType.Edit && invalidDataCheckResult.NewData == null))
                        {
                            return null;
                        }
                        else if (invalidDataCheckResult.ActionType == ActionResultType.Edit)
                        {
                            result = invalidDataCheckResult.NewData;
                            continue;
                        }
                    }
                }
                break;
            }
            return result;
        }

        private List<UnitCheckerResult<TOdrInf, TOdrDetail>> GetErrorFromOrder(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            List<UnitCheckerResult<TOdrInf, TOdrDetail>> listError = new List<UnitCheckerResult<TOdrInf, TOdrDetail>>();
            if (CheckerCondition.IsCheckingDuplication)
            {
                UnitCheckerResult<TOdrInf, TOdrDetail> duplicationCheckResult = CheckDuplication(currentListOdr, checkingOrder);
                if (duplicationCheckResult.IsError)
                {
                    listError.Add(duplicationCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingKinki)
            {
                UnitCheckerResult<TOdrInf, TOdrDetail> kinkiCheckResult = CheckKinki(currentListOdr, checkingOrder);
                if (kinkiCheckResult.IsError)
                {
                    listError.Add(kinkiCheckResult);
                }

                UnitCheckerResult<TOdrInf, TOdrDetail> kinkiUserCheckResult = CheckKinkiUser(currentListOdr, checkingOrder);
                if (kinkiUserCheckResult.IsError)
                {
                    listError.Add(kinkiUserCheckResult);
                }
            }

            return listError;
        }

        private List<UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>> GetErrorFromListOrder(List<TOdrInf> checkingOrderList)
        {
            List<UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>> listError = new List<UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>>();

            if (CheckerCondition.IsCheckingAllergy)
            {
                var foodAllergyCheckResult = CheckFoodAllergy(checkingOrderList);
                if (foodAllergyCheckResult.IsError)
                {
                    listError.Add(foodAllergyCheckResult);
                }

                var drugAllergyCheckResult = CheckDrugAllergy(checkingOrderList);
                if (drugAllergyCheckResult.IsError)
                {
                    listError.Add(drugAllergyCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingAge)
            {
                var ageCheckResult = CheckAge(checkingOrderList);
                if (ageCheckResult.IsError)
                {
                    listError.Add(ageCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingDisease)
            {
                var diseaseCheckResult = CheckDisease(checkingOrderList);
                if (diseaseCheckResult.IsError)
                {
                    listError.Add(diseaseCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingKinki)
            {
                var kinkiTainCheckResult = CheckKinkiTain(checkingOrderList);
                if (kinkiTainCheckResult.IsError)
                {
                    listError.Add(kinkiTainCheckResult);
                }

                var kinkiOTCCheckResult = CheckKinkiOTC(checkingOrderList);
                if (kinkiOTCCheckResult.IsError)
                {
                    listError.Add(kinkiOTCCheckResult);
                }

                var kinkiSuppleCheckResult = CheckKinkiSupple(checkingOrderList);
                if (kinkiSuppleCheckResult.IsError)
                {
                    listError.Add(kinkiSuppleCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingDays)
            {
                var dayLimitCheckResult = CheckDayLimit(checkingOrderList);
                if (dayLimitCheckResult.IsError)
                {
                    listError.Add(dayLimitCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingDosage)
            {
                var dayLimitCheckResult = CheckDosage(checkingOrderList);
                if (dayLimitCheckResult.IsError)
                {
                    listError.Add(dayLimitCheckResult);
                }
            }

            return listError;
        }

        public List<TOdrInf> CheckListOrder(List<TOdrInf> currentListOdr, List<TOdrInf> listCheckingOrder)
        {
            List<TOdrInf> checkedResult = new List<TOdrInf>();


            foreach (TOdrInf checkingOrder in listCheckingOrder)
            {
                TOdrInf tempResult = HandleValidDataOrder(checkingOrder);
                if (tempResult == null)
                {
                    return new List<TOdrInf>();
                }

                checkedResult.Add(tempResult);
            }

            while (true)
            {
                List<UnitCheckerResult<TOdrInf, TOdrDetail>> listErrorOfAllOrder = new List<UnitCheckerResult<TOdrInf, TOdrDetail>>();
                List<TOdrInf> listOrderError = new List<TOdrInf>();
                List<TOdrInf> tempCurrentListOdr = new List<TOdrInf>(currentListOdr);

                checkedResult.ForEach((order) =>
                {
                    List<UnitCheckerResult<TOdrInf, TOdrDetail>> checkedOrderResult = GetErrorFromOrder(tempCurrentListOdr, order);

                    if (checkedOrderResult.Count > 0)
                    {
                        listErrorOfAllOrder.AddRange(checkedOrderResult);
                        listOrderError.Add(order);
                    }

                    tempCurrentListOdr.Add(order);
                });

                var checkListOrderResultList = GetErrorFromListOrder(checkedResult);

                foreach (var checkListOrderResult in checkListOrderResultList)
                {
                    var notExistErrorOrderList = checkListOrderResult.ErrorOrderList.Where(o => !listOrderError.Contains(o)).ToList();
                    if (notExistErrorOrderList.Count > 0)
                    {
                        listOrderError.AddRange(notExistErrorOrderList);
                    }
                }

                List<UnitCheckInfoModel> listUnitCheckErrorInfo = new List<UnitCheckInfoModel>();
                listErrorOfAllOrder.ForEach((error) =>
                {
                    listUnitCheckErrorInfo.Add(new UnitCheckInfoModel()
                    {
                        CheckerType = error.CheckerType,
                        ErrorInfo = error.ErrorInfo,
                        IsError = error.IsError,
                        PtId = error.PtId,
                        Sinday = error.Sinday,
                    });
                });

                foreach (var error in checkListOrderResultList)
                {
                    listUnitCheckErrorInfo.Add(new UnitCheckInfoModel()
                    {
                        CheckerType = error.CheckerType,
                        ErrorInfo = error.ErrorInfo,
                        IsError = error.IsError,
                        PtId = error.PtId,
                        Sinday = error.Sinday,
                    });
                }

                if (listUnitCheckErrorInfo.Count > 0)
                {
                    ActionResultType actionType = _handler.ShowOrderErrorInfo(listUnitCheckErrorInfo, IsOrderChecking);

                    if (actionType == ActionResultType.Abort)
                    {
                        return new List<TOdrInf>();
                    }

                    if (actionType == ActionResultType.Edit)
                    {
                        for (int i = 0; i < listOrderError.Count; i++)
                        {
                            var orderError = listOrderError[i];
                            TOdrInf newOrder = _handler.EditOrder(orderError);
                            if (newOrder == null)
                            {
                                return new List<TOdrInf>();
                            }
                            int index = checkedResult.IndexOf(orderError);
                            checkedResult[index] = newOrder;
                        }
                        continue;
                    }
                }
                break;
            }



            return checkedResult;
        }

        public TOdrInf CheckOrder(List<TOdrInf> currentListOdr, TOdrInf checkingOrder)
        {
            var checkedResult = CheckListOrder(currentListOdr, new List<TOdrInf>() { checkingOrder });
            if (checkedResult == null || checkedResult.Count == 0)
            {
                return null;
            }
            else
            {
                return checkedResult.First();
            }
        }
    }
}
