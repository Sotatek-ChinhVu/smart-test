namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);
        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();
        SearchOTCModel SearchOTCModels(string searchValue, int pageIndex, int pageSize);
    }
}
