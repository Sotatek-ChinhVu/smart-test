using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using Entity.Tenant;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public interface IRealtimeCheckerFinder
    {
        Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<ItemCodeModel> itemCdList, int sinDate);

        List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate);

        List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate);

        PtInf GetPatientInfo(int hpId, long ptId);

        KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode);

        List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, int level);

        List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<ItemCodeModel> listComparedItemCode, int haigouSetting);

        List<AgeResultModel> CheckAge(int hpID, long ptId, int sinday, int level, int ageTypeCheckSetting, List<ItemCodeModel> listItemCode);

        List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, long ptID, int level, int sinDate, List<ItemCodeModel> listItemCode);

        List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> listItemCode);

        List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> listItemCode);

        List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList);

        List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList);

        List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList);

        List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double currentHeight, double currentWeight);

        List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<ItemCodeModel> listAddedOrderCode, double usingDay);
    }
}
