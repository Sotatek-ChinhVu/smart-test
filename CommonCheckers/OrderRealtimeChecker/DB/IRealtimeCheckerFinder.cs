using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public interface IRealtimeCheckerFinder
    {
        Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<string> itemCdList, int sinDate);

        List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate);

        List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate);

        PtInf GetPatientInfo(int hpId, long ptId);

        KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode);

        List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<string> listItemCode, int level, List<PtAlrgyFoodModel> listPtAlrgyFoods);

        List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting);

        List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode);

        List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting);

        List<AgeResultModel> CheckAge(int hpID, long ptId, int sinday, int level, int ageTypeCheckSetting, List<string> listItemCode);

        List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, int level, int sinDate, List<string> listItemCode, List<string> listDiseaseCode);

        List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<string> listItemCode, List<PtKioReki> listPtKioReki);

        List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<string> listItemCode);

        List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<string> listCurrentOrderCode, List<string> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<string> listCurrentOrderCode, List<string> listAddedOrderCode);

        List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtOtherDrugModel> listPtOtherDrug);

        List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtOtcDrugModel> listPtOtcDrug);

        List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtSuppleModel> listPtSupple);

        List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double currentHeight, double currentWeight);

        List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<string> listAddedOrderCode, double usingDay);
    }
}
