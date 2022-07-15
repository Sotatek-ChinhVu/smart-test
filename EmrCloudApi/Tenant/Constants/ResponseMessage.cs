namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        public static readonly string GetUserListSuccessed = "Get userList successed!!!";

        public static readonly string DataNotExist = "Data Not Exist !";
        //Patient Infor
        public static readonly string GetPatientInforListSuccessed = "Get PatientInfor List Successed";
        public static readonly string GetPatientByIdSuccessed = "Successed";
        public static readonly string GetPatientByIdInvalidId = "Invalid PtId";

        //Insurance List
        public static readonly string GetInsuranceListByPtIdInvalidId = "Invalid PtId !";
        public static readonly string GetInsuranceListByPtIdSuccessed = "Successed!!!";

        //Reception controller
        public static readonly string GetReceptionInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetReceptionNotExisted = "Not existed";
        public static readonly string GetReceptionSuccessed = "Successed";
    }
}
