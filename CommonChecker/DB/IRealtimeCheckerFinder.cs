using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;
using PtKioRekiModelStandard = Domain.Models.SpecialNote.ImportantNote.PtKioRekiModel;
using PtAlrgyDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyDrugModel;
using PtAlrgyFoodModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyFoodModel;
using PtOtcDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtcDrugModel;
using PtSuppleModelStandard = Domain.Models.SpecialNote.ImportantNote.PtSuppleModel;
using PtOtherDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtherDrugModel;


namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public interface IRealtimeCheckerFinder
    {
        Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<ItemCodeModel> itemCdList, int sinDate);

        List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels);

        List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyDrugModelStandard> ptAlrgyDrugModels);

        PtInf GetPatientInfo(int hpId, long ptId);

        KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode);

        List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, int level, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels);

        List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<AgeResultModel> CheckAge(int hpID, long ptId, int sinday, int level, int ageTypeCheckSetting, List<ItemCodeModel> listItemCode, List<KensaInfDetailModel> kensaInfDetailModels);

        List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, long ptID, int level, int sinDate, List<ItemCodeModel> listItemCode, List<PtDiseaseModel> ptDiseaseModels);

        List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> listItemCode, List<PtKioRekiModelStandard> ptKioRekiModels);

        List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> listItemCode, List<FamilyModel> familyModels);

        List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtherDrugModelStandard> ptOtherDrugModels);

        List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtcDrugModelStandard> ptOtcDrugModels);

        List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtSuppleModelStandard> ptSuppleModels);

        List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double currentHeight, double currentWeight, List<KensaInfDetailModel> kensaInfDetailModels);

        List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<ItemCodeModel> listAddedOrderCode, double usingDay);
    }
}
