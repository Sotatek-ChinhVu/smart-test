using Domain.Common;
using Domain.Enum;
using Domain.Models.AuditLog;
using Domain.Models.ContainerMaster;
using Domain.Models.FlowSheet;
using Domain.Models.KensaIrai;
using Domain.Models.MaterialMaster;
using Domain.Models.OrdInf;
using Domain.Models.OrdInfDetails;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Helper.Enum;

namespace Domain.Models.MstItem
{
    public interface IMstItemRepository : IRepositoryBase
    {
        bool ExistUsedKensaItemCd(int hpId, string kensaItemCd, int kensaSeqNo);

        List<TenItemModel> GetTenMstsWithStartDate(int hpId, string itemCd);

        bool IsKensaItemOrdering(int hpId, string tenItemCd);

        double GetTenOfKNItem(int hpId, string itemCd);

        Dictionary<string, double> GetTenOfItem(int hpId);

        Dictionary<string, string> GetKensaCenterMsts(int hpId);

        List<string> GetTenItemCds(int hpId);

        Dictionary<int, string> GetContainerMsts(int hpId);

        Dictionary<int, string> GetMaterialMsts(int hpId);

        List<string> GetUsedKensaItemCds(int hpId);

        List<KensaStdMstModel> GetKensaStdMstModels(int hpId, string kensaItemCd);

        bool UpdateKensaStdMst(int hpId, int userId, List<KensaStdMstModel> kensaStdMstModels);

        bool UpdateKensaMst(int hpId, int userId, List<KensaMstModel> kensaMstModels, List<TenItemModel> tenMstModels, List<KensaMstModel> childKensaMsts, List<TenItemModel> tenMstListGenDate);

        List<KensaMstModel> GetParrentKensaMstModels(int hpId, string keyWord, string itemCd);

        bool ContainerMasterUpdate(int hpId, int userId, List<ContainerMasterModel> containerMasters);

        bool UpsertMaterialMaster(int hpId, int userId, List<MaterialMasterModel> materialMasters);

        bool IsUsingKensa(int hpId, string kensaItemCd, List<string> itemCds);

        List<DosageDrugModel> GetDosages(int hpId, List<string> yjCds);

        List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData(int hpId);

        (List<OtcItemModel>, int) SearchOTCModels(int hpId, string searchValue, int pageIndex, int pageSize);

        List<SearchSupplementModel> GetListSupplement(int hpId, string searchValue);

        (List<TenItemModel> tenItemModels, int totalCount) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage, bool isDeleted, List<int> kouiKbns, List<int> drugKbns, string masterSBT);

        TenItemModel GetTenMst(int hpId, int sinDate, string itemCd);

        TenItemModel GetTenMst(int hpId, string itemCd, int sinDate);

        bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem, int hpId, int userId);

        List<ByomeiMstModel> DiseaseSearch(int hpId, bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, string keyword, int sindate, int pageIndex, int pageSize, bool isHasFreeByomei = true);

        List<ByomeiMstModel> DiseaseSearch(int hpId, List<string> keyCodes);

        bool UpdateAdoptedByomei(int hpId, string byomeiCd, int userId);

        List<TenItemModel> GetCheckTenItemModels(int hpId, int sinDate, List<string> itemCds);

        bool CheckItemCd(int hpId, string itemCd);

        TenItemModel FindTenMst(int hpId, string itemCd, int sinDate);

        List<TenItemModel> FindTenMst(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);

        List<TenItemModel> GetTenMstList(int hpId, List<string> itemCds);

        List<ItemCmtModel> GetCmtCheckMsts(int hpId, int userId, List<string> itemCds);
        List<CommentCheckMstModel> GetAllCmtCheckMst(int hpId, int sinDay);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, List<long> itemGrpCds);

        List<ItemGrpMstModel> FindItemGrpMst(int hpId, int minSinDate, int maxSinDate, int grpSbt, List<long> itemGrpCds);

        List<TenItemModel> GetAdoptedItems(List<string> itemCds, int sinDate, int hpId);

        bool UpdateAdoptedItems(int valueAdopted, List<string> itemCds, int sinDate, int hpId, int userId);

        List<ItemCommentSuggestionModel> GetSelectiveComment(int hpId, List<string> listItemCd, int sinDate, List<int> isInvalidList, bool isRecalculation = false);

        List<string> GetCheckItemCds(int hpId, List<string> itemCds);

        List<Tuple<string, string>> GetCheckIpnCds(List<string> ipnCds);

        List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid);

        //Key of Dictionary is itemCd
        Dictionary<string, (int sinkouiKbn, string itemName, List<TenItemModel>)> GetConversionItem(List<(string itemCd, int sinKouiKbn, string itemName)> expiredItems, int sinDate, int hpId);

        bool ExceConversionItem(int hpId, int userId, Dictionary<string, List<TenItemModel>> values);

        List<HolidayModel> FindHolidayMstList(int hpId, int fromDate, int toDate);

        List<KensaCenterMstModel> GetListKensaCenterMst(int hpId);

        List<TenMstOriginModel> GetGroupTenMst(int hpId, string itemCd);

        string GetMaxItemCdByTypeForAdd(int hpId, string startWithstr);

        int GetMinJihiSbtMst(int hpId);

        bool SaveKensaCenterMst(int userId, List<KensaCenterMstModel> kensaCenterMstModels);

        bool IsTenMstItemCdUsed(int hpId, string itemCd);

        bool SaveDeleteOrRecoverTenMstOrigin(int hpId, DeleteOrRecoverTenMstMode mode, string itemCd, int userId, List<TenMstOriginModel> tenMstModifieds);

        List<CmtKbnMstModel> GetListCmtKbnMstModelByItemCd(int hpId, string itemCd);

        TenMstOriginModel GetTenMstOriginModel(int hpId, string itemCd, int sinDate);

        string GetTenMstName(int hpId, string santeiItemCd);

        List<M10DayLimitModel> GetM10DayLimitModels(int hpId, string yjCdItem);

        List<IpnMinYakkaMstModel> GetIpnMinYakkaMstModels(int hpId, string IpnNameCd);

        List<DrugDayLimitModel> GetDrugDayLimitModels(int hpId, string ItemCd);

        DosageMstModel GetDosageMstModel(int hpId, string ItemCd);

        IpnNameMstModel GetIpnNameMstModel(int hpId, string ipnNameCd, int sinDate);

        string GetYohoInfMstPrefixByItemCd(int hpId, string itemCd);

        List<DrugInfModel> GetDrugInfByItemCd(int hpId, string itemCd);

        PiImageModel GetImagePiByItemCd(int hpId, string itemCd, int imageType);

        List<TeikyoByomeiModel> GetTeikyoByomeiModel(int hpId, string itemCd, bool isFromCheckingView = false);

        TekiouByomeiMstExcludedModel GetTekiouByomeiMstExcludedModelByItemCd(int hpId, string itemCd);

        List<DensiSanteiKaisuModel> GetDensiSanteiKaisuByItemCd(int hpId, string itemCd);

        List<DensiHaihanModel> GetDensiHaihans(int hpId, string itemCd, int haihanKbn);

        List<DensiHoukatuModel> GetListDensiHoukatuByItemCd(int hpId, string itemCd, int sinDate);

        List<DensiHoukatuGrpModel> GetListDensiHoukatuGrpByItemCd(int hpId, string itemCd, int sinDate);

        List<DensiHoukatuModel> GetListDensiHoukatuMaster(int hpId, List<string> listGrpNo);

        List<CombinedContraindicationModel> GetContraindicationModelList(int hpId, int sinDate, string itemCd);

        bool SaveTenMstOriginSetData(IEnumerable<CategoryItemEnums> tabActs, string itemCd, List<TenMstOriginModel> tenMstGrigins, SetDataTenMstOriginModel setDataTen, int userId, int hpId);

        RenkeiMstModel GetRenkeiMst(int hpId, int renkeiId);

        bool IsTenMstUsed(int hpId, string itemCd, int startDate, int endDate);

        List<JihiSbtMstModel> GetJihiSbtMstList(int hpId);

        List<TenMstMaintenanceModel> GetTenMstListByItemType(int hpId, ItemTypeEnums itemType, string startWithstr, int sinDate);

        (List<TenItemModel> tenItemModels, int totalCount) SearchTenMasterItem(int hpId, int pageIndex, int pageCount, string keyword, double? pointFrom, double? pointTo, int kouiKbn, int oriKouiKbn, List<int> kouiKbns, bool includeRosai, bool includeMisai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage, bool onlyUsage, string yJCode, bool isMasterSearch, bool isExpiredSearchIfNoData, bool isAllowSearchDeletedItem, bool isExpired, bool isDeleted, List<int> drugKbns, bool isSearchSanteiItem, bool isSearchKenSaItem, List<ItemTypeEnums> itemFilter, bool isSearch831SuffixOnly, bool isSearchGazoDensibaitaiHozon, SortType sortType, FilterTenMstEnum sortCol);

        (List<TenItemModel> tenItemModels, int totalCount) SearchSuggestionTenMstItem(int hpId, int pageIndex, int pageCount, string keyword, int kouiKbn, int oriKouiKbn, List<int> kouiKbns, bool includeMisai, bool includeRousai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage, bool isDeleted, List<int> drugKbns, List<ItemTypeEnums> itemFilter, bool isSearch831SuffixOnly);

        (int, List<PostCodeMstModel>) SearchAddress(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize);

        (List<KensaMstModel>, int) GetListKensaMst(int hpId, string keyWord, int pageIndex, int pageSize);

        string GetDrugAction(int hpId, string yjCd);

        string GetPrecautions(int hpId, string yjCd);

        bool UpdateCmtCheckMst(int userId, int hpId, List<ItemCmtModel> listData);

        bool SaveAddressMaster(List<PostCodeMstModel> postCodes, int hpId, int userId);

        bool CheckPostCodeExist(int hpId, string zipCD);

        List<SingleDoseMstModel> GetListSingleDoseModel(int hpId);

        List<MedicineUnitModel> GetListMedicineUnitModel(int hpId, int today);

        bool UpdateSingleDoseMst(int hpId, int userId, List<SingleDoseMstModel> listToSave);

        bool UpdateByomeiMst(int userId, int hpId, List<UpdateByomeiMstModel> listData);

        List<ByomeiMstModel> DiseaseNameMstSearch(int hpId, string keyword, bool chkByoKbn0, bool chkByoKbn1, bool chkSaiKbn, bool chkMiSaiKbn, bool chkSidoKbn, bool chkToku, bool chkHiToku1, bool chkHiToku2, bool chkTenkan, bool chkTokuTenkan, bool chkNanbyo, int pageIndex, int pageSize, bool isCheckPage);

        List<KensaIjiSettingModel> GetListKensaIjiSettingModel(int hpId, string keyWords, bool isValid, bool isExpired, bool? isPayment);

        bool UpdateJihiSbtMst(int hpId, int userId, List<JihiSbtMstModel> jihiSbtMsts);

        string GetNameByItemCd(int hpId, string itemCd);
        List<YohoSetMstModel> GetListYohoSetMstModelByUserID(int hpId, int userIdLogin, int sinDate, int userId = 0);

        List<RenkeiConfModel> GetRenkeiConfModels(int hpId, int renkeiSbt);

        List<RenkeiMstModel> GetRenkeiMstModels(int hpId);

        List<RenkeiTemplateMstModel> GetRenkeiTemplateMstModels(int hpId);

        List<EventMstModel> GetEventMstModelList(int hpId);

        bool SaveRenkei(int hpId, int userId, List<(int renkeiSbt, List<RenkeiConfModel> renkeiConfList)> renkeiTabList);

        List<SetNameMntModel> GetSetNameMnt(SetCheckBoxStatusModel checkBoxStatus, int generationId, int hpId);

        List<SetKbnMstModel> GetListSetKbnMst(int generationId, int hpId);

        int GetGenerationId(int hpId);


        List<UserMstModel> GetListUser(int hpId, int userId, int sinDate);

        List<CompareTenMstModel> SearchCompareTenMst(int hpId, int sinDate, List<ActionCompareSearchModel> actions, ComparisonSearchModel comparison);

        bool SaveCompareTenMst(List<SaveCompareTenMstModel> ListData, ComparisonSearchModel comparison, int userId);

        bool UpdateYohoSetMst(int hpId, int userId, List<YohoSetMstModel> listYohoSetMstModels);

        TenItemModel GetTenMstByCode(int hpId, string itemCd, int setKbn, int sinDate);

        ByomeiMstModel GetByomeiByCode(int hpId, string byomeiCd);

        bool SaveSetNameMnt(List<SetNameMntModel> lstModel, int userId, int hpId, int sinDate);

        List<RenkeiTimingModel> GetRenkeiTimingModel(int hpId, int renkeiId);
        bool CheckJihiSbtExistsInTenMst(int hpId, int jihiSbt);

        bool ExistedTenMstItem(int hpId, string itemCd, int sinDate);

        TenItemModel? GetTenMstInfo(int hpId, string itemCd, int sinDate);
    }
}
