namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);
        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();
        (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize);
        (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize);
    }
}
