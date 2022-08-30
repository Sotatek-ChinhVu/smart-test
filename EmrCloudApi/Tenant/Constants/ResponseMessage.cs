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
        public static readonly string InvalidStartIndex = "Invalid StartIndex";
        public static readonly string InvalidKouiKbn = "Invalid KouiKbn";
        public static readonly string InvalidValueAdopted = "Invalid Value Adopted";
        public static readonly string InvalidItemCd = "Invalid ItemCd";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";
        public static readonly string NoData = "No data";
        public static readonly string Failed = "Failed";


        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        //Patient Infor

        //Group Infor

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
        public static readonly string GetMedicalExaminationInvalidPageSize = "Invalid PageSize";
        public static readonly string GetMedicalExaminationInvalidSinDate = "Invalid SinDate";
        public static readonly string GetMedicalExaminationNoData = "No Data";
        public static readonly string GetMedicalExaminationSuccessed = "Successed";
        public static readonly string GetMedicalExaminationInvalidPageIndex = "Invalid PageIndex";

        //OrdInf controller

        //RaiinKubun controller

        //Set
        public static readonly string GetSetListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetListSinDate = "Invalid SinDate";
        public static readonly string GetSetListInvalidSetKbn = "Invalid SetKbn";
        public static readonly string GetSetListInvalidSetKbnEdaNo = "Invalid SetKbnEdaNo";
        public static readonly string GetSetListNoData = "No Data";
        public static readonly string GetSetListSuccessed = "Successed";

        //Set
        public static readonly string GetSetKbnListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetKbnListSinDate = "Invalid SinDate";
        public static readonly string GetSetKbnListInvalidSetKbnFrom = "Invalid SetKbnFrom";
        public static readonly string GetSetKbnListInvalidSetKbnTo = "Invalid SetKbnTo";
        public static readonly string GetSetKbnListInvalidSetKbn = "SetKbnTo must more than SetKbnFrom";
        public static readonly string GetSetKbnListNoData = "No Data";
        public static readonly string GetSetKbntListSuccessed = "Successed";
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
    }
}
