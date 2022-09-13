namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);

        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();

        IEnumerable<TenItemModel> SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired);

        TenItemModel GetTenMst(int hpId, int sinDate, string itemCd);

        bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem);

    }
}
