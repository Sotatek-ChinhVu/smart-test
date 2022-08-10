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
        public static readonly string InvalidUserIdDoctor = "Invalid UserId Of Doctor";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";
        public static readonly string NoData = "No data";


        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        //Patient Infor

        //Group Infor

        //Reception controller

        //PtDisease controller

        //Insurance

        //KarteInf controller

        //OrdInf controller

        //RaiinKubun controller

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
    }
}
