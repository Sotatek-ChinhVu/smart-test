namespace EmrCloudApi.Requests.User
{
    public class UserPermissionDto
    {
        public UserPermissionDto(string functionCd, int permission)
        {
            FunctionCd = functionCd;
            Permission = permission;
        }

        public string FunctionCd { get; private set; }

        public int Permission { get; private set; }
    }
}
