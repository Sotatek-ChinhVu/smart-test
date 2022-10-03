namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        //Invalid parameter
        public static readonly string InvalidKeyword = "Invalid keyword";
        public static readonly string InvalidHpId = "Invalid HpId";
        public static readonly string InvalidPtId = "Invalid PtId";
        public static readonly string InvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string InvalidSinDate = "Invalid SinDate";
        public static readonly string InvalidHokenId = "Invalid HokenId";
        public static readonly string InvalidPageIndex = "Invalid PageIndex";
        public static readonly string InvalidPageCount = "Invalid PageCount";
        public static readonly string InvalidPageSize = "Invalid PageSize";
        public static readonly string InvalidStartIndex = "Invalid StartIndex";
        public static readonly string InvalidKouiKbn = "Invalid KouiKbn";
        public static readonly string InvalidValueAdopted = "Invalid Value Adopted";
        public static readonly string InvalidItemCd = "Invalid ItemCd";
        public static readonly string InvalidFutansyaNo = "Invalid FutansyaNo";
        public static readonly string InvalidUsageKbn = "Invalid UsageKbn";
        public static readonly string InvalidKohiId = "Invalid HokenKohiId";
        public static readonly string InvalidGrpCd = "Invalid GrpCd";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";
        public static readonly string NoData = "No data";
        public static readonly string Failed = "Failed";
        public static readonly string InputDataNull = "Input data is null";


        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        //Patient Infor

        //Group Infor
        public static readonly string DuplicateGroupId = "Can not duplicate GroupId";
        public static readonly string DuplicateGroupName = "Can not duplicate GroupName";
        public static readonly string DuplicateGroupDetailCode = "Can not Duplicate GroupDetail Code";
        public static readonly string DuplicateGroupDetailSeqNo = "Can not Duplicate GroupDetail SeqNo";
        public static readonly string DuplicateGroupDetailName = "Can not Duplicate GroupDetail Name";
        public static readonly string InvalidGroupId = "Invalid GroupId, GroupId > 0";
        public static readonly string InvalidGroupName = "Invalid GroupName, GroupName is required and length must be less than or equal 20";
        public static readonly string InvalidDetailGroupCode = "Invalid GroupCode, GroupCode is required and length must be less than or equal 2";
        public static readonly string InvalidGroupDetailName = "Invalid GroupDetailName, GroupDetailName is required and length must be less than or equal 30";

        //Reception controller

        //PtDisease controller
        public static readonly string UpsertPtDiseaseListSuccess = "Upsert value successfully.";
        public static readonly string UpsertPtDiseaseListFail = "Upsert value fail.";
        public static readonly string UpsertPtDiseaseListInputNoData = "Input no data.";
        public static readonly string UpsertPtDiseaseListInvalidTenkiKbn = "Invalid TenKiKbn.";
        public static readonly string UpsertPtDiseaseListInvalidSikkanKbn = "Invalid SikkanKbn.";
        public static readonly string UpsertPtDiseaseListInvalidNanByoCd = "Invalid NanByoCd.";
        public static readonly string UpsertPtDiseaseListPtIdNoExist = "PtId no exist.";
        public static readonly string UpsertPtDiseaseListHokenPIdNoExist = "HokenPId no exist.";
        public static readonly string UpsertPtDiseaseListInvalidFreeWord = "Free word must be less than or equal 40.";
        public static readonly string UpsertPtDiseaseListInvalidTenkiDateContinue = "Invalid TenkiDate Continue.";
        public static readonly string UpsertPtDiseaseListInvalidTenkiDateAndStartDate = "TenkiDate must more than or equal start date";
        public static readonly string UpsertPtDiseaseListInvalidByomei = "Invalid Byomei";
        public static readonly string UpsertPtDiseaseListInvalidId = "Invalid Id";
        public static readonly string UpsertPtDiseaseListInvalidHpId = "Invalid HpId";
        public static readonly string UpsertPtDiseaseListInvalidPtId = "Invalid PtId";
        public static readonly string UpsertPtDiseaseListInvalidSortNo = "Invalid SortNo";
        public static readonly string UpsertPtDiseaseListInvalidByomeiCd = "Invalid ByomeiCd";
        public static readonly string UpsertPtDiseaseListInvalidStartDate = "Invalid Byomei Start Date";
        public static readonly string UpsertPtDiseaseListInvalidTenkiDate = "Invalid TenkiDate";
        public static readonly string UpsertPtDiseaseListInvalidSyubyoKbn = "Invalid SyubyoKbn";
        public static readonly string UpsertPtDiseaseListInvalidHosokuCmt = "Invalid HosokuCmt";
        public static readonly string UpsertPtDiseaseListInvalidHokenPid = "Invalid HokenPid";
        public static readonly string UpsertPtDiseaseListInvalidIsNodspRece = "Invalid IsNodspRece";
        public static readonly string UpsertPtDiseaseListInvalidIsNodspKarte = "Invalid IsNodspKarte";
        public static readonly string UpsertPtDiseaseListInvalidSeqNo = "Invalid SeqNo";
        public static readonly string UpsertPtDiseaseListInvalidIsImportant = "Invalid IsImportant";
        public static readonly string UpsertPtDiseaseListInvalidIsDeleted = "Invalid IsDeleted";

        //Insurance

        //KarteInf controller
        public static readonly string GetKarteInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string GetKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string GetKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetKarteInfNoData = "No Data";
        public static readonly string GetKarteInfSuccessed = "Successed";

        //OrdInf controller
        public static readonly string GetOrdInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string GetOrdInfInvalidHpId = "Invalid HpId";
        public static readonly string GetOrdInfInvalidPtId = "Invalid PtId";
        public static readonly string GetOrdInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetOrdInfNoData = "No Data";
        public static readonly string GetOrdInfSuccessed = "Successed";

        //RaiinKubun controller

        //Calculation Inf


        //Medical examination controller
        public static readonly string GetMedicalExaminationInvalidPtId = "Invalid PtId";
        public static readonly string GetMedicalExaminationInvalidHpId = "Invalid HpId";
        public static readonly string GetMedicalExaminationInvalidStartIndex = "Invalid Start Index Of Page";
        public static readonly string GetMedicalExaminationInvalidSinDate = "Invalid SinDate";
        public static readonly string GetMedicalExaminationNoData = "No Data";
        public static readonly string GetMedicalExaminationSuccessed = "Successed";
        public static readonly string GetMedicalExaminationInvalidDeleteCondition = "Invalid Delete Condition";
        public static readonly string GetMedicalExaminationInvalidFilterId = "Invalid FilterId";
        public static readonly string GetMedicalExaminationInvalidPageSize = "Invalid PageSize";
        public static readonly string GetMedicalExaminationInvalidSearchType = "Invalid Search Type";
        public static readonly string GetMedicalExaminationInvalidSearchCategory = "Invalid Search Category";
        public static readonly string GetMedicalExaminationInvalidSearchText = "Invalid Search Text";
        public static readonly string GetMedicalExaminationInvalidUserId = "Invalid UserId";

        //OrdInf controller

        //RaiinKubun controller

        //SetMst
        public static readonly string GetSetListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetListSinDate = "Invalid SinDate";
        public static readonly string GetSetListInvalidSetKbn = "Invalid SetKbn";
        public static readonly string GetSetListInvalidSetKbnEdaNo = "Invalid SetKbnEdaNo";
        public static readonly string GetSetListNoData = "No Data";
        public static readonly string GetSetListSuccessed = "Successed";
        public static readonly string InvalidLevel = "Invalid Level, can't move";
        public static readonly string InvalidUserId = "Invalid UserId, can't move";
        public static readonly string InvalidCopySetCd = "Invalid CopySetCd, CopySetCd >= 0";
        public static readonly string InvalidPasteSetCd = "Invalid PasteSetCd, PasteSetCd > 0";
        public static readonly string InvalidDragSetCd = "Invalid DragSetCd, DragSetCd >= 0";
        public static readonly string InvalidDropSetCd = "Invalid DropSetCd, DropSetCd > 0";
        public static readonly string InvalidSetCd = "Invalid SetCd, SetCd >= 0";
        public static readonly string InvalidSetKbn = "Invalid SetKbn, SetKbn >= 1 and SetKbn <= 10";
        public static readonly string InvalidSetKbnEdaNo = "Invalid SetKbnEdaNo, SetKbnEdaNo >= 1 and SetKbnEdaNo <= 6";
        public static readonly string InvalidGenarationId = "Invalid GenarationId, GenarationId >= 0";
        public static readonly string InvalidLevel1 = "Invalid Level1, Level1 > 0";
        public static readonly string InvalidLevel2 = "Invalid Level2, Level2 >= 0";
        public static readonly string InvalidLevel3 = "Invalid Level3, Level3 >= 0";
        public static readonly string InvalidSetName = "Invalid SetName, SetName maxlength is 60";
        public static readonly string InvalidWeightKbn = "Invalid WeightKbn, WeightKbn >= 0";
        public static readonly string InvalidColor = "Invalid Color, Color >= 0";

        //Set
        public static readonly string GetSetKbnListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetKbnListSinDate = "Invalid SinDate";
        public static readonly string GetSetKbnListInvalidSetKbnFrom = "Invalid SetKbnFrom";
        public static readonly string GetSetKbnListInvalidSetKbnTo = "Invalid SetKbnTo";
        public static readonly string GetSetKbnListInvalidSetKbn = "SetKbnTo must more than SetKbnFrom";
        public static readonly string GetSetKbnListNoData = "No Data";
        public static readonly string GetSetKbntListSuccessed = "Successed";
        public static readonly string SaveSetByomeiFailed = "Save SetByomei Failed.";
        public static readonly string SaveSetOrderInfFailed = "Save SetKarteInf Failed.";
        public static readonly string SaveSetKarteInfFailed = "Save SetKarteInf Failed.";
        //Calculation Inf


        // Visiting controller
        //  - UpdateStaticCell
        public static readonly string UpdateReceptionStaticCellUnknownError = "Failed to update cell value.";
        public static readonly string UpdateReceptionStaticCellSuccess = "Cell value updated successfully.";
        public static readonly string UpdateReceptionStaticCellInvalidHpId = "HpId must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidSinDate = "SinDate must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidPtId = "PtId must be greater than 0.";
        //  - UpdateDynamicCell
        public static readonly string UpdateReceptionDynamicCellSuccess = "Cell value updated successfully.";
        public static readonly string UpdateReceptionDynamicCellInvalidHpId = "HpId must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidSinDate = "SinDate must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidPtId = "PtId must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidGrpId = "GrpId cannot be negative.";

        //Flowsheet
        public static readonly string UpsertFlowSheetSuccess = "Upsert value successfully.";
        public static readonly string UpsertFlowSheetInvalidPtId = "PtId must be greater than 0.";
        public static readonly string UpsertFlowSheetInvalidSinDate = "SinDate is no valid.";
        public static readonly string UpsertFlowSheetInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpsertFlowSheetInvalidCmtKbn = "CmtKbn is no valid.";
        public static readonly string UpsertFlowSheetInvalidTagNo = "TagNo is no valid";
        public static readonly string UpsertFlowSheetInvalidRainCmtSeqNo = "RainCmtSeqNo must be greater than or equal 0.";
        public static readonly string UpsertFlowSheetInvalidRainListTagSeqNo = "RainListTagSeqNo must be greater than or equal 0.";
        public static readonly string UpsertFlowSheetUpdateNoSuccess = "Update is no successful.";
        public static readonly string UpsertFlowSheetInputDataNoValid = "Input data no valid.";
        public static readonly string UpsertFlowSheetRainNoNoExist = "RainNo No Exist.";
        public static readonly string UpsertFlowSheetPtIdNoExist = "PtId No Exist.";

        // Schema
        public static readonly string InvalidOldImage = "Invalid old image.";
        public static readonly string DeleteSuccessed = "Delete image successed.";
        public static readonly string InvalidFileImage = "File image is not null.";



        // Today Validate Order
        public static readonly string TodayOrdInvalidSpecialItem = "Special item doesn't contain drug, injection and other";
        public static readonly string TodayOrdIInvalidSpecialStadardUsage = "Special item doesn't contain standard usage";
        public static readonly string TodayOrdInvalidOdrKouiKbn = "Value of OdrKouiKbn is invalid ";
        public static readonly string TodayOrdInvalidSpecialSuppUsage = "Special item doesn't contain supply usage";
        public static readonly string TodayOrdInvalidHasUsageButNotDrug = "Item which differs drug item, it doesn't have drug usage";
        public static readonly string TodayOrdInvalidHasUsageButNotInjectionOrDrug = "Item which differs drug item or injection item, it doesn't have injection usage";
        public static readonly string TodayOrdInvalidHasDrugButNotUsage = "Drug item doesn't have usage";
        public static readonly string TodayOrdInvalidHasInjectionButNotUsage = "Injection item doesn't have usage";
        public static readonly string TodayOrdInvalidHasNotBothInjectionAndUsageOf28 = "Self Injection doesn't have self injection detail and usage";
        public static readonly string TodayOrdInvalidStandardUsageOfDrugOrInjection = "Standard usage of drug item or usage of injection item don't more than 1";
        public static readonly string TodayOrdInvalidSuppUsageOfDrugOrInjection = "Supply usage of drug item or usage of injection item don't more than 1";
        public static readonly string TodayOrdInvalidBunkatu = "Bunkatu item of drug item doesn't more than 1";
        public static readonly string TodayOrdInvalidUsageWhenBuntakuNull = "Bunkatu item doesn't have usage";
        public static readonly string TodayOrdInvalidSumBunkatuDifferentSuryo = "Bunkatu item has sum of suryo not equal bunkatu";
        public static readonly string TodayOrdInvalidQuantityUnit = "Has unit but doesn't have quantity";
        public static readonly string TodayOrdInvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull = "Has unit but yohoKbn and Suryo don't invalid (YohoKbn != 1 and Suryo > 999)";
        public static readonly string TodayOrdInvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu = "Bunkatu item doesn't have suryo and bunkatu";
        public static readonly string TodayOrdInvalidPrice = "Price must more than 0 and (suryo * price) <= 999999999";
        public static readonly string TodayOrdInvalidCmt840 = "CmtOpt is not null and CmtName is not null when CmtCol1 of Cmt840 > 0";
        public static readonly string TodayOrdInvalidCmt842 = "CmtOpt of Cmt842 is not null and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt842CmtOptMoreThan38 = "CmtOpt of Cmt842 is not null and has length less than or equal 38";
        public static readonly string TodayOrdInvalidCmt830CmtOpt = "CmtOpt of Cmt830 is not null and not white space";
        public static readonly string TodayOrdInvalidCmt830CmtOptMoreThan38 = "CmtOpt of Cmt830 is not null and has length less than or equal 38";
        public static readonly string TodayOrdInvalidCmt831 = "CmtOpt of Cmt831 is not null and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt850Date = "CmtOpt of Cmt850 is not map format and CmtName is not null when CmtName contain day";
        public static readonly string TodayOrdInvalidCmt850OtherDate = "CmtOpt of Cmt850 is not map format and CmtName is not null when CmtName doesn't contain day";
        public static readonly string TodayOrdInvalidCmt851 = "CmtOpt of Cmt851 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt852 = "CmtOpt of Cmt852 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt853 = "CmtOpt of Cmt853 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt880 = "CmtOpt of Cmt880 is not null and CmtName is not null";
        public static readonly string TodayOrdDuplicateTodayOrd = "Duplicate RpNo and RpNoEdaNo";
        public static readonly string TodayOrdInvalidKohatuKbn = "Value of KohatuKbn is not invalid";
        public static readonly string TodayOrdInvalidDrugKbn = "Value of DrugKbn is not invalid";
        public static readonly string TodayOrdInvalidSuryoOfReffill = "Suryo must  more than refill setting";
        public static readonly string TodayOrdInvalidRowNo = "RowNo must more than 0";
        public static readonly string TodayOrdInvalidSinKouiKbn = "SinKouiKbn must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidItemCd = "Length of ItemCd must less than 10 or equal 10";
        public static readonly string TodayOrdInvalidItemName = "Length of ItemName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidSuryo = "Suryo must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidUnitName = "Length of UnitName must less than 24 or equal 24";
        public static readonly string TodayOrdInvalidUnitSbt = "UnitSbt must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidTermVal = "TermVal must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidSyohoKbn = "SyohoKbn must more than 0 or equal 0 and less than 3 or equal 3";
        public static readonly string TodayOrdInvalidSyohoLimitKbn = "SyohoLimitKbn must more than 0 or equal 0 and less than 3 or equal 3";
        public static readonly string TodayOrdInvalidYohoKbn = "YohoKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidIsNodspRece = "IsNodspRece  must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidIpnCd = "Length of IpnCd must less than 12 or equal 12";
        public static readonly string TodayOrdInvalidIpnName = "Length of IpnName must less than 120 or equal 120";
        public static readonly string TodayOrdInvalidJissiKbn = "JissiKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidJissiId = "JissiId must more than 0";
        public static readonly string TodayOrdInvalidJissiMachine = "Length of JissiMachine must less than 60 or equal 60";
        public static readonly string TodayOrdInvalidReqCd = "Length of ReqCd must less than 10 or equal 10";
        public static readonly string TodayOrdInvalidCmtName = "Length of CmtName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidCmtOpt = "Length of CmtOpt must less than 38 or equal 38";
        public static readonly string TodayOrdInvalidFontColor = "Length of FontColor must less than 8 or equal 8";
        public static readonly string TodayOrdInvalidCommentNewline = "CommentNewline must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidRpNo = "RpNo must more than 0";
        public static readonly string TodayOrdInvalidRpEdaNo = "RpEdaNo must more than 0";
        public static readonly string TodayOrdInvalidHokenPId = "HokenPId must more than 0";
        public static readonly string TodayOrdInvalidRpName = "Length of RpName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidInoutKbn = "InoutKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidSikyuKbn = "SikyuKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidSyohoSbt = "SyohoSbt must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidSanteiKbn = "SanteiKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidTosekiKbn = "TosekiKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidDaysCnt = "DaysCnt must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidSortNo = "SortNo must more than 0";
        public static readonly string TodayOrdInvalidId = "Id of OrdInf must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidPtId = "PtId must more than 0";
        public static readonly string TodayOrdInvalidRaiinNo = "RaiinNo must more than 0";
        public static readonly string TodayOrdInvalidSinDate = "SinDate must more than 0";
        public static readonly string TodayOrdInvalidHpId = "HpId must more than 0";
        public static readonly string TodayOrdInvalidBunkatuLength = "Length of Bunkatu must lest than 10 or equal 10";
        public static readonly string TodayOrdInvalidIsDeleted = "IsDeleted must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidInsertedExist = "This Rp has been exited";
        public static readonly string TodayOrdInvalidUpdatedNoExist = "This Rp hasn't been exited to update";

        //MaxMoney
        public static readonly string HokenKohiNotValidToGet = "This kohi is not valid to get maxmoney";

        // SuperSetDetail
        public static readonly string InvalidSetByomeiId = "Invalid SetByomeiId, SetByomeiId > 0.";
        public static readonly string InvalidSikkanKbn = "Invalid SikkanKbn, SikkanKbn >.0";
        public static readonly string InvalidNanByoCd = "Invalid NanByoCd, NanByoCd > 0.";
        public static readonly string InvalidByomeiCdOrSyusyokuCd = "Invalid ByomeiCd or SyusyokuCd, ByomeiCd or SyusyokuCd not found.";
        public static readonly string SetCdNotExist = "SetCd Not Exist.";
        public static readonly string FullByomeiMaxlength160 = "Length of FullByomei must less than 160 or equal 160.";
        public static readonly string ByomeiCmtMaxlength80 = "Length of ByomeiCmt must less than 80 or equal 80.";
        public static readonly string RpNameMaxLength240 = "Length of SetOrder RpName must less than 240 or equal 240.";
        public static readonly string InvalidSetOrderInfId = "Id of SetOrderInf must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfRpNo = "SetOrder RpNo must more than 1 or equal 1";
        public static readonly string InvalidSetOrderInfRpEdaNo = "SetOrder RpEdaNo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfKouiKbn = "SetOrder KouiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfInoutKbn = "SetOrder InoutKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSikyuKbn = "SetOrder SikyuKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSyohoSbt = "SetOrder SyohoSbt must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSanteiKbn = "SetOrder SanteiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfTosekiKbn = "SetOrder TosekiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfDaysCnt = "SetOrder DaysCnt must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSortNo = "SetOrder SortNo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSinKouiKbn = "SetOrderDetail SinKouiKbn must more than 0 or equal 0";
        public static readonly string ItemCdMaxLength10 = "Length of SetOrderDetail ItemCd must less than 10 or equal 10.";
        public static readonly string ItemNameMaxLength240 = "Length of SetOrderDetail ItemCd must less than 240 or equal 240.";
        public static readonly string UnitNameMaxLength24 = "Length of SetOrderDetail UnitName must less than 24 or equal 24.";
        public static readonly string InvalidSetOrderSuryo = "SetOrderDetail Suryo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderUnitSBT = "SetOrderDetail UnitSBT must more than 0 or equal 0";
        public static readonly string InvalidSetOrderTermVal = "SetOrderDetail TermVal must more than 0 or equal 0";
        public static readonly string InvalidSetOrderKohatuKbn = "SetOrderDetail KohatuKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSyohoKbn = "SetOrderDetail SyohoKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSyohoLimitKbn = "SetOrderDetail SyohoLimitKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderDrugKbn = "SetOrderDetail DrugKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderYohoKbn = "SetOrderDetail YohoKbn must more than 0 or equal 0";
        public static readonly string Kokuji1MaxLength1 = "Length of SetOrderDetail Kokuji1 must less than 1 or equal 1.";
        public static readonly string Kokuji2MaxLength1 = "Length of SetOrderDetail Kokuji2 must less than 1 or equal 1.";
        public static readonly string InvalidSetOrderIsNodspRece = "SetOrderDetail IsNodspRece must more than 0 or equal 0";
        public static readonly string IpnCdMaxLength12 = "Length of SetOrderDetail IpnCd must less than 12 or equal 12.";
        public static readonly string IpnNameMaxLength120 = "Length of SetOrderDetail IpnName must less than 120 or equal 120.";
        public static readonly string BunkatuMaxLength10 = "Length of SetOrderDetail Bunkatu must less than 10 or equal 10.";
        public static readonly string CmtNameMaxLength240 = "Length of SetOrderDetail CmtName must less than 240 or equal 240.";
        public static readonly string CmtOptMaxLength38 = "Length of SetOrderDetail CmtOpt must less than 38 or equal 38.";
        public static readonly string FontColorMaxLength8 = "Length of SetOrderDetail FontColor must less than 8 or equal 8.";
        public static readonly string InvalidSetOrderCommentNewline = "SetOrderDetail CommentNewline must more than 0 or equal 0";
        public static readonly string RpNoOrRpEdaNoIsNotExist = "RpNo or RpEdaNo is not exist";

        // KaMst
        public static readonly string InvalidKaId = "Invalid KaId, KaId > 0";
        public static readonly string KaSnameMaxLength20 = "Length of KaSname must lest than 20 or equal 20";
        public static readonly string KaNameMaxLength40 = "Length of KaName must lest than 40 or equal 40";
        public static readonly string ReceKaCdNotFound = "ReceKaCd is NotFound";
        public static readonly string CanNotDuplicateKaId = "Can not duplicate KaId";

        
        //Monshin
        public static readonly string InputDataDoesNotExists = "Input Data does not exist";
        
        //Alrgy Drug
        public static readonly string AddAlrgyDrugInvalidCmt = "Invalid Cmt";
        public static readonly string AddAlrgyDrugInvalidPtId = "Invalid PtId";
        public static readonly string AddAlrgyDrugHpIdNoExist = "No exist HpId";
        public static readonly string AddAlrgyDrugPtIdNoExist = "No exist PtId";
        public static readonly string AddAlrgyDrugInvalidItemCd = "Invalid ItemCd";
        public static readonly string AddAlrgyDrugInvalidSortNo = "Invalid SortNo";
        public static readonly string AddAlrgyDrugInvalidStartDate = "Invalid StartDate";
        public static readonly string AddAlrgyDrugInvalidEndDate = "Invalid EndDate";
        public static readonly string AddAlrgyDrugInvalidDrugName = "Invalid DrugName";
        public static readonly string AddAlrgyDrugDuplicate = "This drug has existed";
        public static readonly string AddAlrgyDrugInputNoData = "Hasn't input data";
        public static readonly string AddAlrgyDrugItemCd = "ItemCd no existed";

        //HokenSyamst
        public static readonly string InvalidHokenSyaNo = "HokenSyaNo is null or empty";

        //DetailHokenMst
        public static readonly string DetailHokenMstInvalidHokenEdaNo = "HokenEdaNo is not valid";
        public static readonly string DetailHokenMstInvalidHokenNo = "HokenNo is not valid";
        public static readonly string DetailHokenMstInvalidPrefNo = "PrefNo is not valid";

        //PostCode
        public static readonly string InvalidPostCode = "Invalid PostCode";
    }
}
