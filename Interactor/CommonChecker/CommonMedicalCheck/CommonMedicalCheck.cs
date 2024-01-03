using CommonChecker.Caches;
using CommonChecker.Caches.Interface;
using CommonChecker.DB;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using System.Text;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;
using SpecialNoteFull = Domain.Models.SpecialNote.SpecialNoteModel;

namespace Interactor.CommonChecker.CommonMedicalCheck;

public class CommonMedicalCheck : ICommonMedicalCheck
{
    public RealTimeCheckerCondition CheckerCondition { get; set; } = new RealTimeCheckerCondition();

    public bool IsOrderChecking { get; set; } = true;

    public int _hpID;
    public long _ptID;
    public int _sinday;
    public Dictionary<string, string> _itemNameDictionary;
    public Dictionary<string, string> _componentNameDictionary;
    public Dictionary<string, string> _analogueNameDictionary;
    public Dictionary<string, string> _drvalrgyNameDictionary;
    public Dictionary<string, string> _foodNameDictionary;
    public Dictionary<string, string> _diseaseNameDictionary;
    public Dictionary<string, string> _kinkiCommentDictionary;
    public Dictionary<string, string> _kijyoCommentDictionary;
    public Dictionary<string, string> _oTCItemNameDictionary;
    public Dictionary<string, string> _oTCComponentInfoDictionary;
    public Dictionary<string, string> _supplementComponentInfoDictionary;
    public Dictionary<string, string> _suppleItemNameDictionary;
    public Dictionary<string, string> _usageDosageDictionary;
    public Dictionary<string, string> _itemNameByItemCodeDictionary;
    public readonly IRealtimeOrderErrorFinder _realtimeOrderErrorFinder;

    private readonly double _currentHeight = 0;
    private readonly double _currentWeight = 0;

    private readonly ITenantProvider _tenantProvider;
    private readonly IMasterDataCacheService _masterDataCacheService;

    public CommonMedicalCheck(ITenantProvider tenantProvider, IRealtimeOrderErrorFinder realtimeOrderErrorFinder)
    {
        _masterDataCacheService = new MasterDataCacheService(tenantProvider);
        _tenantProvider = tenantProvider;
        _realtimeOrderErrorFinder = realtimeOrderErrorFinder;
        _itemNameDictionary = new();
        _componentNameDictionary = new();
        _analogueNameDictionary = new();
        _drvalrgyNameDictionary = new();
        _foodNameDictionary = new();
        _diseaseNameDictionary = new();
        _kinkiCommentDictionary = new();
        _kijyoCommentDictionary = new();
        _oTCItemNameDictionary = new();
        _oTCComponentInfoDictionary = new();
        _supplementComponentInfoDictionary = new();
        _suppleItemNameDictionary = new();
        _usageDosageDictionary = new();
        _itemNameByItemCodeDictionary = new();
    }

    public void InitUnitCheck(UnitChecker<OrdInfoModel, OrdInfoDetailModel> unitChecker)
    {
        unitChecker.HpID = _hpID;
        unitChecker.PtID = _ptID;
        unitChecker.Sinday = _sinday;
        unitChecker.InitFinder(_tenantProvider.GetNoTrackingDataContext(), _masterDataCacheService);
    }

    private void InitTenMstCache(List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
    {
        List<string> itemCodeList = new List<string>();

        foreach (var order in currentListOdr)
        {
            itemCodeList.AddRange(order.OdrInfDetailModelsIgnoreEmpty.Select(i => i.ItemCd).ToList());
        }

        foreach (var order in listCheckingOrder)
        {
            itemCodeList.AddRange(order.OdrInfDetailModelsIgnoreEmpty.Select(i => i.ItemCd).ToList());
        }
        _masterDataCacheService.InitCache(itemCodeList.Distinct().ToList(), _sinday, _ptID);
    }

    public List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb, RealTimeCheckerCondition realTimeCheckerCondition)
    {
        CheckerCondition = realTimeCheckerCondition;
        _hpID = hpId;
        _ptID = ptId;
        _sinday = sinday;
        List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> listErrorOfAllOrder = new List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>>();
        List<OrdInfoModel> listOrderError = new List<OrdInfoModel>();
        List<OrdInfoModel> tempCurrentListOdr = new List<OrdInfoModel>(currentListOdr);

        InitTenMstCache(currentListOdr, listCheckingOrder);

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

        var checkListOrderResultList = GetErrorFromListOrder(listCheckingOrder, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);

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

    public List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> listCheckingOrder, RealTimeCheckerCondition checkerCondition, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        _hpID = hpId;
        _ptID = ptId;
        _sinday = sinday;
        CheckerCondition = checkerCondition;
        List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>> listErrorOfAllOrder = new List<UnitCheckerResult<OrdInfoModel, OrdInfoDetailModel>>();
        List<OrdInfoModel> listOrderError = new List<OrdInfoModel>();
        List<OrdInfoModel> tempCurrentListOdr = new();

        InitTenMstCache(new List<OrdInfoModel>(), listCheckingOrder);

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

        var checkListOrderResultList = GetErrorFromListOrder(listCheckingOrder, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);

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

    public List<DayLimitResultModel> CheckOnlyDayLimit(OrdInfoModel checkingOrder)
    {
        UnitChecker<OrdInfoModel, OrdInfoDetailModel> dayLimitChecker =
            new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.Days
            };
        InitUnitCheck(dayLimitChecker);

        UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> checkedResult = dayLimitChecker.CheckOrderList(new List<OrdInfoModel>() { checkingOrder }, new(new(), new(), new()), new(), new(), true);
        List<DayLimitResultModel>? result = checkedResult.ErrorInfo as List<DayLimitResultModel>;
        return result ?? new List<DayLimitResultModel>();
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

    public List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>> GetErrorFromListOrder(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>> listError = new List<UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>>();

        if (CheckerCondition.IsCheckingAllergy)
        {
            var foodAllergyCheckResult = CheckFoodAllergy(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (foodAllergyCheckResult.IsError)
            {
                listError.Add(foodAllergyCheckResult);
            }

            var drugAllergyCheckResult = CheckDrugAllergy(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (drugAllergyCheckResult.IsError)
            {
                listError.Add(drugAllergyCheckResult);
            }
        }

        if (CheckerCondition.IsCheckingAge)
        {
            var ageCheckResult = CheckAge(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (ageCheckResult.IsError)
            {
                listError.Add(ageCheckResult);
            }
        }

        if (CheckerCondition.IsCheckingDisease)
        {
            var diseaseCheckResult = CheckDisease(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (diseaseCheckResult.IsError)
            {
                listError.Add(diseaseCheckResult);
            }
        }

        if (CheckerCondition.IsCheckingKinki)
        {
            var kinkiTainCheckResult = CheckKinkiTain(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (kinkiTainCheckResult.IsError)
            {
                listError.Add(kinkiTainCheckResult);
            }

            var kinkiOTCCheckResult = CheckKinkiOTC(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (kinkiOTCCheckResult.IsError)
            {
                listError.Add(kinkiOTCCheckResult);
            }

            var kinkiSuppleCheckResult = CheckKinkiSupple(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (kinkiSuppleCheckResult.IsError)
            {
                listError.Add(kinkiSuppleCheckResult);
            }
        }

        if (CheckerCondition.IsCheckingDays)
        {
            var dayLimitCheckResult = CheckDayLimit(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (dayLimitCheckResult.IsError)
            {
                listError.Add(dayLimitCheckResult);
            }
        }

        if (CheckerCondition.IsCheckingDosage)
        {
            var dayLimitCheckResult = CheckDosage(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);
            if (dayLimitCheckResult.IsError)
            {
                listError.Add(dayLimitCheckResult);
            }
        }

        return listError;
    }

    #region Check
    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckFoodAllergy(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> foodAllergyChecker =
            new FoodAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.FoodAllergy
            })
        {
            InitUnitCheck(foodAllergyChecker);
            return foodAllergyChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDrugAllergy(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> drugAllergyChecker =
            new DrugAllergyChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.DrugAllergy
            })
        {
            InitUnitCheck(drugAllergyChecker);

            return drugAllergyChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    public UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckAge(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> ageChecker =
           new AgeChecker<OrdInfoModel, OrdInfoDetailModel>()
           {
               CheckType = RealtimeCheckerType.Age
           })
        {
            InitUnitCheck(ageChecker);

            return ageChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    public UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDayLimit(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> dayLimitChecker =
            new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.Days
            })
        {
            InitUnitCheck(dayLimitChecker);

            return dayLimitChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDosage(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> dosageChecker =
           new DosageChecker<OrdInfoModel, OrdInfoDetailModel>()
           {
               CheckType = RealtimeCheckerType.Dosage,
               CurrentHeight = _currentHeight,
               CurrentWeight = _currentWeight,
           })
        {
            InitUnitCheck(dosageChecker);

            return dosageChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDisease(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> diseaseChecker =
            new DiseaseChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.Disease,
            })
        {
            InitUnitCheck(diseaseChecker);

            return diseaseChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiOTC(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiOTCChecker =
            new KinkiOTCChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiOTC
            })
        {
            InitUnitCheck(kinkiOTCChecker);

            return kinkiOTCChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiTain(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiTainChecker =
            new KinkiTainChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiTain
            })
        {
            InitUnitCheck(kinkiTainChecker);

            return kinkiTainChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
        }
    }

    private UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckKinkiSupple(List<OrdInfoModel> checkingOrderList, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb)
    {
        using (UnitChecker<OrdInfoModel, OrdInfoDetailModel> kinkiSuppleChecker =
            new KinkiSuppleChecker<OrdInfoModel, OrdInfoDetailModel>()
            {
                CheckType = RealtimeCheckerType.KinkiSupplement
            })
        {
            InitUnitCheck(kinkiSuppleChecker);

            return kinkiSuppleChecker.CheckOrderList(checkingOrderList, ConvertToSpecialNoteModel(specialNoteItem), ptDiseaseModels, familyItems.Select(f => ConvertToFamilyModel(f)).ToList(), isDataOfDb);
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

        GetItemCdError(listErrorInfo);

        List<ErrorInfoModel> listErrorInfoModel = new();
        listErrorInfo.ForEach((errorInfo) =>
        {
            switch (errorInfo.CheckerType)
            {
                case RealtimeCheckerType.DrugAllergy:
                    List<DrugAllergyResultModel>? drugAllergyInfo = errorInfo.ErrorInfo as List<DrugAllergyResultModel>;
                    if (drugAllergyInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDrugAllergy(drugAllergyInfo));
                    }
                    break;
                case RealtimeCheckerType.FoodAllergy:
                    List<FoodAllergyResultModel>? foodAllergyInfo = errorInfo.ErrorInfo as List<FoodAllergyResultModel>;
                    if (foodAllergyInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForFoodAllergy(foodAllergyInfo));
                    }
                    break;
                case RealtimeCheckerType.Age:
                    List<AgeResultModel>? ageErrorInfo = errorInfo.ErrorInfo as List<AgeResultModel>;
                    if (ageErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForAge(ageErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Disease:
                    List<DiseaseResultModel>? diseaseErrorInfo = errorInfo.ErrorInfo as List<DiseaseResultModel>;
                    if (diseaseErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDisease(diseaseErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Kinki:
                case RealtimeCheckerType.KinkiTain:
                case RealtimeCheckerType.KinkiOTC:
                case RealtimeCheckerType.KinkiSupplement:
                    List<KinkiResultModel>? kinkiErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForKinki(errorInfo.CheckerType, kinkiErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.KinkiUser:
                    List<KinkiResultModel>? kinkiUserErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiUserErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForKinkiUser(kinkiUserErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Days:
                    List<DayLimitResultModel>? dayLimitErrorInfo = errorInfo.ErrorInfo as List<DayLimitResultModel>;
                    if (dayLimitErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDayLimit(dayLimitErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Dosage:
                    List<DosageResultModel>? dosageErrorInfo = errorInfo.ErrorInfo as List<DosageResultModel>;
                    if (dosageErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDosage(dosageErrorInfo));
                    }
                    break;
                case RealtimeCheckerType.Duplication:
                    List<DuplicationResultModel>? duplicationErrorInfo = errorInfo.ErrorInfo as List<DuplicationResultModel>;
                    if (duplicationErrorInfo != null)
                    {
                        listErrorInfoModel.AddRange(ProcessDataForDuplication(duplicationErrorInfo));
                    }
                    break;
            }
        });

        return listErrorInfoModel;
    }

    public void GetItemCdError(List<UnitCheckInfoModel> listErrorInfo)
    {
        List<string> itemNameList = new();
        List<string> componentNameList = new();
        List<string> analogueNameList = new();
        List<string> drvalrgyNameList = new();
        List<string> foodNameList = new();
        List<string> diseaseNameList = new();
        List<string> kinkiCommentList = new();
        List<string> kijyoCommentList = new();
        List<string> oTCItemNameList = new();
        List<string> oTCComponentInfoList = new();
        List<string> supplementComponentInfoList = new();
        List<string> suppleItemNameList = new();
        List<string> usageDosageList = new();
        List<string> itemNameByItemCodeList = new();
        listErrorInfo.ForEach((errorInfo) =>
        {
            switch (errorInfo.CheckerType)
            {
                case RealtimeCheckerType.DrugAllergy:
                    List<DrugAllergyResultModel>? drugAllergyInfo = errorInfo.ErrorInfo as List<DrugAllergyResultModel>;
                    if (drugAllergyInfo != null)
                    {
                        itemNameByItemCodeList.AddRange(drugAllergyInfo.Select(item => item.ItemCd));
                        itemNameList.AddRange(drugAllergyInfo.Select(item => item.YjCd));
                        itemNameList.AddRange(drugAllergyInfo.Select(item => item.AllergyYjCd));
                        componentNameList.AddRange(drugAllergyInfo.Select(item => item.SeibunCd));
                        componentNameList.AddRange(drugAllergyInfo.Select(item => item.AllergySeibunCd));
                        analogueNameList.AddRange(drugAllergyInfo.Select(item => item.Tag));
                        drvalrgyNameList.AddRange(drugAllergyInfo.Select(item => item.Tag));
                    }
                    break;
                case RealtimeCheckerType.FoodAllergy:
                    List<FoodAllergyResultModel>? foodAllergyInfo = errorInfo.ErrorInfo as List<FoodAllergyResultModel>;
                    if (foodAllergyInfo != null)
                    {
                        itemNameList.AddRange(foodAllergyInfo.Select(item => item.YjCd));
                        foodNameList.AddRange(foodAllergyInfo.Select(item => item.AlrgyKbn));
                    }
                    break;
                case RealtimeCheckerType.Age:
                    List<AgeResultModel>? ageErrorInfo = errorInfo.ErrorInfo as List<AgeResultModel>;
                    if (ageErrorInfo != null)
                    {
                        itemNameList.AddRange(ageErrorInfo.Select(item => item.YjCd));
                    }
                    break;
                case RealtimeCheckerType.Disease:
                    List<DiseaseResultModel>? diseaseErrorInfo = errorInfo.ErrorInfo as List<DiseaseResultModel>;
                    if (diseaseErrorInfo != null)
                    {
                        itemNameList.AddRange(diseaseErrorInfo.Select(item => item.YjCd));
                        diseaseNameList.AddRange(diseaseErrorInfo.Select(item => item.ByotaiCd));
                    }
                    break;
                case RealtimeCheckerType.Kinki:
                case RealtimeCheckerType.KinkiTain:
                case RealtimeCheckerType.KinkiOTC:
                case RealtimeCheckerType.KinkiSupplement:
                    List<KinkiResultModel>? kinkiErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiErrorInfo != null)
                    {
                        itemNameList.AddRange(kinkiErrorInfo.Select(item => item.AYjCd));
                        itemNameList.AddRange(kinkiErrorInfo.Select(item => item.BYjCd));
                        kinkiCommentList.AddRange(kinkiErrorInfo.Select(item => item.CommentCode));
                        kijyoCommentList.AddRange(kinkiErrorInfo.Select(item => item.SayokijyoCode));
                        oTCItemNameList.AddRange(kinkiErrorInfo.Select(item => item.BYjCd));
                        oTCComponentInfoList.AddRange(kinkiErrorInfo.Select(item => item.SeibunCd));
                        supplementComponentInfoList.AddRange(kinkiErrorInfo.Select(item => item.SeibunCd));
                        suppleItemNameList.AddRange(kinkiErrorInfo.Select(item => item.SeibunCd));
                    }
                    break;
                case RealtimeCheckerType.KinkiUser:
                    List<KinkiResultModel>? kinkiUserErrorInfo = errorInfo.ErrorInfo as List<KinkiResultModel>;
                    if (kinkiUserErrorInfo != null)
                    {
                        itemNameList.AddRange(kinkiUserErrorInfo.Select(item => item.AYjCd));
                        itemNameList.AddRange(kinkiUserErrorInfo.Select(item => item.BYjCd));
                    }
                    break;
                case RealtimeCheckerType.Days:
                    List<DayLimitResultModel>? dayLimitErrorInfo = errorInfo.ErrorInfo as List<DayLimitResultModel>;
                    if (dayLimitErrorInfo != null)
                    {
                        itemNameList.AddRange(dayLimitErrorInfo.Select(item => item.YjCd));
                    }
                    break;
                case RealtimeCheckerType.Dosage:
                    List<DosageResultModel>? dosageErrorInfo = errorInfo.ErrorInfo as List<DosageResultModel>;
                    if (dosageErrorInfo != null)
                    {
                        itemNameList.AddRange(dosageErrorInfo.Select(item => item.YjCd));
                        usageDosageList.AddRange(dosageErrorInfo.Select(item => item.YjCd));
                    }
                    break;
                case RealtimeCheckerType.Duplication:
                    List<DuplicationResultModel>? duplicationErrorInfo = errorInfo.ErrorInfo as List<DuplicationResultModel>;
                    if (duplicationErrorInfo != null)
                    {
                        itemNameByItemCodeList.AddRange(duplicationErrorInfo.Select(item => item.ItemCd));
                        itemNameByItemCodeList.AddRange(duplicationErrorInfo.Select(item => item.DuplicatedItemCd));
                    }
                    break;
            }
        });
        itemNameList = itemNameList.Distinct().ToList();
        componentNameList = componentNameList.Distinct().ToList();
        analogueNameList = analogueNameList.Distinct().ToList();
        drvalrgyNameList = drvalrgyNameList.Distinct().ToList();
        foodNameList = foodNameList.Distinct().ToList();
        diseaseNameList = diseaseNameList.Distinct().ToList();
        kinkiCommentList = kinkiCommentList.Distinct().ToList();
        kijyoCommentList = kijyoCommentList.Distinct().ToList();
        oTCItemNameList = oTCItemNameList.Distinct().ToList();
        oTCComponentInfoList = oTCComponentInfoList.Distinct().ToList();
        supplementComponentInfoList = supplementComponentInfoList.Distinct().ToList();
        suppleItemNameList = suppleItemNameList.Distinct().ToList();
        usageDosageList = usageDosageList.Distinct().ToList();
        itemNameByItemCodeList = itemNameByItemCodeList.Distinct().ToList();

        _itemNameDictionary = itemNameList.Any() ? _realtimeOrderErrorFinder.FindItemNameDic(itemNameList, _sinday) : new();
        _componentNameDictionary = componentNameList.Any() ? _realtimeOrderErrorFinder.FindComponentNameDic(componentNameList) : new();
        _analogueNameDictionary = analogueNameList.Any() ? _realtimeOrderErrorFinder.FindAnalogueNameDic(analogueNameList) : new();
        _drvalrgyNameDictionary = drvalrgyNameList.Any() ? _realtimeOrderErrorFinder.FindDrvalrgyNameDic(drvalrgyNameList) : new();
        _foodNameDictionary = foodNameList.Any() ? _realtimeOrderErrorFinder.FindFoodNameDic(foodNameList) : new();
        _diseaseNameDictionary = diseaseNameList.Any() ? _realtimeOrderErrorFinder.FindDiseaseNameDic(diseaseNameList) : new();
        _kinkiCommentDictionary = kinkiCommentList.Any() ? _realtimeOrderErrorFinder.FindKinkiCommentDic(kinkiCommentList) : new();
        _kijyoCommentDictionary = kijyoCommentList.Any() ? _realtimeOrderErrorFinder.FindKijyoCommentDic(kijyoCommentList) : new();
        _oTCItemNameDictionary = oTCItemNameList.Any() ? _realtimeOrderErrorFinder.FindOTCItemNameDic(oTCItemNameList) : new();
        _oTCComponentInfoDictionary = oTCComponentInfoList.Any() ? _realtimeOrderErrorFinder.GetOTCComponentInfoDic(oTCComponentInfoList) : new();
        _supplementComponentInfoDictionary = supplementComponentInfoList.Any() ? _realtimeOrderErrorFinder.GetSupplementComponentInfoDic(supplementComponentInfoList) : new();
        _suppleItemNameDictionary = suppleItemNameList.Any() ? _realtimeOrderErrorFinder.FindSuppleItemNameDic(suppleItemNameList) : new();
        _usageDosageDictionary = usageDosageList.Any() ? _realtimeOrderErrorFinder.GetUsageDosageDic(usageDosageList) : new();
        _itemNameByItemCodeDictionary = itemNameByItemCodeList.Any() ? _realtimeOrderErrorFinder.FindItemNameByItemCodeDic(itemNameByItemCodeList, _sinday) : new();
    }

    #endregion

    #region ProcessDataForDrugAllergy
    public List<ErrorInfoModel> ProcessDataForDrugAllergy(List<DrugAllergyResultModel> allergyInfo)
    {
        if (_realtimeOrderErrorFinder.IsNoMasterData())
        {
            return ProcessDataForDrugAllergyWithNoMasterData(allergyInfo);
        }

        List<ErrorInfoModel> result = new List<ErrorInfoModel>();

        var errorGroup = (from a in allergyInfo
                          group a by new { a.YjCd, a.AllergyYjCd, a.Id }
                          into gcs
                          select new { gcs.Key.YjCd, gcs.Key.AllergyYjCd, gcs.Key.Id }
                          ).ToList();

        foreach (var error in errorGroup)
        {
            List<DrugAllergyResultModel> tempData =
                allergyInfo
                .Where(a => a.YjCd == error.YjCd && a.AllergyYjCd == error.AllergyYjCd)
                .OrderByDescending(a => a.Level)
                .ToList();
            string itemName = _itemNameDictionary.ContainsKey(error.YjCd) ? _itemNameDictionary[error.YjCd] : string.Empty;
            string allergyItemName = _itemNameDictionary.ContainsKey(error.AllergyYjCd) ? _itemNameDictionary[error.AllergyYjCd] : string.Empty;
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                ErrorType = CommonCheckerType.DrugAllergyChecker,
                Id = error.Id,
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = allergyItemName
            };

            List<LevelInfoModel> _listLevelInfo = new();
            foreach (var item in tempData)
            {
                LevelInfoModel? levelInfo = _listLevelInfo.FirstOrDefault(c => c.Level == item.Level);
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
                    StringBuilder comment = new();

                    int level = (error.YjCd == error.AllergyYjCd) ? 0 : item.Level;
                    levelInfo.BackgroundCode = LevelConfig.DrugAllegySource[level][0];
                    levelInfo.BorderBrushCode = LevelConfig.DrugAllegySource[level][1];
                    levelInfo.Title = LevelConfig.DrugAllegySource[level][2];

                    if (item.YjCd == item.AllergyYjCd)
                    {
                        comment.Append("※アレルギー登録薬です。" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        switch (item.Level)
                        {
                            case 1:
                                string componentName1 = _componentNameDictionary.ContainsKey(item.SeibunCd) ? _componentNameDictionary[item.SeibunCd] : string.Empty;
                                comment.Append(string.Format(_commentLevel1Template, allergyItemName, componentName1) + Environment.NewLine + Environment.NewLine);
                                break;
                            case 2:
                                string componentName2 = _componentNameDictionary.ContainsKey(item.SeibunCd) ? _componentNameDictionary[item.SeibunCd] : string.Empty;
                                string allergyComponentName2 = _realtimeOrderErrorFinder.FindComponentName(item.AllergySeibunCd);
                                comment.Append(string.Format(_commentLevel2Template, itemName, componentName2, allergyItemName, allergyComponentName2, componentName2) + Environment.NewLine + Environment.NewLine);
                                break;
                            case 3:
                                string componentName3 = _componentNameDictionary.ContainsKey(item.SeibunCd) ? _componentNameDictionary[item.SeibunCd] : string.Empty;
                                string allergyComponentName3 = _componentNameDictionary.ContainsKey(item.AllergySeibunCd) ? _componentNameDictionary[item.AllergySeibunCd] : string.Empty;
                                string analogueName = _analogueNameDictionary.ContainsKey(item.Tag) ? _analogueNameDictionary[item.Tag] : string.Empty;
                                comment.Append(string.Format(_commentLevel3Template, itemName, componentName3, allergyItemName, allergyComponentName3, analogueName) + Environment.NewLine + Environment.NewLine);
                                break;
                            case 4:
                                string drvalrgyName = _drvalrgyNameDictionary.ContainsKey(item.Tag) ? _drvalrgyNameDictionary[item.Tag] : string.Empty;
                                comment.Append(string.Format(_commentLevel4Template, itemName, allergyItemName, drvalrgyName) + Environment.NewLine + Environment.NewLine);
                                break;
                        }
                    }
                    levelInfo.Comment = comment.ToString();
                }
            }
            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForDrugAllergyWithNoMasterData
    public List<ErrorInfoModel> ProcessDataForDrugAllergyWithNoMasterData(List<DrugAllergyResultModel> allergyInfo)
    {
        List<ErrorInfoModel> result = new();
        allergyInfo.ForEach((a) =>
        {
            string itemName = _itemNameByItemCodeDictionary.ContainsKey(a.ItemCd) ? _itemNameByItemCodeDictionary[a.ItemCd] : string.Empty;
            ErrorInfoModel info = new ErrorInfoModel()
            {
                ErrorType = CommonCheckerType.DrugAllergyChecker,
                Id = a.Id,
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = itemName
            };
            List<LevelInfoModel> _listLevelInfo = new()
            {
                new LevelInfoModel()
                {
                    FirstItemName = itemName,
                    SecondItemName = itemName,
                    Level = a.Level,
                    Comment = "※アレルギー登録薬です。"
                }
            };
            info.ListLevelInfo.AddRange(_listLevelInfo);
            result.Add(info);
        });
        return result;
    }
    #endregion

    #region ProcessDataForFoodAllergy
    public List<ErrorInfoModel> ProcessDataForFoodAllergy(List<FoodAllergyResultModel> allergyInfo)
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
            string itemName = _itemNameDictionary.ContainsKey(error.YjCd) ? _itemNameDictionary[error.YjCd] : string.Empty;
            string foodName = _foodNameDictionary.ContainsKey(error.AlrgyKbn) ? _foodNameDictionary[error.AlrgyKbn] : string.Empty;
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                ErrorType = CommonCheckerType.FoodAllergyChecker,
                Id = error.Id,
                FirstCellContent = "アレルギー",
                ThridCellContent = itemName,
                FourthCellContent = foodName
            };

            List<LevelInfoModel> _listLevelInfo = new();
            foreach (var item in tempData)
            {
                int level = item.TenpuLevel.AsInteger();
                LevelInfoModel? levelInfo = _listLevelInfo.FirstOrDefault(c => c.Level == level);
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
                if (1 <= level && level <= 3)
                {
                    levelInfo.BackgroundCode = LevelConfig.FoodAllegySource[level][0];
                    levelInfo.BorderBrushCode = LevelConfig.FoodAllegySource[level][1];
                    levelInfo.Title = LevelConfig.FoodAllegySource[level][2];
                }

                StringBuilder commentStringBuilder = new();
                commentStringBuilder.Append(levelInfo.Comment);
                commentStringBuilder.Append(item.AttentionCmt);
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(item.WorkingMechanism);
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(Environment.NewLine);
                levelInfo.Comment = commentStringBuilder.ToString();
            }
            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForAge
    public List<ErrorInfoModel> ProcessDataForAge(List<AgeResultModel> ages)
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
            string itemName = _itemNameDictionary.ContainsKey(error.YjCd) ? _itemNameDictionary[error.YjCd] : string.Empty;
            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                ErrorType = CommonCheckerType.AgeChecker,
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
                LevelInfoModel? levelInfo = _listLevelInfo.FirstOrDefault(c => c.Level == level);
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

                levelInfo.BackgroundCode = LevelConfig.AgeSource[level][0];
                levelInfo.BorderBrushCode = LevelConfig.AgeSource[level][1];
                levelInfo.Title = LevelConfig.AgeSource[level][2];
                StringBuilder commentStringBuilder = new();
                commentStringBuilder.Append(levelInfo.Comment);
                commentStringBuilder.Append(attention);
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(item.WorkingMechanism);
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(Environment.NewLine);
                levelInfo.Comment = commentStringBuilder.ToString();
            }

            tempModel.ListLevelInfo.AddRange(_listLevelInfo);

            result.Add(tempModel);
        }

        return result;
    }
    #endregion

    #region ProcessDataForDisease
    public List<ErrorInfoModel> ProcessDataForDisease(List<DiseaseResultModel> diseaseInfo)
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


            string itemName = _itemNameDictionary.ContainsKey(drugDiseaseCode.YjCd) ? _itemNameDictionary[drugDiseaseCode.YjCd] : string.Empty;
            string diseaseName = _diseaseNameDictionary.ContainsKey(drugDiseaseCode.ByotaiCd) ? _diseaseNameDictionary[drugDiseaseCode.ByotaiCd] : string.Empty;

            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                ErrorType = CommonCheckerType.DiseaseChecker,
                Id = drugDiseaseCode.Id,
                FirstCellContent = DiseaseTypeName(drugDiseaseCode.DiseaseType),
                ThridCellContent = itemName,
                FourthCellContent = diseaseName
            };

            List<LevelInfoModel> _listLevelInfoModel = tempModel.ListLevelInfo;
            foreach (var item in listFilteredData)
            {
                int level = item.TenpuLevel;
                LevelInfoModel? levelInfoModel = _listLevelInfoModel.FirstOrDefault(c => c.Level == level);
                if (levelInfoModel == null)
                {
                    levelInfoModel = new LevelInfoModel()
                    {
                        FirstItemName = itemName,
                        SecondItemName = diseaseName,
                        Level = level
                    };
                    _listLevelInfoModel.Add(levelInfoModel);
                }

                levelInfoModel.BackgroundCode = LevelConfig.DiseaseSource[level][0];
                levelInfoModel.BorderBrushCode = LevelConfig.DiseaseSource[level][1];
                levelInfoModel.Title = LevelConfig.DiseaseSource[level][2];
                StringBuilder commentStringBuilder = new();
                commentStringBuilder.Append(levelInfoModel.Comment);
                commentStringBuilder.Append(_realtimeOrderErrorFinder.FindDiseaseComment(item.CmtCd));
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(_realtimeOrderErrorFinder.FindDiseaseComment(item.KijyoCd));
                commentStringBuilder.Append(Environment.NewLine);
                commentStringBuilder.Append(Environment.NewLine);
                levelInfoModel.Comment = commentStringBuilder.ToString();
            }


            result.Add(tempModel);
        }
        return result;
    }
    #endregion

    #region ProcessDataForKinki
    public List<ErrorInfoModel> ProcessDataForKinki(RealtimeCheckerType checkingType, List<KinkiResultModel> kinkiErrorInfo)
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
                    return _itemNameDictionary.ContainsKey(code) ? _itemNameDictionary[code] : string.Empty;
                case RealtimeCheckerType.KinkiOTC:
                    return _oTCItemNameDictionary.ContainsKey(code) ? _oTCItemNameDictionary[code] : string.Empty;
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

        for (int x = 0; x < listKinkiCode.Count; x++)
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

            if (listFilteredData.Count == 0) continue;

            string itemAName = _itemNameDictionary.ContainsKey(kikinCode.AYjCd) ? _itemNameDictionary[kikinCode.AYjCd] : string.Empty;
            string itemBName = string.Empty;

            if (checkingType == RealtimeCheckerType.KinkiSupplement)
            {
                string seibunCd = listFilteredData.First().SeibunCd;
                string indexWord = listFilteredData.First().IndexWord;
                string seibunName = _suppleItemNameDictionary.ContainsKey(seibunCd) ? _suppleItemNameDictionary[seibunCd] : string.Empty;

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
                ErrorType = CommonCheckerType.KinkiChecker,
                Id = kikinCode.Id,
                FirstCellContent = GetCheckingTitle(),
                ThridCellContent = itemAName,
                FourthCellContent = itemBName,
                CurrentItemCd = kikinCode.ItemCd,
                CheckingItemCd = kikinCode.KinkiItemCd,
            };
            result.Add(tempModel);

            List<KinkiErrorDetail> listDetail = new();
            listFilteredData.ForEach((f) =>
            {
                KinkiErrorDetail kinkiErrorDetail = new()
                {
                    Level = f.Kyodo.AsInteger(),
                    CommentContent = (_kinkiCommentDictionary.ContainsKey(f.CommentCode) ? _kinkiCommentDictionary[f.CommentCode] : string.Empty).Trim(),
                    SayokijyoContent = (_kijyoCommentDictionary.ContainsKey(f.SayokijyoCode) ? _kijyoCommentDictionary[f.SayokijyoCode] : string.Empty).Trim()
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
                    string otcComponentInfo = _oTCComponentInfoDictionary.ContainsKey(f.SeibunCd) ? _oTCComponentInfoDictionary[f.SeibunCd] : string.Empty;
                    stringToReplace = f.Sbt == 1 ? "の含有成分「" + otcComponentInfo + "」" : "の添加物「" + otcComponentInfo + "」";
                }
                else if (checkingType == RealtimeCheckerType.KinkiSupplement)
                {
                    string supplementComponentInfo = _supplementComponentInfoDictionary.ContainsKey(f.SeibunCd) ? _supplementComponentInfoDictionary[f.SeibunCd] : string.Empty;
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

                LevelInfoModel.BackgroundCode = LevelConfig.KinkiCommonSource[l.Level][0];
                LevelInfoModel.BorderBrushCode = LevelConfig.KinkiCommonSource[l.Level][1];
                LevelInfoModel.Title = LevelConfig.KinkiCommonSource[l.Level][2];

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
    public List<ErrorInfoModel> ProcessDataForKinkiUser(List<KinkiResultModel> kinkiErrorInfo)
    {
        List<ErrorInfoModel> result = new();
        kinkiErrorInfo.ForEach((k) =>
        {
            string itemAName = _itemNameDictionary.ContainsKey(k.AYjCd) ? _itemNameDictionary[k.AYjCd] : string.Empty;
            string itemBName = _itemNameDictionary.ContainsKey(k.BYjCd) ? _itemNameDictionary[k.BYjCd] : string.Empty;

            ErrorInfoModel tempModel = new ErrorInfoModel
            {
                ErrorType = CommonCheckerType.KinkiChecker,
                Id = k.Id,
                FirstCellContent = "相互作用",
                ThridCellContent = itemAName,
                FourthCellContent = itemBName
            };
            result.Add(tempModel);

            List<LevelInfoModel> _listLevelInfoModel = new()
            {
                new LevelInfoModel()
                {
                    FirstItemName = itemAName,
                    SecondItemName = itemBName,
                    Level = 1,
                    BackgroundCode = LevelConfig.KinkiCommonSource[1][0],
                    BorderBrushCode = LevelConfig.KinkiCommonSource[1][1],
                    Title = LevelConfig.KinkiCommonSource[1][2],
                    Comment = "ユーザー設定"
                }
            };
            tempModel.ListLevelInfo = _listLevelInfoModel;
        });
        return result;
    }
    #endregion

    #region ProcessDataForDayLimit
    public List<ErrorInfoModel> ProcessDataForDayLimit(List<DayLimitResultModel> dayLimitError)
    {
        List<ErrorInfoModel> result = new();
        foreach (DayLimitResultModel dayLimit in dayLimitError)
        {
            string itemName = _realtimeOrderErrorFinder.FindItemName(dayLimit.YjCd, _sinday);
            ErrorInfoModel errorInfoModel = new ErrorInfoModel();
            result.Add(errorInfoModel);
            errorInfoModel.ErrorType = CommonCheckerType.DayLimitChecker;
            errorInfoModel.Id = dayLimit.Id;
            errorInfoModel.FirstCellContent = "投与日数";
            errorInfoModel.SecondCellContent = "ー";
            errorInfoModel.ThridCellContent = itemName;
            errorInfoModel.FourthCellContent = dayLimit.UsingDay.AsString() + "日";
            errorInfoModel.SuggestedContent = "／" + dayLimit.LimitDay.AsString() + "日";
            errorInfoModel.HighlightColorCode = "#f12c47";

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
    public List<ErrorInfoModel> ProcessDataForDosage(List<DosageResultModel> listDosageError)
    {
        List<ErrorInfoModel> result = new();
        foreach (DosageResultModel dosage in listDosageError)
        {
            ErrorInfoModel errorInfoModel = new();
            result.Add(errorInfoModel);
            string itemName = _itemNameDictionary.ContainsKey(dosage.YjCd) ? _itemNameDictionary[dosage.YjCd] : string.Empty;
            errorInfoModel.ErrorType = CommonCheckerType.DosageChecker;
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
                    errorInfoModel.HighlightColorCode = "#0000ff";
                    break;
                case DosageLabelChecking.OneMax:
                    levelTitle = "一回量／最大値";
                    errorInfoModel.HighlightColorCode = "#f12c47";
                    break;
                case DosageLabelChecking.OneLimit:
                    levelTitle = "一回量／上限値";
                    errorInfoModel.HighlightColorCode = "#f12c47";
                    break;
                case DosageLabelChecking.DayMin:
                    levelTitle = "一日量／最小値";
                    errorInfoModel.HighlightColorCode = "#0000ff";
                    break;
                case DosageLabelChecking.DayMax:
                    levelTitle = "一日量／最大値";
                    errorInfoModel.HighlightColorCode = "#f12c47";
                    break;
                case DosageLabelChecking.DayLimit:
                    levelTitle = "一日量／上限値";
                    errorInfoModel.HighlightColorCode = "#f12c47";
                    break;
                case DosageLabelChecking.TermLimit:
                    levelTitle = "期間上限";
                    errorInfoModel.HighlightColorCode = "#f12c47";
                    break;
            }
            string comment = string.Empty;
            if (dosage.IsFromUserDefined)
            {
                comment = "ユーザー設定";
            }
            else
            {
                comment = _usageDosageDictionary.ContainsKey(dosage.YjCd) ? _usageDosageDictionary[dosage.YjCd] : string.Empty;
            }

            LevelInfoModel LevelInfoModel = new()
            {
                Title = levelTitle,
                BorderBrushCode = "#ff66b3",
                BackgroundCode = "#ff9fcf",
                FirstItemName = itemName,
                Comment = comment
            };
            errorInfoModel.ListLevelInfo = new List<LevelInfoModel>() { LevelInfoModel };
        }
        return result;
    }
    #endregion

    #region ProcessDataForDuplication
    public List<ErrorInfoModel> ProcessDataForDuplication(List<DuplicationResultModel> listDuplicationError)
    {
        List<ErrorInfoModel> result = new();
        foreach (DuplicationResultModel duplicationError in listDuplicationError)
        {
            string itemName = _itemNameByItemCodeDictionary.ContainsKey(duplicationError.ItemCd) ? _itemNameByItemCodeDictionary[duplicationError.ItemCd] : string.Empty;
            string duplicatedItemName = _itemNameByItemCodeDictionary.ContainsKey(duplicationError.DuplicatedItemCd) ? _itemNameByItemCodeDictionary[duplicationError.DuplicatedItemCd] : string.Empty;

            ErrorInfoModel errorInfoModel = new ErrorInfoModel();
            result.Add(errorInfoModel);
            errorInfoModel.ErrorType = CommonCheckerType.DuplicationChecker;
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

            LevelInfoModel levelInfoModel = new()
            {
                BackgroundCode = LevelConfig.DuplicationCommonSource[duplicationError.Level][0],
                BorderBrushCode = LevelConfig.DuplicationCommonSource[duplicationError.Level][1],
                Title = LevelConfig.DuplicationCommonSource[duplicationError.Level][2],
                FirstItemName = itemName,
                SecondItemName = duplicationError.IsComponentDuplicated || duplicationError.IsIppanCdDuplicated ? duplicatedItemName : string.Empty,
                Level = duplicationError.Level
            };

            if (duplicationError.IsIppanCdDuplicated)
            {
                levelInfoModel.Comment = "「" + itemName + "」と「" + duplicatedItemName + "」は一般名（" + _realtimeOrderErrorFinder.FindIppanNameByIppanCode(duplicationError.IppanCode) + "）が同じです。";
            }
            else if (duplicationError.IsComponentDuplicated)
            {
                StringBuilder comment = new();
                comment.Append(levelInfoModel.Comment);
                switch (duplicationError.Level)
                {
                    case 1:
                        string componentName1 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        comment.Append(string.Format(_duplicatedComponentTemplate, itemName, duplicatedItemName, componentName1) + Environment.NewLine + Environment.NewLine);
                        break;
                    case 2:
                        string componentName2 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        string allergyComponentName2 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.AllergySeibunCd);
                        comment.Append(string.Format(_proDrupTemplate, itemName, componentName2, duplicatedItemName, allergyComponentName2, componentName2) + Environment.NewLine + Environment.NewLine);
                        break;
                    case 3:
                        string componentName3 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.SeibunCd);
                        string allergyComponentName3 = _realtimeOrderErrorFinder.FindComponentName(duplicationError.AllergySeibunCd);
                        string analogueName = _realtimeOrderErrorFinder.FindAnalogueName(duplicationError.Tag);
                        comment.Append(string.Format(_sameComponentTemplate, itemName, componentName3, duplicatedItemName, allergyComponentName3, analogueName) + Environment.NewLine + Environment.NewLine);
                        break;
                    case 4:
                        string className = _realtimeOrderErrorFinder.FindClassName(duplicationError.Tag);
                        comment.Append(string.Format(_duplicatedClassTemplate, itemName, duplicatedItemName, className) + Environment.NewLine + Environment.NewLine);
                        break;
                }
                levelInfoModel.Comment = comment.ToString();
            }
            else
            {
                levelInfoModel.Comment = "同一薬剤（" + itemName + "）が処方されています。";
            }

            errorInfoModel.ListLevelInfo = new() { levelInfoModel };
        }
        return result;
    }
    #endregion

    #region RemoveDuplicatedErrorInfo
    public List<KinkiResultModel> RemoveDuplicatedErrorInfo(List<KinkiResultModel> originList)
    {
        List<KinkiResultModel> subResult = new();
        originList.ForEach(k =>
        {
            var tempError = subResult.FirstOrDefault(a => a.AYjCd == k.AYjCd &&
                                                 a.BYjCd == k.BYjCd &&
                                                 a.CommentCode == k.CommentCode &&
                                                 a.SayokijyoCode == k.SayokijyoCode &&
                                                 a.Kyodo == k.Kyodo &&
                                                 a.IsNeedToReplace == k.IsNeedToReplace &&
                                                 a.IndexWord == k.IndexWord);

            if (tempError == null)
            {
                subResult.Add(k);
            }
        });


        return subResult;
    }
    #endregion

    private SpecialNoteFull ConvertToSpecialNoteModel(SpecialNoteItem specialNoteItem)
    {
        var summaryInfModel = new SummaryInfModel(
                specialNoteItem.SummaryTab.Id,
                specialNoteItem.SummaryTab.HpId,
                specialNoteItem.SummaryTab.PtId,
                specialNoteItem.SummaryTab.SeqNo,
                specialNoteItem.SummaryTab.Text,
                specialNoteItem.SummaryTab.Rtext,
                DateTime.MinValue,
                DateTime.MinValue
            );

        var pregnancies = specialNoteItem.PatientInfoTab.PregnancyItems.Select(p =>
            new PtPregnancyModel(
                    p.Id,
                    p.HpId,
                    p.PtId,
                    p.SeqNo,
                    p.StartDate,
                    p.EndDate,
                    p.PeriodDate,
                    p.PeriodDueDate,
                    p.OvulationDate,
                    p.OvulationDueDate,
                    p.IsDeleted,
                    DateTime.MinValue,
                    0,
                    string.Empty,
                    p.SinDate
                )
            ).ToList();

        var kensaDetailInfs = specialNoteItem.PatientInfoTab.KensaInfDetailItems.Select(k =>
          new KensaInfDetailModel(
                k.HpId,
                k.PtId,
                k.IraiCd,
                k.SeqNo,
                k.IraiDate,
                k.RaiinNo,
                k.KensaItemCd,
                k.ResultVal,
                k.ResultType,
                k.AbnormalKbn,
                k.IsDeleted,
                k.CmtCd1,
                k.CmtCd2,
                DateTime.MinValue,
                string.Empty,
                string.Empty,
                0
              )
          ).ToList();

        var physicalModel = new PhysicalInfoModel(kensaDetailInfs);

        var patientInfModel = new PatientInfoModel(
                pregnancies,
                specialNoteItem.PatientInfoTab.PtCmtInfItems,
                specialNoteItem.PatientInfoTab.SeikatureInfItems,
                new List<PhysicalInfoModel> { physicalModel }
            );

        var specialNoteModel = new SpecialNoteFull(
            specialNoteItem.ImportantNoteTab,
            patientInfModel,
            summaryInfModel
            );

        return specialNoteModel;
    }

    private FamilyModel ConvertToFamilyModel(FamilyItem familyItem)
    {
        return new FamilyModel(
                familyItem.FamilyId,
                familyItem.PtId,
                0,
                familyItem.ZokugaraCd,
                familyItem.FamilyPtId,
                familyItem.FamilyPtNum,
                familyItem.Name,
                familyItem.KanaName,
                familyItem.Sex,
                familyItem.Birthday,
                familyItem.Age,
                familyItem.IsDead,
                familyItem.IsSeparated,
                familyItem.Biko,
                familyItem.SortNo,
                familyItem.PtFamilyRekiList.Select(
                        p => new PtFamilyRekiModel(
                                p.Id,
                                p.ByomeiCd,
                                p.Byomei,
                                p.Cmt,
                                p.SortNo,
                                p.IsDeleted
                            )
                    ).ToList(),
                string.Empty
            );
    }

    public void ReleaseResource()
    {
        _tenantProvider.DisposeDataContext();
    }
}
