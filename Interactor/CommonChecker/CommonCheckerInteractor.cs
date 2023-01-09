using CommonChecker;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using UseCase.CommonChecker;

namespace Interactor.CommonChecker
{
    public class CommonCheckerInteractor : IGetOrderCheckerInputPort
    {
        public RealTimeCheckerCondition CheckerCondition { get; set; } = new RealTimeCheckerCondition();

        public bool IsOrderChecking { get; set; } = true;

        private int _hpID;
        private long _ptID;
        private int _sinday;
        private bool _termLimitCheckingOnly;

        private IRealtimeCheckerFinder _finder;
        private IMasterFinder _masterFinder;
        private ISystemConfig _systemConfig;

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

        public CommonCheckerInteractor(IRealtimeCheckerFinder finder, IMasterFinder masterFinder, ISystemConfig systemConfig)
        {
            _finder = finder;
            _masterFinder = masterFinder;
            _systemConfig = systemConfig;
        }

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

        public void InitUnitCheck(UnitChecker<OrdInfoModel, OrdInfoDetailModel> unitChecker)
        {
            unitChecker.Finder = _finder;
            unitChecker.MasterFinder = _masterFinder;
            unitChecker.SystemConfig = _systemConfig;
            unitChecker.HpID = _hpID;
            unitChecker.PtID = _ptID;
            unitChecker.Sinday = _sinday;
        }

        public GetOrderCheckerOutputData Handle(GetOrderCheckerInputData inputData)
        {
            try
            {
                _hpID = inputData.HpId;
                _ptID = inputData.PtId;
                _sinday = inputData.SinDay;

                var checkedResult = CheckListOrder(inputData.CurrentListOdr, inputData.ListCheckingOrder);
                if (checkedResult == null || checkedResult.Count == 0)
                {
                    return new GetOrderCheckerOutputData(new(), GetOrderCheckerStatus.Successed);
                }
                else
                {
                    return new GetOrderCheckerOutputData(checkedResult.FirstOrDefault() ?? new(), GetOrderCheckerStatus.Error);
                }
            }
            catch (Exception)
            {

                return new GetOrderCheckerOutputData(new(), GetOrderCheckerStatus.Failed);
            }
           
        }

        public List<UnitCheckInfoModel> CheckListOrder(List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
        {
            List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> listErrorOfAllOrder = new List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>>();
            List<OrdInfoModel> listOrderError = new List<OrdInfoModel>();
            List<OrdInfoModel> tempCurrentListOdr = new List<OrdInfoModel>(currentListOdr);

            listCheckingOrder.ForEach((order) =>
            {
                List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> checkedOrderResult = GetErrorFromOrder(tempCurrentListOdr, order);

                if (checkedOrderResult.Count > 0)
                {
                    listErrorOfAllOrder.AddRange(checkedOrderResult);
                    listOrderError.Add(order);
                }

                tempCurrentListOdr.Add(order);
            });

            var checkListOrderResultList = GetErrorFromListOrder(listCheckingOrder);

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
            return listUnitCheckErrorInfo;

        }

        private List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> GetErrorFromOrder(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
        {
            List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> listError = new List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>>();
            if (CheckerCondition.IsCheckingDuplication)
            {
                UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> duplicationCheckResult = CheckDuplication(currentListOdr, checkingOrder);
                if (duplicationCheckResult.IsError)
                {
                    listError.Add(duplicationCheckResult);
                }
            }

            if (CheckerCondition.IsCheckingKinki)
            {
                UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> kinkiCheckResult = CheckKinki(currentListOdr, checkingOrder);
                if (kinkiCheckResult.IsError)
                {
                    listError.Add(kinkiCheckResult);
                }

                UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> kinkiUserCheckResult = CheckKinkiUser(currentListOdr, checkingOrder);
                if (kinkiUserCheckResult.IsError)
                {
                    listError.Add(kinkiUserCheckResult);
                }
            }

            return listError;
        }

        private List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>> GetErrorFromListOrder(List<OrdInfoModel> checkingOrderList)
        {
            List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>> listError = new List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>>();

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


        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckFoodAllergy(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> foodAllergyChecker =
                new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.FoodAllergy,
                    ListPtAlrgyFoods = _listPtAlrgyFood
                };
            InitUnitCheck(foodAllergyChecker);

            return foodAllergyChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDrugAllergy(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> drugAllergyChecker =
                new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.DrugAllergy,
                    ListPtAlrgyDrugCode = ListPtAlrgyDrugCode
                };
            InitUnitCheck(drugAllergyChecker);

            return drugAllergyChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckAge(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> ageChecker =
                new AgeChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.Age
                };
            InitUnitCheck(ageChecker);

            return ageChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDayLimit(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> dayLimitChecker =
                new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.Days
                };
            InitUnitCheck(dayLimitChecker);

            return dayLimitChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDosage(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> dosageChecker =
                new DosageChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.Dosage,
                    CurrentHeight = _currentHeight,
                    CurrentWeight = _currentWeight,
                    TermLimitCheckingOnly = _termLimitCheckingOnly
                };
            InitUnitCheck(dosageChecker);

            return dosageChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDisease(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> diseaseChecker =
                new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.Disease,
                    ListDiseaseCode = _listDiseaseCode,
                    ListPtKioReki = _listPtKioReki,
                };
            InitUnitCheck(diseaseChecker);

            return diseaseChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiOTC(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiOTCChecker =
                new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.KinkiOTC,
                    ListPtOtcDrug = _listPtOtcDrug
                };
            InitUnitCheck(kinkiOTCChecker);

            return kinkiOTCChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiTain(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiTainChecker =
                new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.KinkiTain,
                    ListPtOtherDrug = _listPtOtherDrug
                };
            InitUnitCheck(kinkiTainChecker);

            return kinkiTainChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiSupple(List<OrdInfoModel> checkingOrderList)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiSuppleChecker =
                new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CheckType = RealtimeCheckerType.KinkiSupplement,
                    ListPtSupple = _listPtSupple
                };
            InitUnitCheck(kinkiSuppleChecker);

            return kinkiSuppleChecker.CheckOrderList(checkingOrderList);
        }

        private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckDuplication(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> duplicationChecker =
                new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.Duplication
                };
            InitUnitCheck(duplicationChecker);

            return duplicationChecker.CheckOrder(checkingOrder);
        }

        private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckKinki(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiChecker =
                new KinkiChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.Kinki
                };
            InitUnitCheck(kinkiChecker);

            return kinkiChecker.CheckOrder(checkingOrder);
        }

        private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiUser(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
        {
            UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiUserChecker =
                new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>()
                {
                    CurrentListOrder = currentListOdr,
                    CheckType = RealtimeCheckerType.KinkiUser
                };
            InitUnitCheck(kinkiUserChecker);

            return kinkiUserChecker.CheckOrder(checkingOrder);
        }

    }
}
