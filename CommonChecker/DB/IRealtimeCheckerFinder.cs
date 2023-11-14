using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;
using PtAlrgyDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyDrugModel;
using PtAlrgyFoodModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyFoodModel;
using PtKioRekiModelStandard = Domain.Models.SpecialNote.ImportantNote.PtKioRekiModel;
using PtOtcDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtcDrugModel;
using PtOtherDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtherDrugModel;
using PtSuppleModelStandard = Domain.Models.SpecialNote.ImportantNote.PtSuppleModel;


namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public interface IRealtimeCheckerFinder
    {
        Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<ItemCodeModel> itemCdList, int sinDate);

        List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels, bool isDataOfDb);

        List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyDrugModelStandard> ptAlrgyDrugModels, bool isDataOfDb);

        KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode);

        List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, int level, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels, bool isDataOfDb);

        List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting);

        List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting);

        List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting);

        List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting);

        List<AgeResultModel> CheckAge(int hpID, long ptID, int sinday, int level, int ageTypeCheckSetting, List<ItemCodeModel> listItemCode, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb);

        List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, long ptID, int level, int sinDate, List<ItemCodeModel> listItemCode, List<PtDiseaseModel> ptDiseaseModels, bool isDataOfDb);

        List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> itemCodeModelList, List<PtKioRekiModelStandard> ptKioRekiModels, bool isDataOfDb);

        List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> itemCodeModelList, List<FamilyModel> familyModels, bool isDataOfDb);

        List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtherDrugModelStandard> ptOtherDrugModels, bool isDataOfDb);

        List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtcDrugModelStandard> ptOtcDrugModels, bool isDataOfDb);

        List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtSuppleModelStandard> ptSuppleModels, bool isDataOfDb);

        List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double height, double weight, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb);

        List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<ItemCodeModel> listAddedOrderCodes, double usingDay);

        (double weight, double height) GetPtBodyInfo(int hpId, long ptId, int sinday, double currentHeight, double currentWeight, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb);
    }
}
