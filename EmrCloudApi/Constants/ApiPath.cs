namespace EmrCloudApi.Constants
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
        public const string RemoveCache = "RemoveCache";
        public const string RemoveAllCache = "RemoveAllCache";

        // SuperSet
        public const string Validate = "Validate";
        public const string Reorder = "Reorder";
        public const string Paste = "Paste";
        public const string GetSuperSetDetail = "GetSuperSetDetail";
        public const string DiseaseSearch = "DiseaseSearch";
        public const string DiseaseNameMstSearch = "DiseaseNameMstSearch";
        public const string SaveSuperSetDetail = "SaveSuperSetDetail";
        public const string GetToolTip = "GetToolTip";
        public const string GetSuperSetDetailForTodayOrder = "GetSuperSetDetailForTodayOrder";
        public const string ParrentKensaMst = "ParrentKensaMst";
        public const string GetConversion = "GetConversion";
        public const string SaveConversion = "SaveConversion";
        public const string GetOdrSetName = "GetOdrSetName";
        public const string SaveOdrSet = "SaveOdrSet";
        public const string GetSetGenerationMstList = "GetSetGenerationMstList";
        public const string GetSetKbnMstListByGenerationId = "GetSetKbnMstListByGenerationId";

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
        public const string GetAllCmtCheckMst = "GetAllCmtCheckMst";
        public const string GetJihiMstList = "GetJihiMstList";
        public const string SearchTenMstItem = "SearchTenMstItem";
        public const string ConvertStringChkJISKj = "ConvertStringChkJISKj";
        public const string GetTeikyoByomei = "GetTeikyoByomei";
        public const string GetDrugAction = "GetDrugAction";
        public const string GetDefaultPrecautions = "GetDefaultPrecautions";
        public const string UploadImageDrugInf = "UploadImageDrugInf";
        public const string GetDiseaseList = "GetDiseaseList";
        public const string UpdateCmtCheckMst = "UpdateCmtCheckMst";
        public const string SaveAddressMst = "SaveAddressMst";
        public const string ContainerMasterUpdate = "ContainerMasterUpdate";
        public const string UpsertMaterialMaster = "UpsertMaterialMaster";
        public const string GetSingleDoseMstAndMedicineUnitList = "GetSingleDoseMstAndMedicineUnitList";
        public const string UpdateSingleDoseMst = "UpdateSingleDoseMst";
        public const string UpdateKensaMst = "UpdateKensaMst";
        public const string UpdateByomeiMst = "UpdateByomeiMst";
        public const string IsUsingKensa = "IsUsingKensa";
        public const string UpdateKensaStdMst = "UpdateKensaStdMst";
        public const string GetKensaStdMst = "GetKensaStdMst";
        public const string GetUsedKensaItemCds = "GetUsedKensaItemCds";
        public const string GetTenItemCds = "GetTenItemCds";
        public const string UpdateJihiSbtMst = "UpdateJihiSbtMst";
        public const string GetMaterialMsts = "GetMaterialMsts";
        public const string GetContainerMsts = "GetContainerMsts";
        public const string GetKensaCenterMsts = "GetKensaCenterMsts";
        public const string GetTenOfHRTItem = "GetTenOfHRTItem";
        public const string GetListKensaMst = "GetListKensaMst";
        public const string GetListYohoSetMstModelByUserID = "GetListYohoSetMstModelByUserID";
        public const string GetRenkeiConf = "GetRenkeiConf";
        public const string SaveRenkei = "SaveRenkei";
        public const string GetSetNameMnt = "GetSetNameMnt";
        public const string GetListKensaIjiSetting = "GetListKensaIjiSetting";
        public const string GetTreeListSet = "GetTreeListSet";
        public const string GetTreeByomeiSet = "GetTreeByomeiSet";
        public const string GetListUser = "GetListUser";
        public const string GetListSetGeneration = "GetListSetGeneration";
        public const string GetListByomeiSetGeneration = "GetListByomeiSetGeneration";
        public const string UpdateYohoSetMst = "UpdateYohoSetMst";
        public const string F17Common = "F17Common";
        public const string IsKensaItemOrdering = "IsKensaItemOrdering";
        public const string ExistUsedKensaItemCd = "ExistUsedKensaItemCd";
        public const string GetTenMstByCode = "GetTenMstByCode";
        public const string GetByomeiByCode = "GetByomeiByCode";
        public const string CheckJihiSbtExistsInTenMst = "CheckJihiSbtExistsInTenMst";

        //Schema
        public const string SaveImageTodayOrder = "SaveImageTodayOrder";
        public const string SaveImageSuperSetDetail = "SaveImageSuperSetDetail";
        public const string SaveInsuranceScanImage = "SaveInsuranceScanImage";
        public const string UploadListFileKarte = "UploadListFileKarte";
        public const string GetListInsuranceScan = "GetListInsuranceScan";

        //Special Note
        public const string AddAlrgyDrugList = "AddAlrgyDrugList";
        public const string GetPtWeight = "GetPtWeight";
        public const string GrowthCurve = "GrowthCurve";
        public const string GetStdPoint = "GetStdPoint";

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
        public const string CheckOrdInfInDrug = "CheckOrdInfInDrug";
        public const string InDrug = "InDrug";

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
        public const string ReceiptPreview = "ReceiptPreview";
        public const string ReceiptCheck = "ReceiptCheck";
        public const string ReceiptList = "ReceiptList";
        public const string ReceiptReport = "ReceiptReport";
        public const string AccountingReport = "AccountingReport";
        public const string PeriodReceiptReport = "PeriodReceiptReport";
        public const string StaticReport = "StaticReport";
        public const string PatientManagement = "PatientManagement";
        public const string SyojyoSyoki = "SyojyoSyoki";
        public const string Kensalrai = "Kensalrai";
        public const string ReceiptPrint = "ReceiptPrint";
        public const string MemoMsgPrint = "MemoMsgPrint";
        public const string ReceTarget = "ReceTarget";
        public const string DrugNoteSeal = "DrugNoteSeal";
        public const string Yakutai = "Yakutai";
        public const string AccountingCard = "AccountingCard";
        public const string ExportKarte2 = "ExportKarte2";
        public const string ExportKarte3 = "ExportKarte3";
        public const string KensaLabel = "KensaLabel";
        public const string AccountingCardList = "AccountingCardList";
        public const string WelfareDisk = "WelfareDisk";
        public const string ReceListCsv = "ReceListCsv";
        public const string ExportPeriodReceipt = "ExportPeriodReceipt";
        public const string CheckExistTemplateAccounting = "CheckExistTemplateAccounting";
        public const string ExportStatics = "ExportStatics";
        public const string ExportSta9000Csv = "ExportSta9000Csv";
        public const string KensaHistoryReport = "KensaHistoryReport";
        public const string SetDownloadNameReport = "SetDownloadNameReport";

        //PatientInfor
        public const string SearchEmptyId = "SearchEmptyId";
        public const string GetInsuranceMasterLinkage = "GetInsuranceMasterLinkage";
        public const string SaveInsuranceMasterLinkage = "SaveInsuranceMasterLinkage";
        public const string SavePatientInfo = "SavePatientInfo";
        public const string GetListPatient = "GetListPatient";
        public const string GetPatientInfoBetweenTimesList = "GetPatientInfoBetweenTimesList";
        public const string SearchPatientInfoByPtNum = "SearchPatientInfoByPtNum";
        public const string GetTokkiMstList = "GetTokkiMstList";
        public const string DeletePatientInfo = "DeletePatientInfo";
        public const string CheckValidSamePatient = "CheckValidSamePatient";
        public const string CheckAllowDeletePatientInfo = "CheckAllowDeletePatientInfo";
        public const string SearchPatientInfoByPtIdList = "SearchPatientInfoByPtIdList";
        public const string SavePtKyusei = "SavePtKyusei";
        public const string GetPtInfByRefNo = "GetPtInfByRefNo";
        public const string GetPtInfModelsByName = "GetPtInfModelsByName";
        public const string GetPtInfModelsByRefNo = "GetPtInfModelsByRefNo";
        public const string GetVisitTimesManagementModels = "GetVisitTimesManagementModels";
        public const string UpdateVisitTimesManagement = "UpdateVisitTimesManagement";
        public const string UpdateVisitTimesManagementNeedSave = "UpdateVisitTimesManagementNeedSave";

        //HokenMst
        public const string GetDetailHokenMst = "GetDetailHokenMst";
        public const string GetHokenMasterReadOnly = "GetHokenMasterReadOnly";
        public const string FindHokenInfByPtId = "FindHokenInfByPtId";

        //Validate Main Insurance
        public const string ValidateMainInsurance = "ValidateMainInsurance";

        //Validate Insurance rousai jibai
        public const string ValidateRousaiJibai = "ValidateRousaiJibai";

        //Validate Kohi
        public const string ValidateKohi = "ValidateKohi";

        //validate ValidHokenInf AllType
        public const string ValidHokenInfAllType = "ValidHokenInfAllType";

        //Validate InputItem
        public const string ValidateInputItem = "ValidateInputItem";
        public const string GetSelectiveComment = "GetSelectiveComment";

        //Drug Infor
        public const string GetDrugMenuTree = "GetDrugMenuTree";
        public const string DrugDataSelectedTree = "DrugDataSelectedTree";
        public const string ShowProductInf = "ShowProductInf";
        public const string ShowKanjaMuke = "ShowKanjaMuke";
        public const string ShowMdbByomei = "ShowMdbByomei";
        public const string GetDataPrintDrugInfo = "GetDataPrintDrugInfo";

        //PtKyuseiInf
        public const string GetPtKyuseiInf = "GetPtKyuseiInf";

        // Reception
        public const string GetLastRaiinInfs = "GetLastRaiinInfs";
        public const string GetDataReceptionDefault = "GetDataReceptionDefault";
        public const string GetDefaultSelectedTime = "GetDefaultSelectedTime";
        public const string UpdateTimeZoneDayInf = "UpdateTimeZoneDayInf";
        public const string InitDoctorCombo = "InitDoctorCombo";
        public const string GetRaiinListWithKanInf = "GetRaiinListWithKanInf";
        public const string GetLastKarute = "GetLastKarute";
        public const string RevertDeleteNoRecept = "RevertDeleteNoRecept";
        public const string GetOutDrugOrderList = "GetOutDrugOrderList";
        public const string GetYoyakuRaiinInf = "GetYoyakuRaiinInf";
        public const string GetRaiinInfBySinDate = "GetRaiinInfBySinDate";
        public const string GetHpInf = "GetHpInf";

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
        public const string CheckTrialAccounting = "CheckTrialAccounting";
        public const string GetKensaAuditTrailLog = "GetKensaAuditTrailLog";
        public const string GetContainerMst = "GetContainerMst";
        public const string GetSinkouCountInMonth = "GetSinkouCountInMonth";
        public const string GetHeaderVistitDate = "GetHeaderVistitDate";
        public const string SaveKensaIrai = "SaveKensaIrai";
        public const string GetLastDayInfoList = "GetLastDayInfoList";
        public const string SaveSettingLastDayInfoList = "SaveSettingLastDayInfoList";

        //User Config
        public const string UpdateAdoptedByomeiConfig = "UpdateAdoptedByomeiConfig";
        public const string Sagaku = "Sagaku";
        public const string GetListMedicalExaminationConfig = "GetListMedicalExaminationConfig";
        public const string UpsertUserConfList = "UpsertUserConfList";
        public const string GetUserConfParam = "GetUserConfParam";

        //Get KohiPriority
        public const string GetKohiPriorityList = "GetKohiPriorityList";

        //hoken sya
        public const string GetHokenSyaMst = "GetHokenSyaMst";

        //User
        public const string CheckLockMedicalExamination = "CheckLockMedicalExamination";
        public const string GetPermissionByScreen = "GetPermissionByScreen";
        public const string GetAllPermission = "GetAllPermission";
        public const string GetListUserByCurrentUser = "GetListUserByCurrentUser";
        public const string GetListJobMst = "GetListJobMst";
        public const string GetListFunctionPermission = "GetListFunctionPermission";
        public const string SaveListUserMst = "SaveListUserMst";
        public const string GetUserInfo = "GetUserInfo";

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
        public const string ValidateCreateUKEFile = "ValidateCreateUKEFile";
        public const string GetListSinKoui = "GetListSinKoui";
        public const string DeleteReceiptInfEdit = "DeleteReceiptInfEdit";
        public const string GetListKaikeiInf = "GetListKaikeiInf";
        public const string GetListRaiinInf = "GetListRaiinInf";
        public const string CheckExisReceInfEdit = "CheckExisReceInfEdit";
        public const string GetListSokatuMst = "GetListSokatuMst";
        public const string CheckExistsReceInf = "CheckExistsReceInf";
        public const string CheckExistSyobyoKeika = "CheckExistSyobyoKeika";
        public const string GetNextUketukeNoBySetting = "GetNextUketukeNoBySetting";

        //Accounting
        public const string PaymentMethod = "PaymentMethod";
        public const string WarningMemo = "WarningMemo";
        public const string PtByoMei = "PtByoMei";
        public const string SaveAccounting = "SaveAccounting";
        public const string CheckAccounting = "CheckAccounting";
        public const string GetSystemConfig = "GetSystemConfig";
        public const string GetMeiHoGai = "GetMeiHoGai";
        public const string CheckOpenAccounting = "CheckOpenAccounting";
        public const string IsNyukinExisted = "IsNyukinExisted";
        public const string GetListHokenSelect = "GetListHokenSelect";

        public const string GetMaxMoneyByPtId = "GetMaxMoneyByPtId";

        // Family
        public const string GetFamilyReverserList = "GetFamilyReverserList";
        public const string GetGroupNameMst = "GetGroupNameMst";
        public const string GetListGroupInfo = "GetListGroupInfo";
        public const string GetRaiinInfList = "GetRaiinInfList";
        public const string CheckAllowDeleteGroupMst = "CheckAllowDeleteGroupMst";
        public const string ValidateFamilyList = "ValidateFamilyList";
        public const string GetMaybeFamilyList = "GetMaybeFamilyList";

        //TreeSet Byomei
        public const string GetSetByomeiTree = "GetSetByomeiTree";
        public const string GetAllByomeiByPtId = "GetAllByomeiByPtId";

        //System Conf
        public const string GetSystemConfForPrint = "GetSystemConfForPrint";
        public const string GetDrugCheckSetting = "GetDrugCheckSetting";
        public const string SaveDrugCheckSetting = "SaveDrugCheckSetting";
        public const string GetSystemSetting = "GetSystemSetting";
        public const string SaveSystemSetting = "SaveSystemSetting";
        public const string GetSystemConfListXmlPath = "GetSystemConfListXmlPath";
        public const string GetAllPath = "GetAllPath";
        public const string SavePath = "SavePath";

        //ReceSeikyus
        public const string GetListReceSeikyu = "GetListReceSeikyu";
        public const string SearchReceInf = "SearchReceInf";
        public const string SaveReceSeikyu = "SaveReceSeikyu";
        public const string CancelSeikyu = "CancelSeikyu";
        public const string ImportFileReceSeikyu = "ImportFileReceSeikyu";
        public const string GetReceSeikyModelByPtNum = "GetReceSeikyModelByPtNum";

        //WeightedSetConfirmation
        public const string IsOpenWeightChecking = "IsOpenWeightChecking";

        //ReceiptCreation
        public const string CreateUKEFile = "CreateUKEFile";

        //TenMstMaintenance
        public const string GetListTenMstOrigin = "GetListTenMstOrigin";
        public const string GetTenMstOriginInfoCreate = "GetTenMstOriginInfoCreate";
        public const string DeleteOrRecoverTenMst = "DeleteOrRecoverTenMst";
        public const string GetSetDataTenMst = "GetSetDataTenMst";
        public const string SaveSetDataTenMst = "SaveSetDataTenMst";
        public const string GetRenkeiMst = "GetRenkeiMst";
        public const string CheckIsTenMstUsed = "CheckIsTenMstUsed";
        public const string GetTenMstListByItemType = "GetTenMstListByItemType";

        //Lock
        public const string AddLock = "AddLock";
        public const string CheckLock = "CheckLock";
        public const string RemoveLock = "RemoveLock";
        public const string CheckExistFunctionCode = "CheckExistFunctionCode";
        public const string RemoveAllLock = "RemoveAllLock";
        public const string RemoveAllLockPtId = "RemoveAllLockPtId";
        public const string ExtendTtl = "ExtendTtl";
        public const string GetLockInfo = "GetLockInfo";
        public const string CheckLockVisiting = "CheckLockVisiting";
        public const string CheckLockOpenAccounting = "CheckLockOpenAccounting";
        public const string RemoveLockWhenLogOut = "RemoveLockWhenLogOut"; 
        public const string GetLockInf = "GetLockInf"; 
        public const string Unlock = "Unlock";
        public const string CheckIsExistedOQLockInfo = "CheckIsExistedOQLockInfo";

        //Monshin
        public const string GetMonshinInf = "GetMonshinInf";
        public const string SaveMonshinInf = "SaveMonshinInf";
        public const string GetListDrugImage = "GetListDrugImage";

        //ChartApporval
        public const string CheckSaveLogOutChartApporval = "CheckSaveLogOutOut";

        // MainMenu
        public const string GetStatisticMenuList = "GetStatisticMenuList";
        public const string SaveStatisticMenuList = "SaveStatisticMenuList";
        public const string GetListStaticReport = "GetListStaticReport";
        public const string FindPtHokenList = "FindPtHokenList";
        public const string GetKensaIrai = "GetKensaIrai";
        public const string GetKensaIraiByList = "GetKensaIraiByList";
        public const string GetKensaCenterMstList = "GetKensaCenterMstList";
        public const string CreateDataKensaIraiRenkei = "CreateDataKensaIraiRenkei";
        public const string ReCreateDataKensaIraiRenkei = "ReCreateDataKensaIraiRenkei";
        public const string GetKensaInf = "GetKensaInf";
        public const string DeleteKensaInf = "DeleteKensaInf";
        public const string GetKensaIraiLog = "GetKensaIraiLog";
        public const string GetStaCsvMst = "GetStaCsvMst";
        public const string KensaIraiReport = "KensaIraiReport";
        public const string SaveStaCsvMst = "SaveStaCsvMst";
        public const string ImportKensaIrai = "ImportKensaIrai";
        public const string GetRsvInfToConfirm = "GetRsvInfToConfirm";
        public const string GetListQualificationInf = "GetListQualificationInf";
        public const string GetLoadListVersion = "GetLoadListVersion";
        public const string UpdateListReleasenote = "UpdateListReleasenote";

        //TimeZoneConf.
        public const string GetTimeZoneConfGroup = "GetTimeZoneConfGroup";
        public const string SaveTimeZoneConf = "SaveTimeZoneConf";

        //Holiday
        public const string SaveHolidayMst = "SaveHolidayMst";

        //NextOrder
        public const string CheckNextOrdHaveOdr = "CheckNextOrdHaveOdr";
        public const string CheckUpsertNextOrder = "CheckUpsertNextOrder";

        // Online
        public const string InsertOnlineConfirmHistory = "InsertOnlineConfirmHistory";
        public const string GetRegisterdPatientsFromOnline = "GetRegisterdPatientsFromOnline";
        public const string UpdateOnlineConfirmationHistory = "UpdateOnlineConfirmationHistory";
        public const string UpdateOnlineHistoryById = "UpdateOnlineHistoryById";
        public const string UpdateOQConfirmation = "UpdateOQConfirmation";
        public const string SaveAllOQConfirmation = "SaveAllOQConfirmation";
        public const string SaveOQConfirmation = "SaveOQConfirmation";
        public const string UpdateRefNo = "UpdateRefNo";
        public const string UpdateOnlineInRaiinInf = "UpdateOnlineInRaiinInf";
        public const string UpdatePtInfOnlineQualify = "UpdatePtInfOnlineQualify";
        public const string GetListOnlineConfirmationHistoryByPtId = "GetListOnlineConfirmationHistoryByPtId";
        public const string GetListOnlineConfirmationHistoryModel = "GetListOnlineConfirmationHistoryModel";
        public const string ConvertXmlToQCXmlMsg = "ConvertXmlToQCXmlMsg";
        public const string GetOnlineConsent = "GetOnlineConsent";
        public const string UpdateOnlineConsents = "UpdateOnlineConsents";
        public const string UpdateOnlineConfirmation = "UpdateOnlineConfirmation";
        public const string InsertOnlineConfirmation = "InsertOnlineConfirmation";

        //AccountingFormMst
        public const string GetAccountingFormMst = "GetAccountingFormMstResponse";
        public const string UpdateAccountingFormMst = "UpdateAccountingFormMst";

        //Ka
        public const string GetKaCodeMstYossi = "GetKaCodeMstYossi";
        public const string GetKaCodeYousikiMst = "GetKaCodeYousikiMst";

        public const string Test = "Test";

        //ListSetMst
        public const string UpdateListSetMst = "UpdateListSetMst";

        //ByomeiSetMst
        public const string UpdateByomeiSetMst = "UpdateByomeiSetMst";
        public const string IsHokenInfInUsed = "IsHokenInfInUsed";
        //PatientManagement
        public const string SearchPtInfs = "SearchPtInfs";
        public const string GetHokenMst = "GetHokenMst";
        public const string SaveStaConfMenu = "SaveStaConfMenu";
        public const string GetStaConfMenu = "GetStaConfMenu";

        //SetSendaiGeneration
        public const string Restore = "Restore";

        //KensaHistory
        public const string UpdateKensaSet = "UpdateKensaSet";
        public const string GetListKensaSet = "GetListKensaSet";
        public const string GetListKensaSetDetail = "GetListKensaSetDetail";
        public const string GetListKensaCmtMst = "GetListKensaCmtMst";
        public const string UpdateKensaInfDetail = "UpdateKensaInfDetail";
        public const string GetListKensaInfDetail = "GetListKensaInfDetail";
        public const string GetKensaInfDetailByIraiCd = "GetKensaInfDetailByIraiCd";

        //Search Compare Tenmst
        public const string SearchCompareTenMst = "SearchCompareTenMst";
        public const string SaveCompareTenMst = "SaveCompareTenMst";
        public const string SaveSetNameMnt = "SaveSetNameMnt";
        public const string GetColumnSettingByTableNameList = "GetColumnSettingByTableNameList";
        public const string GetRenkeiTiming = "GetRenkeiTiming";

        //log
        public const string WriteLog = "WriteLog";
        public const string WriteListLog = "WriteListLog";

        //PrescriptionHistory
        public const string GetSinrekiFilterMstList = "GetSinrekiFilterMstList";
        public const string SaveSinrekiFilterMstList = "SaveSinrekiFilterMstList";
        public const string GetContentDrugUsageHistory = "GetContentDrugUsageHistory";
    }
}
