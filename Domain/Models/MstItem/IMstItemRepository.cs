namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);

        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData();

        (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize);

        (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize);

        (List<TenItemModel>, int) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem);

        TenItemModel GetTenMst(int hpId, int sinDate, string itemCd);

        bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem, int hpId, int userId);

        List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, string keyword, int sindate, int pageIndex, int pageSize);

        List<ByomeiMstModel> DiseaseSearch(List<string> keyCodes);

        bool UpdateAdoptedByomei(int hpId, string byomeiCd, int userId);

        List<TenItemModel> GetCheckTenItemModels(int hpId, int sinDate, List<string> itemCds);

        bool CheckItemCd(string ItemCd);

        (int, List<PostCodeMstModel>) PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize);

        TenItemModel FindTenMst(int hpId, string itemCd, int sinDate);

        List<TenItemModel> FindTenMst(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);

        List<ItemCmtModel> GetCmtCheckMsts(int hpId, int userId, List<string> itemCds);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, List<long> itemGrpCds);

        List<TenItemModel> GetAdoptedItems(List<string> itemCds, int sinDate, int hpId);

        bool UpdateAdoptedItems(int valueAdopted, List<string> itemCds, int sinDate, int hpId, int userId);

        List<ItemCommentSuggestionModel> GetSelectiveComment(int hpCd, List<string> listItemCd, int sinDate, List<int> isInvalidList, bool isRecalculation = false);
    }
}
