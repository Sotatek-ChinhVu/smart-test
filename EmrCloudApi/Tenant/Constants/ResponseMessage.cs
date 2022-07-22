namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        public static readonly string GetUserListSuccessed = "Get userList successed!!!";

        //Patient Infor
        public static readonly string GetPatientInforNotExist = "Not exist data!";
        public static readonly string GetPatientByIdSuccessed = "Successed";
        public static readonly string GetPatientByIdInvalidId = "Invalid PtId";

        //Reception controller
        public static readonly string GetReceptionInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetReceptionNotExisted = "Not existed";
        public static readonly string GetReceptionSuccessed = "Successed";

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

    }
}
