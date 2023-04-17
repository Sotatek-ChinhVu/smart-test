﻿namespace EmrCloudApi.Constants
{
    public static class ApiPath
    {
        public const string Get = "Get";
        public const string GetList = "GetList";
        public const string Update = "Update";
        public const string Insert = "Insert";
        public const string Upsert = "Upsert";
        public const string UpsertList = "UpsertList";
        public const string SaveList = "SaveList";
        public const string Save = "Save";
        public const string Revert = "Revert";
        public const string Delete = "Delete";
        public const string GetSettingValue = "GetSettingValue";

        // SuperSet
        public const string Validate = "Validate";
        public const string Reorder = "Reorder";
        public const string Paste = "Paste";
        public const string GetSuperSetDetail = "GetSuperSetDetail";
        public const string DiseaseSearch = "DiseaseSearch";
        public const string SaveSuperSetDetail = "SaveSuperSetDetail";
        public const string GetToolTip = "GetToolTip";
        public const string GetSuperSetDetailForTodayOrder = "GetSuperSetDetailForTodayOrder";

        //Mst Item
        public const string GetDosageDrugList = "GetDosageDrugList";
        public const string GetFoodAlrgy = "GetFoodAlrgy";
        public const string SearchOTC = "SearchOTC";
        public const string SearchSupplement = "SearchSupplement";
        public const string SearchTenItem = "SearchTenItem";
        public const string UpdateAdoptedInputItem = "UpdateAdoptedInputItem";
        public const string UpdateAdoptedByomei = "UpdateAdoptedByomei";
        public const string FindTenMst = "FindTenMst";
        public const string GetAdoptedItemList = "GetAdoptedItemList";
        public const string UpdateAdoptedItemList = "UpdateAdoptedItemList";
        public const string GetCmtCheckMstList = "GetCmtCheckMstList";

        //Schema
        public const string SaveImageTodayOrder = "SaveImageTodayOrder";
        public const string SaveImageSuperSetDetail = "SaveImageSuperSetDetail";
        public const string SaveInsuranceScanImage = "SaveInsuranceScanImage";
        public const string UploadListFileKarte = "UploadListFileKarte";
        public const string GetListInsuranceScan = "GetListInsuranceScan";

        //Special Note
        public const string AddAlrgyDrugList = "AddAlrgyDrugList";
        public const string GetPtWeight = "GetPtWeight";

        //Today Oder
        public const string GetMaxRpNo = "GetMaxRpNo";
        public const string GetHeaderInf = "GetHeaderInf";
        public const string GetDefaultSelectPattern = "GetDefaultSelectPattern";
        public const string GetInsuranceComboList = "GetInsuranceComboList";
        public const string GetValidGairaiRiha = "GetValidGairaiRiha";
        public const string GetValidJihiYobo = "GetValidJihiYobo";
        public const string GetAddedAutoItem = "GetAddedAutoItem";
        public const string AddAutoItem = "AddAutoItem";
        public const string ConvertInputItemToTodayOrder = "ConvertInputItemToTodayOrder";
        public const string CheckedExpired = "CheckedExpired";
        public const string AutoCheckOrder = "AutoCheckOrder";
        public const string ChangeAfterAutoCheckOrder = "ChangeAfterAutoCheckOrder";
        public const string Recaculation = "Recaculation";
        public const string ConvertItem = "ConvertItem";

        // KaCode
        public const string GetListKaCode = "GetListKaCode";
        public const string SaveListKaMst = "SaveListKaMst";

        //PostCode
        public const string SearchPostCode = "SearchPostCode";

        //PatientGroupMst
        public const string SavePatientGroupMst = "SavePatientGroupMst";

        // ExportReport
        public const string ExportKarte1 = "ExportKarte1";
        public const string ExportDrugInfo = "ExportDrugInfo";
        public const string ExportByomei = "ExportByomei";
        public const string ExportOrderLabel = "ExportOrderLabel";
        public const string ExportSijisen = "ExportSijisen";
        public const string ExportNameLabel = "ExportNameLabel";
        public const string MedicalRecordWebId = "MedicalRecordWebId";
        public const string OutDrug = "OutDrug";

        //PatientInfor
        public const string SearchEmptyId = "SearchEmptyId";
        public const string GetInsuranceMasterLinkage = "GetInsuranceMasterLinkage";
        public const string SaveInsuranceMasterLinkage = "SaveInsuranceMasterLinkage";
        public const string SavePatientInfo = "SavePatientInfo";
        public const string GetListPatient = "GetListPatient";
        public const string GetPatientInfoBetweenTimesList = "GetPatientInfoBetweenTimesList";
        public const string SearchPatientInfoByPtNum = "SearchPatientInfoByPtNum";
        public const string GetTokkiMstList = "GetTokkiMstList";

        //HokenMst
        public const string GetDetailHokenMst = "GetDetailHokenMst";

        //Validate Main Insurance
        public const string ValidateMainInsurance = "ValidateMainInsurance";

        //Validate Insurance rousai jibai
        public const string ValidateRousaiJibai = "ValidateRousaiJibai";

        //Validate Kohi
        public const string ValidateKohi = "ValidateKohi";

        //validate ValidHokenInf AllType
        public const string ValidHokenInfAllType = "ValidHokenInfAllType";

        // Validate Insurance Other
        public const string ValidateInsuranceOther = "ValidateInsuranceOther";

        //Validate InputItem
        public const string ValidateInputItem = "ValidateInputItem";
        public const string GetSelectiveComment = "GetSelectiveComment";

        //Drug Infor
        public const string GetDrugMenuTree = "GetDrugMenuTree";
        public const string DrugDataSelectedTree = "DrugDataSelectedTree";
        public const string ShowProductInf = "ShowProductInf";
        public const string ShowKanjaMuke = "ShowKanjaMuke";
        public const string ShowMdbByomei = "ShowMdbByomei";


        //PtKyuseiInf
        public const string GetPtKyuseiInf = "GetPtKyuseiInf";

        // Reception
        public const string GetLastRaiinInfs = "GetLastRaiinInfs";
        public const string GetDataReceptionDefault = "GetDataReceptionDefault";
        public const string GetDefaultSelectedTime = "GetDefaultSelectedTime";
        public const string UpdateTimeZoneDayInf = "UpdateTimeZoneDayInf";
        public const string InitDoctorCombo = "InitDoctorCombo";

        // Validate list pattern
        public const string ValidateListPattern = "ValidateListPattern";

        //Swaphoken
        public const string SwapHoken = "SwapHoken";
        public const string CheckHokenPatternUsed = "CheckHokenPatternUsed";
        public const string ValidateSwapHoken = "ValidateSwapHoken";
        public const string CalculationSwapHoken = "CalculationSwapHoken";

        //RaiinKubun
        public const string GetColumnName = "GetColumnName";

        //GetYohoSetMst
        public const string GetYohoSetMstByItemCd = "GetYohoSetMstByItemCd";

        // Document
        public const string GetListDocumentCategory = "GetListDocumentCategory";
        public const string GetDetailDocumentCategory = "GetDetailDocumentCategory";
        public const string SaveListDocumentCategory = "SaveListDocumentCategory";
        public const string SortDocCategory = "SortDocCategory";
        public const string UploadTemplateToCategory = "UploadTemplateToCategory";
        public const string CheckExistFileName = "CheckExistFileName";
        public const string SaveDocInf = "SaveDocInf";
        public const string DeleteDocInf = "DeleteDocInf";
        public const string DeleteDocTemplate = "DeleteDocTemplate";
        public const string MoveTemplateToOtherCategory = "MoveTemplateToOtherCategory";
        public const string DeleteDocCategory = "DeleteDocCategory";
        public const string DowloadDocumentTemplate = "DowloadDocumentTemplate";
        public const string GetListParamTemplate = "GetListParamTemplate";
        public const string GetListDocComment = "GetListDocComment";
        public const string ConfirmReplaceDocParam = "ConfirmReplaceDocParam";

        //Medical Examination
        public const string GetCheckDiseases = "GetCheckDiseases";
        public const string GetInfCheckedSpecialItem = "GetInfCheckedSpecialItem";
        public const string GetInfCheckedItemName = "GetInfCheckedItemName";
        public const string Search = "Search";
        public const string InitKbnSetting = "InitKbnSetting";
        public const string GetCheckedOrder = "GetCheckedOrder";
        public const string CheckedAfter327Screen = "CheckedAfter327Screen";
        public const string GetHistoryIndex = "GetHistoryIndex";
        public const string ConvertNextOrderToTodayOrder = "ConvertNextOrderToTodayOrder";
        public const string GetSummaryInf = "GetSummaryInf";
        public const string GetMaxAuditTrailLogDateForPrint = "GetMaxAuditTrailLogDateForPrint";
        public const string ConvertFromHistoryToTodayOrder = "ConvertFromHistoryToTodayOrder";
        public const string Calculate = "Calculate";
        public const string GetHistoryFollowSinDate = "GetHistoryFollowSinDate";
        public const string GetOrderSheetGroup = "GetOrderSheetGroup";
        public const string GetOrdersForOneOrderSheetGroup = "GetOrdersForOneOrderSheetGroup";
        public const string TrialAccounting = "TrialAccounting";
        public const string GetKensaAuditTrailLog = "GetKensaAuditTrailLog";
        public const string GetContainerMst = "GetContainerMst";

        //User Config
        public const string UpdateAdoptedByomeiConfig = "UpdateAdoptedByomeiConfig";
        public const string Sagaku = "Sagaku";
        public const string GetListMedicalExaminationConfig = "GetListMedicalExaminationConfig";
        public const string UpsertUserConfList = "UpsertUserConfList";

        //Get KohiPriority
        public const string GetKohiPriorityList = "GetKohiPriorityList";

        //hoken sya
        public const string GetHokenSyaMst = "GetHokenSyaMst";

        //User
        public const string CheckLockMedicalExamination = "CheckLockMedicalExamination";
        public const string GetPermissionByScreen = "GetPermissionByScreen";

        //PtGroupMst 
        public const string SaveGroupNameMst = "SaveGroupNameMst";

        //Order RealtimeChecker
        public const string OrderRealtimeChecker = "OrderRealtimeChecker";

        // Karte
        public const string ConvertTextToRichText = "ConvertTextToRichText";

        // Rece check
        public const string GetReceCmtList = "GetReceCmtList";
        public const string SaveReceCmtList = "SaveReceCmtList";
        public const string GetSyoukiInfList = "GetSyoukiInfList";
        public const string SaveSyoukiInfList = "SaveSyoukiInfList";
        public const string GetSyobyoKeikaList = "GetSyobyoKeikaList";
        public const string SaveSyobyoKeikaList = "SaveSyobyoKeikaList";
        public const string GetReceHenReason = "GetReceHenReason";
        public const string GetReceiCheckList = "GetReceiCheckList";
        public const string SaveReceCheckCmtList = "SaveReceCheckCmtList";
        public const string GetInsuranceReceInfList = "GetInsuranceReceInfList";
        public const string GetDiseaseReceList = "GetDiseaseReceList";
        public const string Recalculation = "Recalculation";
        public const string GetReceCheckOptionList = "GetReceCheckOptionList";
        public const string SaveReceCheckOpt = "SaveReceCheckOpt";
        public const string GetInsuranceInf = "GetInsuranceInf";
        public const string ReceCmtHistory = "ReceCmtHistory";
        public const string SyoukiInfHistory = "SyoukiInfHistory";
        public const string SyobyoKeikaHistory = "SyobyoKeikaHistory";
        public const string GetMedicalDetails = "GetMedicalDetails";
        public const string GetRecePreviewList = "GetRecePreviewList";
        public const string DoReceCmt = "DoReceCmt";
        public const string GetReceiptEdit = "GetReceiptEdit";
        public const string GetSinMeiInMonthList = "GetSinMeiInMonthList";
        public const string GetSinDateRaiinInfList = "GetSinDateRaiinInfList";
        public const string GetReceByomeiChecking = "GetReceByomeiChecking";
        public const string SaveReceiptEdit = "SaveReceiptEdit";
        public const string SaveReceStatus = "SaveReceStatus";
        public const string GetReceStatus = "GetReceStatus";

        //Accounting
        public const string PaymentMethod = "PaymentMethod";
        public const string WarningMemo = "WarningMemo";
        public const string PtByoMei = "PtByoMei";
        public const string SaveAccounting = "SaveAccounting";
        public const string CheckAccounting = "CheckAccounting";
        public const string GetSystemConfig = "GetSystemConfig";
        public const string GetMeiHoGai = "GetMeiHoGai";
        public const string CheckOpenAccounting = "CheckOpenAccounting";

        public const string GetMaxMoneyByPtId = "GetMaxMoneyByPtId";

        // Family
        public const string GetFamilyReverserList = "GetFamilyReverserList";
        public const string GetGroupNameMst = "GetGroupNameMst";
        public const string GetRaiinInfList = "GetRaiinInfList";
        public const string CheckAllowDeleteGroupMst = "CheckAllowDeleteGroupMst";
        public const string ValidateFamilyList = "ValidateFamilyList";

        //TreeSet Byomei
        public const string GetSetByomeiTree = "GetSetByomeiTree";

        //System Conf
        public const string GetSystemConfForPrint = "GetSystemConfForPrint";
        public const string GetDrugCheckSetting = "GetDrugCheckSetting";
        public const string SaveDrugCheckSetting = "SaveDrugCheckSetting";
        public const string GetSystemSetting = "GetSystemSetting";
        public const string SaveSystemSetting = "SaveSystemSetting";

        //ReceSeikyus
        public const string GetListReceSeikyu = "GetListReceSeikyu";
        public const string SearchReceInf = "SearchReceInf";
        public const string SaveReceSeikyu = "SaveReceSeikyu";

        //WeightedSetConfirmation
        public const string IsOpenWeightChecking = "IsOpenWeightChecking";

        //ReceiptCreation
        public const string CreateUKEFile = "CreateUKEFile";

        //TenMstMaintenance
        public const string GetListTenMstOrigin = "GetListTenMstOrigin";
        public const string GetTenMstOriginInfoCreate = "GetTenMstOriginInfoCreate";

        //Monshin
        public const string GetMonshinInf = "GetMonshinInf";
        public const string SaveMonshinInf = "SaveMonshinInf";
    }
}
