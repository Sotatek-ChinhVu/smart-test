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
        public static readonly string InvalidFutansyaNo = "Invalid FutansyaNo";

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
        public static readonly string InvalidDragSetCd = "Invalid DragSetCd, CopySetCd >= 0";
        public static readonly string InvalidDropSetCd = "Invalid DropSetCd, CopySetCd > 0";
        public static readonly string InvalidSetCd = "Invalid SetCd, SetCd >= 0";
        public static readonly string InvalidSetKbn = "Invalid SetKbn, SetKbn >= 1 and SetKbn <= 10";
        public static readonly string InvalidSetKbnEdaNo = "Invalid SetKbnEdaNo, SetKbnEdaNo >= 1 and SetKbnEdaNo <= 6";
        public static readonly string InvalidGenarationId = "Invalid GenarationId, GenarationId >= 0";
        public static readonly string InvalidLevel1 = "Invalid Level1, Level1 > 0";
        public static readonly string InvalidLevel2 = "Invalid Level2, Level2 >= 0";
        public static readonly string InvalidLevel3 = "Invalid Level3, Level3 >= 0";
        public static readonly string InvalidSetName = "Invalid SetName, SetName maxlength is 80";
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
    }
}
