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
        public static readonly string GetReceptionSuccessed = "Success";

        //PtDisease controller
        public static readonly string GetPtIdNotExisted = "Not existed";
        public static readonly string GetPtDiseaseSuccessed = "Success";
    }
}
