namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        //Invalid parameter
        public static readonly string InvalidKeyword = "Invalid keyword";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";


        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        public static readonly string GetUserListSuccessed = "Get userList successed!!!";

        //Patient Infor
        public static readonly string GetPatientInforNotExist = "Not exist data!";
        public static readonly string GetPatientByIdSuccessed = "Successed";
        public static readonly string GetPatientByIdInvalidId = "Invalid PtId";

        //Group Infor
        public static readonly string GetGroupInfInvalidHpId = "Invalid HpId";
        public static readonly string GetGroupInfInvalidPtId = "Invalid PtId";
        public static readonly string GetGroupInfSuccessed = "Successed";

        //Reception controller
        public static readonly string GetReceptionInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetReceptionNotExisted = "Not existed";
        public static readonly string GetReceptionSuccessed = "Success";

        //PtDisease controller
        public static readonly string GetListNotExisted = "Not existed";
        public static readonly string GetPtDiseaseSuccessed = "Success";

        //Insurance
        public static readonly string GetInsuranceListInvalidPtId = "Invalid PtId !";
        public static readonly string GetInsuranceListInvalidHpId = "Invalid HpId !";
        public static readonly string GetInsuranceListInvalidSinDate = "Invalid SinDate !";
        public static readonly string GetInsuranceListSuccessed = "Successed!!!";

        //KarteInf controller
        public static readonly string GetKarteInfInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string GetKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetKarteInfNoData = "No Data";
        public static readonly string GetKarteInfSuccessed = "Successed";

        //OrdInf controller
        public static readonly string GetOrdInfInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetOrdInfInvalidHpId = "Invalid hpId";
        public static readonly string GetOrdInfInvalidPtId = "Invalid ptId";
        public static readonly string GetOrdInfInvalidSinDate = "Invalid sinDate";
        public static readonly string GetOrdInfNoData = "No Data";
        public static readonly string GetOrdInfSuccessed = "Successed";

        //RaiinKubun controller
        public static readonly string GetRaiinKubunMstListSuccessed = "Successed";

        //Calculation Inf
        public static readonly string CalculationInfNotExist = "Not exist data!";
        public static readonly string CalculationInfSuccessed = "Successed";
        public static readonly string CalculationInfInvalidHpId = "Invalid HpId";
        public static readonly string CalculationInfInvalidPtId = "Invalid PtId";

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
    }
}
