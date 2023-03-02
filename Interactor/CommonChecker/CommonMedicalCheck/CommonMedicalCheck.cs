using CommonChecker.DB;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Helper.Extension;
using Infrastructure.Interfaces;

namespace Interactor.CommonChecker.CommonMedicalCheck;

public class CommonMedicalCheck : ICommonMedicalCheck
{
    public RealTimeCheckerCondition CheckerCondition { get; set; } = new RealTimeCheckerCondition();

    public bool IsOrderChecking { get; set; } = true;

    private int _hpID;
    private long _ptID;
    private int _sinday;
    private bool _termLimitCheckingOnly;
    private readonly IRealtimeOrderErrorFinder _realtimeOrderErrorFinder;

    private readonly double _currentHeight = 0;
    private readonly double _currentWeight = 0;

    private readonly ITenantProvider _tenantProvider;

    public CommonMedicalCheck(ITenantProvider tenantProvider, IRealtimeOrderErrorFinder realtimeOrderErrorFinder)
    {
        _tenantProvider = tenantProvider;
        _realtimeOrderErrorFinder = realtimeOrderErrorFinder;
    }


    public void InitUnitCheck(UnitChecker<OrdInfoModel, OrdInfoDetailModel> unitChecker)
    {
        unitChecker.DataContext = _tenantProvider.GetNoTrackingDataContext();
        unitChecker.HpID = _hpID;
        unitChecker.PtID = _ptID;
        unitChecker.Sinday = _sinday;
    }

    public List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
    {
        _hpID = hpId;
        _ptID = ptId;
        _sinday = sinday;
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

    public List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> listCheckingOrder, RealTimeCheckerCondition checkerCondition)
    {
        _hpID = hpId;
        _ptID = ptId;
        _sinday = sinday;
        CheckerCondition = checkerCondition;
        List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> listErrorOfAllOrder = new List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>>();
        List<OrdInfoModel> listOrderError = new List<OrdInfoModel>();
        List<OrdInfoModel> tempCurrentListOdr = new();

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

    #region Check
    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckFoodAllergy(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> foodAllergyChecker =
            new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.FoodAllergy
            })
        {
            InitUnitCheck(foodAllergyChecker);
            return foodAllergyChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDrugAllergy(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> drugAllergyChecker =
            new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.DrugAllergy
            })
        {
            InitUnitCheck(drugAllergyChecker);

            return drugAllergyChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckAge(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> ageChecker =
           new AgeChecker<OrdInfoModel, OrdInfoDetailModel>()
           {
               CheckType = RealtimeCheckerType.Age
           })
        {
            InitUnitCheck(ageChecker);

            return ageChecker.CheckOrderList(checkingOrderList);
        }
    }

    public UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDayLimit(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> dayLimitChecker =
            new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.Days
            })
        {
            InitUnitCheck(dayLimitChecker);

            return dayLimitChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDosage(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> dosageChecker =
           new DosageChecker<OrdInfoModel, OrdInfoDetailModel>()
           {
               CheckType = RealtimeCheckerType.Dosage,
               CurrentHeight = _currentHeight,
               CurrentWeight = _currentWeight,
               TermLimitCheckingOnly = _termLimitCheckingOnly
           })
        {
            InitUnitCheck(dosageChecker);

            return dosageChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDisease(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> diseaseChecker =
            new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.Disease,
            })
        {
            InitUnitCheck(diseaseChecker);

            return diseaseChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiOTC(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiOTCChecker =
            new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiOTC
            })
        {
            InitUnitCheck(kinkiOTCChecker);

            return kinkiOTCChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiTain(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiTainChecker =
            new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiTain
            })
        {
            InitUnitCheck(kinkiTainChecker);

            return kinkiTainChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiSupple(List<OrdInfoModel> checkingOrderList)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiSuppleChecker =
            new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiSupplement
            })
        {
            InitUnitCheck(kinkiSuppleChecker);

            return kinkiSuppleChecker.CheckOrderList(checkingOrderList);
        }
    }

    private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckDuplication(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> duplicationChecker =
            new DuplicationChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CurrentListOrder = currentListOdr,
                CheckType = RealtimeCheckerType.Duplication
            })
        {
            InitUnitCheck(duplicationChecker);

            return duplicationChecker.CheckOrder(checkingOrder);
        }
    }

    private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckKinki(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiChecker =
            new KinkiChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CurrentListOrder = currentListOdr,
                CheckType = RealtimeCheckerType.Kinki
            })
        {
            InitUnitCheck(kinkiChecker);

            return kinkiChecker.CheckOrder(checkingOrder);
        }
    }

    private UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiUser(List<OrdInfoModel> currentListOdr, OrdInfoModel checkingOrder)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiUserChecker =
            new KinkiUserChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CurrentListOrder = currentListOdr,
                CheckType = RealtimeCheckerType.KinkiUser
            })
        {
            InitUnitCheck(kinkiUserChecker);

            return kinkiUserChecker.CheckOrder(checkingOrder);
        }
    }
    #endregion

    #region string error
    private readonly string _commentLevel1Template = "※アレルギー登録薬「{0}」と成分（{1}）が同じです。";
    private readonly string _commentLevel2Template = "※「{0}」の成分（{1}）はアレルギー登録薬「{2}」の成分（{3}）と活性体成分（{4}）が同じです。";
    private readonly string _commentLevel3Template = "※「{0}」の成分（{1}）はアレルギー登録薬「{2}」の成分（{3}）の類似成分（{4}）です。";
    private readonly string _commentLevel4Template = "※「{0}」はアレルギー登録薬「{1}」と同じ系統（{2}）の成分を含みます。";

    private readonly string _duplicatedComponentTemplate = "※「{0}」 と「{1}」 は成分（{2}）が重複しています。";
    private readonly string _proDrupTemplate = "※「{0}」 の成分（{1}）と「{2}」 の成分（{3}）は活性対成分（{4}）が同じです。";
    private readonly string _sameComponentTemplate = "※「{0}」の成分（{1}）と「{2}」 の成分（{3}）は類似成分（{4}）です。";
    private readonly string _duplicatedClassTemplate = "※「{0}」 と「{1}」 は同じ系統（{2}）の成分を含みます。";
    #endregion

    #region Get Error Details
    public List<ErrorInfoModel> GetErrorDetails(int hpId, long ptId, int sinday, List<UnitCheckInfoModel> listErrorInfo)
    {
        _hpID = hpId;
        _ptID = ptId;
        _sinday = sinday;
        List<ErrorInfoModel> listErrorInfoModel = new List<ErrorInfoModel>();
        listErrorInfo.ForEach((errorInfo) =>
        {
            switch (errorInfo.CheckerType)
            {
                case RealtimeCheckerType.DrugAllergy:
                    List<DrugAllergyResultModel> drugAllergyInfo = errorInfo.ErrorInfo as List<DrugAllergyResultModel>;
                    if (drugAllergyInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDrugAllergy(drugAllergyInfo));
                    }
                    break;
                case RealtimeCheckerType.FoodAllergy:
                    List<FoodAllergyResultModel> foodAllergyInfo = errorInfo.ErrorInfo as List<FoodAllergyResultModel>;
                    if (foodAllergyInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForFoodAllergy(foodAllergyInfo));
                    }
                    break;
                case RealtimeCheckerType.Age:
                    List<AgeResultModel> ageErrorInfo = errorInfo.ErrorInfo as List<AgeResultModel>;
                    if (ageErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForAge(ageErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Disease:
                    List<DiseaseResultModel> diseaseErrorInfo = errorInfo.ErrorInfo as List<DiseaseResultModel>;
                    if (diseaseErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDisease(diseaseErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Kinki:
                case RealtimeCheckerType.KinkiTain:
                case RealtimeCheckerType.KinkiOTC:
                case RealtimeCheckerType.KinkiSupplement:
                    List<KinkiResultModel> kinkiErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForKinki(errorInfo.CheckerType, kinkiErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.KinkiUser:
                    List<KinkiResultModel> kinkiUserErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiUserErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForKinkiUser(kinkiUserErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Days:
                    List<DayLimitResultModel> dayLimitErrorInfo = errorInfo.ErrorInfo as List<DayLimitResultModel>;
                    if (dayLimitErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDayLimit(dayLimitErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Dosage:
                    List<DosageResultModel> dosageErrorInfo = errorInfo.ErrorInfo as List<DosageResultModel>;
                    if (dosageErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDosage(dosageErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Duplication:
                    List<DuplicationResultModel> duplicationErrorInfo = errorInfo.ErrorInfo as List<DuplicationResultModel>;
                    if (duplicationErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDuplication(duplicationErrorInfo));
                    }
                    break;
            }
        });

        return listErrorInfoModel;
    }
    #endregion

    #region ProcessDataForDrugAllergy
    private List<ErrorInfoModel> ProcessDataForDrugAllergy(List<DrugAllergyResultModel> allergyInfo)
    {
        if (_realtimeOrderErrorFinder.IsNoMasterData())
        {
            return ProcessDataForDrugAllergyWithNoMasterData(allergyInfo);
        }

        List<ErrorInfoModel> result = new List<ErrorInfoModel>();

        var errorGroup = (from a in allergyInfo
                          group a by new { a.YjCd, a.AllergyYjCd }
                          into gcs
                          select new { gcs.Key.YjCd, gcs.Key.AllergyYjCd }
                          ).ToList();

        foreach (var error in errorGroup)
        {
            List<DrugAllergyResultModel> tempData =
                allergyInfo
                .Where(a => a.YjCd == error.YjCd && a.AllergyYjCd == error.AllergyYjCd)
                .OrderByDescending(a => a.Level)
                .ToList();
            string itemName = _realtimeOrderErrorFinder.FindItemName(error.YjCd, _sinday);
            string allergyItemName = _realtimeOrderErrorFinder.FindItemName(error.AllergyYjCd, _sinday);
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = allergyItemName
            };

            List<LevelInfoModel> _listLevelInfo = new List<LevelInfoModel>();
            foreach (var item in tempData)
            {
                LevelInfoModel levelInfo = _listLevelInfo.Where(c => c.Level == item.Level).FirstOrDefault();
                if (levelInfo == null)
                {
                    levelInfo = new LevelInfoModel()
                    {
                        FirstItemName = itemName,
                        SecondItemName = allergyItemName,
                        Level = item.Level
                    };
                    _listLevelInfo.Add(levelInfo);
                }

                if (0 <= item.Level && item.Level <= 4)
                {
                    int level = (error.YjCd == error.AllergyYjCd) ? 0 : item.Level;

                    if (item.YjCd == item.AllergyYjCd)
                    {
                        levelInfo.Comment += "※アレルギー登録薬です。" + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        switch (item.Level)
                        {
                            case 1:
                                string componentName1 = _realtimeOrderErrorFinder.FindComponentName(item.SeibunCd);
                                levelInfo.Comment += string.Format(_commentLevel1Template, allergyItemName, componentName1) + Environment.NewLine + Environment.NewLine;
                                break;
                            case 2:
                                string componentName2 = _realtimeOrderErrorFinder.FindComponentName(item.SeibunCd);
                                string allergyComponentName2 = _realtimeOrderErrorFinder.FindComponentName(item.AllergySeibunCd);
                                levelInfo.Comment += string.Format(_commentLevel2Template, itemName, componentName2, allergyItemName, allergyComponentName2, componentName2) + Environment.NewLine + Environment.NewLine;
                                break;
                            case 3:
                                string componentName3 = _realtimeOrderErrorFinder.FindComponentName(item.SeibunCd);
                                string allergyComponentName3 = _realtimeOrderErrorFinder.FindComponentName(item.AllergySeibunCd);
                                string analogueName = _realtimeOrderErrorFinder.FindAnalogueName(item.Tag);
                                levelInfo.Comment += string.Format(_commentLevel3Template, itemName, componentName3, allergyItemName, allergyComponentName3, analogueName) + Environment.NewLine + Environment.NewLine;
                                break;
                            case 4:
                                string drvalrgyName = _realtimeOrderErrorFinder.FindDrvalrgyName(item.Tag);
                                levelInfo.Comment += string.Format(_commentLevel4Template, itemName, allergyItemName, drvalrgyName) + Environment.NewLine + Environment.NewLine;
                                break;
                        }
                    }
                }
            }
            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForDrugAllergyWithNoMasterData
    private List<ErrorInfoModel> ProcessDataForDrugAllergyWithNoMasterData(List<DrugAllergyResultModel> allergyInfo)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        allergyInfo.ForEach((a) =>
        {
            string itemName = _realtimeOrderErrorFinder.FindItemNameByItemCode(a.ItemCd, _sinday);
            ErrorInfoModel info = new ErrorInfoModel()
            {
                Id = a.Id,
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = itemName
            };
            List<LevelInfoModel> _listLevelInfo = new List<LevelInfoModel>();
            _listLevelInfo.Add(new LevelInfoModel()
            {
                FirstItemName = itemName,
                SecondItemName = itemName,
                Level = a.Level,
                Comment = "※アレルギー登録薬です。"
            });
            info.ListLevelInfo.AddRange(_listLevelInfo);
            result.Add(info);
        });
        return result;
    }
    #endregion

    #region ProcessDataForFoodAllergy
    private List<ErrorInfoModel> ProcessDataForFoodAllergy(List<FoodAllergyResultModel> allergyInfo)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();

        var errorGroup = (from a in allergyInfo
                          group a by new { a.YjCd, a.AlrgyKbn, a.Id }
                          into gcs
                          select new { gcs.Key.YjCd, gcs.Key.AlrgyKbn, gcs.Key.Id }
                          ).ToList();

        foreach (var error in errorGroup)
        {
            List<FoodAllergyResultModel> tempData =
                allergyInfo
                .Where(a => a.YjCd == error.YjCd && a.AlrgyKbn == error.AlrgyKbn)
                .OrderByDescending(a => a.TenpuLevel)
                .ToList();
            string itemName = _realtimeOrderErrorFinder.FindItemName(error.YjCd, _sinday);
            string foodName = _realtimeOrderErrorFinder.FindFoodName(error.AlrgyKbn);
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                Id = error.Id,
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = foodName
            };

            List<LevelInfoModel> _listLevelInfo = new List<LevelInfoModel>();
            foreach (var item in tempData)
            {
                int level = item.TenpuLevel.AsInteger();
                LevelInfoModel levelInfo = _listLevelInfo.Where(c => c.Level == level).FirstOrDefault();
                if (levelInfo == null)
                {
                    levelInfo = new LevelInfoModel()
                    {
                        FirstItemName = itemName,
                        SecondItemName = foodName,
                        Level = level
                    };
                    _listLevelInfo.Add(levelInfo);
                }
                levelInfo.Comment += item.AttentionCmt + Environment.NewLine + item.WorkingMechanism + Environment.NewLine + Environment.NewLine;
            }
            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForAge
    private List<ErrorInfoModel> ProcessDataForAge(List<AgeResultModel> ages)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        var errorGroup = (from a in ages
                          group a by new { a.YjCd, a.Id }
                          into gcs
                          select new { gcs.Key.YjCd, gcs.Key.Id }
                          ).ToList();

        foreach (var error in errorGroup)
        {
            List<AgeResultModel> tempData =
                ages
                .Where(a => a.YjCd == error.YjCd)
                .OrderByDescending(a => a.TenpuLevel)
                .ToList();
            string itemName = _realtimeOrderErrorFinder.FindItemName(error.YjCd, _sinday);
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                Id = error.Id,
                FirstCellContent = "投与年齢",
                ThridCellContent = itemName,
                FourthCellContent = "ー",
            };

            List<LevelInfoModel> _listLevelInfo = new List<LevelInfoModel>();

            foreach (var item in tempData)
            {
                int level = item.TenpuLevel.AsInteger();
                string attention = _realtimeOrderErrorFinder.FindAgeComment(item.AttentionCmtCd);
                LevelInfoModel levelInfo = _listLevelInfo.Where(c => c.Level == level).FirstOrDefault();
                if (levelInfo == null)
                {
                    levelInfo = new LevelInfoModel()
                    {
                        FirstItemName = itemName,
                        SecondItemName = string.Empty,
                        Level = level
                    };
                    _listLevelInfo.Add(levelInfo);
                }

                levelInfo.Comment += attention + Environment.NewLine + item.WorkingMechanism + Environment.NewLine + Environment.NewLine;
            }

            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForDisease
    private List<ErrorInfoModel> ProcessDataForDisease(List<DiseaseResultModel> diseaseInfo)
    {
        string DiseaseTypeName(int DiseaseType)
        {
            switch (DiseaseType)
            {
                case 1:
                    return "既往歴";
                case 2:
                    return "家族歴";
                default:
                    return "現疾患";
            }
        }

        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        var listDrugDiseaseCode = (from a in diseaseInfo
                                   group a by new { a.YjCd, a.ByotaiCd, a.DiseaseType, a.Id }
                                   into gcs
                                   select new { gcs.Key.YjCd, gcs.Key.ByotaiCd, gcs.Key.DiseaseType, gcs.Key.Id }
                                   ).ToList();

        foreach (var drugDiseaseCode in listDrugDiseaseCode)
        {
            List<DiseaseResultModel> listFilteredData =
                diseaseInfo.Where(
                    d =>
                    d.YjCd == drugDiseaseCode.YjCd &&
                    d.ByotaiCd == drugDiseaseCode.ByotaiCd &&
                    d.DiseaseType == drugDiseaseCode.DiseaseType
                    ).ToList();


            string itemName = _realtimeOrderErrorFinder.FindItemName(drugDiseaseCode.YjCd, _sinday);
            string diseaseName = _realtimeOrderErrorFinder.FindDiseaseName(drugDiseaseCode.ByotaiCd);

            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                Id = drugDiseaseCode.Id,
                FirstCellContent = DiseaseTypeName(drugDiseaseCode.DiseaseType),
                ThridCellContent = itemName,
                FourthCellContent = diseaseName
            };

            List<LevelInfoModel> _listLevelInfoModel = tempModel.ListLevelInfo;
            foreach (var item in listFilteredData)
            {
                int level = item.TenpuLevel;
                LevelInfoModel LevelInfoModel = _listLevelInfoModel.Where(c => c.Level == level).FirstOrDefault();
                if (LevelInfoModel == null)
                {
                    LevelInfoModel = new LevelInfoModel()
                    {
                        FirstItemName = itemName,
                        SecondItemName = diseaseName,
                        Level = level
                    };
                    _listLevelInfoModel.Add(LevelInfoModel);
                }

                LevelInfoModel.Comment += _realtimeOrderErrorFinder.FindDiseaseComment(item.CmtCd) + Environment.NewLine + _realtimeOrderErrorFinder.FindDiseaseComment(item.KijyoCd) + Environment.NewLine + Environment.NewLine;
            }


            result.Add(tempModel);
        }
        return result;
    }
    #endregion

    #region ProcessDataForKinki
    private List<ErrorInfoModel> ProcessDataForKinki(RealtimeCheckerType checkingType, List<KinkiResultModel> kinkiErrorInfo)
    {
        string GetCheckingTitle()
        {
            switch (checkingType)
            {
                case RealtimeCheckerType.KinkiTain:
                    return "相互作用(他院)";
                case RealtimeCheckerType.KinkiOTC:
                    return "相互作用(OTC)";
                case RealtimeCheckerType.KinkiSupplement:
                    return "相互作用(サプリ)";
                default:
                    return "相互作用";
            }
        }

        string GetBName(string code)
        {
            switch (checkingType)
            {
                case RealtimeCheckerType.Kinki:
                case RealtimeCheckerType.KinkiTain:
                    return _realtimeOrderErrorFinder.FindItemName(code, _sinday);
                case RealtimeCheckerType.KinkiOTC:
                    return _realtimeOrderErrorFinder.FindOTCItemName(code.AsInteger());
                default:
                    return string.Empty;
            }
        }

        List<KinkiResultModel> listErrorIgnoreDuplicated = RemoveDuplicatedErrorInfo(kinkiErrorInfo);

        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        var listKinkiCode = (from a in listErrorIgnoreDuplicated
                             group a by new { a.AYjCd, a.BYjCd, a.ItemCd, a.KinkiItemCd, a.Id }
                                   into gcs
                             select new { gcs.Key.AYjCd, gcs.Key.BYjCd, gcs.Key.ItemCd, gcs.Key.KinkiItemCd, gcs.Key.Id }
                                   ).ToList();

        for (int x = 0; x < listKinkiCode.Count(); x++)
        {
            var kikinCode = listKinkiCode[x];
            List<KinkiResultModel> listFilteredData =
                listErrorIgnoreDuplicated
                .Where
                (
                    d =>
                    d.AYjCd == kikinCode.AYjCd &&
                    d.BYjCd == kikinCode.BYjCd
                )
                .ToList();

            if (listFilteredData.Count() == 0) continue;

            string itemAName = _realtimeOrderErrorFinder.FindItemName(kikinCode.AYjCd, _sinday);
            string itemBName = string.Empty;

            if (checkingType == RealtimeCheckerType.KinkiSupplement)
            {
                string seibunCd = listFilteredData.First().SeibunCd;
                string indexWord = listFilteredData.First().IndexWord;
                string seibunName = _realtimeOrderErrorFinder.FindSuppleItemName(seibunCd);

                if (indexWord != seibunName)
                {
                    itemBName = indexWord + "（" + seibunName + "）";
                }
                else
                {
                    itemBName = seibunName;
                }
            }
            else
            {
                itemBName = GetBName(kikinCode.BYjCd);
            }

            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                Id = kikinCode.Id,
                FirstCellContent = GetCheckingTitle(),
                ThridCellContent = itemAName,
                FourthCellContent = itemBName,
                CurrentItemCd = kikinCode.ItemCd,
                CheckingItemCd = kikinCode.KinkiItemCd,
            };
            result.Add(tempModel);

            List<KinkiErrorDetail> listDetail = new List<KinkiErrorDetail>();
            listFilteredData.ForEach((f) =>
            {
                KinkiErrorDetail kinkiErrorDetail = new KinkiErrorDetail()
                {
                    Level = f.Kyodo.AsInteger(),
                    CommentContent = _realtimeOrderErrorFinder.FindKinkiComment(f.CommentCode).Trim(),
                    SayokijyoContent = _realtimeOrderErrorFinder.FindKijyoComment(f.SayokijyoCode).Trim()
                };
                if (f.IsNeedToReplace)
                {
                    kinkiErrorDetail.CommentContent = kinkiErrorDetail.CommentContent.Replace("Ａ", "Ｃ").Replace("Ｂ", "Ａ").Replace("Ｃ", "Ｂ");
                    kinkiErrorDetail.SayokijyoContent = kinkiErrorDetail.SayokijyoContent.Replace("Ａ", "Ｃ").Replace("Ｂ", "Ａ").Replace("Ｃ", "Ｂ");
                }

                #region Fix comment 3704
                string stringToReplace = string.Empty;
                if (checkingType == RealtimeCheckerType.KinkiOTC)
                {
                    string otcComponentInfo = _realtimeOrderErrorFinder.GetOTCComponentInfo(f.SeibunCd);
                    stringToReplace = f.Sbt == 1 ? "の含有成分「" + otcComponentInfo + "」" : "の添加物「" + otcComponentInfo + "」";
                }
                else if (checkingType == RealtimeCheckerType.KinkiSupplement)
                {
                    string supplementComponentInfo = _realtimeOrderErrorFinder.GetSupplementComponentInfo(f.SeibunCd);
                    stringToReplace = "の成分「" + supplementComponentInfo + "」";

                }
                if (!string.IsNullOrEmpty(stringToReplace))
                {
                    kinkiErrorDetail.CommentContent = kinkiErrorDetail.CommentContent.Replace("Ｂ", "Ｂ" + stringToReplace);
                    kinkiErrorDetail.SayokijyoContent = kinkiErrorDetail.SayokijyoContent.Replace("Ｂ", "Ｂ" + stringToReplace);
                }
                #endregion
                listDetail.Add(kinkiErrorDetail);
            });

            List<LevelInfoModel> listLevelInfoModel = tempModel.ListLevelInfo;
            var listLevel =
                listDetail
                .GroupBy(f => f.Level)
                .Select(f => new { Level = f.Key })
                .OrderBy(f => f.Level)
                .ToList();
            listLevel.ForEach(l =>
            {
                LevelInfoModel LevelInfoModel = new LevelInfoModel()
                {
                    FirstItemName = "Ａ. " + itemAName,
                    SecondItemName = "Ｂ. " + itemBName,
                    Level = l.Level
                };
                listLevelInfoModel.Add(LevelInfoModel);

                var listItemAsLevel = listDetail.Where(d => l.Level <= d.Level)
                                                .OrderBy(d => d.Level)
                                                .ToList();

                var listGroupByCommentContent =
                    listItemAsLevel
                    .GroupBy(f => f.CommentContent)
                    .Where(f => f.Count() > 1)
                    .Select(f => new { CommentContent = f.Key, ListItem = f.ToList() })
                    .ToList();

                List<string> listCommentContent = listGroupByCommentContent.Select(g => g.CommentContent).ToList();
                var temList = listItemAsLevel.Where(f => !listCommentContent.Contains(f.CommentContent)).ToList();

                var listGroupBySayokijyoContent =
                    temList
                    .GroupBy(f => f.SayokijyoContent)
                    .Where(f => f.Count() > 1)
                    .Select(f => new { SayokijyoContent = f.Key, ListItem = f.ToList() })
                    .ToList();

                List<string> listSayokijyoContent = listGroupBySayokijyoContent.Select(g => g.SayokijyoContent).ToList();
                var restOfListItemAsLevel = temList.Where(f => !listSayokijyoContent.Contains(f.SayokijyoContent)).ToList();

                string content = string.Empty;

                listGroupByCommentContent.ForEach((g) =>
                {
                    content += g.CommentContent + Environment.NewLine;
                    g.ListItem.ForEach((i) =>
                    {
                        content += "※" + i.SayokijyoContent + Environment.NewLine;
                    });
                    content += Environment.NewLine;
                });

                listGroupBySayokijyoContent.ForEach((g) =>
                {
                    g.ListItem.ForEach((i) =>
                    {
                        content += i.CommentContent + Environment.NewLine;
                    });
                    content += "※" + g.SayokijyoContent + Environment.NewLine + Environment.NewLine;
                });

                restOfListItemAsLevel.ForEach((g) =>
                {
                    content += g.CommentContent + Environment.NewLine;
                    content += "※" + g.SayokijyoContent + Environment.NewLine + Environment.NewLine;
                });

                LevelInfoModel.Comment = content;
            });
        }
        return result;
    }
    #endregion

    #region ProcessDataForKinkiUser
    private List<ErrorInfoModel> ProcessDataForKinkiUser(List<KinkiResultModel> kinkiErrorInfo)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        kinkiErrorInfo.ForEach((k) =>
        {
            string itemAName = _realtimeOrderErrorFinder.FindItemName(k.AYjCd, _sinday);
            string itemBName = _realtimeOrderErrorFinder.FindItemName(k.BYjCd, _sinday);

            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                Id = k.Id,
                FirstCellContent = "相互作用",
                ThridCellContent = itemAName,
                FourthCellContent = itemBName
            };
            result.Add(tempModel);

            List<LevelInfoModel> _listLevelInfoModel = new List<LevelInfoModel>()
            {
                new LevelInfoModel()
                {
                    FirstItemName = itemAName,
                    SecondItemName = itemBName,
                    Level = 1,
                    Comment = "ユーザー設定"
                }
            };
            tempModel.ListLevelInfo = _listLevelInfoModel;
        });
        return result;
    }
    #endregion

    #region ProcessDataForDayLimit
    private List<ErrorInfoModel> ProcessDataForDayLimit(List<DayLimitResultModel> dayLimitError)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        foreach (DayLimitResultModel dayLimit in dayLimitError)
        {
            string itemName = _realtimeOrderErrorFinder.FindItemName(dayLimit.YjCd, _sinday);
            ErrorInfoModel errorInfoModel = new ErrorInfoModel();
            result.Add(errorInfoModel);
            errorInfoModel.Id = dayLimit.Id;
            errorInfoModel.FirstCellContent = "投与日数";
            errorInfoModel.SecondCellContent = "ー";
            errorInfoModel.ThridCellContent = itemName;
            errorInfoModel.FourthCellContent = dayLimit.UsingDay.AsString() + "日";
            errorInfoModel.SuggestedContent = "／" + dayLimit.LimitDay.AsString() + "日";

            LevelInfoModel LevelInfoModel = new LevelInfoModel()
            {
                FirstItemName = itemName,
                Comment = "投与日数制限（" + dayLimit.LimitDay.AsString() + "日）を超えています。"
            };
            errorInfoModel.ListLevelInfo = new List<LevelInfoModel>() { LevelInfoModel };
        }
        return result;
    }
    #endregion

    #region ProcessDataForDosage
    private List<ErrorInfoModel> ProcessDataForDosage(List<DosageResultModel> listDosageError)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        foreach (DosageResultModel dosage in listDosageError)
        {
            ErrorInfoModel errorInfoModel = new ErrorInfoModel();
            result.Add(errorInfoModel);
            string itemName = _realtimeOrderErrorFinder.FindItemName(dosage.YjCd, _sinday);
            errorInfoModel.Id = dosage.Id;
            errorInfoModel.FirstCellContent = "投与量";
            errorInfoModel.ThridCellContent = itemName;
            errorInfoModel.FourthCellContent = dosage.CurrentValue + dosage.UnitName;
            errorInfoModel.SuggestedContent = "／" + dosage.SuggestedValue + dosage.UnitName;

            string levelTitle = string.Empty;
            switch (dosage.LabelChecking)
            {
                case DosageLabelChecking.OneMin:
                    levelTitle = "一回量／最小値";
                    break;
                case DosageLabelChecking.OneMax:
                    levelTitle = "一回量／最大値";
                    break;
                case DosageLabelChecking.OneLimit:
                    levelTitle = "一回量／上限値";
                    break;
                case DosageLabelChecking.DayMin:
                    levelTitle = "一日量／最小値";
                    break;
                case DosageLabelChecking.DayMax:
                    levelTitle = "一日量／最大値";
                    break;
                case DosageLabelChecking.DayLimit:
                    levelTitle = "一日量／上限値";
                    break;
                case DosageLabelChecking.TermLimit:
                    levelTitle = "期間上限";
                    break;
            }
            string comment = string.Empty;
            if (dosage.IsFromUserDefined)
            {
                comment = "ユーザー設定";
            }
            else
            {
                comment = _realtimeOrderErrorFinder.GetUsageDosage(dosage.YjCd);
            }

            LevelInfoModel LevelInfoModel = new LevelInfoModel()
            {
                Title = levelTitle,
                FirstItemName = itemName,
                Comment = comment
            };
            errorInfoModel.ListLevelInfo = new List<LevelInfoModel>() { LevelInfoModel };
        }
        return result;
    }
    #endregion

    #region ProcessDataForDuplication
    private List<ErrorInfoModel> ProcessDataForDuplication(List<DuplicationResultModel> listDuplicationError)
    {
        List<ErrorInfoModel> result = new List<ErrorInfoModel>();
        foreach (DuplicationResultModel duplicationError in listDuplicationError)
        {
            string itemName = _realtimeOrderErrorFinder.FindItemNameByItemCode(duplicationError.ItemCd, _sinday);
            string duplicatedItemName = _realtimeOrderErrorFinder.FindItemNameByItemCode(duplicationError.DuplicatedItemCd, _sinday);

            ErrorInfoModel errorInfoModel = new ErrorInfoModel();
            result.Add(errorInfoModel);
            errorInfoModel.Id = duplicationError.Id;
            errorInfoModel.FirstCellContent = duplicationError.IsComponentDuplicated ? "成分重複" : "同一薬剤";
            errorInfoModel.SecondCellContent = "ー";
            errorInfoModel.ThridCellContent = itemName;
            errorInfoModel.CheckingItemCd = duplicationError.ItemCd;
            errorInfoModel.CurrentItemCd = duplicationError.DuplicatedItemCd;
            if (duplicationError.IsIppanCdDuplicated || duplicationError.IsComponentDuplicated)
            {
                errorInfoModel.FourthCellContent = duplicatedItemName;
            }
            else
            {
                errorInfoModel.FourthCellContent = "ー";
            }

            LevelInfoModel LevelInfoModel = new LevelInfoModel()
            {
                FirstItemName = itemName,
                SecondItemName = duplicationError.IsComponentDuplicated || duplicationError.IsIppanCdDuplicated ? duplicatedItemName : string.Empty
            };

            if (duplicationError.IsIppanCdDuplicated)
            {
                LevelInfoModel.Comment = "「" + itemName + "」と「" + duplicatedItemName + "」は一般名（" + _realtimeOrderErrorFinder.FindIppanNameByIppanCode(duplicationError.IppanCode) + "）が同じです。";
            }
            else if (duplicationError.IsComponentDuplicated)
            {
                switch (duplicationError.Level)
                {
                    case 1:
                        string componentName1 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        LevelInfoModel.Comment += string.Format(_duplicatedComponentTemplate, itemName, duplicatedItemName, componentName1) + Environment.NewLine + Environment.NewLine;
                        break;
                    case 2:
                        string componentName2 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        string allergyComponentName2 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.AllergySeibunCd);
                        LevelInfoModel.Comment += string.Format(_proDrupTemplate, itemName, componentName2, duplicatedItemName, allergyComponentName2, componentName2) + Environment.NewLine + Environment.NewLine;
                        break;
                    case 3:
                        string componentName3 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        string allergyComponentName3 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.AllergySeibunCd);
                        string analogueName = _realtimeOrderErrorFinder.FindAnalogueName(duplicationError.Tag);
                        LevelInfoModel.Comment += string.Format(_sameComponentTemplate, itemName, componentName3, duplicatedItemName, allergyComponentName3, analogueName) + Environment.NewLine + Environment.NewLine;
                        break;
                    case 4:
                        string className = _realtimeOrderErrorFinder.FindClassName(duplicationError.Tag);
                        LevelInfoModel.Comment += string.Format(_duplicatedClassTemplate, itemName, duplicatedItemName, className) + Environment.NewLine + Environment.NewLine;
                        break;
                }
            }
            else
            {
                LevelInfoModel.Comment = "同一薬剤（" + itemName + "）が処方されています。";
            }

            errorInfoModel.ListLevelInfo = new List<LevelInfoModel>() { LevelInfoModel };
        }
        return result;
    }
    #endregion

    #region RemoveDuplicatedErrorInfo
    private List<KinkiResultModel> RemoveDuplicatedErrorInfo(List<KinkiResultModel> originList)
    {
        List<KinkiResultModel> subResult = new List<KinkiResultModel>();
        originList.ForEach(k =>
        {
            var tempError = subResult.Where(a => a.AYjCd == k.AYjCd &&
                                                 a.BYjCd == k.BYjCd &&
                                                 a.CommentCode == k.CommentCode &&
                                                 a.SayokijyoCode == k.SayokijyoCode &&
                                                 a.Kyodo == k.Kyodo &&
                                                 a.IsNeedToReplace == k.IsNeedToReplace &&
                                                 a.IndexWord == k.IndexWord)
            .FirstOrDefault();

            if (tempError == null)
            {
                subResult.Add(k);
            }
        });


        return subResult;
    }
    #endregion

    public void ReleaseResource()
    {
        _tenantProvider.DisposeDataContext();
    }
}
