using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class RealtimeChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {

        public void InjectProperties(int hpID, long ptID, int sinday, bool termLimitCheckingOnly = false)
        {
            _hpID = hpID;
            _ptID = ptID;
            _sinday = sinday;
            _termLimitCheckingOnly = termLimitCheckingOnly;
        }


        public void InjectFinder(IRealtimeCheckerFinder finder)
        {
            _finder = finder;
        }

        #region properties

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

        #endregion

        private List<PtAlrgyDrugModel> _listPtAlrgyDrug = new List<PtAlrgyDrugModel>();
        private List<PtAlrgyFoodModel> _listPtAlrgyFood = new List<PtAlrgyFoodModel>();
        private List<PtOtherDrugModel> _listPtOtherDrug = new List<PtOtherDrugModel>();
        private List<PtOtcDrugModel> _listPtOtcDrug = new List<PtOtcDrugModel>();
        private List<PtSuppleModel> _listPtSupple = new List<PtSuppleModel>();
        private List<PtKioRekiModel> _listPtKioReki = new List<PtKioRekiModel>();
        private List<string> _listDiseaseCode = new List<string>();
        private double _currentHeight = 0;
        private double _currentWeight = 0;

        private List<string> _listPtAlrgyDrugCode = new List<string>();

        public RealtimeChecker(IRealtimeCheckerFinder finder)
        {
            _finder = finder;
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
            List<DayLimitResultModel>? result = checkedResult.ErrorInfo as List<DayLimitResultModel>;
            return result ?? new List<DayLimitResultModel>();
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
    }
}
