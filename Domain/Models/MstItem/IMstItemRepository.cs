namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);

        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();

        (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize);

        (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize);

        (List<TenItemModel>, int) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith);

        TenItemModel GetTenMst(int hpId, int sinDate, string itemCd);

        bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem);

        List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, string keyword, int pageIndex, int pageCount);

        List<ByomeiMstModel> DiseaseSearch(List<string> keyCodes);

        bool UpdateAdoptedByomei(int hpId, string byomeiCd);

        List<TenItemModel> GetCheckTenItemModels(int hpId, int sinDate, List<string> itemCds);

        bool CheckItemCd(string ItemCd);

        List<PostCodeMstModel> PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize);

        TenItemModel FindTenMst(int hpId, string itemCd, int sinDate);

        List<TenItemModel> FindTenMst(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);

        List<ItemCmtModel> GetCmtCheckMsts(List<string> itemCds);
    }
}
