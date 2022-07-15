namespace EmrCloudApi.Tenant.Constants
{
    public static class ResponseMessage
    {
        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";

        public static readonly string GetUserListSuccessed = "Get userList successed!!!";
        public static readonly string GetPatientInforListSuccessed = "Get PatientInfor List successed!!!";
        public static readonly string GetPatientByIdSuccessed = "Get Patient By Id successed!!!";
        public static readonly string NotFoundData = "Not Found Data !";


        //Reception controller
        public static readonly string GetReceptionInvalidRaiinNo = "Invalid raiinNo";
        public static readonly string GetReceptionNotExisted = "Not existed";
        public static readonly string GetReceptionSuccessed = "Successed";
    }
}
