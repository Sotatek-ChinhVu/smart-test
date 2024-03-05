using CommonCheckers;
using Entity.Tenant;

namespace CommonChecker.Caches.Interface
{
    public interface IMasterDataCacheService
    {
        void InitCache(int hpId, List<string> itemCodeList, int sinday, long ptId);

        TenMst? GetTenMst(int hpId, string itemCode);

        List<TenMst> GetTenMstList(int hpId, List<string> itemCodeList);

        List<M56ExIngrdtMain> GetM56ExIngrdtMainList(int hpId, List<string> itemCodeList);

        List<M56ExEdIngredients> GetM56ExEdIngredientList(int hpId, List<string> itemCodeList);

        List<M56ProdrugCd> GetM56ProdrugCdList(int hpId, List<string> itemCodeList);

        List<M56ExAnalogue> GetM56ExAnalogueList(int hpId, List<string> itemCodeList);

        List<M56YjDrugClass> GetM56YjDrugClassList(int hpId, List<string> itemCodeList);

        List<M56DrugClass> GetM56DrugClassList(int hpId, List<string> itemCodeList);

        List<KinkiMst> GetKinkiMstList(int hpId, List<string> itemCodeList);

        List<DosageDrug> GetDosageDrugList(int hpId, List<string> itemCodeList);

        List<DosageMst> GetDosageMstList(int hpId, List<string> itemCodeList);

        List<DosageDosage> GetDosageDosageList(int hpId, List<string> itemCodeList);

        PtInf? GetPtInf();

        SystemConfig GetSystemConfig();
    }
}
