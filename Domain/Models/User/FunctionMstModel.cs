namespace Domain.Models.User
{
    public class FunctionMstModel
    {
        public FunctionMstModel(string functionCd, string functionName, List<PermissionMstModel> permissions)
        {
            FunctionCd = functionCd;
            FunctionName = functionName;
            Permissions = permissions;
            UserMstModel = new();
        }
        public FunctionMstModel(string functionCd, string functionName, int jobCd, List<PermissionMstModel> permissions, UserPermissionModel userMstModel)
        {
            FunctionCd = functionCd;
            FunctionName = functionName;
            JobCd = jobCd;
            Permissions = permissions;
            UserMstModel = userMstModel;
        }

        public string FunctionCd { get; private set; }

        public string FunctionName { get; private set; }

        public int JobCd { get; private set; }

        public UserPermissionModel UserMstModel { get; private set; }

        public List<PermissionMstModel> Permissions { get; private set; }

    }
}
