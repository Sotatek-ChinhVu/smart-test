﻿namespace EmrCloudApi.Tenant.Constants
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
        public static readonly string InvalidHpIdNotExist = "HpId not exist";
        public static readonly string InvalidStartDate = "Invalid startDate";
        public static readonly string InvalidIsDeleted = "Invalid isDeleted";
        public static readonly string InvalidSeqNo = "Invalid SeqNo";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";
        public static readonly string NoData = "No data";
        public static readonly string Failed = "Failed";
        public static readonly string InputDataNull = "Input data is null";
        public static readonly string Valid = "Valid";

        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        //Patient Infor
        public static readonly string InvalidPtNum = "Invalid PtNum";

        // RousaiJibai
        public static readonly string InvalidHokenKbn = "Invalid HokenKbn";
        public static readonly string InvalidSelectedHokenInfRousaiSaigaiKbn = "Invalid SelectedHokenInf RousaiSaigaiKbn";
        public static readonly string InvalidSelectedHokenInfRousaiSyobyoDate = "Invalid SelectedHokenInf RousaiSyobyoDate";
        public static readonly string InvalidSelectedHokenInfRyoyoStartDate = "Invalid SelectedHokenInf RyoyoStartDate";
        public static readonly string InvalidSelectedHokenInfRyoyoEndDate = "Invalid SelectedHokenInf RyoyoEndDate";

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
        // Validate Main Insurance
        public static readonly string InvalidPtBirthday = "Invalid PtBirthday";
        public static readonly string InvalidSelectedHokenInfHokenNo = "Invalid SelectedHokenInf HokenNo";
        public static readonly string InvalidSelectedHokenInfStartDate = "Invalid SelectedHokenInf StartDate";
        public static readonly string InvalidSelectedHokenInfEndDate = "Invalid SelectedHokenInf EndDate";
        public static readonly string InvaliSelectedHokenInfHokensyaMstIsKigoNa = "Invalid SelectedHokenInf HokensyaMst IsKigoNa";
        public static readonly string InvalidSelectedHokenInfHonkeKbn = "Invalid SelectedHokenInf HonkeKbn";
        public static readonly string InvalidSelectedHokenInfTokureiYm1 = "Invalid SelectedHokenInf TokureiYm1";
        public static readonly string InvalidSelectedHokenInfTokureiYm2 = "Invalid SelectedHokenInf TokureiYm2";
        public static readonly string InvalidSelectedHokenInfConfirmDate = "Invalid SelectedHokenInf ConfirmDate";
        public static readonly string InvalidSelectedHokenMstHokenNo = "Invalid SelectedHokenMst HokenNo";
        public static readonly string InvalidSelectedHokenMstCheckDegit = "Invalid SelectedHokenMst CheckDegit";
        public static readonly string InvalidSelectedHokenMstAgeStart = "Invalid SelectedHokenMst AgeStart";
        public static readonly string InvalidSelectedHokenMstAgeEnd = "Invalid SelectedHokenMst AgeEnd";
        public static readonly string InvalidSelectedHokenMstStartDate = "Invalid SelectedHokenMst StartDate";
        public static readonly string InvalidSelectedHokenMstEndDate = "Invalid SelectedHokenMst EndDate";

        //KarteInf controller
        public static readonly string GetKarteInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string GetKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string GetKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetKarteInfNoData = "No Data";
        public static readonly string GetKarteInfSuccessed = "Successed";
        public static readonly string UpsertKarteInfInvalidHpId = "Invalid HpId";
        public static readonly string UpsertKarteInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string UpsertKarteInfInvalidKarteKbn = "Invalid KarteKbn";
        public static readonly string UpsertKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string UpsertKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string UpsertKarteInfInvalidIsDeleted = "Invalid IsDeleted";
        public static readonly string UpsertKarteInfRaiinNoNoExist = "RaiinNo No Exist";
        public static readonly string UpsertKarteInfPtIdNoExist = "PtId No Exist";
        public static readonly string UpsertKarteInfKarteKbnNoExist = "KarteKbn No Exist";

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
        public static readonly string SaveSetOrderInfFailed = "Save SetOrderInf Failed.";
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
        public static readonly string InvalidSetOrderInfRpNo = "SetOrder RpNo must more than 0 or equal 0";
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

        //TodayOdr Field
        public static readonly string TodayOdrSuryo = "Suryo";
        public static readonly string TodayOdrCmt = "CmtOpt,CmtName";
        public static readonly string TodayOdrCmt842_830 = "CmtOpt";
        public static readonly string TodayOdrKohatuKbn = "KohatuKbn";
        public static readonly string TodayOdrDrugKbn = "DrugKbn";
        public static readonly string TodayOdrId = "Id";
        public static readonly string TodayOdrHpId = "HpId";
        public static readonly string TodayOdrRaiinNo = "RaiinNo";
        public static readonly string TodayOdrRpNo = "RpNo";
        public static readonly string TodayOdrRpEdaNo = "RpEdaNo";
        public static readonly string TodayOdrPtId = "PtId";
        public static readonly string TodayOdrSinDate = "SinDate";
        public static readonly string TodayOdrHokenPid = "HokenPId";
        public static readonly string TodayOdrRpName = "RpName";
        public static readonly string TodayOdrInOutKbn = "InOutKbn";
        public static readonly string TodayOdrSikyuKbn = "SikyuKbn";
        public static readonly string TodayOdrSyohoSbt = "SyohoSbt";
        public static readonly string TodayOdrSanteiKbn = "SanteiKbn";
        public static readonly string TodayOdrTosekiKbn = "TosekiKbn";
        public static readonly string TodayOdrDaysCnt = "DaysCnt";
        public static readonly string TodayOdrSortNo = "SortNo";
        public static readonly string TodayOdrRowNo = "RowNo";
        public static readonly string TodayOdrSinKouiKbn = "SinKouiKbn";
        public static readonly string TodayOdrItemCd = "ItemCd";
        public static readonly string TodayOdrItemName = "ItemName";
        public static readonly string TodayOdrUnitName = "UnitName";
        public static readonly string TodayOdrUnitSbt = "UnitSbt";
        public static readonly string TodayOdrTermVal = "TermVal";
        public static readonly string TodayOdrSyohoKbn = "SyohoKbn";
        public static readonly string TodayOdrSyohoLimitKbn = "SyohoLimitKbn";
        public static readonly string TodayOdrYohoKbn = "YohoKbn";
        public static readonly string TodayOdrIsNodspRece = "IsNodspRece";
        public static readonly string TodayOdrIpnCd = "IpnCd";
        public static readonly string TodayOdrIpnName = "IpnName";
        public static readonly string TodayOdrJissiKbn = "JissiKbn";
        public static readonly string TodayOdrJissiId = "JissiId";
        public static readonly string TodayOdrJissiMachine = "JissiMachine";
        public static readonly string TodayOdrReqCd = "ReqCd";
        public static readonly string TodayOdrBunkatu = "Bunkatu";
        public static readonly string TodayOdrCmtName = "CmtName";
        public static readonly string TodayOdrCmtOpt = "CmtOpt";
        public static readonly string TodayOdrFontColor = "FontColor";
        public static readonly string TodayOdrCommentNewline = "CommentNewline";
        public static readonly string TodayOdrIsDeleted = "IsDeleted";
        public static readonly string TodayOdrSuryoYohoKbn = "Suryo,YohoKbn";
        public static readonly string TodayOdrSuryoBunkatu = "Suryo,Bunkatu";
        public static readonly string TodayOdrPriceSuryo = "Suryo,Price";

        //Raiin Info TodayOdr
        public static readonly string RaiinInfTodayOdrInvalidSyosaiKbn = "Invalid SyosaiKbn";
        public static readonly string RaiinInfTodayOdrInvalidJikanKbn = "Invalid JikanKbn";
        public static readonly string RaiinInfTodayOdrInvalidHokenPid = "Invalid HokenPid";
        public static readonly string RaiinInfTodayOdrHokenPidNoExist = "HokenPid no exist";
        public static readonly string RaiinInfTodayOdrInvalidSanteiKbn = "Invalid SanteiKbn";
        public static readonly string RaiinInfTodayOdrInvalidTantoId = "Invalid TantoId";
        public static readonly string RaiinInfTodayOdrTatoIdNoExist = "TantoId no exist";
        public static readonly string RaiinInfTodayOdrInvalidKaId = "Invalid KaId";
        public static readonly string RaiinInfTodayOdrKaIdNoExist = "KaId no exist";
        public static readonly string RaiinInfTodayOdrInvalidUKetukeTime = "Invalid UKetukeTime";
        public static readonly string RaiinInfTodayOdrInvalidSinStartTime = "Invalid SinStartTime";
        public static readonly string RaiinInfTodayOdrInvalidSinEndTime = "Invalid SinEndTime";
        public static readonly string RaiinInfTodayOdrPtIdNoExist = "PtId no exist";
        public static readonly string RaiinInfTodayOdrHpIdNoExist = "HpId no exist";
        public static readonly string RaiinInfTodayOdrRaiinNoExist = "RaiinNo no exist";

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

        //Valid Kohi
        public static readonly string InvalidKohiEmptyModel1 = "Invalid kohi1 empty model";
        public static readonly string InvalidKohiHokenMstEmpty1 = "Invalid kohi1 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty1 = "Invalid kohi1 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo1 = "Invalid kohi1 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo1 = "Invalid kohi1 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo01 = "Invalid kohi1 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate1 = "Invalid kohi1 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate1 = "Invalid kohi1 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate1 = "Invalid kohi1 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate1 = "Invalid kohi1 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT1 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo1 = "Invalid kohi1 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo1 = "Invalid kohi1 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge1 = "Invalid kohi1 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull1 = "Invalid kohi1 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel2 = "Invalid kohi2 empty model";
        public static readonly string InvalidKohiHokenMstEmpty2 = "Invalid kohi2 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty2 = "Invalid kohi2 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo2 = "Invalid kohi2 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo2 = "Invalid kohi2 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo02 = "Invalid kohi2 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate2 = "Invalid kohi2 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate2 = "Invalid kohi2 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate2 = "Invalid kohi2 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate2 = "Invalid kohi2 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT2 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo2 = "Invalid kohi2 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo2 = "Invalid kohi2 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge2 = "Invalid kohi2 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull2 = "Invalid kohi2 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel3 = "Invalid kohi3 empty model";
        public static readonly string InvalidKohiHokenMstEmpty3 = "Invalid kohi3 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty3 = "Invalid kohi3 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo3 = "Invalid kohi3 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo3 = "Invalid kohi3 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo03 = "Invalid kohi3 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate3 = "Invalid kohi3 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate3 = "Invalid kohi3 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate3 = "Invalid kohi3 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate3 = "Invalid kohi3 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT3 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo3 = "Invalid kohi3 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo3 = "Invalid kohi3 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge3 = "Invalid kohi3 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull3 = "Invalid kohi3 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel4 = "Invalid kohi4 empty model";
        public static readonly string InvalidKohiHokenMstEmpty4 = "Invalid kohi4 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty4 = "Invalid kohi4 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo4 = "Invalid kohi4 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo4 = "Invalid kohi4 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo04 = "Invalid kohi4 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate4 = "Invalid kohi4 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate4 = "Invalid kohi4 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate4 = "Invalid kohi4 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate4 = "Invalid kohi4 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT4 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo4 = "Invalid kohi4 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo4 = "Invalid kohi4 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge4 = "Invalid kohi4 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull4 = "Invalid kohi4 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmpty21 = "Invalid kohi2 kohi1 empty model";
        public static readonly string InvalidKohiEmpty31 = "Invalid kohi3 kohi1 empty model";
        public static readonly string InvalidKohiEmpty32 = "Invalid kohi3 kohi2 empty model";
        public static readonly string InvalidKohiEmpty41 = "Invalid kohi4 kohi1 empty model";
        public static readonly string InvalidKohiEmpty42 = "Invalid kohi4 kohi2 empty model";
        public static readonly string InvalidKohiEmpty43 = "Invalid kohi4 kohi3 empty model";
        public static readonly string InvalidDuplicateKohi1 = "Invalid duplicate kohi1 empty model";
        public static readonly string InvalidDuplicateKohi2 = "Invalid duplicate kohi2 empty model";
        public static readonly string InvalidDuplicateKohi3 = "Invalid duplicate kohi3 empty model";
        public static readonly string InvalidDuplicateKohi4 = "Invalid duplicate kohi4 empty model";

        // Invalid Insurance Other
        public static readonly string InvalidPatternOtherAge75 = "Warning hokenInf age >= 75 and hokensyaNo length 8 and start 39";
        public static readonly string InvalidPatternOtherAge65 = "Warning hokenInf age < 65 and hokensyaNo length 8 and start 39";
        public static readonly string InvalidCheckDuplicatePattern = "Warning pattern duplicate";

        //Message Error Common
        public static readonly string MInp00010 = "{0}を入力してください。";
        public static readonly string MConf01020 = "{0}ため、{1}が確定できません。";
        public static readonly string MEnt01020 = "既に登録されているため、{0}は登録できません。";
        public static readonly string MInp00041 = "{0}は {1}を入力してください。";
        public static readonly string MFree00030 = "{0}";
        public static readonly string MInp00070 = "{0}は {1}以下を入力してください。";
        public static readonly string MInp00040 = "{0}ため、{1}は登録できません。";

        //Sup Message
        public static readonly string MDrug = "薬剤";
        public static readonly string MInjection = "手技";
        public static readonly string MUsage = "用法";
        public static readonly string MSupUsage1 = "用法";
        public static readonly string MSupUsage2 = "補助用法";
        public static readonly string MEnt01040 = "{0}ため、{1}は登録できません。";
        public static readonly string ErrorCaptionDrugOrInject = "行為や加算が登録されている";
        public static readonly string MQuantity = "数量";
        public static readonly string MTooLargeQuantity = "数量が大きすぎます。";
        public static readonly string MUsageQuantity = "用法の数量";
        public static readonly string MMaxQuantity = "999";
        public static readonly string MMaxLengthOfCmt = "コメントに対する入力値は３８文字以内にしてください。";
        public static readonly string MCmtOptOf830 = "文字情報";
        public static readonly string MCmt831 = "診療行為コード";
        public static readonly string MDateInfor850_1 = "年月日情報";
        public static readonly string MDateInfor850_2 = "年月情報";
        public static readonly string MTimeInfor851 = "時刻情報";
        public static readonly string MTimeInfor852 = "時間（分）情報";
        public static readonly string MDateTimeInfor853 = "日時情報（日、時間及び分を6桁）";
        public static readonly string MDateTimeInfor880 = "年月日情報及び数字情報を入力してください。" + "\r\n" + "※区切り文字「/」スラッシュを間に入力" + "\r\n" + @"　数字情報は数字または次の文字　．－＋≧≦＞＜±";
        public static readonly string MBunkatu = "分割調剤";
        public static readonly string MSumBunkatu = "分割調剤の合計";
        public static readonly string MCommonError = "無効なデータを受信しました。";
        public static readonly string MProcedure = "・手技が入力されているか確認してください。";
    }
}
